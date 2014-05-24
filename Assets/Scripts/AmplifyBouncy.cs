using UnityEngine;
using System.Collections;

public class AmplifyBouncy : MonoBehaviour {

	public float bounceForce;
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
			
			if ( pControl.transform.position.y > this.transform.position.y )
			{
				if ( pControl.Animation_GetFacingRight() )
				{
					pControl.transform.rigidbody2D.velocity = Vector2.zero;
					pControl.transform.rigidbody2D.AddForce( new Vector2(200, 500) * bounceForce );
				}
				else
				{
					pControl.transform.rigidbody2D.velocity = Vector2.zero;
					pControl.transform.rigidbody2D.AddForce( new Vector2(-200, 500) * bounceForce );
				}
			}
		}
	}
}
