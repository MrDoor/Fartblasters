using UnityEngine;
using System.Collections;

public class FixedBounce : MonoBehaviour {

	public float bounceForce;
	public float xForce;
	public float yForce;
	
	// Use this for initialization
	void Start () {
		if( bounceForce <= 0 )
		{
			bounceForce = 8f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if ( coll.gameObject.CompareTag( "Player" ) )
		{			
			PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();	
			pControl.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			
			if ( this.transform.position.y < pControl.transform.position.y )
			{
				if ( pControl.playerAnimation.isFacingRight )
				{
					pControl.transform.GetComponent<Rigidbody2D>().AddForce( new Vector2(xForce, yForce) * bounceForce );
				}
				else
				{				
					pControl.transform.GetComponent<Rigidbody2D>().AddForce( new Vector2(-xForce, yForce) * bounceForce );
				}
			}
		}
	}
}
