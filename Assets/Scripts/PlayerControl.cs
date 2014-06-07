using UnityEngine;
using System.Collections;
using System.Timers; 

public class PlayerControl : MonoBehaviour 
{
	// Player Control
	private Transform groundCheck;
	private Transform groundCheck2;
	private bool onGround	= false;
	private bool isMoving	= false;
	private bool isStuck	= false;
    private bool inVortex 	= false;
    private bool isStopped  = false;
    private bool isEating	= false;

	// Pull Line	
	public float maxLineLength = 1.5f;
	public float minLineLength = 0.6f;	

	private const int maxLineVerts		= 2;
	private const int maxFartClouds		= 6;
	private const int maxTrajectoryDots = 6;
	private Transform[] fartClouds;
	private Transform[] trajectoryDots;
	private float pullFraction			= 0.0f;
	private float pullDist				= 0.0f;
	private float juiceToUse			= 0.0f;

	// Launch
	public float maxLaunchForce			= 4500.0f;
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
    private AudioHandler fartSource;
    
    //Trajectory
    public GameObject trajectoryDot;

	// Debug
	public GameObject debugSpawnFoodObj;

	private int maxDebugFood			= 3;
	private int debugFoodCount			= 0;
	private float debugFoodDestroyTime	= 3.0f;
	
	// Zoom
	public bool zoomOn					= false;
	public int maxZoom					= 5;
	public int minZoom					= 4;
	
	// Hop	
	public float hopX 					= 1000f;
	public float hopY 					= 2800f;
	public bool canHop					= true;

	//DB counts
	public static int puCount			= 0;
	public static Timer levelTime;
	public static int playTime = 0;

	void Start()
	{
		levelTime = new System.Timers.Timer(1000);
		resetCount ();
		levelTime.Elapsed += new ElapsedEventHandler(OnTimedEvent);
		levelTime.Enabled = true;
		levelTime.Start ();
	}

	private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
	{
		playTime++;
		Debug.Log ("Tic: "+ playTime);
	}
	void Awake()
	{
		groundCheck = transform.Find( "groundCheck" );
		groundCheck2 = transform.Find( "groundCheck2" );
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
		onGround = Physics2D.Linecast( transform.position, groundCheck.position, layerMask ) | Physics2D.Linecast( transform.position, groundCheck2.position, layerMask );
		
		if( !onGround )
		{
			onGround = isStuck;
		}
		else
		{
			canHop = true;		
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
		
		if (Input.GetKeyDown ("d") && onGround) 
		{
			//added for sticky block testing
			if (this.transform.rigidbody2D.gravityScale == 0) {
					this.transform.rigidbody2D.gravityScale = 1;
			}
			this.transform.rigidbody2D.AddForce (new Vector2 (100, 500));//scooch!
			scoochPoot ();		
		} 
		else if (Input.GetKeyDown ("a") && onGround) 
		{
			//added for sticky block testing
			if (this.transform.rigidbody2D.gravityScale == 0) {
					this.transform.rigidbody2D.gravityScale = 1;
			}
			this.transform.rigidbody2D.AddForce (new Vector2 (-100, 500));//scooch!
			scoochPoot ();
		}
		else if ( Input.GetKeyDown ("w") )
		{
			if ( canHop )
			{
				Debug.Log ( "velocity: " + this.transform.rigidbody2D.velocity.magnitude );
				canHop = false;
				
				this.transform.rigidbody2D.velocity = Vector2.zero;	
				this.transform.rigidbody2D.AddForce ( new Vector2 ( Animation_GetFacingRight() ? hopX : -hopX, hopY ) );//hop!	
				fartSource.PlayClip(Random.Range( 0, fartSource.farts.Length ));
				currentLaunchJuice -= 1;
								
			}
		}
		//Added to test dying transition menu
		else if (Input.GetKeyDown ("x")) 
		{
			Health_DecHealth( 100.0f );
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
			transform.parent = null;
			
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
		foreach( GameObject go in GameObject.FindGameObjectsWithTag( "TrajectoryDot" ))
		{
			Destroy( go );
		}
		//added for sticky block testing
		if(this.transform.rigidbody2D.gravityScale == 0) 
		{
			this.transform.rigidbody2D.gravityScale = 1;
		}
		
		Launch( transform );

		Launch_Reset();
		PullLine_Reset();
		
		//Zooming in and out		
		if( zoomOn )
		{
			StopCoroutine("zoomOut");
			StartCoroutine("zoomIn");		
		}
		
	}
	
	void OnMouseDown()
	{
		//Zooming in and out
		if( zoomOn )
		{	
			StopCoroutine("zoomIn");				
			StartCoroutine("zoomOut");
		}		
	}

	void OnMouseDrag()
	{
		Vector3 pullEndPoint = PullLine_GetEndPoint( transform.position );
		PullLine_PositionClouds( transform.position, pullEndPoint );
		PullLine_PositionTrajectoryDots( transform.position, pullEndPoint );
	}
	
	//public TextAsset txtAsst;
	void OnGUI()
	{
		if(!alive)
		{			
			GUI.color = Color.red;// Not working at the moment...
			GUIStyle textStyle = new GUIStyle();
			textStyle.fontSize = 80;
			textStyle.fontStyle = FontStyle.Bold;
			GUI.Label(new Rect(700,200 ,Screen.width,Screen.height),"Game Over", textStyle);


			//GUI.Label (new Rect (750, 250, 300, 50), "GAME OVER");
			GUI.Box (new Rect (700, 300, 300, 200), "");
			
			
			if (GUI.Button (new Rect (720, 320, 100, 50), "Restart Level")) {
				Debug.Log ("Load Level: " + Application.loadedLevelName);
				Application.LoadLevel (PlayerPrefs.GetInt ("currentLevel"));
				Time.timeScale = 1;
			}
			
			if (GUI.Button (new Rect (720, 380, 100, 50), "Return to Main menu")) {
				//Debug.Log (Util.getlevel);
				Application.LoadLevel ("test_menu_Nick");
				Time.timeScale = 1;
			}
			
			if (GUI.Button (new Rect (720, 440, 100, 50), "Quit Game")) {
				Application.Quit ();
			}

			GUI.Label (new Rect(700, 500, Screen.width, Screen.height), "You have died " + DBFunctions.getTimesDied () + " Times.");

		}
		//Just a temp spot for health
		//Texture2D healthBubble = new Texture2D(32, 32);
		//healthBubble.LoadImage(txtAsst.bytes);
		//GUI.Label(new Rect(0,0,Screen.width,Screen.height),currentHealth.ToString());
	}
	
	//Testing for prewall collision detection
	void OnTriggerEnter2D(Collider2D obj)
	{	
		if (obj.gameObject.tag.Equals ("PickUp")) 
		{
			CollectFood pickUp = obj.GetComponent<CollectFood>();
			if(!pickUp.getCheck ())
			{	
				pickUp.Check();
				puCount ++;
				Debug.Log ("PU count = " + puCount);
			}
		}
				
		if(obj.gameObject.tag.Equals( "Block" ))
		{
			willHit = true;
		}
		else
		{
			willHit = false;
		}
	}
	
	// Not sure if this should go here or in a different script file?
	// Camera Zoom
	// -------------------------------------------------------------------------------------
	
	IEnumerator zoomOut()
	{		
		while(Camera.main.orthographicSize <= maxZoom)
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,maxZoom + 1, Time.deltaTime * 1.25f);						
			yield return new WaitForSeconds(.025f);
		}		
		yield break;		
	}
	
	IEnumerator zoomIn()
	{
		yield return new WaitForSeconds(1.25f);
		while(Camera.main.orthographicSize > minZoom)
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,minZoom, Time.deltaTime * .2f);
			//Could not figure this out so I just added this crap.  Needs to be redone.
			if(Camera.main.orthographicSize <= .31)
			{
				Camera.main.orthographicSize = minZoom;
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
	
	public void Health_KillPlayer ()
	{
		currentHealth = 0;
		hControl.updateHealth ( 0 );
		this.transform.collider2D.isTrigger = true;
		StartCoroutine( "Die" );
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
		levelTime.Stop ();
		PlayerPrefs.SetInt ("currentLevel", Application.loadedLevel);
		alive = false;
		DBFunctions.updateTimesDied (1);
		Time.timeScale = 0;
		yield return new WaitForSeconds(4f);
		/*
		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 35;
		GUI.color = Color.red;
		GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 ,Screen.width,Screen.height),"Game Over", textStyle);
		Debug.Log ( "Dead" );		
		yield return new WaitForSeconds(2f);
		*/
		//Application.LoadLevel(Application.loadedLevel);

		PlayerPrefs.SetString ("death", "Dead");

		Debug.Log ("loaded level" + Util.getlevel());
		//Application.LoadLevel ("test_death_menu_Nick");//Opens Death menu
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
    
    public void SetIsEating(bool eating)
    {
    	isEating = eating;
    }
    
    public bool GetIsEating()
    {
    	return isEating;
    }

	// Pull Line Control
	// -------------------------------------------------------------------------------------

	public void PullLine_Init( Transform myTransform )
	{
		pullFraction	= 0.0f;
		juiceToUse		= 0.0f;
		pullDist		= 0.0f;
		fartClouds		= null;
		trajectoryDots	= null;
		dotTime 		= Time.time + dotDelay;
		
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "TrajectoryDot" ), true);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer( "Enemies" ), LayerMask.NameToLayer( "TrajectoryDot" ), true);

		Transform pullContainer 		= myTransform.FindChild( "pullContainer" );
		Transform trajectoryContainer 	= myTransform.FindChild( "trajectoryDotContainer" );
		// TODO: Change this to assert
		if( pullContainer )
		{
			fartClouds = new Transform[ maxFartClouds ];
			for( int cloudIndex = 0; cloudIndex < maxFartClouds; ++cloudIndex )
			{
				fartClouds[ cloudIndex ] = pullContainer.FindChild( "FartCloud" + cloudIndex ); 
			}
			
			trajectoryDots = new Transform[ maxTrajectoryDots ];
			for( int dotIndex = 0; dotIndex < maxTrajectoryDots; ++dotIndex )
			{
				trajectoryDots[ dotIndex ] = trajectoryContainer.FindChild( "TrajectoryDot" + dotIndex ); 
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
		
		for( int dotIndex = 0; dotIndex < maxTrajectoryDots; ++dotIndex )
		{
			trajectoryDots[ dotIndex ].transform.position = transform.position; 
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
	
	private float dotDelay = .5f;
	private float dotTime;
	public void PullLine_PositionTrajectoryDots( Vector3 playerPos, Vector3 pullEndPoint )
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
		}*/
		
		if( Time.time >= dotTime )
		{
			PullLine_LaunchTrajectoryDot();
		}
	}
	
	public void PullLine_LaunchTrajectoryDot()
	{		
		dotTime = Time.time + dotDelay;
		float pullPercent = PullLine_GetFraction();
		float launchForce = minLaunchForce + ( ( maxLaunchForce - minLaunchForce ) * pullPercent );
		GameObject newDot = (GameObject)Instantiate( trajectoryDot, this.transform.position, Quaternion.identity );
		newDot.transform.rigidbody2D.AddForce( launchForce * launchDir );
		StartCoroutine ( Destroy_Now( newDot, 1f ) );
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
		
		this.collider2D.transform.parent = null;

		currentLaunchJuice -= juiceToUse;
		juiceToUse = 0.0f;

		Launch_SetAllowed( false );

		// TODO: assert if not transform.rigidbody2D
		transform.rigidbody2D.AddForce( launchDir * launchForce );	
		
		
		if(launchDir != Vector2.zero)
		{
			AudioSource[] farts = this.gameObject.GetComponents<AudioSource>();			
			farts[(int)Random.Range(0, farts.Length)].Play ();
		}
	}


	bool willHit = false;
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
		
			/*
			if( inVortex )
			{
				playerAnimator.Play ("Vortex");
			}
			else if( isMoving && !isHolding )
			{
				playerAnimator.Play ( "flying" );	
			}
			else if( isHolding )
			{
				playerAnimator.Play( "HoldingItIn" );
			}
			else if( onGround )
			{
				playerAnimator.Play( "Idle" );
			}	
			*/
		
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
            fartSource = (AudioHandler)GameObject.Find("Fart_Audio_Source").GetComponent<AudioHandler>();            
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
	//DB Counts
	//--------------------------------------------------------------------------------------------
	public static void resetCount()
	{	
		puCount = 0;
		playTime = 0;
	}
	public static int[] getPlayTime()
	{
		int[] lvlTimes = new int[2];
		lvlTimes [0] = (playTime / 60);
		lvlTimes [1] = (playTime % 60);

		return lvlTimes;
	}


}
