using UnityEngine;
using System.Collections;

public class StopCollision : MonoBehaviour 
{
	void OnCollisionEnter2D( Collision2D coll )
	{	
		// Don't set it to zero, want it to keep moving just a little
		float newVelocity = 0.005f;
		try
		{
			coll.transform.rigidbody2D.velocity = new Vector2( newVelocity, newVelocity );
			coll.transform.rigidbody2D.angularVelocity = newVelocity;
		}
		catch(UnityException ex)
		{
			Debug.Log("Error: " + ex.Message);
		}
	}
}
