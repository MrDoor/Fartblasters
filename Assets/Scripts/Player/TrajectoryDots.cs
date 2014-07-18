using UnityEngine;
using System.Collections;

public class TrajectoryDots : MonoBehaviour 
{
    public LaunchControl launchControl;
    public GameObject trajectoryDotPrefab;
    
    private Transform[] trajectoryDots;
    private const int maxTrajectoryDots = 6;
    private float dotDelay              = 0.5f;
    private float dotTime               = 0f;



    public void Init()
    {
        trajectoryDots  = null;
        dotTime         = Time.time + dotDelay;
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "TrajectoryDot" ), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer( "Enemies" ), LayerMask.NameToLayer( "TrajectoryDot" ), true);

        trajectoryDots = new Transform[ maxTrajectoryDots ];
        for( int dotIndex = 0; dotIndex < maxTrajectoryDots; ++dotIndex )
        {
            trajectoryDots[ dotIndex ] = transform.FindChild( "TrajectoryDot" + dotIndex ); 
        }
    }
    
    public void Reset()
    {
        for( int dotIndex = 0; dotIndex < maxTrajectoryDots; ++dotIndex )
        {
            trajectoryDots[ dotIndex ].transform.position = transform.position; 
        }
    }
    
    public void Position( Vector3 playerPos, Vector3 pullEndPoint )
    {   
        /*          
        Vector3 direction = playerPos - pullEndPoint;       
        Vector3 dotEndPoint = playerPos + ( direction / 3 );
        float stepDistance = 4.0f / maxTrajectoryDots;
        
        Debug.Log ( "pullEndPoint = " + pullEndPoint + " direction = " + direction );
        
        for( int dotIndex = 0; dotIndex < maxTrajectoryDots; ++dotIndex )
        {
            float stepAmount = ( dotIndex * Mathf.Pow( ( stepDistance + 0.0004f ), 1.001f ) );
            Vector3 step = direction * stepAmount;
            trajectoryDots[ maxTrajectoryDots - dotIndex - 1 ].transform.position = dotEndPoint + step ;            
        }
        */
        
        if( Time.time >= dotTime )
        {
            LaunchDot();
        }
    }
    
    public void LaunchDot()
    {       
        dotTime = Time.time + dotDelay;

        float launchForce = launchControl.GetLaunchForce();
        Vector2 launchDir = launchControl.GetDir();

        GameObject newDot = (GameObject)Instantiate( trajectoryDotPrefab, this.transform.position, Quaternion.identity );
        newDot.transform.rigidbody2D.AddForce( launchForce * launchDir );
        StartCoroutine( Util.Destroy_Now( newDot, 1f ) );
    }
}
