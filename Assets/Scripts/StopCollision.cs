using UnityEngine;
using System.Collections;

public class StopCollision : MonoBehaviour 
{
	void OnCollisionEnter2D( Collision2D coll )
	{		
		try
		{
			coll.transform.rigidbody2D.velocity = new Vector2( 0.005f, 0.005f );
			coll.transform.rigidbody2D.angularVelocity = 0.005f;
		}
		catch(UnityException ex)
		{
			Debug.Log("Error: " + ex.Message);
		}
	}
}
