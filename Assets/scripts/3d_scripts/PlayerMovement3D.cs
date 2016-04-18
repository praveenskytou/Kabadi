using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement3D: MonoBehaviour
{
	public static PlayerMovement3D instance = null;
	public float moveSpeed;
	private float currentMovementValueX,currentMovementValueZ;
    private Animator animator;

    //game vars
    public bool hasTouchedAnyone;

	void Awake()
	{
		instance = this;
	}
		
	void Start ()
	{
        animator = this.GetComponent<Animator>();
    }

	void Update () 
	{
        BoundsCheck();
        MovePlayer ();
	}

	void MovePlayer()
	{
		currentMovementValueX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		currentMovementValueZ = CrossPlatformInputManager.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
		this.transform.Translate(currentMovementValueX, 0 , currentMovementValueZ,0);

        if (currentMovementValueX != 0 || currentMovementValueZ != 0)
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
        if (transform.position.z > GameManager.field_EndLineBack_Limit)
            transform.position = new Vector3(transform.position.x,0, GameManager.field_EndLineBack_Limit);


        if (hasTouchedAnyone)
        {
            //bound check upto Left and right Endlines
            if (transform.position.x > GameManager.field_EndLineRight_Limit)
                transform.position = new Vector3(GameManager.field_EndLineRight_Limit,0, transform.position.z);

            if (transform.position.x < GameManager.field_EndLineLeft_Limit)
                transform.position = new Vector3(GameManager.field_EndLineLeft_Limit, 0, transform.position.z);

        }
        else
        {
            //bound check within lobby
            if (transform.position.x > GameManager.field_LobbyRight_Limit)
                transform.position = new Vector3(GameManager.field_LobbyRight_Limit,0, transform.position.z);

            if (transform.position.x < GameManager.field_LobbyLeft_Limit)
                transform.position = new Vector3(GameManager.field_LobbyLeft_Limit,0, transform.position.z);
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "BaulkLine")
        {
            Debug.Log("BaulkLine has been touched");
        }

        //Debug.Log("Other collider : " + other.gameObject.name);
    }

}
