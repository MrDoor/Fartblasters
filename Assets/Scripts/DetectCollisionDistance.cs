using UnityEngine;
using System.Collections;

public class DetectCollisionDistance : MonoBehaviour {

	private bool nearWall 	= false;
	private float distance 	= 0f;
	
	private PlayerControl pControl;
	private Transform wallTransform;
	// Use this for initialization
	void Start () {
		pControl = Util.SafePlayerControlFind();
	}
	/*
	// Update is called once per frame
	void Update () {
		Debug.Log ( "Near Wall: " + nearWall );
		if ( nearWall )
		{
			distance = pControl.transform.position.x - wallTransform.position.x;
			Debug.Log ( "distance: " + distance );
		}
	}
	*/
	
	/*
	void OnTriggerEnter2D ( Collider2D coll )
	{		
		wallTransform = coll.transform;
		nearWall = true;		
	}
	*/
}
