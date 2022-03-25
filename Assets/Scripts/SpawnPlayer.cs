using UnityEngine;
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
        int tempSpawnCheck = PlayerPrefs.GetInt("died");
        Vector3 lastSafeSpot = new Vector3(PlayerPrefs.GetFloat("safeSpotX"), PlayerPrefs.GetFloat("safeSpotY"), 0);
        Vector3 deathSpot = new Vector3(PlayerPrefs.GetFloat("deathSpotX"), PlayerPrefs.GetFloat("deathSpotY"), 0);
        Vector3 playerOriginSpawn = Util.SafeGameObjectFind("PlayerSpawn").transform.position;
        Vector3 spawnSpot = playerOriginSpawn;

        if (tempSpawnCheck == 1)
        {
            spawnSpot = lastSafeSpot;
            PlayerPrefs.SetInt("died", 0);
        }

        newPlayer = (GameObject)Instantiate(player, spawnSpot, Quaternion.identity);
        newPlayer.name = newPlayer.name.Replace("(Clone)", "");

        if ((newPlayer != null) && (fartometer != null))
        {
            fartometer.playerControlRef = newPlayer.GetComponent<PlayerControl>();
        }

        //GameObject pObj = GameObject.FindGameObjectWithTag( "Player" );
        //Camera.main.GetComponent<CameraFollow>().SetPlayerGameObject(pObj);
        if (newPlayer == null)
        {
            Debug.Log("newPlayer was null");
            return;
        }
        Camera.main.GetComponent<CameraFollow>().SetPlayerGameObject(newPlayer);

        PlayerPrefs.SetFloat("safeSpotX", playerOriginSpawn.x);
        PlayerPrefs.SetFloat("safeSpotY", playerOriginSpawn.y);

        //Camera.main.GetComponent<CameraFollow>().SetPlayerTransform( pObj.transform );
        Debug.Log("MainCamera.PlayerSet to:");
        Debug.Log(newPlayer.name);
    }
}
