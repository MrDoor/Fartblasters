using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
	// Pull Line	
	public float maxLineLength = 3.0f;
	public float minLineLength = 0.6f;	

	private int maxLineVerts	= 2;
	private float pullFraction	= 0.0f;
	private float pullDist		= 0.0f;
	private float juiceToUse	= 0.0f;

	// Launch
	public float maxLaunchForce			= 7000.0f;
	public float minLaunchForce			= 2000.0f;
	public float maxJuiceUsedPerLaunch	= 10;
	public float launchesAvailable		= 10;

	private float maxLaunchJuice		= 0.0f;
	private float currentLaunchJuice	= 0.0f;	
	private bool launchAllowed			= false;
	private bool bouncyBlockHitLast		= false;
	private Vector2 launchDir;

	// Player 
	private Transform groundCheck;
	private LineRenderer pullLine;

	// Control
	private bool onGround	= false;
	private bool isMoving	= false;
	private bool isStuck	= false;

	// Animation
	public AnimationClip testAnimation;

	private Animator playerAnimator;
	private bool facingRight	= true;
	private float lastXPos		= 0.0f;
	private bool showTestAnim	= false;

	private enum Direction
	{
		Left,
		None,
		Right
	};	
	private Direction movingDir	= Direction.Right;
	
	//Didn't know where to put this
	public void setIsStuck(bool stuck)
	{
		this.isStuck = stuck;
		float stuckTime = 0.0f;
		float maxStuckTime = 10.0f;
		if( stuckTime >= maxStuckTime )
		{
			stuckTime = 0.0f;
		}
	}

	void Awake()
	{
		groundCheck = transform.Find( "groundCheck" );
		pullLine = PullLine_Init( transform );
		Launch_Init();
		Animation_Init();
		isStuck = false;
	}

	void Update () 
	{
		// The player is on the ground if a linecast from the player to the groundCheck hits a block.
		int layerMask = 1 << LayerMask.NameToLayer( "DefaultBlock" ) | 1 << LayerMask.NameToLayer( "SlipperyBlock" );
		onGround = Physics2D.Linecast( transform.position, groundCheck.position, layerMask );
		if(!onGround)
		{
			onGround = isStuck;
		}

		// TODO: Find a way to do this that doesn't involve two line casts
		layerMask = 1 << LayerMask.NameToLayer( "BouncyBlock" ); 
		if( Physics2D.Linecast( transform.position, groundCheck.position, layerMask ) )
		{
			bouncyBlockHitLast = true;
		}
		else if( onGround )
		{
			bouncyBlockHitLast = false;
		}
		
		Animation_Update( onGround );

		// Player is only allowed to launch if they're resting on a block
		// TODO: Will probably have to add more checks to set launch allowed
		//		 Does he need to be at rest as well?
		if( ( onGround || bouncyBlockHitLast ) && currentLaunchJuice > 0.0f )
		{
			Launch_SetAllowed( true );
		}
		else
		{
			Launch_SetAllowed( false );
		}
	}

	void FixedUpdate()
	{
		isMoving = ( transform.rigidbody2D.velocity.sqrMagnitude >= 0.01f || transform.rigidbody2D.angularVelocity >= 0.01f );

		if( lastXPos < transform.position.x )
		{
			movingDir = Direction.Right;
		}
		else if( lastXPos > transform.position.x )
		{
			movingDir = Direction.Left;
		}
		else
		{
			movingDir = Direction.None;
		}
		lastXPos = transform.position.x;
	}
	
	void OnMouseUp()
	{
		//added for sticky block testing
		if(this.transform.rigidbody2D.gravityScale == 0) {this.transform.rigidbody2D.gravityScale = 1;}
		
		if( pullLine != null )
		{
			Launch( transform );

			Launch_Reset();
			PullLine_Reset();

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

			Vector3 pullEndPoint = PullLine_GetEndPoint( transform.position );
			pullLine.SetPosition( 1, pullEndPoint );
		}
	}
	
	void OnGUI()
	{

	}


	// Pull Line Control
	// -------------------------------------------------------------------------------------

	public LineRenderer PullLine_Init( Transform myTransform )
	{
		pullFraction	= 0.0f;
		juiceToUse		= 0.0f;
		pullDist		= 0.0f;
		
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
	
	public void PullLine_Reset()
	{
		pullFraction	= 0.0f;
		juiceToUse		= 0.0f;
		pullDist		= 0.0f;
	}
	
	public Vector3 PullLine_GetDirection( Vector3 playerPos )
	{
		Vector3 pullDir = new Vector3( 0, 0, 0 );
		
		if( Launch_GetAllowed() )
		{
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			playerPos.z = 0.0f;
			mouseWorldPos.z = 0.0f;
			pullDir = playerPos - mouseWorldPos;
		}
		
		return pullDir;
	}
	
	public float PullLine_GetFraction()
	{
		return pullFraction;
	}
	
	public Vector3 PullLine_GetEndPoint( Vector3 playerPos )
	{
		Vector3 pullEndPoint	= playerPos;		
		Vector3 pullDir			= PullLine_GetDirection( playerPos );
		float lineLength		= 0.0f;
		Vector3 launchDir		= new Vector3( 0.0f, 0.0f, 0.0f );

		pullFraction	= 0.0f;
		juiceToUse		= 0.0f;
		pullDist		= pullDir.magnitude;
		
		if( pullDist >= minLineLength )
		{
			launchDir		= pullDir / pullDist;
			lineLength		= Mathf.Min( pullDist, maxLineLength );			
			pullFraction	= ( lineLength - minLineLength ) / ( maxLineLength - minLineLength );
			juiceToUse		= maxJuiceUsedPerLaunch * pullFraction;
			pullEndPoint	= playerPos - ( launchDir * lineLength );
		}
		
		Launch_SetDir( new Vector2( launchDir.x, launchDir.y ) );
		
		return pullEndPoint;
	}

	public bool PullLine_IsHolding()
	{
		return pullDist >= minLineLength;
	}


	// Launch Control
	// -------------------------------------------------------------------------------------
	
	public void Launch_Init()
	{
		launchAllowed		= false;
		maxLaunchJuice		= maxJuiceUsedPerLaunch * launchesAvailable;
		currentLaunchJuice	= maxLaunchJuice;
		launchDir.Set( 0.0f, 0.0f );
	}
	
	public void Launch_Reset()
	{
		launchAllowed	= false;
		launchDir.Set( 0.0f, 0.0f );
	}
	
	public void Launch_SetDir( Vector2 direction )
	{
		launchDir = direction;
	}
	
	public void Launch_SetAllowed( bool isAllowed )
	{
		launchAllowed = isAllowed;
	}
	
	public bool Launch_GetAllowed()
	{
		return launchAllowed;
	}

	public float Launch_GetMaxJuice()
	{
		return maxLaunchJuice;
	}

	public float Launch_GetCurrentJuice()
	{
		return currentLaunchJuice;
	}

	public float Launch_GetPotentialJuice()
	{
		return juiceToUse;
	}
	
	public void Launch( Transform transform )
	{
		float pullPercent = PullLine_GetFraction();
		float launchForce = minLaunchForce + ( ( maxLaunchForce - minLaunchForce ) * pullPercent );

		if( juiceToUse > currentLaunchJuice )
		{
			juiceToUse = currentLaunchJuice;
		}

		currentLaunchJuice -= juiceToUse;
		juiceToUse = 0.0f;

		Launch_SetAllowed( false );

		// TODO: assert if not transform.rigidbody2D
		transform.rigidbody2D.AddForce( launchDir * launchForce );
	}


	// Animation Control
	// -------------------------------------------------------------------------------------

	public void Animation_Init()
	{
		playerAnimator	= transform.Find( "body" ).GetComponent( "Animator" ) as Animator;
		lastXPos		= transform.position.x;
	}

	public void Animation_Update( bool onGround )
	{
		if( !string.IsNullOrEmpty( testAnimation.name ) && Input.GetButton( "Test Anim" ) )
		{
			// TODO: Add check to make sure the Animator HAS an animation with this name
			//		 Put in debug warning about the Animator state has a different name than this one.
			playerAnimator.Play( testAnimation.name );
		}
		else
		{
			bool isHolding = PullLine_IsHolding();

			if( isMoving && !isHolding )
			{
				playerAnimator.Play( "Squint" );
			}
			else if( isHolding )
			{
				playerAnimator.Play( "HoldingItIn" );
			}
			else if( onGround )
			{
				playerAnimator.Play( "Idle" );
			}
		}

		Animation_UpdateFacingDir();
	}

	public void Animation_SetFacingRight( bool isFacingRight )
	{
		facingRight = isFacingRight;
	}

	public bool Animation_GetFacingRight()
	{
		return facingRight;
	}

	public void Animation_FlipHorizontal( bool isFacingRight )
	{
		Vector3 newLocalScale = transform.localScale;
		newLocalScale.x *= -1;
		transform.localScale = newLocalScale;
		Animation_SetFacingRight( isFacingRight );
	}

	public void Animation_UpdateFacingDir()
	{
		if( PullLine_IsHolding() )
		{
			Vector3 pullDir = PullLine_GetDirection( transform.position );

			if( Animation_GetFacingRight() )
			{
				if( pullDir.x < 0 )
				{
					Animation_FlipHorizontal( false );
				}
			}
			else
			{
				if( pullDir.x > 0 )
				{
					Animation_FlipHorizontal( true );
				}
			}
		}
		else
		{
			if( Animation_GetFacingRight() )
			{
				if( movingDir == Direction.Left )
				{
					Animation_FlipHorizontal( false );
				}
			}
			else
			{
				if( movingDir == Direction.Right )
				{
					Animation_FlipHorizontal( true );
				}
			}
		}
	}
	
	// PickUp Control
	// -------------------------------------------------------------------------------------
	
	void OnTriggerEnter2D(Collider2D obj)
	{
		Debug.Log ("Food Eaten!");
		//Just temporary.  I'm sure there is a better way to do this
		float pickupJuice = 0.0f;
		if(obj.gameObject.name == "Jalupeno")
		{
			pickupJuice += 5f;
		}
		else if(obj.gameObject.name == "Broccoli")
		{
			pickupJuice += 25f;
		}		
		if(pickupJuice < 100)
		{
			currentLaunchJuice += pickupJuice;
		}
		else
		{
			currentLaunchJuice = 100.0f;
		}
		
		Destroy (obj.gameObject);	
	}
	
}
