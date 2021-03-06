﻿using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour 
{
	//public string tempSpawnCheck = "claw";
	public GameObject player;
	public GameObject claw;
	public FartometerControl fartometer;
	
	private GameObject newClaw;
	private GameObject newPlayer;
	
	// Use this for initialization
	void Start() 
    {
		int tempSpawnCheck = PlayerPrefs.GetInt ( "died" );
		Vector3 deathSpot = new Vector3(PlayerPrefs.GetFloat( "deathSpotX" ), PlayerPrefs.GetFloat ( "deathSpotY" ), 0);
		Vector3 spawnSpot;
				
		switch ( tempSpawnCheck )
		{
			case 1: 		
            {
                spawnSpot = deathSpot;
				spawnSpot.y += (Camera.main.orthographicSize / 2) + 1f;
				newClaw = (GameObject)Instantiate ( claw, spawnSpot, Quaternion.identity );
				newClaw.name = newClaw.name.Replace( "(Clone)", "" );
				break;
            }
			default:
            {
                spawnSpot = Util.SafeGameObjectFind( "PlayerSpawn" ).transform.position;
				newPlayer = (GameObject)Instantiate ( player, spawnSpot, Quaternion.identity );
				newPlayer.name = newPlayer.name.Replace( "(Clone)", "" );							
				break;
            }
		}

        if((newPlayer != null) && (fartometer != null))
        {
            fartometer.playerControlRef = newPlayer.GetComponent<PlayerControl>();
        }

		GameObject pObj = GameObject.FindGameObjectWithTag( "Player" );
		Camera.main.GetComponent<CameraFollow>().SetPlayer( pObj.transform );
		Debug.Log ( "MainCamera.PlayerSet = true" );
	}
}
