﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
	// Player Control
	private Transform groundCheck;
	private bool onGround	= false;
	private bool isMoving	= false;
	private bool isStuck	= false;
    private bool inVortex 	= false;
    private bool isStopped  = false;

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
	
	// Health
	public float maxHealth				= 100f;
	private float currentHealth			= 0.0f;
	private float lastHit				= 0.0f;

	// Animation
	public AnimationClip testAnimation;
	public bool showTestAnim	= true;

	private Animator playerAnimator;
	private bool facingRight	= true;
	private float lastXPos		= 0.0f;
	private Direction movingDir	= Direction.RIGHT;

    // Sounds
    private AudioSource playerBodyAudioSource;

	// Debug
	public GameObject debugSpawnFoodObj;

	private int maxDebugFood			= 3;
	private int debugFoodCount			= 0;
	private float debugFoodDestroyTime	= 3.0f;

	void Awake()
	{
		groundCheck = transform.Find( "groundCheck" );
		PullLine_Init( transform );
		Launch_Init();
		Animation_Init();
		isStuck = false;
        isStopped = false;
        Sound_Init();
        Health_init();
	}

	void Update () 
	{		
		// The player is on the ground if a linecast from the player to the groundCheck hits a block.
        int layerMask = Constants.LayerMask_Ground;
		onGround = Physics2D.Linecast( transform.position, groundCheck.position, layerMask );
		if( !onGround )
		{
			onGround = isStuck;
		}

		// TODO: Find a way to do this that doesn't involve two line casts
		layerMask = Constants.BLOCKLAYER_BOUNCY; 
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
			this.transform.rigidbody2D.AddForce(new Vector2(100, 500));//scooch!
			scoochPoot();		
		}
		else if(Input.GetKeyDown("a") && onGround)
		{
			//added for sticky block testing
			if(this.transform.rigidbody2D.gravityScale == 0) 
			{
				this.transform.rigidbody2D.gravityScale = 1;
			}
			this.transform.rigidbody2D.AddForce(new Vector2(-100, 500));//scooch!
			scoochPoot();
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
		
		//Zooming in and out		
		StopCoroutine("zoomOut");
		StartCoroutine("zoomIn");		
		
		this.collider2D.transform.parent = null;
	}
	
	void OnMouseDown()
	{
		//Zooming in and out	
		StopCoroutine("zoomIn");				
		StartCoroutine("zoomOut");		
	}

	void OnMouseDrag()
	{
		Vector3 pullEndPoint = PullLine_GetEndPoint( transform.position );
		PullLine_PositionClouds( transform.position, pullEndPoint );
	}
	
	//public TextAsset txtAsst;
	void OnGUI()
	{
		if(!alive)
		{			
			GUI.color = Color.red;
			GUIStyle textStyle = new GUIStyle();
			textStyle.fontSize = 80;
			textStyle.fontStyle = FontStyle.Bold;
			GUI.Label(new Rect(Screen.width / 3, Screen.height / 2 ,Screen.width,Screen.height),"Game Over", textStyle);	
		}
		//Just a temp spot for health
		//Texture2D healthBubble = new Texture2D(32, 32);
		//healthBubble.LoadImage(txtAsst.bytes);
		//GUI.Label(new Rect(0,0,Screen.width,Screen.height),currentHealth.ToString());
	}
	
	// Not sure if this should go here or in a different script file?
	// Camera Zoom
	// -------------------------------------------------------------------------------------
	
	IEnumerator zoomOut()
	{		
		while(Camera.main.orthographicSize <= 5)
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,6, Time.deltaTime * 1.25f);						
			yield return new WaitForSeconds(.025f);
		}		
		yield break;		
	}
	
	IEnumerator zoomIn()
	{
		yield return new WaitForSeconds(1.25f);
		while(Camera.main.orthographicSize > 3)
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,3, Time.deltaTime * .2f);
			//Could not figure this out so I just added this crap.  Needs to be redone.
			if(Camera.main.orthographicSize <= .31)
			{
				Camera.main.orthographicSize = 3;
				break;
			}
			yield return new WaitForSeconds(.025f);
		}
		
		yield break;
	}
	
	
	// Health
	// -------------------------------------------------------------------------------------
	HealthControl hControl;
	bool alive = true;


	public void Health_init()
	{
		currentHealth 	= maxHealth;
		lastHit 		= Time.time;
		hControl		= (HealthControl)GameObject.Find( "Health" ).GetComponent<HealthControl>();
		hControl.updateHealth(currentHealth);
	}
	
	public float Health_GetCurrentHealth()
	{
		return currentHealth;
	}
	
	public void Health_DecHealth()
	{
		if( Time.time >= lastHit )
		{
			currentHealth -= 10f;
		}
	}
	
	public void Health_DecHealth( float amount )
	{
		if( Time.time >= lastHit )
		{
			currentHealth -= amount;
		}
	}
	
	public void Health_IncHealth( float amount )
	{
		if( currentHealth + amount < maxHealth )
		{
			currentHealth += amount;
		}
		else
		{
			currentHealth = maxHealth;
		}
		hControl.updateHealth( currentHealth );
	}
	
	/*
	public void Health_DefaultHit()
	{
		if(Time.time > lastHit)
		{
			Health_DecHealth();
			if( currentHealth <= 0 )
			{
				//DIE!
				StartCoroutine( "Die" );
			}
			else
			{
				lastHit = Time.time + 3;//Invincibility time	
				if( facingRight )
				{			
					this.transform.rigidbody2D.AddForce(new Vector2(30, 10) * 50);
				}
				else
				{
					this.transform.rigidbody2D.AddForce(new Vector2(-30, 10) * 50);
				}
			}
		}
	}
	*/
	
	public void Health_DefaultHit(Transform hitter)
	{
		if(Time.time > lastHit)
		{
			Health_DecHealth();
			StartCoroutine( "Health_DamageFlash" );			
			hControl.updateHealth( currentHealth );
			if( currentHealth <= 0 )
			{
				//DIE!
				this.transform.collider2D.isTrigger = true;
				StartCoroutine( "Die" );
			}
			else
			{
				lastHit = Time.time + 3;//Invincibility time
				this.transform.rigidbody2D.velocity = Vector3.zero;	
				if(this.transform.position.x > hitter.position.x)
				{
					this.transform.rigidbody2D.AddForce(new Vector2(30, 10) * 50);
				}
				else
				{
					this.transform.rigidbody2D.AddForce(new Vector2(-30, 10) * 50);
				}				
			}
		}
	}
	
	private int flashCount = 20;
	IEnumerator Health_DamageFlash()
	{
		bool colorSwitch = false;
		for(int i=0;i<flashCount;i++)
		{
			if(colorSwitch)
			{				
				this.transform.Find("body").renderer.material.color = Color.white;
			}
			else
			{				
				this.transform.Find("body").renderer.material.color = Color.red;
			}
			colorSwitch = !colorSwitch;
			yield return new WaitForSeconds(.10f);
		}
	}
	
	IEnumerator Die()
	{
		Debug.Log ( "Dying" );
		alive = false;
		yield return new WaitForSeconds(4f);
		/*
		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 35;
		GUI.color = Color.red;
		GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 ,Screen.width,Screen.height),"Game Over", textStyle);
		Debug.Log ( "Dead" );		
		yield return new WaitForSeconds(2f);
		*/
		Application.LoadLevel(Application.loadedLevel);
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
    
    public bool GetIsStopped()
    {
        return isStopped;
    }
    
    public void SetIsStopped(bool stopped)
    {
        isStopped = stopped;
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
        
        SetIsStopped( false );
		
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
	
	public void Animation_PlayAnimation(string animationName)
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
	
	
	// Sound    
    // -------------------------------------------------------------------------------------

    void Sound_Init()
    {
        try
        {
            playerBodyAudioSource = this.transform.Find("body").audio;
            if(!playerBodyAudioSource)
            {
                Debug.LogError("Init_Sound: could not find GameObject 'body'.");
            }
        }
        catch(UnityException ue)
        {
            Debug.Log ("Error with getting body: " + ue.ToString());
        } 
    }
    
    void scoochPoot()
    {   
        if( !playerBodyAudioSource.isPlaying )
        {
            playerBodyAudioSource.Play();              
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
            if( Util.IsObjectDebug( go ) )
			{
				Debug_DecFoodCount();
			}
			Destroy( go );
		}
	}
}
