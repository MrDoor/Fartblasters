using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndLevelControl : MonoBehaviour {

	public bool win = false;

	//If this is a secret exit set to true and give the name of the bonus level to go to 
	//If this is the end block IN the secret level set to true and give the name of the next level to go to .
	public bool secretLevel = false;
	public string nextLevel = " ";


	void OnTriggerEnter2D( Collider2D coll )
	{
		Debug.Log ("End Block Collison: " + coll.gameObject.tag);
		if (coll.tag == "Player") 
		{
			win = true;
			int num = Application.loadedLevel;
			Time.timeScale = 0;
			PlayerPrefs.SetInt("loaded level", num);
			DBFunctions.updateLevelComplete(num);
			DBFunctions.updateLevelProgress(num + 1);
			DBFunctions.updateLastLevel(num);
            StatsManager.Instance.EndLevelUpdate(num);
		}
   }

	void OnGUI() 
    {
		if(win) 
		{
			if(!secretLevel)
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

                int[] tm = StatsManager.Instance.GetPlayTime();
				textStyle.fontSize = 18;
				textStyle.normal.textColor = Color.black;
				textStyle.fontStyle= FontStyle.Bold;

				GUI.Label (new Rect(910,190, 300, 50),"Your Score: ",textStyle);
                GUI.Label (new Rect (920, 205, 300, 50), "Pick Up percentage: " + DBFunctions.calcPickUpPercentage (Application.loadedLevel, StatsManager.Instance.pickUpCount)+"%",textStyle);
				GUI.Label (new Rect(920, 220, 300,50), "Level Time: " + tm[0] + " Min " + tm[1] + " Sec", textStyle);
			}
			else
			{
				Application.LoadLevel (nextLevel);
				Time.timeScale = 1;
			}
		}
	}
}
