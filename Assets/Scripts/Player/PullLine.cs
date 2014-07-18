using UnityEngine;
using System.Collections;

public class PullLine : MonoBehaviour 
{
    public PlayerControl playerControl;
    public LaunchControl launchControl;

    public float maxLineLength;
    public float minLineLength; 
    
    private Transform[] fartClouds;
    private const int maxFartClouds = 6;
    private float pullFraction      = 0.0f;
    private float pullDist          = 0.0f;


    
    public void Init()
    {
        pullFraction    = 0.0f;
        pullDist        = 0.0f;      
  
        fartClouds = new Transform[ maxFartClouds ];
        for( int cloudIndex = 0; cloudIndex < maxFartClouds; ++cloudIndex )
        {
            fartClouds[ cloudIndex ] = transform.FindChild( "FartCloud" + cloudIndex ); 
        }
    }
    
    public void Reset()
    {
        pullFraction    = 0.0f;
        pullDist        = 0.0f;
        
        for( int cloudIndex = 0; cloudIndex < maxFartClouds; ++cloudIndex )
        {
            fartClouds[ cloudIndex ].transform.position = transform.position; 
        }
    }
    
    public Vector3 GetDirection( Vector3 playerPos )
    {
        Vector3 pullDir = new Vector3( 0, 0, 0 );
        
        if( launchControl.GetAllowed() )
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            playerPos.z = 0.0f;
            mouseWorldPos.z = 0.0f;
            pullDir = playerPos - mouseWorldPos;
        }
        
        return pullDir;
    }
    
    public float GetFraction()
    {
        return pullFraction;
    }
    
    public Vector3 GetEndPoint( Vector3 playerPos )
    {
        Vector3 pullEndPoint    = playerPos;        
        Vector3 pullDir         = GetDirection( playerPos );
        float lineLength        = 0.0f;
        Vector3 launchDir       = new Vector3( 0.0f, 0.0f, 0.0f );
        
        pullFraction    = 0.0f;
        pullDist        = pullDir.magnitude;
        
        if( pullDist >= minLineLength )
        {
            launchDir       = pullDir / pullDist;
            lineLength      = Mathf.Min( pullDist, maxLineLength );         
            pullFraction    = ( lineLength - minLineLength ) / ( maxLineLength - minLineLength );
            pullEndPoint    = playerPos - ( launchDir * lineLength );
        }
        
        launchControl.SetDir( new Vector2( launchDir.x, launchDir.y ) );
        playerControl.SetIsStopped( false );
        
        return pullEndPoint;
    }
    
    public bool IsHolding()
    {
        return pullDist >= minLineLength;
    }
    
    public void PositionClouds( Vector3 playerPos, Vector3 pullEndPoint )
    {
        Vector3 direction = playerPos - pullEndPoint;
        float stepDistance = 1.0f / maxFartClouds;
        
        for( int cloudIndex = 0; cloudIndex < maxFartClouds; ++cloudIndex )
        {
            float stepAmount = ( cloudIndex * Mathf.Pow( ( stepDistance + 0.004f ), 1.05f ) );
            Vector3 step = direction * stepAmount;
            fartClouds[ maxFartClouds - cloudIndex - 1 ].transform.position = pullEndPoint + step; 
        }
    }
}
