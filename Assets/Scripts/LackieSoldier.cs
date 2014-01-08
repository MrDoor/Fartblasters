﻿using UnityEngine;
using System.Collections;

public class LackieSoldier : MonoBehaviour {

	private Transform groundCheck;
	private bool onGround = false;	
	//private bool isMoving	= false;
	private float detectPause = 0.0f;
	
	private Direction lastDirection = Direction.NONE;
	private Direction moveTo = Direction.LEFT;
	
	private Animator playerAnimator;
	private float lastXPos		= 0.0f;
	
	// Use this for initialization
	void Start () {
		groundCheck = transform.Find("Lackie_Soldier_groundCheck");
		Animation_Init();
		Move();
	}
	
	// Update is called once per frame
	void Update () {
        int layerMask = Constants.LayerMask_Ground;
		onGround = Physics2D.Linecast( transform.position, groundCheck.position, layerMask );
		
		if(!onGround)
		{
			if(lastDirection == Direction.LEFT)
			{
				moveTo = Direction.RIGHT;
			}
			else
			{
				moveTo = Direction.LEFT;
			}
		}
		Move ();
		
		Animation_Update(onGround);
	}
	
	void FixedUpdate()
	{		
		if( lastXPos < transform.position.x)
		{
			lastDirection = Direction.RIGHT;
		}
		else if(lastXPos > transform.position.x)
		{
			lastDirection = Direction.LEFT;
		}
		else
		{
			lastDirection = Direction.NONE;
		}		
		lastXPos = transform.position.x;
	}
	
	void Move()
	{
		if(moveTo == Direction.RIGHT)
		{
			MoveRight();
		}
		else
		{
			MoveLeft();
		}
	}
	
	void MoveRight()
	{
		Vector3 temp = this.transform.position;
		temp.x += .0075f;
		this.transform.position = temp;
	}
	
	void MoveLeft()
	{
		Vector3 temp = this.transform.position;
		temp.x -= .0075f;
		this.transform.position = temp;
	}
	
	//Animation
	public void Animation_Init()
	{
		playerAnimator	= this.transform.Find ("body").GetComponent( "Animator" ) as Animator;
		lastXPos		= transform.position.x;
	}
	
	public void Animation_Update( bool onGround )
	{
		if(moveTo == Direction.LEFT)
		{
			playerAnimator.Play("lackie walk");
		}
		else
		{
			//Not WORKING!!
			playerAnimator.Play("lackie walk_right");
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.name.Equals("Player"))
		{
			Debug.Log("Player!");
		}
		else
		{
			if(coll.gameObject.transform.position.y >= this.transform.position.y)
			{
				if(Physics2D.Linecast(transform.position, coll.gameObject.transform.position))
				{
					if(detectPause <= Time.time)
					{
						if(moveTo == Direction.LEFT)
						{
							MoveRight();
							moveTo = Direction.RIGHT;
						}
						else
						{
							MoveLeft ();
							moveTo = Direction.LEFT;
						}
						detectPause = Time.time + 2.0f;
					}
				}
				else
				{
					//Debug.Log ("No Hit: " + this.transform.position + " | " + coll.gameObject.transform.position);
				}
			}
		}
	}
}