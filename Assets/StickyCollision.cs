using UnityEngine;
using System.Collections;

public class StickyCollision : MonoBehaviour {
	
	public PlayerControl playerControlRef;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{		
		try
		{
			Vector3 temp = this.transform.position;
			if(coll.transform.position.y > this.transform.position.y)
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
			playerControlRef.setIsStuck(true);		
		}
		catch(UnityException ex)
		{
			Debug.Log("Error: " + ex.Message);
		}
	}
	
	
}
