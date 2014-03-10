using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	private PlayerControl pControl;
	private Animator pAnimator;
	
	// Use this for initialization
	void Start () {
		pControl = Util.SafePlayerControlFind();
		pAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{			
	}
	
	void Update()
	{		
		//Testing Animation Transition
		if(pAnimator)
		{
			pAnimator.SetBool ( "isCharging", pControl.PullLine_IsHolding() );
			pAnimator.SetBool ( "isMoving", pControl.GetIsMoving() );
			pAnimator.SetBool ( "inVortex", pControl.GetInVortex() );
			pAnimator.SetBool ( "isEating", pControl.GetIsEating() );
		}
	}
}
