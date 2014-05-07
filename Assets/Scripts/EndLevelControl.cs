using UnityEngine;
using System.Collections;

public class EndLevelControl : MonoBehaviour {

	public bool win = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		Debug.Log ("End Block Collison: " + coll.gameObject.tag);
		if (coll.tag == "Player") 
		{
			//PlayerPrefs.SetInt ("loaded level", Application.loadedLevel);
			//Application.LoadLevel ("test_winMenu_Nick");
			win = true;
			Time.timeScale = 0;
			PlayerPrefs.SetInt ("loaded level", Application.loadedLevel);
				}
		
   }

	void OnGUI () {
		if (win) {

			GUIStyle textStyle = new GUIStyle();
			textStyle.fontSize = 35;
			textStyle.normal.textColor = Color.yellow;
						
			GUI.Label (new Rect (750, 150, 300, 50), "You Beat the Level!\n\tCongratulations!", textStyle);
			GUI.backgroundColor = Color.black;
			GUI.Box (new Rect (750, 250, 300, 210), "");

			//GUI.contentColor = Color.black;
		
		
						if (GUI.Button (new Rect (840, 260, 100, 50), "Next Level")) {
								Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
								if (PlayerPrefs.HasKey ("loaded level")) {
					                    Time.timeScale = 1;
										Application.LoadLevel (PlayerPrefs.GetInt ("loaded level") + 1);
								}
						}
		
						if (GUI.Button (new Rect (840, 330, 100, 50), "Replay?")) {
								Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
								if (PlayerPrefs.HasKey ("loaded level")) {
					                    Time.timeScale = 1;
										Application.LoadLevel (PlayerPrefs.GetInt ("loaded level"));
								}
						}
		
						if (GUI.Button (new Rect (840, 400, 100, 50), "Quit Game")) {
								Application.Quit ();
						}
		
				}
	}
}
