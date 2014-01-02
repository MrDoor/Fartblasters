using UnityEngine;
using System.Collections;

public class FallingBlockCollision : MonoBehaviour {
	
	float fallTime = 0.0f;
	float nextFlash = 0.0f;
	float killTime = 0.0f;
	
	const float FLASH_TIME = 0.33f;
	bool flashFlag = false;
	bool canFall = false;	
	bool visibleFlag = false;
	
	// Use this for initialization
	void Start () {		
		//Debug.Log("Start:gravity = " + this.transform.rigidbody2D.gravityScale);
		//this.transform.rigidbody2D.gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(nextFlash > 0)
		{
			if(Time.time >= nextFlash)
			{
				if(flashFlag)
				{
					flashFlag = false;
				}
				else
				{
					flashFlag = true;
				}
				
				this.transform.renderer.enabled = flashFlag;
				nextFlash = Time.time + FLASH_TIME;
			}
		}
		
		if(canFall && Time.time >= fallTime)
		{		
			Debug.Log ("Begin Fall - fallTime: " + fallTime + " - Time.time: " + Time.time);
			//GameObject gObject = new GameObject();
			if(this.transform.rigidbody2D == null)
			{
				Rigidbody2D rigidBody = this.gameObject.AddComponent<Rigidbody2D>();
				this.transform.rigidbody2D.gravityScale = 1;
				this.transform.rigidbody2D.mass = 10;
				nextFlash = 0;
				this.transform.renderer.enabled = true;
				//canFall = false;				
			}
			
			if(killTime > 0)
			{
				if(visibleFlag)
				{
					visibleFlag = false;
				}
				else
				{
					visibleFlag = true;
				}
				this.transform.renderer.enabled = visibleFlag;
				
				if(Time.time >= killTime)
				{					
					Destroy(this.gameObject);
				}
			}
			//this.transform.rigidbody2D.isKinematic = false;
			//this.transform.rigidbody2D.gravityScale = 1;
			//this.transform.rigidbody2D.mass += 1;
		}		
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.name.Equals("Player"))
		{
			PlayerControl pControl = (PlayerControl)coll.gameObject.GetComponent<PlayerControl>();
			if(pControl)
			{
				if(fallTime == 0.0f)
				{
					fallTime = Time.time + 3.0f;
					nextFlash = Time.time + FLASH_TIME;
					//this.transform.rigidbody2D.isKinematic = true;
					canFall = true;
				}
			}
		}
		else
		{	
			if(killTime <= 0)
			{
				killTime = Time.time + 1f;
			}
		}	
	}		
	
}
