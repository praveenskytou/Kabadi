using UnityEngine;
using System.Collections;


public enum AIStates3D
{
    idle, 
    attack,
    defense
}

public class AIBehaviour3D : MonoBehaviour
{

    AIBehaviour3D instance = null;

    public GameObject playerRef;
    private Vector3 attackPosition;
    public AIStates3D aiStates3D;
    private Animator animator;
    private Vector3 aiPrevPosition, aiPrevRotation;

    //ai coords : params
    private Vector3 startPosition;
    public float aiSafeDistance;
    public float aiVerticalMoveSpeed, aiHorizontalMoveSpeed;
    public float aiAttackSpeed;//it is always greater than normal speed
    private float xpos, zpos;

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

	// Use this for initialization
	void Start ()
    {
        timeElapsed_StateChange = Time.time;
        timeElapsed_Evade = Time.time;
        aiEvadeDir = aiEvadeDirections[0];
        evadeToPosition = new Vector3(transform.position.x - 0.5f, 0 , transform.position.z);
        animator = this.GetComponent<Animator>();
        aiPrevPosition = transform.position;
        aiPrevRotation = transform.rotation.eulerAngles;

    }


    // Update is called once per frame
    void Update()
    {
        aiPrevPosition = transform.position;
        aiPrevRotation = transform.rotation.eulerAngles;

        boundsCheck();

        //Evasion change logic
        if (Time.time - timeElapsed_Evade > nextEvadeStartDuration)
        {
            aiEvadeDir = aiEvadeDirections[ Random.Range(0,aiEvadeDirections.Length-1) ];

            if (aiEvadeDir == aiEvadeDirections[0])//left
                evadeToPosition = new Vector3(transform.position.x - 0.5f, 0, transform.position.z);
            else
            if (aiEvadeDir == aiEvadeDirections[1])//right
                evadeToPosition = new Vector3(transform.position.x + 0.5f, 0, transform.position.z);
            else
            if (aiEvadeDir == aiEvadeDirections[2])//front
                evadeToPosition = new Vector3(transform.position.x , 0, transform.position.z-0.5f);

            timeElapsed_Evade = Time.time;
        }

        performEvasion(evadeToPosition);
        
        //State change logic
        if (Time.time - timeElapsed_StateChange > nextStateChangeDuration)
        {
            if (aiStates3D == AIStates3D.defense)
                aiStates3D = AIStates3D.attack;
            else
                aiStates3D = AIStates3D.defense;

            attackPosition = playerRef.transform.position;//get the recent player pos

            timeElapsed_StateChange = Time.time;
        }
     
        //If the player is nearer to AI
        if (Vector3.Distance(playerRef.transform.position, this.transform.position) < aiSafeDistance)
        {
            switch (aiStates3D)
            {
                case AIStates3D.attack:
                    Attack();
                    break;

                case AIStates3D.defense:
                    Defense();
                    break;
            }
        }

        
        if ( Vector3.Distance( aiPrevPosition , transform.position ) !=0 )
        {
            //animator.play
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        transform.LookAt(playerRef.transform);
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


        zpos = playerRef.transform.position.z + aiSafeDistance;

        //movement 
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xpos, 0, zpos), moveStep);

    }

    private void Attack()
    {

        moveStep = aiAttackSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, attackPosition, moveStep);

        if(Vector3.Distance(transform.position, attackPosition) < 1f)
        {
            //revert to defense state
            aiStates3D = AIStates3D.defense;
        }

    }

    private void performEvasion(Vector3 evadeToPosition)
    {
        if (transform.position.x > GameManager.field_LobbyLeft_Limit + 1  &&
            transform.position.x < GameManager.field_EndLineRight_Limit - 1)
        {
            //evade a bit
            moveStep = 1 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, evadeToPosition, moveStep);

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && aiStates3D == AIStates3D.attack)
        {
            Debug.Log("AI caught the Player");
        }
    }

    private void boundsCheck()
    {

        //Endline_back check
        if (transform.position.z > GameManager.field_EndLineBack_Limit)
            transform.position = new Vector3(transform.position.x, 0, GameManager.field_EndLineBack_Limit);

        if (transform.position.z < GameManager.field_MidLine_Limit)
            transform.position = new Vector3(transform.position.x, 0, GameManager.field_MidLine_Limit);

        if (transform.position.x > GameManager.field_LobbyRight_Limit)
            transform.position = new Vector3(GameManager.field_LobbyRight_Limit, 0, transform.position.z);

        if (transform.position.x < GameManager.field_LobbyLeft_Limit)
            transform.position = new Vector3(GameManager.field_LobbyLeft_Limit, 0, transform.position.z);

    }
}
