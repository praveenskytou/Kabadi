﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement instance = null;
	public float moveSpeed;
	private float currentMovementValueX,currentMovementValueY;

	void Awake()
	{
		instance = this;
	}
		
	void Start ()
	{
	
	}

	void Update () 
	{
		MovePlayer ();
	}

	void MovePlayer()
	{
		currentMovementValueX = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		currentMovementValueY = CrossPlatformInputManager.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
		this.transform.Translate(currentMovementValueX, currentMovementValueY,0);
	}
}