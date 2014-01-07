using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
	// Player Control
	private Transform groundCheck;
	private bool onGround	= false;
	private bool isMoving	= false;
	private bool isStuck	= false;
	private bool inVortex 	= false;

	// Pull Line	
	public float maxLineLength = 1.5f;
	public float minLineLength = 0.6f;	

	private const int maxLineVerts	= 2;
	private const int maxFartClouds	= 6;
	private Transform[] fartClouds;
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

	// Animation
	public AnimationClip testAnimation;
	public bool showTestAnim	= true;

	private Animator playerAnimator;
	private bool facingRight	= true;
	private float lastXPos		= 0.0f;
	private Direction movingDir	= Direction.RIGHT;

	// Debug
	public GameObject debugSpawnFoodObj;

	private int maxDebugFood			= 3;
	private int debugFoodCount			= 0;
	private float debugFoodDestroyTime	= 3.0f;



	private enum Direction
	{
		LEFT,
		NONE,
		RIGHT
	};	

	// These are hard coded to the values of the layers in the editor
	// Have to update by hand if they change
	const int BLOCKLAYER_DEFAULT		= 1 << 11;
	const int BLOCKLAYER_SLIPPERY		= 1 << 12;
	const int BLOCKLAYER_BOUNCY			= 1 << 13;
	const int BLOCKLAYER_STICKY			= 1 << 14;
	const int BLOCKLAYER_TELEPORT1		= 1 << 15;
	const int BLOCKLAYER_TELEPORT2		= 1 << 16;
	const int BLOCKLAYER_STOP			= 1 << 17;
	const int BLOCKLAYER_MAGNET			= 1 << 18;
	const int BLOCKLAYER_VORTEX			= 1 << 19;
	const int BLOCKLAYER_DISAPPEAR		= 1 << 20;	
	const int BLOCKLAYER_MOVING			= 1 << 21;
	const int BLOCKLAYER_FALLING		= 1 << 22;
	const int BLOCKLAYER_BOOST			= 1 << 23;


	void Awake()
	{
		groundCheck = transform.Find( "groundCheck" );
		PullLine_Init( transform );
		Launch_Init();
		Animation_Init();
		isStuck = false;
	}

	void Update () 
	{		
		// The player is on the ground if a linecast from the player to the groundCheck hits a block.
		int layerMask = BLOCKLAYER_DEFAULT | BLOCKLAYER_SLIPPERY | BLOCKLAYER_STICKY | BLOCKLAYER_TELEPORT1 | BLOCKLAYER_TELEPORT2 | BLOCKLAYER_STOP | BLOCKLAYER_MAGNET | BLOCKLAYER_VORTEX | BLOCKLAYER_DISAPPEAR 
						| BLOCKLAYER_MOVING | BLOCKLAYER_FALLING;
		onGround = Physics2D.Linecast( transform.position, groundCheck.position, layerMask );
		if( !onGround )
		{
			onGround = isStuck;
		}

		// TODO: Find a way to do this that doesn't involve two line casts
		layerMask = BLOCKLAYER_BOUNCY; 
		if( Physics2D.Linecast( transform.position, groundCheck.position, layerMask ) )
		{
			bouncyBlockHitLast = true;
		}
		else if( onGround )
		{
			bouncyBlockHitLast = false;
		}
		
		Animation_Update( onGround );
		Launch_Update( onGround, bouncyBlockHitLast );

		// TODO: Put in a check to only allow this in debug
		Debug_CheckSpawnFood();
		
		if(Input.GetKeyDown("d") && onGround)
		{
			//added for sticky block testing
			if(this.transform.rigidbody2D.gravityScale == 0) 
			{
				this.transform.rigidbody2D.gravityScale = 1;
			}
			this.transform.rigidbody2D.AddForce(new Vector2(100, 500));//schooch!
			scoochPoot();		
		}
		else if(Input.GetKeyDown("a") && onGround)
		{
			//added for sticky block testing
			if(this.transform.rigidbody2D.gravityScale == 0) 
			{
				this.transform.rigidbody2D.gravityScale = 1;
			}
			this.transform.rigidbody2D.AddForce(new Vector2(-100, 500));//schooch!
			scoochPoot();
		}		
	}
	
	void scoochPoot()
	{	
		try
		{
			if(!this.transform.Find ("body").audio.isPlaying)
			{
				this.transform.Find("body").audio.Play ();				
			}
		}
		catch(UnityException ue)
		{
			Debug.Log ("Error with getting body: " + ue.ToString());
		}	
	}

	void FixedUpdate()
	{
		isMoving = ( transform.rigidbody2D.velocity.sqrMagnitude >= 0.01f || transform.rigidbody2D.angularVelocity >= 0.01f );
		
		if(!onGround && collider2D.transform.parent != null)
		{			
			this.collider2D.transform.parent = null;
		}

		if( isMoving )
		{
			if( lastXPos < transform.position.x )
			{
				movingDir = Direction.RIGHT;
			}
			else if( lastXPos > transform.position.x )
			{
				movingDir = Direction.LEFT;
			}
			else
			{
				movingDir = Direction.NONE;
			}
			lastXPos = transform.position.x;
		}
	}
	
	void OnMouseUp()
	{
		//added for sticky block testing
		if(this.transform.rigidbody2D.gravityScale == 0) 
		{
			this.transform.rigidbody2D.gravityScale = 1;
		}
		
		Launch( transform );

		Launch_Reset();
		PullLine_Reset();
		
		this.collider2D.transform.parent = null;
	}


	void OnMouseDrag()
	{
		Vector3 pullEndPoint = PullLine_GetEndPoint( transform.position );
		PullLine_PositionClouds( transform.position, pullEndPoint );
	}
	
	void OnGUI()
	{

	}
	
	
	// Player Control
	// -------------------------------------------------------------------------------------
	public bool GetOnGround()
	{
		return onGround;
	}

	public void SetOnGround( bool isOnGround )
	{
		onGround = isOnGround;
	}

	public bool GetIsMoving()
	{
		return isMoving;
	}
	
	public void SetIsMoving( bool moving )
	{
		isMoving = moving;
	}

	public bool GetIsStuck()
	{
		return isStuck;
	}

	public void SetIsStuck( bool stuck )
	{
		isStuck = stuck;
	}
	
	public bool GetInVortex()
	{
		return inVortex;
	}
	
	public void SetInVortex(bool isInVortex)
	{
		inVortex = isInVortex;
	}


	// Pull Line Control
	// -------------------------------------------------------------------------------------

	public void PullLine_Init( Transform myTransform )
	{
		pullFraction	= 0.0f;
		juiceToUse		= 0.0f;
		pullDist		= 0.0f;
		fartClouds		= null;

		Transform pullContainer = myTransform.FindChild( "pullContainer" );
		// TODO: Change this to assert
		if( pullContainer )
		{
			fartClouds = new Transform[ maxFartClouds ];
			for( int cloudIndex = 0; cloudIndex < maxFartClouds; ++cloudIndex )
			{
				fartClouds[ cloudIndex ] = pullContainer.FindChild( "FartCloud" + cloudIndex ); 
			}
		}
	}
	
	public void PullLine_Reset()
	{
		pullFraction	= 0.0f;
		juiceToUse		= 0.0f;
		pullDist		= 0.0f;

		for( int cloudIndex = 0; cloudIndex < maxFartClouds; ++cloudIndex )
		{
			fartClouds[ cloudIndex ].transform.position = transform.position; 
		}
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

	public void PullLine_PositionClouds( Vector3 playerPos, Vector3 pullEndPoint )
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

	public void Launch_Update( bool onGround, bool bouncyBlockHitLast )
	{
		// Player is only allowed to launch if they're resting on a block
		// TODO: Will probably have to add more checks to set launch allowed
		//		 Does he need to be at rest as well?
		if( ( onGround || bouncyBlockHitLast ) && Launch_GetCurrentJuice() > 0.0f )
		{
			Launch_SetAllowed( true );
		}
		else
		{
			Launch_SetAllowed( false );
		}
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

	public void Launch_IncCurrentJuice( float amount )
	{
		if( amount > 0.0f && currentLaunchJuice < maxLaunchJuice )
		{
			currentLaunchJuice = Mathf.Min( maxLaunchJuice, currentLaunchJuice + amount );
		}
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
		
		Debug.Log ("Fart sound play now! launchForce: " + launchForce + " | launchDir: " + launchDir.ToString());
		if(launchDir != Vector2.zero)
		{
			AudioSource[] farts = this.gameObject.GetComponents<AudioSource>();			
			farts[(int)Random.Range(0, farts.Length)].Play ();
		}
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
		if( showTestAnim && !string.IsNullOrEmpty( testAnimation.name ) && Input.GetButton( "Test Anim" ) )
		{
			// TODO: Add check to make sure the Animator HAS an animation with this name
			//		 Put in debug warning about the Animator state has a different name than this one.
			playerAnimator.Play( testAnimation.name );
		}
		else
		{
			bool isHolding = PullLine_IsHolding();

			
			if( inVortex )
			{
				playerAnimator.Play ("Vortex");
			}
			else if( isMoving && !isHolding )
			{
				playerAnimator.Play( "flying" );
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
				if( movingDir == Direction.LEFT )
				{
					Animation_FlipHorizontal( false );
				}
			}
			else
			{
				if( movingDir == Direction.RIGHT )
				{
					Animation_FlipHorizontal( true );
				}
			}
		}
	}

	
	// Misc Debug (add new stuff above this)
	// -------------------------------------------------------------------------------------
	private void Debug_CheckSpawnFood()
	{
		if( Input.GetButtonDown( "Debug Spawn Food" ) )
		{
			Debug_SpawnFood();
		}
	}

	private void Debug_SpawnFood()
	{
		if( debugSpawnFoodObj )
		{
			if( debugFoodCount < maxDebugFood )
			{
				float foodYOffset	= 1.5f;
				Vector3 newFoodPos	= transform.position;
				newFoodPos.y += foodYOffset;

				GameObject newFood = (GameObject)Instantiate( debugSpawnFoodObj, newFoodPos, Quaternion.identity );
				Debug_IncFoodCount();

				Destroy_Self( newFood, debugFoodDestroyTime );
			}
			else
			{
				Debug.Log( "Debug Food Count: " + debugFoodCount );
			}
		}
		else
		{
			Debug.Log( "Pressed Space but no prefab was set to Debug Spawn Food." );
		}
	}

	public void Debug_DecFoodCount()
	{
		debugFoodCount--;
	}

	public void Debug_IncFoodCount()
	{
		debugFoodCount++;
	}

	public void Destroy_Self( GameObject go, float delayTime )
	{
		StartCoroutine( Destroy_Now( go, delayTime ) );
	}

	public IEnumerator Destroy_Now( GameObject go, float delayTime )
	{
		yield return new WaitForSeconds( delayTime );
		if( go )
		{
			if( Util.IsSpaceBarSpawnedFood( go ) )
			{
				Debug_DecFoodCount();
			}
			Destroy( go );
		}
	}
}
