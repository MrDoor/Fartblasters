using UnityEngine;
using System.Collections;

public class StickyCollision : MonoBehaviour 
{	
	private PlayerControl playerControlRef;
	private float nextLaunch = 0.0f;
	private bool canLaunch = true;

	void Start() 
    {	
        playerControlRef = GameObject.Find("Player").GetComponent<PlayerControl>() as PlayerControl;
	}
	
	void Update() 
    {
		if( nextLaunch <= Time.time )
		{
			canLaunch = true;
		}
	}
	
	void OnCollisionEnter2D( Collision2D coll )
	{		
		try
		{
			if( canLaunch )
			{
				Vector3 temp = this.transform.position;
				Debug.Log (this.transform.rotation.z);
				if( this.transform.rotation.z > 0 )
				{
					if( coll.transform.position.x > this.transform.position.x )
					{
						temp.x += this.renderer.bounds.size.x;
					}
					else
					{
						temp.x -= this.renderer.bounds.size.x;
					}
				}			
				else if( coll.transform.position.y > this.transform.position.y )
				{
					temp.y += this.renderer.bounds.size.y;
				}
				else
				{
					temp.y -= this.renderer.bounds.size.y;
				}			
				
				coll.transform.position = temp ;
				coll.transform.rigidbody2D.gravityScale = 0;
				coll.transform.rigidbody2D.velocity = Vector2.zero;
				playerControlRef.SetIsStuck(true);	
				nextLaunch = Time.time + 3.0f;	
				canLaunch = false;
			}
		}
		catch( UnityException ex )
		{
			Debug.Log("Error: " + ex.Message);
		}
	}
	
	
}
