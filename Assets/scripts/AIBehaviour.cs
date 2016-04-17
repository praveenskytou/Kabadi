using UnityEngine;
using System.Collections;


public enum AIStates
{
    idle, 
    attack,
    defense
}

public enum AIEvadeDirection
{
    left,
    right,

}

public class AIBehaviour : MonoBehaviour
{

    AIBehaviour instance = null;

    public GameObject playerRef;
    private Vector2 attackPosition;
    public AIStates aiStates;

    //ai coords : params
    private Vector3 startPosition;
    public float aiSafeDistance;
    public float aiVerticalMoveSpeed, aiHorizontalMoveSpeed;
    public float aiAttackSpeed;//it is always greater than normal speed
    private float xpos, ypos;

    //ai state change vars
    private float timeElapsed_StateChange;
    public float nextStateChangeDuration;//more than 5sec wud be gud

    //ai evade vars
    public AIEvadeDirection aiEvadeDir;
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
        evadeToPosition = new Vector2(transform.position.x , transform.position.y);
    }


    // Update is called once per frame
    void Update()
    {
        boundsCheck();

        //Evasion change logic
        if (Time.time - timeElapsed_Evade > nextEvadeStartDuration)
        {
            if (aiEvadeDir == AIEvadeDirection.right)
            {
                aiEvadeDir = AIEvadeDirection.left;
                evadeToPosition = new Vector2(transform.position.x - 0.5f, transform.position.y);
            }
            else
            {
                aiEvadeDir = AIEvadeDirection.right;
                evadeToPosition = new Vector2(transform.position.x + 0.5f, transform.position.y);
            }

            timeElapsed_Evade = Time.time;
        }

        performEvasion(evadeToPosition);


        //State change logic
        if (Time.time - timeElapsed_StateChange > nextStateChangeDuration)
        {
            if (aiStates == AIStates.defense)
                aiStates = AIStates.attack;
            else
                aiStates = AIStates.defense;

            attackPosition = playerRef.transform.position;//get the recent player pos

            timeElapsed_StateChange = Time.time;
        }


        //If the player is nearer to AI
        if (Vector2.Distance(playerRef.transform.position, this.transform.position) < aiSafeDistance)
        {
            switch (aiStates)
            {
                case AIStates.attack:
                    Attack();
                    break;

                case AIStates.defense:
                    Defense();
                    break;
            }
        }
    }

    private void Defense()
    {

        moveStep = aiVerticalMoveSpeed * Time.deltaTime;

        xpos = playerRef.transform.position.x + 0.2f;
        ypos = playerRef.transform.position.y + aiSafeDistance;

        //movement 
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(xpos, ypos), moveStep);

    }

    private void Attack()
    {

        moveStep = aiAttackSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, attackPosition, moveStep);

        if(Vector3.Distance(transform.position, attackPosition) < 0.2f)
        {
            //revert to defense state
            aiStates = AIStates.defense;
        }

    }

    private void performEvasion(Vector2 evadeToPosition)
    {
            //evade a bit
            moveStep = 1 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, evadeToPosition, moveStep);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && aiStates == AIStates.attack)
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
