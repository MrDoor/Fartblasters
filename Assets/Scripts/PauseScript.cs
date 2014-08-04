using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	public Texture2D btn_pause;
	// Use this for initialization
	private float boxSize = 100f;
	private Rect backgroundRect;
	private bool paused = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseUp()
	{
		Pause ();
	}
	

	public void Pause()
	{
		if (paused) 
		{
			paused = false;
			Time.timeScale = 1;
		}
		else
		{
			paused = true;
			Time.timeScale = 0;
		}
	}

	void OnGUI()
	{
		float left		= 20;
		float top		= (Screen.height / 2f) + (Screen.height / 2.5f);
		float width		= boxSize;
		float height	= boxSize / 2;
		
		//backgroundRect = new Rect(left, top, width, height);
		backgroundRect = new Rect(50f, top, 100f, 40f);
		//GUI.DrawTexture(backgroundRect, btn_pause);
		GUI.Box ( backgroundRect, "" );	
		if( GUI.Button ( backgroundRect, "Pause" ) )
		{
			Pause();
		}		
	
				if (paused) {
						/*GUIStyle textStyle = new GUIStyle();
			textStyle.normal.textColor = Color.red;
			textStyle.fontSize = 80;
			textStyle.fontStyle = FontStyle.Bold;
			
			GUI.Label(new Rect(700,200 ,Screen.width,Screen.height),"Game Over", textStyle);
			*/
			
						//GUI.Label (new Rect (750, 250, 300, 50), "GAME OVER");
						GUI.Box (new Rect (810, 300, 200, 200), "");
			
			
						if (GUI.Button (new Rect (860, 315, 100, 50), "Restart Level")) {
								Debug.Log ("Load Level: " + Application.loadedLevelName);
				
								//if (GUI.Button (new Rect (720, 320, 100, 50), "Restart Level")) {
								//Debug.Log ("Load Level: " + Application.loadedLevelName);
				
								Application.LoadLevel (Application.loadedLevel);
								Time.timeScale = 1;
						}
			
						if (GUI.Button (new Rect (860, 375, 100, 50), "Return to \nMain Menu")) {
								//Debug.Log (Util.getlevel);
								Application.LoadLevel ("test_menu_Nick");
								Time.timeScale = 1;
						}
			
						if (GUI.Button (new Rect (860, 435, 100, 50), "Quit Game")) {
								Application.Quit ();
						}
			
						//GUI.Label (new Rect(850, 500, Screen.width, Screen.height), "You have died " + DBFunctions.getTimesDied () + " Times.");
				}
		}
}
