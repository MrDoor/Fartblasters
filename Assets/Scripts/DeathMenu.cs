using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI()
	{
		GUI.Label (new Rect(750, 250, 300,50), "GAME OVER");
		GUI.Box (new Rect (700, 300, 300, 200),"");
		
		
		if (GUI.Button (new Rect (720, 320, 100, 50), "Restart Level")) {
			Debug.Log ("Load Level: " + Application.loadedLevelName);
			Application.LoadLevel("test_level_Nick");
		}
		
		if(GUI.Button (new Rect(720, 380, 100,50), "Return to Main menu"))
		{
			Application.LoadLevel("test_menu_Nick");
		}
		
		if(GUI.Button (new Rect(720, 440, 100,50), "Quit Game"))
		{
			Application.Quit();
		}

		}
}
