using UnityEngine;
using System.Collections;

public class StopCollision : MonoBehaviour 
{
    private PlayerControl playerControlRef;

    void Start() 
    {   
        playerControlRef = Util.SafePlayerControlFind();
    }

	void OnCollisionEnter2D( Collision2D coll )
	{	
		try
		{
            if( !playerControlRef.GetIsStopped() )
            {
                playerControlRef.SetIsStopped( false );
                StopPlayer( coll );
            }
		}
		catch( UnityException ex )
		{
			Debug.Log( "Error: " + ex.Message );
		}
	}

    public static void StopPlayer( Collision2D coll )
    {
        // Don't set it to zero, want it to keep moving just a little
        float newVelocity = 0.005f;
        coll.transform.rigidbody2D.velocity = new Vector2( newVelocity, newVelocity );
        coll.transform.rigidbody2D.angularVelocity = newVelocity;
    }
}
