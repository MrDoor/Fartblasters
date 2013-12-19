using UnityEngine;
using System.Collections;

public class TeleportCollision : MonoBehaviour {
	
	public PlayerControl playerControlRef;
	public static float nextTeleport;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		//This only works if there is one of each block in the level... got it to work but will need to be modified to handle more than 2 teleport objects
		
		if(nextTeleport <= Time.time)
		{	
			Vector2 tempVelocity = playerControlRef.transform.rigidbody2D.velocity;
			Vector3 tempPosition;
			try
			{
				if(this.gameObject.name.EndsWith("1"))
				{
					tempPosition = GameObject.Find("TeleportBlock2").transform.position;
					if(coll.transform.position.x > this.transform.position.x)
					{
						tempPosition.x -= this.renderer.bounds.size.x;
					}
					else
					{
						tempPosition.x += this.renderer.bounds.size.x;
					}
					playerControlRef.transform.position = tempPosition;
					playerControlRef.transform.rigidbody2D.AddForce(tempVelocity);
					nextTeleport = Time.time + 2;
					Debug.Log ("Teleport! " + nextTeleport);			
				}
				else
				{
					tempPosition = GameObject.Find("TeleportBlock1").transform.position;
					if(coll.transform.position.x > this.transform.position.x)
					{
						tempPosition.x -= this.renderer.bounds.size.x;
					}
					else
					{
						tempPosition.x += this.renderer.bounds.size.x;
					}
					playerControlRef.transform.position = tempPosition;
					playerControlRef.transform.rigidbody2D.AddForce(tempVelocity);
					nextTeleport = Time.time + 2;
					Debug.Log ("Teleport! " + nextTeleport);
				}
			}
			catch(UnityException ex)
			{
				Debug.Log ("Error: " + ex.Message);
			}	
		}
	}	
}
