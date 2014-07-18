using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour 
{
    public Animator pAnimator;
	public PlayerControl pControl;
    public PullLine pullLine;
	
	// Use this for initialization
	void Start () 
    {
//		pControl = Util.SafePlayerControlFind();
//		pAnimator = this.GetComponent<Animator>();
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
			pAnimator.SetBool ( "isCharging", pullLine.IsHolding() );
			pAnimator.SetBool ( "isMoving", pControl.GetIsMoving() );
			pAnimator.SetBool ( "inVortex", pControl.GetInVortex() );
			pAnimator.SetBool ( "isEating", pControl.GetIsEating() );
		}
	}
}
