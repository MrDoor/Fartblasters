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
			PlayerControl.levelTime.Stop ();
			win = true;
			Time.timeScale = 0;
			PlayerPrefs.SetInt ("loaded level", Application.loadedLevel);
			DBFunctions.updateLevelProgress (Application.loadedLevel +1);
			}
		
   }

	void OnGUI () {
		if (win) 
		{

			GUIStyle textStyle = new GUIStyle();
			textStyle.fontSize = 35;
			textStyle.normal.textColor = Color.yellow;
						
			GUI.Label (new Rect (600, 100, 300, 50), "You Beat the Level!\n\tCongratulations!", textStyle);
			GUI.backgroundColor = Color.black;
			GUI.Box (new Rect (600, 190, 300, 300), "");

			//GUI.contentColor = Color.black;
		
		
			if (GUI.Button (new Rect (690, 210, 120, 50), "Next Level")) 
			{
				Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
				if (PlayerPrefs.HasKey ("loaded level"))
				{
					Time.timeScale = 1;
					Application.LoadLevel (PlayerPrefs.GetInt ("loaded level") + 1);
				}
			}
		
			if (GUI.Button (new Rect (690, 280, 120, 50), "Replay?")) 
			{
				Debug.Log ("Next Level: " + PlayerPrefs.GetInt ("loaded level"));
				if (PlayerPrefs.HasKey ("loaded level")) 
				{
					Time.timeScale = 1;
					Application.LoadLevel (PlayerPrefs.GetInt ("loaded level"));
				}
			}
						
			if(GUI.Button (new Rect(690, 350, 120, 50), "Return to \nMain Menu"))
			{
				Time.timeScale = 1;
				Application.LoadLevel (0);
			}

			               
			if (GUI.Button (new Rect (690, 420, 120, 50), "Quit Game")) 
			{
				Application.Quit ();
			}
			//----------------Level Stats----------------//
			GUI.Label (new Rect (600, 500, 300, 50), "You got " + PlayerControl.puCount + " PickUps");

			int[] tm = PlayerControl.getPlayTime ();

			GUI.Label (new Rect(600, 515, 300,50), "Level Time: " + tm[0] + " Min " + tm[1] + " Sec");
		
		}
	}
}
