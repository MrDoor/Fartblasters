using UnityEngine;
using System.Collections;

public class ClawController : MonoBehaviour {

	public GameObject leftClaw;
	public GameObject rightClaw;
	public float movementZone;
	public float movementSpeed;
	
	private float maxDistance;
	private float minDistance;
	// Use this for initialization
	void Start () {
		maxDistance = this.transform.position.x + ( movementZone / 2 );
		minDistance = this.transform.position.x - ( movementZone / 2 );		
	}
	
	// Update is called once per frame
	void Update () {	
		
		if(Input.GetKey("left"))
		{
			if(this.transform.position.x >= minDistance)
			{
				Vector2 newPosition = this.transform.position;
				newPosition.x = newPosition.x - movementSpeed / 100;
				
				this.transform.position = newPosition;
			}
		}
		else if(Input.GetKey("right"))
		{
			if(this.transform.position.x <= maxDistance)
			{
				Vector2 newPosition = this.transform.position;
				newPosition.x = newPosition.x + movementSpeed / 100;
				
				this.transform.position = newPosition;
			}
		}
		else if(Input.GetKey("down"))
		{
			Vector2 newPosition = this.transform.position;
			newPosition.y = newPosition.y - movementSpeed / 100;
			
			this.transform.position = newPosition;
		}
		else if(Input.GetKey("up"))
		{
			Vector2 newPosition = this.transform.position;
			newPosition.y = newPosition.y + movementSpeed / 100;
			
			this.transform.position = newPosition;
		}
		
		if(rightClaw && leftClaw)
		{
			JointMotor2D tempLeftJoint 	= leftClaw.GetComponent<HingeJoint2D>().motor;
			JointMotor2D tempRightJoint = rightClaw.GetComponent<HingeJoint2D>().motor;
			
			if(Input.GetKey("right shift"))
			{
				/*
				tempLeftJoint.motorSpeed = 100;
				leftClaw.GetComponent<HingeJoint2D>().motor = tempLeftJoint;
				
				tempRightJoint.motorSpeed = -100;
				rightClaw.GetComponent<HingeJoint2D>().motor = tempRightJoint;
				*/
				
				leftClaw.GetComponent<HingeJoint2D>().useMotor = true;
				rightClaw.GetComponent<HingeJoint2D>().useMotor = true;
			}
			else
			{
				/*
				tempLeftJoint.motorSpeed = -100;
				leftClaw.GetComponent<HingeJoint2D>().motor = tempLeftJoint;
				
				tempRightJoint.motorSpeed = 100;
				rightClaw.GetComponent<HingeJoint2D>().motor = tempRightJoint;
				*/
				leftClaw.GetComponent<HingeJoint2D>().useMotor = false;
				rightClaw.GetComponent<HingeJoint2D>().useMotor = false;
			}
		}
	}
}
