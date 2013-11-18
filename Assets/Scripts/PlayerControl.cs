using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
	// Pull Line	
	public float maxLineLength = 3.0f;
	public float minLineLength = 0.6f;	

	private int maxLineVerts	= 2;
	private float pullFraction	= 0.0f;

	// Launch
	public float maxLaunchForce			= 7000.0f;
	public float minLaunchForce			= 3000.0f;
	public float maxJuiceUsedPerLaunch	= 10;
	public float launchesAvailable		= 10;

	private float maxLaunchJuice		= 0.0f;
	private float currentLaunchJuice	= 0.0f;	
	private bool launchAllowed			= false;
	private Vector2 launchDir;

	// Player 
	private Transform groundCheck;
	private LineRenderer pullLine;

	// Control
	private bool onGround = false;

	void Awake()
	{
		groundCheck = transform.Find( "groundCheck" );
		pullLine = PullLineInit( transform );
		LaunchInit();
	}

	void Update () 
	{
		// The player is on the ground if a linecast from the player to the groundCheck hits a block.
		onGround = true; //Physics2D.Linecast( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer( "Block" ) );
	
		// Player is only allowed to launch if they're resting on a block
		// TODO: Will probably have to add more checks to set launch allowed
		//		 Does he need to be at rest as well?
		if( onGround && currentLaunchJuice > 0.0f )
		{
			SetLaunchAllowed( true );
		}
		else
		{
			SetLaunchAllowed( false );
		}
	}

	void FixedUpdate()
	{
	}
	
	void OnMouseUp()
	{
		if( pullLine != null )
		{
			Launch( transform );

			LaunchReset();
			PullLineReset();

			pullLine.SetPosition( 0, transform.position );
			pullLine.SetPosition( 1, transform.position );

		}
	}


	void OnMouseDrag()
	{
		// TODO: use an assert here instead. there should always be a pullLine
		if( pullLine )
		{
			pullLine.SetPosition( 0, transform.position );

			Vector3 pullEndPoint = GetPullEndPoint( transform.position );
			pullLine.SetPosition( 1, pullEndPoint );
		}
	}

	
	void OnGUI()
	{
		GUI.Label( new Rect( 0, 0, 1000, 200 ), currentLaunchJuice.ToString() );
	}




	// Pull Line Control
	// -------------------------------------------------------------------------------------

	public LineRenderer PullLineInit( Transform myTransform )
	{
		pullFraction = 0.0f;
		
		LineRenderer pullLine = null;
		Transform pullContainer = myTransform.FindChild( "pullContainer" );
		// TODO: Change this to assert
		if( pullContainer )
		{
			pullLine = pullContainer.GetComponent<LineRenderer>();
			// TODO: put asset on pullLine here, too
			pullLine.SetVertexCount( maxLineVerts );
		}
		return pullLine;
	}
	
	public void PullLineReset()
	{
		pullFraction = 0.0f;
	}
	
	public Vector3 GetPullDirection( Vector3 playerPos )
	{
		Vector3 pullDir = new Vector3( 0, 0, 0 );
		
		if( GetLaunchAllowed() )
		{
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			playerPos.z = 0.0f;
			mouseWorldPos.z = 0.0f;
			pullDir = playerPos - mouseWorldPos;
		}
		
		return pullDir;
	}
	
	public float GetPullFraction()
	{
		return pullFraction;
	}
	
	public Vector3 GetPullEndPoint( Vector3 playerPos )
	{
		Vector3 pullEndPoint = playerPos;
		
		Vector3 pullDir = GetPullDirection( playerPos );
		
		float pullDist = pullDir.magnitude;
		float lineLength = 0;
		Vector3 launchDir = new Vector3( 0.0f, 0.0f, 0.0f );
		pullFraction = 0;
		
		if( pullDist >= minLineLength )
		{
			launchDir = pullDir / pullDist;
			lineLength = Mathf.Min( pullDist, maxLineLength );			
			pullFraction = ( lineLength - minLineLength ) / ( maxLineLength - minLineLength );
			pullEndPoint = playerPos - ( launchDir * lineLength );
		}
		
		SetLaunchDir( new Vector2( launchDir.x, launchDir.y ) );
		
		return pullEndPoint;
	}



	// Launch Control
	// -------------------------------------------------------------------------------------
	
	public void LaunchInit()
	{
		launchAllowed = false;
		launchDir.Set( 0.0f, 0.0f );
		maxLaunchJuice = maxJuiceUsedPerLaunch * launchesAvailable;
		currentLaunchJuice = maxLaunchJuice;
	}
	
	public void LaunchReset()
	{
		launchAllowed = false;
		launchDir.Set( 0.0f, 0.0f );
	}
	
	public void SetLaunchDir( Vector2 direction )
	{
		launchDir = direction;
	}
	
	public void SetLaunchAllowed( bool isAllowed )
	{
		launchAllowed = isAllowed;
	}
	
	public bool GetLaunchAllowed()
	{
		return launchAllowed;
	}
	
	public void Launch( Transform transform )
	{
		float pullPercent = GetPullFraction();
		float launchForce = minLaunchForce + ( ( maxLaunchForce - minLaunchForce ) * pullPercent );
		float juiceToUse = maxJuiceUsedPerLaunch * pullPercent;

		if( juiceToUse > currentLaunchJuice )
		{
			juiceToUse = currentLaunchJuice;
		}

		currentLaunchJuice -= juiceToUse;

		// TODO: assert if not transform.rigidbody2D
		transform.rigidbody2D.AddForce( launchDir * launchForce );
	}
}
