using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement2D : MonoBehaviour
{
	public static PlayerMovement2D instance = null;
	public float moveSpeed;
	private float currentMovementValueX,currentMovementValueY;
    private Animator animator;
    private Vector3 initialPosition;

    //game vars
    public bool hasTouchedAnyone;

	void Awake()
	{
		instance = this;
	}
		
	void Start ()
	{
        animator = this.GetComponent<Animator>();
        initialPosition = this.transform.position;
    }

    void Update () 
	{
        BoundsCheck();

        if(GameManager.instRef.isRaidOver)
        {
            transform.position = initialPosition;
            animator.SetBool("isMoving", false);
        }
        else
        {
            MovePlayer();
        }
    }

	void MovePlayer()
	{
		currentMovementValueX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		currentMovementValueY = CrossPlatformInputManager.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
		this.transform.Translate(currentMovementValueX, currentMovementValueY,0);

        if (currentMovementValueX != 0 || currentMovementValueY != 0)
        {
            //animator.play
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void BoundsCheck()
    {
        
        //Endline_back check
        if (transform.position.y > GameManager.field_EndLineBack_Limit)
            transform.position = new Vector2(transform.position.x, GameManager.field_EndLineBack_Limit);

        //Midline check
        if (transform.position.y < GameManager.field_MidLine_Limit)
            transform.position = new Vector2(transform.position.x, GameManager.field_MidLine_Limit);

        if (hasTouchedAnyone)
        {
            //bound check upto Left and right Endlines
            if (transform.position.x > GameManager.field_EndLineRight_Limit)
                transform.position = new Vector2(GameManager.field_EndLineRight_Limit, transform.position.y);

            if (transform.position.x < GameManager.field_EndLineLeft_Limit)
                transform.position = new Vector2(GameManager.field_EndLineLeft_Limit, transform.position.y);

        }
        else
        {
            //bound check within lobby
            if (transform.position.x > GameManager.field_LobbyRight_Limit)
                transform.position = new Vector2(GameManager.field_LobbyRight_Limit, transform.position.y);

            if (transform.position.x < GameManager.field_LobbyLeft_Limit)
                transform.position = new Vector2(GameManager.field_LobbyLeft_Limit, transform.position.y);
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "BaulkLine")
        {
            Debug.Log("BaulkLine has been touched");
        }

        //Debug.Log("Player touched : " + other.gameObject.name);
        if (other.gameObject.name == "Enemy_2D")
        {
            if (other.gameObject.GetComponent<AIBehaviour2D>().currentAIState == AIStates2D.defense )
            {
                other.gameObject.GetComponent<AIBehaviour2D>().hasTouchedByPlayer = true;
                other.gameObject.GetComponent<AIBehaviour2D>().hasTouchedByPlayer
            }
        }

    }

}
