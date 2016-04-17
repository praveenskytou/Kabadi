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
    right
}

public class AIBehaviour : MonoBehaviour
{

    AIBehaviour instance = null;

    public GameObject playerRef;
    public AIStates aiStates;

    //ai params
    private Vector3 startPosition;
    public float aiSafeDistance;
    public float aiVerticalMoveSpeed, aiHorizontalMoveSpeed;
    private float xpos, ypos;
    public AIEvadeDirection currentEvadeDirection;

    //misc params
    private float moveStep;

	// Use this for initialization
	void Start ()
    {
	
	}

    void Idle()
    {

    }


    void Defense()
    {

        ////Evasion
        //if (transform.position.x > playerRef.transform.position.x)
        //    currentEvadeDirection = AIEvadeDirection.right;
        //else
        //if (transform.position.x < playerRef.transform.position.x)
        //    currentEvadeDirection = AIEvadeDirection.left;


        //If player comes near the AI, then evade
        if (Vector2.Distance(playerRef.transform.position, this.transform.position) < aiSafeDistance)
        {
            moveStep = aiVerticalMoveSpeed * Time.deltaTime;

            xpos = playerRef.transform.position.x + 0.2f;
            ypos = playerRef.transform.position.y + aiSafeDistance;

            //transform.position = Vector3.MoveTowards(transform.position, playerRef.transform.position, moveStep);
        }
        else
        {
            //If player moves away, then step forward


        }



        //movement 
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(xpos, ypos), moveStep);

    }

    void Attack()
    {

    }

    // Update is called once per frame
    void Update()
    {
        boundsCheck();

        switch (aiStates)
        {
            case AIStates.idle:
                Idle();
                break;

            case AIStates.defense:
                Defense();
                break;
        }
    }


    void boundsCheck()
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
