using UnityEngine;
using System.Collections;

public class FartometerControl : MonoBehaviour 
{
    public RectTransform backfillRect;
    public RectTransform fillRect;
	
	public PlayerControl playerControlRef;

    public float maxHeight = 128;
    public float minHeight = 0;

    private float lastPotentialJuice = 0;
    private float minAnimationTime = 1;


	void Update () 
    {
        if( playerControlRef != null )
        {
    		float currentLaunchJuice	= playerControlRef.launchControl.GetCurrentJuice();
    		float fillPercent			= ( currentLaunchJuice - playerControlRef.launchControl.GetPotentialJuice() ) / playerControlRef.launchControl.GetMaxJuice();
            float backfillPercent		= currentLaunchJuice / playerControlRef.launchControl.GetMaxJuice();

    		if( currentLaunchJuice <= 0.0f )
            {
                backfillPercent = 0.0f;
    			fillPercent		= 0.0f;
    		}

            Vector2 backfillSizeDelta = backfillRect.sizeDelta;
            backfillSizeDelta.y = maxHeight * backfillPercent;
            backfillRect.sizeDelta = backfillSizeDelta;
            
            Vector2 fillRectSizeDelta = fillRect.sizeDelta;
            fillRectSizeDelta.y = maxHeight * fillPercent;
            fillRect.sizeDelta = fillRectSizeDelta;
        }
	}

}
