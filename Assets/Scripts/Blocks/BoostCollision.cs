﻿using UnityEngine;
using System.Collections;

public class BoostCollision : MonoBehaviour 
{
	const float FORCE = 10000f;
	public float xAxis = -0.9f;
	public float yAxis = 0.0f;

	void OnTriggerEnter2D( Collider2D coll )
	{
		try
		{
			if( coll.tag == "Player" )
			{
				audio.PlayOneShot( audio.clip );

				Vector2 boostDirection = new Vector2( xAxis, yAxis );
				coll.gameObject.rigidbody2D.AddForce(boostDirection * FORCE);
			}
		}
		catch( UnityException ex )
		{
			Debug.LogError("Error: " + ex);
		}
	}

}
