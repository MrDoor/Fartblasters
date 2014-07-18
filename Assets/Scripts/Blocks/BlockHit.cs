using UnityEngine;
using System.Collections;

public class BlockHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Note Done
	void OnCollisionEnter2D(Collision2D coll)
	{
	/*
		PlayerControl pControl;
		//Animator pAnimator;
		if(coll.gameObject.tag.Equals("Player"))
		{			
			pControl = (PlayerControl)coll.gameObject.GetComponent<PlayerControl>();
			if(pControl != null)
			{
				pControl.Animation_PlayAnimation ( "HitWall" );
			}
		}
		*/
	}
}
