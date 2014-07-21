using UnityEngine;
using System.Collections;

public class AngledBouncy : MonoBehaviour {
	
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
			/*
			if ( pControl.Animation_GetFacingRight() )
			{
				pControl.transform.rigidbody2D.velocity = Vector2.zero;
				pControl.transform.rigidbody2D.AddForce( new Vector2(-500, 500) * bounceForce );
			}
			else
			{
				pControl.transform.rigidbody2D.velocity = Vector2.zero;
				pControl.transform.rigidbody2D.AddForce( new Vector2(500, 500) * bounceForce );
			}
			*/
			Debug.Log ( "z: " + this.transform.eulerAngles.z );
			switch ( (int)this.transform.eulerAngles.z )
			{
				case 0: 	pControl.transform.rigidbody2D.velocity = Vector2.zero;
							pControl.transform.rigidbody2D.AddForce( new Vector2(200, 200) * bounceForce );
							break;				
				case 90: 	pControl.transform.rigidbody2D.velocity = Vector2.zero;
							pControl.transform.rigidbody2D.AddForce( new Vector2(-300, -500) * bounceForce );
							break;
				case 135: 	pControl.transform.rigidbody2D.velocity = Vector2.zero;
							pControl.transform.rigidbody2D.AddForce( new Vector2(-500, -500) * bounceForce );
							break;
				case 270: 	pControl.transform.rigidbody2D.velocity = Vector2.zero;
							pControl.transform.rigidbody2D.AddForce( new Vector2(300, -700) * bounceForce );
							break;		
			}
			
		}
	}
}
