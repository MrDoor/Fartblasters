using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour 
{
    public Animator playerAnimator;
	public PlayerControl playerControl;
    public PullLine pullLine;
    
    public AnimationClip testAnimation;
    public bool showTestAnim    = true;

    public bool isFacingRight { get; private set; }
	
	// Use this for initialization
	void Start () 
    {
        isFacingRight = true;
	}
		
	void Update()
	{		
        if(playerAnimator)
		{
            playerAnimator.SetBool( "isCharging",  pullLine.IsHolding() );
            playerAnimator.SetBool( "isMoving",    playerControl.GetIsMoving() );
            playerAnimator.SetBool( "inVortex",    playerControl.GetInVortex() );
            playerAnimator.SetBool( "isEating",    playerControl.GetIsEating() );
		}

        if( showTestAnim && !string.IsNullOrEmpty( testAnimation.name ) && Input.GetButton( "Test Anim" ) )
        {
            playerAnimator.Play( testAnimation.name );
        }
        
        UpdateFacingDir();
	}

    public void Init()
    {
        playerControl.SetLastXPos( transform.position.x );
    }
        
    public void FlipHorizontal()
    {
        Vector3 newLocalScale = playerControl.transform.localScale;
        newLocalScale.x *= -1;
        playerControl.transform.localScale = newLocalScale;
        isFacingRight = ( playerControl.transform.localScale.x >= 0 );
    }
    
    public void UpdateFacingDir()
    {
        if( pullLine.IsHolding() )
        {
            Vector3 pullDir = pullLine.GetDirection( transform.position );
            
            if( isFacingRight )
            {
                if( pullDir.x < 0 )
                {
                    FlipHorizontal();
                }
            }
            else
            {
                if( pullDir.x > 0 )
                {
                    FlipHorizontal();
                }
            }
        }
        else
        {
            if( isFacingRight )
            {
                if( playerControl.GetMovingDir() == Direction.LEFT )
                {
                    FlipHorizontal();
                }
            }
            else
            {
                if( playerControl.GetMovingDir() == Direction.RIGHT )
                {
                    FlipHorizontal();
                }
            }
        }
    }
    
    public void PlayAnimation( string animationName )
    {
        try
        {       
            playerAnimator.Play( animationName );
        }
        catch(UnityException ue)
        {
            Debug.LogError ( "Error: " + ue.ToString() );
        }
    }   
}
