using UnityEngine;
using System.Collections;

public class BoostCollision : MonoBehaviour {
	public PlayerControl playerControlRef;
	const float FORCE = 10000f;
	const float xAxis = -0.9f;
	const float yAxis = 0.0f;

	// Use this for initialization
	void Start () {
		}
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		try{
			Vector2 boostDirection = new Vector2 ();
			boostDirection.x = xAxis;
			boostDirection.y = yAxis;
			GameObject.Find ("Player").transform.rigidbody2D.AddForce(boostDirection * FORCE);
			}
		catch(UnityException ex)
			{
			Debug.Log ("Error: " + ex);
			}
	}

}
