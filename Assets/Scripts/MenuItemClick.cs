﻿using UnityEngine;
using System.Collections;

public class MenuItemClick : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*void OnMouseDown()
	{
		Application.LoadLevel (2);
		Debug.Log ("Click");
	}*/
	void OnGUI () {
				

		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 35;
		textStyle.fontStyle = FontStyle.BoldAndItalic;
		textStyle.normal.textColor = Color.green;

		//GUI.Label (new Rect(650, 150, 300,50), "FART BLASTERS", textStyle );
		//GUI.Box (new Rect (660, 200, 270, 200),"");
		
				
		//if (GUI.Button (new Rect (740, 220, 100, 50), "New Game")) 
		
		float top = Screen.height / 2 + 140;
		//GUI.Label (new Rect(750, 250, 300,50), "Welcome to: \nFART BLASTERS");
		//GUI.Box (new Rect (700, 300, 300, 200),"");
		GUI.Box (new Rect (700, top-40, 250, 200),"");
		
				
		//if (GUI.Button (new Rect (720, 320, 100, 50), "New Game")) {
		if (GUI.Button (new Rect (765, top-20, 100, 50), "New Game")) {

			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel("World_1_X-1");
			}
				

		//if(GUI.Button (new Rect(740, 280, 100,50), "Continue"))

		//if(GUI.Button (new Rect(720, 380, 100,50), "Continue"))
		if(GUI.Button (new Rect(765, top + 40, 100,50), "Continue"))

			{
				Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
				if(DBFunctions.getLevelProgress() != -1)
					Application.LoadLevel(DBFunctions.getLevelProgress());
				else {
					Debug.Log ("Error retreiving level from the database");
					Application.LoadLevel (0);
				}
			}


		//if(GUI.Button (new Rect(740, 340, 100,50), "Quit Game"))

		//if(GUI.Button (new Rect(720, 440, 100,50), "Quit Game"))
		if(GUI.Button (new Rect(765, top + 100, 100,50), "Quit Game"))

			{
				Application.Quit();
			}


	}
}
