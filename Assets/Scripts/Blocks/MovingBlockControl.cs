using UnityEngine;
using System.Collections;

public class MovingBlockControl : MonoBehaviour 
{
	public float rightLeftSize = 1.0f;
	public float upDownSize = 2.0f; 
	public bool isLeftRight = false;
	
	private Transform transform_;
	private float positionDifference = 0.0f;	
	private Vector3 currentPosition;
	
	//private Direction lastDirection = Direction.NONE;
	private Direction moveTo = Direction.LEFT;
	
	void Start () 
    {
		transform_ = this.transform;
		currentPosition = transform_.position;
		if( isLeftRight )
		{	
			MoveLeft();	
		}
		else
		{
			MoveUp();
		}
	}
	
	// Update is called once per frame
	void Update() 
    { 
		if (Time.timeScale != 0) {
						if (isLeftRight) {
								positionDifference = currentPosition.x - transform_.position.x;	
								if (Mathf.Abs (positionDifference) >= rightLeftSize) {
										CheckDirection ();
								}	
						} else {
								positionDifference = currentPosition.y - transform_.position.y;
								if (Mathf.Abs (positionDifference) >= upDownSize) {
										CheckDirection ();
								}
						}
						Move ();
				}
	}	
	
	void Move()
	{
		if( isLeftRight )
		{
			if( moveTo == Direction.LEFT )
			{
				MoveLeft();
			}
			else
			{
				MoveRight();
			}
		}
		else
		{
			if( moveTo == Direction.UP )
			{
				MoveDown();
			}
			else
			{
				MoveUp();
			}
		}		
	}
	
	void MoveRight()
	{
		Vector3 temp = transform_.position;
		temp.x += .01f;
		transform_.position = temp;
	}
	
	void MoveLeft()
	{
		Vector3 temp = transform_.position;
		temp.x -= .01f;
		transform_.position = temp;
	}
	
	void MoveUp()
	{
		Vector3 temp = transform_.position;
		temp.y += .01f;
		transform_.position = temp;
	}
	
	void MoveDown()
	{
		Vector3 temp = transform_.position;
		temp.y -= .01f;
		transform_.position = temp;	
	}
	
	void CheckDirection()
	{
		if( isLeftRight )
		{
			if( moveTo == Direction.LEFT )
			{
				moveTo = Direction.RIGHT;
			}
			else
			{
				moveTo = Direction.LEFT;
			}
		}
		else
		{
			if( moveTo == Direction.UP )
			{
				moveTo = Direction.DOWN;
			}
			else
			{
				moveTo = Direction.UP;
			}
		}
	}
	
	void OnCollisionEnter2D( Collision2D coll )
	{
		if( coll.gameObject.tag.Equals( "Player" ) )
		{
			PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();
			if( pControl )
			{				
				//Debug.Log(coll.transform.parent == null ? "No Parent to start with." : "Has a parent: " + coll.transform.parent.name);
				if( pControl.transform.parent == null )
				{
					pControl.transform.parent = transform_;
				}
			}
		}
	}
}
