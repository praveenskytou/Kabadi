using UnityEngine;
using System.Collections;


public enum AIStates
{
    idle, 
    attack,
    defence
}

public class AIBehaviour : MonoBehaviour
{

    AIBehaviour instance = null;

    public GameObject playerRef;
    public AIStates aiStates;

    //ai params
    private Vector3 startPosition;
    public float aiMovementTriggerDistance, aiSafeDistance;
    public float aiMoveSpeed;
    public float moveStep;
    private Vector3 prevPlayerPosition;

    void Awake()
    {
        instance = this;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        startPosition = this.transform.position;
        prevPlayerPosition = playerRef.transform.position;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	

    void Idle()
    {

    }

    void Attack()
    {
        if(Vector2.Distance(playerRef.transform.position, this.transform.position) < aiMovementTriggerDistance )
        {

            if (Vector2.Distance(playerRef.transform.position, this.transform.position) > aiSafeDistance)
            {
                moveStep = aiMoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, playerRef.transform.position, moveStep);
            }
            else
            {

                if (prevPlayerPosition != playerRef.transform.position)
                {
                    //move away from player to maintain the safe dist
                    moveStep = aiMoveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + aiSafeDistance/2, transform.position.y + aiSafeDistance/2, 0), moveStep);

                    prevPlayerPosition = playerRef.transform.position;
                }

            }

        }

        //retreat
        if (Vector2.Distance(playerRef.transform.position, this.transform.position) > aiMovementTriggerDistance ) 
        {
            moveStep = aiMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveStep);
        }
    }

	// Update is called once per frame
	void Update ()
    {
	

        switch(aiStates)
        {
            case AIStates.idle:
                Idle();
                break;

            case AIStates.attack:
                Attack();
                break;
        }
	}
}
