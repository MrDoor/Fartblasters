using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

	//public string tempSpawnCheck = "claw";
	public GameObject player;
	public GameObject claw;
	public FartometerControl fartMeter;
	
	private GameObject newClaw;
	private GameObject newPlayer;
	private FartometerControl newFartMeter;
	
	// Use this for initialization
	void Start () {
		int tempSpawnCheck = PlayerPrefs.GetInt ( "died" );
		Vector3 deathSpot = new Vector3(PlayerPrefs.GetFloat( "deathSpotX" ), PlayerPrefs.GetFloat ( "deathSpotY" ), 0);
		Vector3 spawnSpot;
				
		switch ( tempSpawnCheck )
		{
			case 1	: 		spawnSpot = deathSpot;
							spawnSpot.y += (Camera.main.orthographicSize / 2) + 1f;
							newClaw = (GameObject)Instantiate ( claw, spawnSpot, Quaternion.identity );
							newClaw.name = newClaw.name.Replace( "(Clone)", "" );
							break;
			default		:	spawnSpot = Util.SafeGameObjectFind( "PlayerSpawn" ).transform.position;
							newPlayer = (GameObject)Instantiate ( player, spawnSpot, Quaternion.identity );
							newPlayer.name = newPlayer.name.Replace( "(Clone)", "" );							
							break;
		}
		newFartMeter = (FartometerControl)Instantiate ( fartMeter, new Vector3 ( 0, 0 ,0 ), Quaternion.identity );
		newFartMeter.name = newFartMeter.name.Replace( "(Clone)", "" );
		//Camera.main.camera.GetComponent<CameraFollow>().SetPlayer ( newPlayer.transform );
		GameObject pObj = GameObject.FindGameObjectWithTag ( "Player" );
		Camera.main.GetComponent<CameraFollow>().SetPlayer ( pObj.transform );
		Debug.Log ( "MainCamera.PlayerSet = true" );
		//camera.GetComponent<CameraFollow>().SetPlayer ( newPlayer.transform );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
