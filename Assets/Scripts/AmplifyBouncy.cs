using UnityEngine;
using System.Collections;

public class AmplifyBouncy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		float bounceForce = 5.5f;
		if ( coll.gameObject.CompareTag( "Player" ) )
		{			
			PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();
			
			Debug.Log( "Player: " + pControl.transform.position.y + " " + " Mushroom: " + this.transform.position.y );
			if ( pControl.transform.position.y > this.transform.position.y )
			{
				if ( pControl.Animation_GetFacingRight() )
				{
					pControl.transform.rigidbody2D.AddForce( new Vector2(200, 500) * bounceForce );
				}
				else
				{
					pControl.transform.rigidbody2D.AddForce( new Vector2(-200, 500) * bounceForce );
				}
			}
		}
	}
}
