﻿using UnityEngine;
using System.Collections;

public class EndLevelControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		Debug.Log ("End Block Collison: " + coll.gameObject.tag);
		if (coll.tag == "Player") {
			PlayerPrefs.SetInt ("loaded level", Application.loadedLevel);
			Application.LoadLevel ("test_winMenu_Nick");
				} 
		/*else {
						Application.LoadLevel ("test_leve_zack");
				}*/

		}
}