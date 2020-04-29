using UnityEngine;
using System.Collections;
// using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour 
{
	// Player Control
	private Transform groundCheck;
	private Transform groundCheck2;

	private Vector3 lastPullBackPosition = Vector3.zero;
	private float lastAngle = 0.0f;
	private float lastAngleChange = 0f;
	private float angleChangeTimer = 0f;

	private bool canScooch	= true;
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
    
    // Hop  
    public float hopX                   = 1000f;
    public float hopY                   = 2800f;
    public bool canHop                  = true;
	
	// Particle System
	public ParticleSystem particleSystem;
	
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
    public FartAudioControl fartControl;

    // Food spawner
    public FoodSpawner foodSpawner;	
	
	public bool shouldDie = false;


	void Start()
	{
        if(pullLine == null)
        {
            Debug.LogError("PlayerControl PullLine is null! Set in Editor.");
        }
        amplifyBounceCount = 0;
        StatsManager.Instance.Init();
		// Debug.Log(string.Join("\n", Gamepad.all)); //Does not yet work apparently
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
        playerHealth.Init();
	}

	void Update () 
	{			
		angleChangeTimer += Time.deltaTime;
		if( shouldDie && isAlive) {
			StartDying();
		}

		// The player is on the ground if a linecast from the player to the groundCheck hits a block.
        int layerMask = Constants.LayerMask_Ground;
		onGround = Physics2D.Linecast( transform.position, groundCheck.position, layerMask ) | Physics2D.Linecast( transform.position, groundCheck2.position, layerMask );
		
		if( !onGround )
		{
			onGround = isStuck;
			// canHop = false;
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
			canScooch = true;
		}
		
        launchControl.UpdatePermission( onGround, bouncyBlockHitLast );

		foodSpawner.CheckSpawnFood();

		// Debug.Log("Horizontal: " + Input.GetAxis("Horizontal")); 
		// Debug.Log("AButton: " + Input.GetButton("AButton"));
		// Debug.Log("BButton: " + Input.GetButton("BButton"));
		// Debug.Log("StartButton: " + Input.GetButton("StartButton"));
		// Debug.Log("SelectButton: " + Input.GetButton("SelectButton"));

		// Debug.Log("HorizontalDpadButton: " + Input.GetAxis("HorizontalDpad"));
		// Debug.Log("VerticalDpadButton: " + Input.GetAxis("VerticalDpad"));

		float horizontal = Input.GetAxis("HorizontalDpad");
		Debug.Log("horizontal: " + horizontal + " onGround: " + onGround + " canScooch: " + canScooch);
		// if (horizontal == 0) { canScooch = true; }

		// if (canScooch && (Input.GetKeyDown ("d") || horizontal == 1.0)) 
		if (Input.GetButtonDown("BButton")) {
			Debug.Log("C: GetButtonDown BButton!");
			PullBackLauncher(Vector2.zero);
		}

		if (Input.GetButton("BButton") && (horizontal == 1 || horizontal == -1)) {
			Debug.Log("angleChangeTimer: " + angleChangeTimer);
			if (angleChangeTimer < 0.25f) {
				return; 
			} else {
				angleChangeTimer = 0f;
			}

			Debug.Log("C:GetButton BButton!");
			float radius = 3f;
			float angle = .09f;
			float newAngle = 0.0f;
			// var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
			// transform.position = _centre + offset;	
			if (horizontal == 1) { //Right
				newAngle = newAngle > 360 ? 0.0f : lastAngle + angle;

				// Vector3 newDirection = new Vector3(Mathf.Sin(newAngle), Mathf.Cos(newAngle), 0) * radius;
				// Vector2 newDirection = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
				// Debug.Log("C: New Direction: " + newDirection);
				// lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, transform.position + newDirection);
				// MovePullLineTo(lastPullBackPosition);
				// lastAngle = newAngle;
				// lastAngleChange = Time.deltaTime;
				// Debug.Log("C: lastPullback: " + lastPullBackPosition);
			} else { //Left
				newAngle = newAngle < 0 ? 360 - angle : lastAngle - angle;
				// Vector3 newDirection = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
				// Debug.Log("C: New Direction: " + newDirection);
				// lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, transform.position - newDirection);
				// MovePullLineTo(lastPullBackPosition);
				// Debug.Log("C: lastPullback: " + lastPullBackPosition);
			}

			Vector3 newDirection = new Vector3(Mathf.Sin(newAngle), Mathf.Cos(newAngle), 0) * radius;			lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, transform.position + newDirection);
			Debug.Log("C: New Direction: " + newDirection);
			MovePullLineTo(lastPullBackPosition);
			Debug.Log("C: lastPullback: " + lastPullBackPosition);
			lastAngle = newAngle;
			lastAngleChange = Time.deltaTime;

			return;
		}

		if (canScooch && horizontal == 1.0) {
			canScooch = false;
			// ScoochRight();
			Scooch(10, 50);
		} else if (canScooch && horizontal == -1.0) {
			canScooch = false;
			// ScoochLeft();
			Scooch(-10, 50);
		} else if (canHop && Input.GetButtonDown("AButton")) {
			canHop = false;
			Debug.Log("hopX: " + hopX + " hopY: " + hopY + " aplifyBounceCount: " + amplifyBounceCount);
			Hop(hopX, hopY);
			// Hop((hopX * 0.90f), (hopY * 0.90f));
			// Hop(1000f, 2800f);
		} else if (Input.GetButtonUp("BButton")) {
			Launch();
		}

		if (canScooch && Input.GetKeyDown ("d")) 
		{
			canScooch = false;
			ScoochRight();	
		} 
		else if (canScooch && Input.GetKeyDown ("a")) 
		{
			canScooch = false;
			ScoochLeft();
		}
		else if ( Input.GetKeyDown ("w"))
		{
			if ( canHop )
			{
				// canHop = false;
				
				// if ( onGround )
				// {
				// 	SetRespawn();				
				// }
				
				// this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;	
                // this.transform.GetComponent<Rigidbody2D>().AddForce ( new Vector2 ( playerAnimation.isFacingRight ? hopX : -hopX, hopY ) ); //hop!	
                // fartControl.PlayScoochPoot();
				// launchControl.DecrementCurrentJuice( 1 );
				Hop(hopX, hopY);
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
		isMoving = ( transform.GetComponent<Rigidbody2D>().velocity.sqrMagnitude >= 0.01f || transform.GetComponent<Rigidbody2D>().angularVelocity >= 0.01f );
		
		if(!onGround && GetComponent<Collider2D>().transform.parent != null)
		{			
			this.GetComponent<Collider2D>().transform.parent = null;
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

	public void Launch() {
		foreach( GameObject go in GameObject.FindGameObjectsWithTag( "TrajectoryDot" ))
		{
			Destroy( go );
		}

		this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		
		SetRespawn();
		launchControl.Launch( transform );

		launchControl.Reset();
		pullLine.Reset();
        trajectoryDots.Reset();
	}
	
	void OnMouseUp()
	{
		// foreach( GameObject go in GameObject.FindGameObjectsWithTag( "TrajectoryDot" ))
		// {
		// 	Destroy( go );
		// }

		// //added for sticky block testing
		// if(this.transform.GetComponent<Rigidbody2D>().gravityScale == 0) 
		// {
		// 	this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		// }
		
		// SetRespawn();
		// launchControl.Launch( transform );

		// launchControl.Reset();
		// pullLine.Reset();
        // trajectoryDots.Reset();
		Launch();
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

			GUI.Label(new Rect(700, 200, Screen.width, Screen.height), "Game Over", textStyle);


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
                StatsManager.Instance.AddPickUp(obj.gameObject.name);
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
	
	public void StartDying()
    {
        this.transform.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine( Die() );
    }	
	
	private IEnumerator Die()
	{
		//Debug.Log ( "Dying" );		
		PlayerPrefs.SetInt( "died", 1 );
        PlayerPrefs.SetInt ("currentLevel", Application.loadedLevel);
        StatsManager.Instance.StopLevelTime();
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

	public void Hop(float x, float y) {
		canHop = false;
				
		if ( onGround )
		{
			SetRespawn();				
		}
		Rigidbody2D rb = this.transform.GetComponent<Rigidbody2D>();
		// this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		// this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;	
		// this.transform.GetComponent<Rigidbody2D>().AddForce ( new Vector2 ( playerAnimation.isFacingRight ? hopX : -hopX, hopY ) ); //hop!	
		rb.gravityScale = 1;
		rb.velocity = Vector2.zero;	
		rb.AddForce ( new Vector2 ( playerAnimation.isFacingRight ? x : -x, y ) ); //hop!	
		fartControl.PlayScoochPoot();
		launchControl.DecrementCurrentJuice( 1 );
	}

	public void MovePullLineTo(Vector3 direction) {
		pullLine.PositionClouds( transform.position, direction );
        trajectoryDots.Position( transform.position, direction );
	}

	public void PullBackLauncher(Vector3 positionModifier) {
		float startPos = movingDir == Direction.RIGHT ? -5f : 5f;
		Vector3 direction = new Vector3((transform.position.x + startPos), transform.position.y, 0);
		lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, direction);
		MovePullLineTo(direction);
		Debug.Log("C: startPos: " + startPos + " lastPullBack: " + lastPullBackPosition + " direction: " + direction + " playerDir: " + movingDir);

		// Debug.Log("Position: " + transform.position);
		// Debug.Log("Direction: " + pullLine.GetDirection(transform.position));
		// Debug.Log("Magnitude: " + direction.magnitude);
	}

	public void Scooch(int x, int y) {
		//added for sticky block testing
		// if (this.transform.GetComponent<Rigidbody2D>().gravityScale == 0) 
		// {
		// 	this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		// }
		this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		SetRespawn();
		this.transform.GetComponent<Rigidbody2D>().AddForce (new Vector2 (x, y));//scooch!
		fartControl.PlayScoochPoot();
	}

	public void ScoochRight() {
		// //added for sticky block testing
		// if (this.transform.GetComponent<Rigidbody2D>().gravityScale == 0) 
		// {
		// 	this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		// }			
		
		// SetRespawn();
		// this.transform.GetComponent<Rigidbody2D>().AddForce (new Vector2 (100, 500));//scooch!
		// fartControl.PlayScoochPoot();
		Debug.Log("Scooching right.");
		Scooch(100, 500);	
	}

	public void ScoochLeft() {
		Debug.Log("Scooching left.");
		Scooch(-100, 500);
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
}
