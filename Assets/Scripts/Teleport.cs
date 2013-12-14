using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
	
	public PlayerControl playerControlRef;
	public static float nextTeleport;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		//This only works if there is one of each block in the level... got it to work but will need to be modified to handle more than 2 teleport objects
		if(nextTeleport > Time.time)
		{
			Debug.Log(Time.time);
			return;
		}
		
		try
		{
			if(this.gameObject.name.EndsWith("1"))
			{
				playerControlRef.transform.position = GameObject.Find("TeleportBlock2").transform.position;
				nextTeleport = Time.time + 2;
				Debug.Log ("Teleport! " + nextTeleport);			
			}
			else
			{
				playerControlRef.transform.position = GameObject.Find("TeleportBlock1").transform.position;
				nextTeleport = Time.time + 2;
				Debug.Log ("Teleport! " + nextTeleport);
			}
		}
		catch(UnityException ex)
		{
			Debug.Log ("Error: " + ex.Message);
		}	
	}	
}
