using UnityEngine;
using System.Collections;

public class StickyCollision : MonoBehaviour 
{	
	private PlayerControl playerControlRef;
	private float nextLaunch = 0.0f;
	private bool canLaunch = true;

	void Start() 
    {	
        playerControlRef = Util.SafePlayerControlFind();
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
						temp.x += this.GetComponent<Renderer>().bounds.size.x;
					}
					else
					{
						temp.x -= this.GetComponent<Renderer>().bounds.size.x;
					}
				}			
				else if( coll.transform.position.y > this.transform.position.y )
				{
					temp.y += this.GetComponent<Renderer>().bounds.size.y;
				}
				else
				{
					temp.y -= this.GetComponent<Renderer>().bounds.size.y;
				}			
				
				coll.transform.position = temp ;
				coll.transform.GetComponent<Rigidbody2D>().gravityScale = 0;
				coll.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
