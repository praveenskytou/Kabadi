using UnityEngine;
using System.Collections;


public enum AIStates2D
{
    idle, 
    attack,
    defense
}

public class AIBehaviour2D : MonoBehaviour
{

    AIBehaviour2D instance = null;

    public GameObject playerRef;
    private Vector2 attackPosition;
    public AIStates2D currentAIState;
    private Animator animator;
    private Vector3 aiPrevPosition, aiPrevRotation;
    private bool isTurnedRight, isTurnedLeft, isFacingCenter;
    public bool hasTouchedByPlayer;

    //ai coords : params
    public Vector3 initialPosition;
    public float aiSafeDistance;
    public float aiVerticalMoveSpeed, aiHorizontalMoveSpeed;
    public float aiAttackSpeed;//it is always greater than normal speed
    private float xpos, ypos;

    //ai state change vars
    private float timeElapsed_StateChange;
    public float nextStateChangeDuration;//more than 5sec wud be gud

    //ai evade vars
    private string[] aiEvadeDirections = {"left", "right", "front"};
    public string aiEvadeDir;
    private float timeElapsed_Evade;
    public float nextEvadeStartDuration;//more than 3sec wud be gud
    public Vector3 evadeToPosition;

    //misc params
    private float moveStep;
    public Color aiDefaultColor;

	// Use this for initialization
	void Start ()
    {
        timeElapsed_StateChange = Time.time;
        timeElapsed_Evade = Time.time;
        evadeToPosition = new Vector2(transform.position.x - 0.5f, transform.position.y);
        animator = this.GetComponent<Animator>();

        initialPosition = transform.position;
        aiDefaultColor = this.GetComponent<SpriteRenderer>().material.GetColor("_Color");
    }


    // Update is called once per frame
    void Update()
    {

        aiPrevPosition = transform.position;
        aiPrevRotation = transform.rotation.eulerAngles;

        boundsCheck();
        turnTowardsPlayer();

        PerformEvasion();
        PerformStateChange();

        //Animation playback
        if (Vector3.Distance(aiPrevPosition, transform.position) != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        //Check raid state 
        if (GameManager.instRef.isRaidOver)
        {
            transform.position = initialPosition;
            animator.SetBool("isMoving", false);

        }

    }

    private void PerformStateChange()
    {
        //State change logic
        if (Time.time - timeElapsed_StateChange > nextStateChangeDuration)
        {
            if (currentAIState == AIStates2D.defense)
                currentAIState = AIStates2D.attack;
            else
                currentAIState = AIStates2D.defense;

            attackPosition = playerRef.transform.position;//get the recent player pos

            timeElapsed_StateChange = Time.time;
        }


        //If the player is nearer to AI
        if (Vector2.Distance(playerRef.transform.position, this.transform.position) < aiSafeDistance)
        {
            switch (currentAIState)
            {
                case AIStates2D.attack:
                    Attack();
                    break;

                case AIStates2D.defense:
                    Defense();
                    break;
            }
        }
    }

    private void PerformEvasion()
    {
        //Evasion change logic
        ///Set the direction to evade
        if (Time.time - timeElapsed_Evade > nextEvadeStartDuration)
        {
            aiEvadeDir = aiEvadeDirections[Random.Range(0, aiEvadeDirections.Length - 1)];

            if (aiEvadeDir == aiEvadeDirections[0])//left
                evadeToPosition = new Vector2(transform.position.x - 0.5f, transform.position.y);
            else
            if (aiEvadeDir == aiEvadeDirections[1])//right
                evadeToPosition = new Vector2(transform.position.x + 0.5f, transform.position.y);
            else
            if (aiEvadeDir == aiEvadeDirections[2])//front
                evadeToPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);

            timeElapsed_Evade = Time.time;
        }

        //translate the object 
        if (transform.position.x > GameManager.field_LobbyLeft_Limit + 1 &&
              transform.position.x < GameManager.field_EndLineRight_Limit - 1)
        {
            //evade a bit
            moveStep = 1 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, evadeToPosition, moveStep);

        }
    }


    private void Defense()
    {

        moveStep = aiVerticalMoveSpeed * Time.deltaTime;

        if(aiEvadeDir == aiEvadeDirections[0])
            xpos = playerRef.transform.position.x - aiSafeDistance - 1;//left
        else
        if (aiEvadeDir == aiEvadeDirections[1])
            xpos = playerRef.transform.position.x + aiSafeDistance - 1;//right
        else
            xpos = playerRef.transform.position.x ;//if front


        ypos = playerRef.transform.position.y + aiSafeDistance;

        //movement 
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(xpos, ypos), moveStep);

    }

    private void Attack()
    {

        moveStep = aiAttackSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, attackPosition, moveStep);

        if(Vector3.Distance(transform.position, attackPosition) < 1f)
        {
            //revert to defense state
            currentAIState = AIStates2D.defense;
        }

    }

    private void turnTowardsPlayer()
    {
        if(playerRef.transform.position.x - transform.position.x > 2  && !isTurnedRight)//right
        {
            if(isTurnedLeft)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 60f);
            else
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 30f);

            //Debug.Log("Changed the angles");
            isTurnedRight = true;
            isTurnedLeft = false;
            isFacingCenter = false;

        }
        else
        if (playerRef.transform.position.x - transform.position.x < - 2  && !isTurnedLeft)//left
        {

            if(isTurnedRight)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 60f);
            else
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 30f);

            //Debug.Log("Changed the angles");
            isTurnedLeft = true;
            isTurnedRight = false;
            isFacingCenter = false;

        }
        else
        if (playerRef.transform.position.x - transform.position.x > -2 && playerRef.transform.position.x - transform.position.x < 2 && !isFacingCenter)//for centre view
        {

            transform.eulerAngles = new Vector3(0,0, 160);

            //Debug.Log("Changed the angles");
            isFacingCenter = true;
            isTurnedLeft = false;
            isTurnedRight = false;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && currentAIState == AIStates2D.attack)
        {
            Debug.Log("AI caught the Player");
        }
    }

    private void boundsCheck()
    {

        //Endline_back check
        if (transform.position.y > GameManager.field_EndLineBack_Limit)
            transform.position = new Vector2(transform.position.x, GameManager.field_EndLineBack_Limit);

        if (transform.position.y < GameManager.field_MidLine_Limit)
            transform.position = new Vector2(transform.position.x, GameManager.field_MidLine_Limit);

        if (transform.position.x > GameManager.field_LobbyRight_Limit)
            transform.position = new Vector2(GameManager.field_LobbyRight_Limit, transform.position.y);

        if (transform.position.x < GameManager.field_LobbyLeft_Limit)
            transform.position = new Vector2(GameManager.field_LobbyLeft_Limit, transform.position.y);

    }
}
