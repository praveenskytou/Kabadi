using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {


    public float moveSpeed;
	public GameObject centerEnemy, leftEnemy, rightEnemy;
    private float currentMovementValueX,currentMovementValueY;
	public float  leftDistance, centerDistance, rightDistance;
	public float enemyStartPosZ, rideDist, enemyMoveSpeed;

	// Use this for initialization
	void Start () 
	{
	
	}
	

	void Update () 
	{
        currentMovementValueX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		currentMovementValueY = CrossPlatformInputManager.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
        this.transform.Translate(currentMovementValueX, 0, currentMovementValueY);
		CalculateDistance ();
		AdvaceCover ();
    }

	void CalculateDistance()
	{
		centerDistance = Vector3.Distance (transform.position, centerEnemy.transform.position);
		leftDistance = Vector3.Distance (transform.position, leftEnemy.transform.position);
		rightDistance = Vector3.Distance (transform.position, rightEnemy.transform.position);
	}

	void AdvaceCover()
	{
		if ((leftDistance <rideDist) && (rightEnemy.transform.position.z >rideDist))
		{
			rightEnemy.transform.Translate (0, 0, -enemyMoveSpeed);
		} 

		else if((leftDistance > rideDist) && (rightEnemy.transform.position.z < enemyStartPosZ )) 
		{
			rightEnemy.transform.Translate (0, 0,enemyMoveSpeed);
		} 

		if ((rightDistance < rideDist) && (leftEnemy.transform.position.z >rideDist))
		{
			leftEnemy.transform.Translate (0, 0, -enemyMoveSpeed);
		} 

		else if((rightDistance > rideDist) && (leftEnemy.transform.position.z < enemyStartPosZ )) 
		{
			leftEnemy.transform.Translate (0, 0,enemyMoveSpeed);
		} 
	}
}

