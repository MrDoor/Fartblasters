using UnityEngine;
using System.Collections;

public class LackieSoldier : MonoBehaviour {

	private Transform groundCheck;
	private bool onGround = false;	
	private bool isMoving	= false;
	private Direction lastDirection = Direction.NONE;
	private Direction moveTo = Direction.LEFT;
	
	// These are hard coded to the values of the layers in the editor
	// Have to update by hand if they change
	const int BLOCKLAYER_DEFAULT	= 1 << 11;
	const int BLOCKLAYER_SLIPPERY	= 1 << 12;
	const int BLOCKLAYER_BOUNCY		= 1 << 13;
	const int BLOCKLAYER_STICKY		= 1 << 14;
	const int BLOCKLAYER_TELEPORT1	= 1 << 15;
	const int BLOCKLAYER_TELEPORT2	= 1 << 16;
	const int BLOCKLAYER_STOP		= 1 << 17;
	const int BLOCKLAYER_MAGNET		= 1 << 18;
	const int BLOCKLAYER_VORTEX		= 1 << 19;
	
	private enum Direction
	{
		LEFT,
		NONE,
		RIGHT
	};
	
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
		int layerMask = BLOCKLAYER_DEFAULT | BLOCKLAYER_SLIPPERY | BLOCKLAYER_STICKY | BLOCKLAYER_TELEPORT1 | BLOCKLAYER_TELEPORT2 | BLOCKLAYER_STOP | BLOCKLAYER_MAGNET | BLOCKLAYER_VORTEX;
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
	/*
		isMoving = ( transform.rigidbody2D.velocity.sqrMagnitude >= 0.01f || transform.rigidbody2D.angularVelocity >= 0.01f );
		
		if( isMoving )
		{
			if( lastXPos < transform.position.x )
			{
				movingDir = Direction.RIGHT;
			}
			else if( lastXPos > transform.position.x )
			{
				movingDir = Direction.LEFT;
			}
			else
			{
				movingDir = Direction.NONE;
			}
			
		}
		*/
		
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
		temp.x += .005f;
		this.transform.position = temp;
	}
	
	void MoveLeft()
	{
		Vector3 temp = this.transform.position;
		temp.x -= .005f;
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
		playerAnimator.Play("lackie walk");
	}
}
