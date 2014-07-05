using UnityEngine;
using System.Collections;

public class LaunchControl : MonoBehaviour 
{
    public PlayerControl playerControl;
    public PullLine pullLine;

    public float maxLaunchForce         = 4000.0f;
    public float minLaunchForce         = 2000.0f;
    public float maxJuiceUsedPerLaunch  = 10;
    public float launchesAvailable      = 10;

    private float maxLaunchJuice        = 0.0f;
    private float currentLaunchJuice    = 0.0f; 
    private bool launchAllowed          = false;
    private Vector2 launchDir;


    
    public void Init()
    {
        launchAllowed       = false;
        maxLaunchJuice      = maxJuiceUsedPerLaunch * launchesAvailable;
        currentLaunchJuice  = maxLaunchJuice;
        launchDir.Set( 0.0f, 0.0f );
    }
    
    public void Reset()
    {
        launchAllowed = false;
        launchDir.Set( 0.0f, 0.0f );
    }
    
    public void UpdatePermission( bool onGround, bool bouncyBlockHitLast )
    {
        // Player is only allowed to launch if they're resting on a block
        // TODO: Will probably have to add more checks to set launch allowed
        //       Does he need to be at rest as well?
        if( ( onGround || bouncyBlockHitLast ) && ( GetCurrentJuice() > 0.0f ) )
        {
            SetAllowed( true );
        }
        else
        {
            SetAllowed( false );
        }
    }
    
    public void SetDir( Vector2 direction )
    {
        launchDir = direction;
    }
    
    public Vector2 GetDir()
    {
        return launchDir;
    }
    
    public void SetAllowed( bool isAllowed )
    {
        launchAllowed = isAllowed;
    }
    
    public bool GetAllowed()
    {
        return launchAllowed;
    }
    
    public float GetMaxJuice()
    {
        return maxLaunchJuice;
    }
    
    public float GetCurrentJuice()
    {
        return currentLaunchJuice;
    }
    
    public void IncrementCurrentJuice( float amount )
    {
        if( ( amount > 0f ) && ( currentLaunchJuice < maxLaunchJuice ) )
        {
            currentLaunchJuice = Mathf.Min( maxLaunchJuice, currentLaunchJuice + amount );
        }
    }

    public void DecrementCurrentJuice( float amount )
    {
        if( (amount > 0f) && ( currentLaunchJuice > 0 ) )
        {
            currentLaunchJuice -= Mathf.Min( currentLaunchJuice, amount );
        }
    }
    
    public float GetPotentialJuice()
    {
        float potentialLaunchJuice = maxJuiceUsedPerLaunch * pullLine.GetFraction();
        
        if( potentialLaunchJuice > currentLaunchJuice )
        {
            potentialLaunchJuice = currentLaunchJuice;
        }       
        
        return potentialLaunchJuice;
    }
    
    public float GetLaunchForce()
    {
        float pullPercent = pullLine.GetFraction();
        return minLaunchForce + ( ( maxLaunchForce - minLaunchForce ) * pullPercent );
    }
    
    public void Launch( Transform transform )
    {
        float potentialLaunchJuice  = GetPotentialJuice();
        float launchForce           = GetLaunchForce();
        
        this.collider2D.transform.parent = null;
        
        currentLaunchJuice -= potentialLaunchJuice;
        
        SetAllowed( false );
        
        transform.rigidbody2D.AddForce( launchDir * launchForce );      
        
        if(launchDir != Vector2.zero)
        {
            AudioSource[] farts = playerControl.GetAudioSources();         
            farts[(int)Random.Range(0, farts.Length)].Play ();
        }
    }
}
