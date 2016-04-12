using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {


    public float moveSpeed=5;
    private float currentMovementValueX,currentMovementValueY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        currentMovementValueX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        currentMovementValueY = CrossPlatformInputManager.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

      //  Debug.Log("Move value : " + currentMovementValue);

        this.transform.Translate(currentMovementValueX, 0, currentMovementValueY);


    }
}
