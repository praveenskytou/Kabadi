using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement instance = null;
	public float moveSpeed;
	private float currentMovementValueX,currentMovementValueY;

    //game vars
    public bool hasTouchedAnyone;

	void Awake()
	{
		instance = this;
	}
		
	void Start ()
	{
	
	}

	void Update () 
	{
        BoundsCheck();
        MovePlayer ();
	}

	void MovePlayer()
	{
		currentMovementValueX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		currentMovementValueY = CrossPlatformInputManager.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
		this.transform.Translate(currentMovementValueX, currentMovementValueY,0);
	}

    void BoundsCheck()
    {
        
        //Endline_back check
        if (transform.position.y > GameManager.field_EndLineBack_Limit)
            transform.position = new Vector2(transform.position.x, GameManager.field_EndLineBack_Limit);


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
        if(other.gameObject.name == "BaulkLine")
        {
            Debug.Log("BaulkLine has been touched");
        }

        //Debug.Log("Other collider : " + other.gameObject.name);
    }

}
