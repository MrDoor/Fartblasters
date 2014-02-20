using UnityEngine;
using System.Collections;

public class WinMenuControl : MonoBehaviour {
	
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
		
		GUI.Label (new Rect(750, 250, 300,50), "You Beat the Level!");
		GUI.Box (new Rect (700, 300, 300, 200),"");
		
		
		if (GUI.Button (new Rect (720, 320, 100, 50), "Next Level")) {
			Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
			if(PlayerPrefs.HasKey ("loaded level"))
			{
				Application.LoadLevel (PlayerPrefs.GetInt ("loaded level") + 1);
			}
		}
		
		if(GUI.Button (new Rect(720, 380, 100,50), "Replay?"))
		{
			Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
			if(PlayerPrefs.HasKey ("loaded level"))
			{
				Application.LoadLevel (PlayerPrefs.GetInt ("loaded level"));
			}
		}
		
		if(GUI.Button (new Rect(720, 440, 100,50), "Quit Game"))
		{
			Application.Quit();
		}
		
		
	}
}
