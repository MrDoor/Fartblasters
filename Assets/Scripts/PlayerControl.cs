using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private bool isAlive    = true;
    private bool willHit    = false;

    private bool bouncyBlockHitLast = false;
    private float lastXPos          = 0.0f;
    private Direction movingDir     = Direction.RIGHT;
        
    public int amplifyBounceCount = 0;

	// Pull Line
    public PullLine pullLine;

    // Trajectory Dots
    public TrajectoryDots trajectoryDots;

	// Launch Control
    public LaunchControl launchControl;
	
	// Health
    public PlayerHealth playerHealth;

	// Animation
    public PlayerAnimation playerAnimation;

    // Sounds
    private AudioSource playerBodyAudioSource;
    private AudioHandler fartSource;

    // Food spawner
    public FoodSpawner foodSpawner;
	
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
	public static int playTime 			= 0;
	public static IDictionary<string, int> pUps = new Dictionary<string, int>();

	void Start()
	{
        if(pullLine == null)
        {
            Debug.LogError("PlayerControl PullLine is null! Set in Editor.");
        }
        amplifyBounceCount = 0;
		//levelTime = new System.Timers.Timer(1000);
		levelTime = new Timer( 1000 );
		resetCount ();
		levelTime.Elapsed += new ElapsedEventHandler(OnTimedEvent);
		levelTime.Enabled = true;
		levelTime.Start ();
	}

	private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
	{
		playTime++;
	}
	
	void Awake()
	{
		groundCheck = transform.Find( "groundCheck" );
        groundCheck2 = transform.Find( "groundCheck2" );
        
        isStuck = false;
        isStopped = false;

		pullLine.Init();
        trajectoryDots.Init();
		launchControl.Init();
        playerAnimation.Init();
        Sound_Init();
        playerHealth.Init();
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
        layerMask = Constants.LAYER_MASK_BOUNCY_BLOCK; 
		if( Physics2D.Linecast( transform.position, groundCheck.position, layerMask ) )
		{
			bouncyBlockHitLast = true;
		}
		else if( onGround )
		{
			bouncyBlockHitLast = false;
            amplifyBounceCount = 0;
		}
		
        launchControl.UpdatePermission( onGround, bouncyBlockHitLast );

		foodSpawner.CheckSpawnFood();
		
		if (Input.GetKeyDown ("d") && onGround) 
		{
			//added for sticky block testing
			if (this.transform.rigidbody2D.gravityScale == 0) 
            {
                this.transform.rigidbody2D.gravityScale = 1;
			}			
			
			SetRespawn();
			this.transform.rigidbody2D.AddForce (new Vector2 (100, 500));//scooch!
			scoochPoot ();		
		} 
		else if (Input.GetKeyDown ("a") && onGround) 
		{
			//added for sticky block testing
			if (this.transform.rigidbody2D.gravityScale == 0) 
            {
				this.transform.rigidbody2D.gravityScale = 1;
			}
			this.transform.rigidbody2D.AddForce (new Vector2 (-100, 500));//scooch!
			scoochPoot ();
		}
		else if ( Input.GetKeyDown ("w") )
		{
			if ( canHop )
			{
				canHop = false;
				
				if ( onGround )
				{
					SetRespawn();				
				}
				
				this.transform.rigidbody2D.velocity = Vector2.zero;	
                this.transform.rigidbody2D.AddForce ( new Vector2 ( playerAnimation.isFacingRight ? hopX : -hopX, hopY ) ); //hop!	
				fartSource.PlayClip(Random.Range( 0, fartSource.farts.Length ));
				launchControl.DecrementCurrentJuice( 1 );
								
			}
		}
		//Added to test dying transition menu
		else if (Input.GetKeyDown ("x")) 
		{
			playerHealth.DecHealth( 100.0f );
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
		
		SetRespawn();
		launchControl.Launch( transform );

		launchControl.Reset();
		pullLine.Reset();
        trajectoryDots.Reset();
		
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
        Vector3 pullEndPoint = pullLine.GetEndPoint( transform.position );
		pullLine.PositionClouds( transform.position, pullEndPoint );
        trajectoryDots.Position( transform.position, pullEndPoint );
	}
	
	//public TextAsset txtAsst;
	void OnGUI()
	{
		if(!isAlive)
		{			
			GUIStyle textStyle = new GUIStyle();
			textStyle.normal.textColor = Color.red;
			textStyle.fontSize = 80;
			textStyle.fontStyle = FontStyle.Bold;

			GUI.Label(new Rect(700,200 ,Screen.width,Screen.height),"Game Over", textStyle);


			//GUI.Label (new Rect (750, 250, 300, 50), "GAME OVER");
			GUI.Box (new Rect (810, 300, 200, 200), "");
			

			if (GUI.Button (new Rect (860, 315, 100, 50), "Restart Level")) {
				Debug.Log ("Load Level: " + Application.loadedLevelName);
			
			//if (GUI.Button (new Rect (720, 320, 100, 50), "Restart Level")) {
				//Debug.Log ("Load Level: " + Application.loadedLevelName);

				Application.LoadLevel (PlayerPrefs.GetInt ("currentLevel"));
				Time.timeScale = 1;
			}
			
			if (GUI.Button (new Rect (860, 375, 100, 50), "Return to \nMain Menu")) {
				//Debug.Log (Util.getlevel);
				Application.LoadLevel ("test_menu_Nick");
				Time.timeScale = 1;
			}
			
			if (GUI.Button (new Rect (860, 435, 100, 50), "Quit Game")) {
				Application.Quit ();
			}

			GUI.Label (new Rect(850, 500, Screen.width, Screen.height), "You have died " + DBFunctions.getTimesDied () + " Times.");

		}
		//Just a temp spot for health
		//Texture2D healthBubble = new Texture2D(32, 32);
		//healthBubble.LoadImage(txtAsst.bytes);
		//GUI.Label(new Rect(0,0,Screen.width,Screen.height),currentHealth.ToString());
	}
	
	//Testing for prewall collision detection
	void OnTriggerEnter2D( Collider2D obj )
	{	
		if( obj.gameObject.tag.Equals("PickUp") ) 
		{
			CollectFood pickUp = obj.GetComponent<CollectFood>();

			if( !pickUp.getCheck() )
			{	
				pickUp.Check();
				puCount ++;

				if( pUps.ContainsKey( obj.gameObject.name ) )
                {
					pUps[obj.gameObject.name] += 1;
                }
				else
                {
					pUps.Add( obj.gameObject.name, 1 );
                }
				Debug.Log( "Name : " + obj.gameObject.name );
			}
		}
				
		if( obj.gameObject.tag.Equals( "Block" ) )
		{
			willHit = true;
		}
		else
		{
			willHit = false;
		}
	}

    void OnCollisionEnter2D( Collision2D coll )
    {
        if( coll.gameObject.layer != Constants.LAYER_INDEX_BOUNCY_BLOCK )
        {
            amplifyBounceCount = 0;
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
	
	public void StartDying()
    {
        this.transform.collider2D.isTrigger = true;
        StartCoroutine( Die() );
    }	
	
	private IEnumerator Die()
	{
		//Debug.Log ( "Dying" );		
		PlayerPrefs.SetInt( "died", 1 );
		levelTime.Stop ();
		PlayerPrefs.SetInt ("currentLevel", Application.loadedLevel);
		isAlive = false;
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

		//Debug.Log ("loaded level" + Util.getlevel());
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

	public void SetRespawn()
	{
		PlayerPrefs.SetFloat ( "deathSpotX", this.transform.position.x );
		PlayerPrefs.SetFloat ( "deathSpotY", this.transform.position.y );
		Debug.Log ( "Respawn Spot: " + this.transform.position.x + ", " + this.transform.position.y );
	}
	
	public void SetLastXPos( float lastX )
    {
        lastXPos = lastX;
    }

    public Direction GetMovingDir()
    {
        return movingDir;
    }

	// Sound    
    // -------------------------------------------------------------------------------------

    public void Sound_Init()
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
    
    public void scoochPoot()
    {   
        if( !playerBodyAudioSource.isPlaying )
        {
            playerBodyAudioSource.Play();              
        }  
    }

    public AudioSource[] GetAudioSources()
    {
        return this.gameObject.GetComponents<AudioSource>();
    }
	

	//DB Counts
	//--------------------------------------------------------------------------------------------
	public static void resetCount()
	{	
		puCount = 0;
		playTime = 0;
		pUps.Clear ();
	}
	public static int[] getPlayTime()
	{
		int[] lvlTimes = new int[2];
		lvlTimes [0] = (playTime / 60);
		lvlTimes [1] = (playTime % 60);

		return lvlTimes;
	}


}
