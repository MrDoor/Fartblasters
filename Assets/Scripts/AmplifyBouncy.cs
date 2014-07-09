using UnityEngine;
using System.Collections;

public class AmplifyBouncy : MonoBehaviour 
{
	public static float bounceForce;

    private static readonly int maxBounceCount      = 4;
    private static readonly float startForce        = 7f;
    private static readonly float forceIncAmount    = 0.3f; 

	void Start () 
    {
        bounceForce = startForce;
	}
		
	void OnCollisionEnter2D(Collision2D coll)
	{
		if( coll.gameObject.CompareTag( "Player" ) )
		{			
			PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();

            ContactPoint2D hit = coll.contacts[0];     // the first contact point
            Vector3 hitNormal = new Vector3( hit.normal.x, hit.normal.y, 0f );

            Vector3 velocity = Vector3.Reflect( -coll.relativeVelocity, -hitNormal );

            pControl.transform.rigidbody2D.velocity = Vector2.zero;
            pControl.transform.rigidbody2D.AddForce( new Vector2( velocity.x, velocity.y ) * bounceForce * 100 );       

            if( pControl.amplifyBounceCount < maxBounceCount )
            {
                if( pControl.amplifyBounceCount == 0 )
                {
                    bounceForce = startForce;
                }
                else
                {
                    bounceForce += forceIncAmount;
                }
            }

            pControl.amplifyBounceCount++;
		}
	}
}
