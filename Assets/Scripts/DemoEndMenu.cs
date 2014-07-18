using UnityEngine;
using System.Collections;

public class DemoEndMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI () {
		
		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 25;
		textStyle.fontStyle = FontStyle.BoldAndItalic;
		textStyle.normal.textColor = Color.blue;
		
		float top = Screen.height / 2 + 140;
		GUI.Label(new Rect(450, top-60, 500,50), "\t\t\t\t\tThank you for playing the FARTBLASTERS! demo.\nLook for the full version comming soon to an app store near you!", textStyle);
		GUI.Box (new Rect (650, top+20, 360, 200),"");
		
		if (GUI.Button (new Rect (670, top+40, 100, 50), "Level 1")) {
			
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel(3);
		}
		
		if(GUI.Button (new Rect(780, top + 40, 100,50), "Level 2"))
			
		{
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel (4);
		}
		
		if(GUI.Button (new Rect(890, top + 40, 100,50), "Level 3"))
			
		{
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel (5);
		}

		if(GUI.Button (new Rect(670, top + 100, 100,50), "Level 4"))
			
		{
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel (6);
		}

		if(GUI.Button (new Rect(780, top + 100, 100,50), "Level 5"))
			
		{
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel (7);
		}

		if(GUI.Button (new Rect(890, top + 100, 100,50), "Level 6"))
			
		{
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel (8);
		}

		if(GUI.Button (new Rect(780, top + 160, 100,50), "Return to \nMain Menu"))
			
		{
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel (0);
		}



	}
}
