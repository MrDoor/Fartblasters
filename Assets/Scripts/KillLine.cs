using UnityEngine;
using System.Collections;

public class KillLine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D ( Collision2D coll )
	{
		if( coll.gameObject.CompareTag ( "Player" ) )
		{
			PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();
			pControl.playerHealth.KillPlayer();
		}
	}
}
