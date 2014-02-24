using UnityEngine;
using System.Collections;

public class TrajectoryCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D( Collision2D coll )
	{
		if( !coll.gameObject.CompareTag( "Player" ) )
		{
			Debug.Log ( "Destroy: " + this.gameObject.name + " touched: " + coll.gameObject.tag );
			Destroy ( this.gameObject );
		}
	}
	/*
	void OnTriggerEnter2D( Collider2D coll )
	{
		if( !coll.gameObject.CompareTag( "Player" ) )
		{
			Debug.Log ( "Destroy: " + this.gameObject.name + " touched: " + coll.gameObject.tag );
			Destroy ( this.gameObject );
		}
	}	
	*/
}
