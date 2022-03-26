using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Player Control
    private Transform groundCheck;
    private Transform groundCheck2;
    PlayerControls controls;

    // Controller Support
    public bool inClaw;
    public Vector3 lastPullBackPosition = Vector3.zero;
    private float lastAngle = 0.0f;
    private float angleChangeTimer = 0f;
    private bool initialPullBackSet = false;
    float pullBackRadius = 1.5f;
    float pullBackAngle = .18f;

    private bool canScooch = true;
    private bool onGround = false;
    private bool isMoving = false;
    private bool isStuck = false;
    private bool inVortex = false;
    private bool isStopped = false;
    private bool isEating = false;
    private bool isAlive = true;
    private bool willHit = false;

    private bool bouncyBlockHitLast = false;
    private float lastXPos = 0.0f;
    private Direction movingDir = Direction.RIGHT;

    public int amplifyBounceCount = 0;

    // New Input Action Controller Support -------------
    private bool controllsInitialized = false;

    private bool rightPressed = false;
    private bool leftPressed = false;
    private bool inLaunchMode = false;
    private bool upPressed = false;
    private bool downPressed = false;

    private float scoochTimeLimit = 0.1f;
    private float nextScoochTimer = 0f;

    private bool leftMouseDown = false;

    private bool inTouchLaunchMode = false;
    private bool touchHeld = false;
    private Vector2 touchPosition;
    private Vector2 startTouchPosition;

    // -------------------------------------------------

    // Hop  
    public float hopX = 1000f;
    public float hopY = 2300f;
    public bool canHop = true;

    // Particle System
    public ParticleSystem fartParticles;

    // Pull Line
    public PullLine pullLine;

    // Trajectory Dots
    public TrajectoryDots trajectoryDots;

    // Launch Control
    public LaunchControl launchControl;
    public bool shouldRegenerateFartJuice;
    public int regenerationTrigger;
    public int regenerateLevel;
    public float regenerationPerTick;
    private bool isRegenerating = false;

    // Health
    public PlayerHealth playerHealth;

    // Animation
    public PlayerAnimation playerAnimation;

    // Sounds
    public FartAudioControl fartControl;

    // Food spawner
    public FoodSpawner foodSpawner;

    public bool shouldDie = false;

    // Spawn Support
    private float safeSpotTimer = 0.0f;
    private float safeSpotLimit = 0.5f;


    void Start()
    {
        if (pullLine == null)
        {
            Debug.LogError("PlayerControl PullLine is null! Set in Editor.");
        }
        amplifyBounceCount = 0;
        StatsManager.Instance.Init();
    }

    void Awake()
    {
        groundCheck = transform.Find("groundCheck");
        groundCheck2 = transform.Find("groundCheck2");

        isStuck = false;
        isStopped = false;

        pullLine.Init();
        trajectoryDots.Init();
        launchControl.Init();
        playerAnimation.Init();
        playerHealth.Init();

        // NEW INPUT SYSTEM
        initializeControlSupport();
        initializeTouchSupport();

        Init();
    }

    // void OnEnable()
    // {
    //     TouchSimulation.Enable();
    // }

    private void initializeControlSupport()
    {
        // B Button = South
        controls = GetControls();

        // Controller
        controls.Gameplay.ScoochLeft.started += ctx => leftPressed = true;
        controls.Gameplay.ScoochLeft.canceled += ctx => leftPressed = false;

        controls.Gameplay.ScoochRight.started += ctx => rightPressed = true;
        controls.Gameplay.ScoochRight.canceled += ctx => rightPressed = false;

        controls.Gameplay.Launch.started += ctx => inLaunchMode = true;
        controls.Gameplay.Launch.canceled += ctx => { inLaunchMode = false; DoLaunch(); };

        controls.Gameplay.Up.started += ctx => upPressed = true;
        controls.Gameplay.Up.canceled += ctx => upPressed = false;

        controls.Gameplay.Down.started += ctx => downPressed = true;
        controls.Gameplay.Down.canceled += ctx => downPressed = false;

        controls.Gameplay.Hop.performed += DoHop;

        // Mouse
        controls.Gameplay.LeftMouseClick.started += ctx => { leftMouseDown = true; };
        controls.Gameplay.LeftMouseClick.canceled += ctx => { leftMouseDown = false; OnMouseUp(); };

        // Touchscreen
        // controls.Gameplay.TouchOccurred.performed += ctx => { touchPosition = ctx.ReadValue<Vector2>(); };

        // var hopAction = new InputAction("Hop");
        // hopAction.AddBinding("<Gamepad>/dPadUp").WithInteraction("press;pressOnly");

        // hopAction.performed += DoHop;


        // controls.Gameplay.TouchOccurred.performed += ctx => Debug.Log($"Touched: {touchPosition.x}, {touchPosition.y}");
        // controls.Gameplay.LeftMouseClick.canceled += ctx => Debug.Log("Mouse up");

        //  void Awake()
        //     {
        //         controller = new TestController();
        //         controller.Player.TouchPoint.performed += x => CallSomething(x.ReadValue<Vector2>());
        //     }
        //     private void CallSomething(Vector2 touch) {
        //         Debug.Log($"Touch Screen Position: {touch}");
        //         var world = Camera.main.ScreenToWorldPoint(touch);
        //         Debug.Log($"Touch World Position: {world}");
        //         Debug.DrawLine(world,world + Vector3.one,Color.magenta,5f);
        //     }

        // controls.Enable();able();

        controllsInitialized = true;
        Debug.Log("Controller has been initialized.");
    }

    private void initializeTouchSupport()
    {
        // controls = GetControls();
        EnsureControls();

        // controls.Touch.TouchPress.started += ctx => { Debug.Log("Touch occurred."); touchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>()); handleTouchInput(); };
        controls.Touch.TouchPress.performed += ctx => { Debug.Log("Touch occurred."); touchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>()); handleTouchPressInput(); };

        controls.Touch.TouchHold.started += ctx =>
        {
            Debug.Log("Hold occurred.");
            touchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>());
            startTouchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>());
            touchHeld = true; handleTouchHoldInput();
        };
        controls.Touch.TouchHold.canceled += ctx => { Debug.Log("Hold stopped."); touchHeld = false; handleTouchLaunch(); };

        // controls.Touch.TouchHold.performed += ctx => { Debug.Log("Hold occurred."); touchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>()); touchHeld = true; };

        touchPosition = new Vector2(transform.position.x, transform.position.y);
    }

    private void Init()
    {
        shouldRegenerateFartJuice = true;
        regenerationTrigger = regenerationTrigger <= 0 ? 5 : regenerationTrigger;
        regenerateLevel = regenerateLevel <= 0 ? 20 : regenerateLevel;
        regenerationPerTick = regenerationPerTick <= 0 ? .7f : regenerationPerTick;

        // Debug.Log("Fart Particles: " + fartParticles);
        if (fartParticles == null)
        {
            ParticleSystem ps = transform.GetComponentInChildren<ParticleSystem>();
            if (ps == null)
            {
                // Debug.Log("Instantiating Fart Particles");
                GameObject fp = Instantiate(Resources.Load("Prefabs/fartParticles")) as GameObject;
                ps = fp.GetComponent<ParticleSystem>();
            }
            fartParticles = ps;
            // Debug.Log("Fart Particles After Null: " + fartParticles);
        }
    }

    void Update()
    {
        if (!controllsInitialized)
        {
            initializeControlSupport();
        }

        if (touchHeld)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>());
        }

        if (touchHeld && canScooch && !inLaunchMode)
        {
            handleTouchHoldInput();
        }

        Debug.Log($"touchHeld: {touchHeld} inTouchLaunchMode: {inTouchLaunchMode}");
        if (touchHeld && inTouchLaunchMode)
        {
            handleTouchLaunchMode();
        }

        if (touchPosition != null)
        {
            Debug.Log($"touchPosition: {touchPosition.x}, {touchPosition.y}");
        }
        else
        {
            Debug.Log("no touch position set.");
        }

        safeSpotTimer += Time.deltaTime;
        if (safeSpotTimer > safeSpotLimit && onGround)
        {
            setSafeSpot();
        }

        if (leftMouseDown)
        {
            OnMouseDrag();
        }

        angleChangeTimer += Time.deltaTime;
        nextScoochTimer += Time.deltaTime;
        if (shouldDie && isAlive)
        {
            StartDying();
        }

        if (shouldRegenerateFartJuice)
        {
            RegenerateFartJuiceTo(regenerateLevel);
        }

        // The player is on the ground if a linecast from the player to the groundCheck hits a block.
        int layerMask = Constants.LayerMask_Ground;
        onGround = Physics2D.Linecast(transform.position, groundCheck.position, layerMask) | Physics2D.Linecast(transform.position, groundCheck2.position, layerMask);

        if (!onGround)
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
        if (Physics2D.Linecast(transform.position, groundCheck.position, layerMask))
        {
            bouncyBlockHitLast = true;
        }
        else if (onGround)
        {
            bouncyBlockHitLast = false;
            amplifyBounceCount = 0;
            canScooch = true;
        }

        launchControl.UpdatePermission(onGround, bouncyBlockHitLast);

        foodSpawner.CheckSpawnFood();

        // Checking for controller/keyboard input ===============================================================


        if (inLaunchMode)
        {
            if (handleLaunchMode())
            {
                return;
            }
        }

        // Debug.Log($"shouldScoochLeft: {leftPressed} canScooch: {canScooch} isOnGround: {onGround}");
        if (leftPressed && canScooch && !inLaunchMode)
        {
            DoScoochLeft();
        }
        // Debug.Log($"shouldScoochRight: {rightPressed} canScooch: {canScooch} isOnGround: {onGround}");
        if (rightPressed && canScooch && !inLaunchMode)
        {
            DoScoochRight();
        }

        //Added to test dying transition menu
        // else if (Input.GetKeyDown("x"))
        // {
        //     playerHealth.DecHealth(100.0f);
        // }
    }

    void FixedUpdate()
    {
        isMoving = (transform.GetComponent<Rigidbody2D>().velocity.sqrMagnitude >= 0.01f || transform.GetComponent<Rigidbody2D>().angularVelocity >= 0.01f);

        if (!onGround && GetComponent<Collider2D>().transform.parent != null)
        {
            this.GetComponent<Collider2D>().transform.parent = null;
        }

        if (isMoving)
        {
            transform.parent = null;

            if (lastXPos < transform.position.x)
            {
                movingDir = Direction.RIGHT;
            }
            else if (lastXPos > transform.position.x)
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

    public void Launch()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("TrajectoryDot"))
        {
            Destroy(go);
        }

        this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;

        SetRespawn();
        launchControl.Launch(transform);

        ResetLaunch();
    }

    public void ResetLaunch()
    {
        launchControl.Reset();
        pullLine.Reset();
        trajectoryDots.Reset();
    }

    void OnMouseUp()
    {
        Launch();
    }

    void OnMouseDrag()
    {
        // Debug.Log("Dragging...");
        Vector3 pullEndPoint = pullLine.GetEndPoint(transform.position);
        // Debug.Log($"pullEndPoint: {pullEndPoint}");
        pullLine.PositionClouds(transform.position, pullEndPoint);
        trajectoryDots.Position(transform.position, pullEndPoint);
    }

    //public TextAsset txtAsst;
    void OnGUI()
    {
        if (!isAlive)
        {
            GUIStyle textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.red;
            textStyle.fontSize = 80;
            textStyle.fontStyle = FontStyle.Bold;

            GUI.Label(new Rect(700, 200, Screen.width, Screen.height), "Game Over", textStyle);


            //GUI.Label (new Rect (750, 250, 300, 50), "GAME OVER");
            GUI.Box(new Rect(810, 300, 200, 200), "");


            if (GUI.Button(new Rect(860, 315, 100, 50), "Restart Level"))
            {
                Debug.Log("Load Level: " + Application.loadedLevelName);

                //if (GUI.Button (new Rect (720, 320, 100, 50), "Restart Level")) {
                //Debug.Log ("Load Level: " + Application.loadedLevelName);

                Application.LoadLevel(PlayerPrefs.GetInt("currentLevel"));
                Time.timeScale = 1;
            }

            if (GUI.Button(new Rect(860, 375, 100, 50), "Return to \nMain Menu"))
            {
                //Debug.Log (Util.getlevel);
                Application.LoadLevel("test_menu_Nick");
                Time.timeScale = 1;
            }

            if (GUI.Button(new Rect(860, 435, 100, 50), "Quit Game"))
            {
                Application.Quit();
            }

            // GUI.Label(new Rect(850, 500, Screen.width, Screen.height), "You have died " + DBFunctions.getTimesDied() + " Times.");

        }
        //Just a temp spot for health
        //Texture2D healthBubble = new Texture2D(32, 32);
        //healthBubble.LoadImage(txtAsst.bytes);
        //GUI.Label(new Rect(0,0,Screen.width,Screen.height),currentHealth.ToString());
    }

    //Testing for prewall collision detection
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag.Equals("PickUp"))
        {
            CollectFood pickUp = obj.GetComponent<CollectFood>();

            if (!pickUp.getCheck())
            {
                pickUp.Check();
                StatsManager.Instance.AddPickUp(obj.gameObject.name);
            }
        }

        if (obj.gameObject.tag.Equals("Block"))
        {
            willHit = true;
        }
        else
        {
            willHit = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer != Constants.LAYER_INDEX_BOUNCY_BLOCK)
        {
            amplifyBounceCount = 0;
        }
    }

    public void StartDying()
    {
        this.transform.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        //Debug.Log ( "Dying" );		
        PlayerPrefs.SetInt("died", 1);
        PlayerPrefs.SetInt("currentLevel", Application.loadedLevel);
        StatsManager.Instance.StopLevelTime();
        isAlive = false;
        // DBFunctions.updateTimesDied(1);
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

        PlayerPrefs.SetString("death", "Dead");

        //Debug.Log ("loaded level" + Util.getlevel());
        //Application.LoadLevel ("test_death_menu_Nick");//Opens Death menu
    }


    // Player Control
    // -------------------------------------------------------------------------------------

    // New Controller Support --------------------------------------------------------------
    private void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.Touch.Disable();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Touch.Enable();
    }

    void DoHop(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (canHop)
        {
            Hop(hopX, hopY);
        }
    }

    void DoLaunch()
    {
        if (launchControl.GetAllowed())
        {
            Launch();
        }
        else
        {
            // Do something maybe?
            ResetLaunch();
        }
        // Debug.Log("Launch Allowed: " + launchControl.GetAllowed());
        initialPullBackSet = false;
        lastPullBackPosition = transform.position;
        lastAngle = 0f;
        pullBackRadius = 1.5f;
    }

    // void DoScoochRight(InputAction.CallbackContext context)
    void DoScoochRight()
    {
        if (nextScoochTimer < scoochTimeLimit) { return; }
        canScooch = false;
        ScoochRight();
    }

    // void DoScoochLeft(InputAction.CallbackContext context)
    void DoScoochLeft()
    {
        if (nextScoochTimer < scoochTimeLimit) { return; }
        canScooch = false;
        ScoochLeft();
    }

    public void EnsureControls()
    {
        if (controls == null)
        {
            controls = new PlayerControls();
        }
    }

    public PlayerControls GetControls()
    {
        EnsureControls();
        return controls;
    }

    public bool GetInLaunchMode()
    {
        return inLaunchMode;
    }

    public Vector2 GetTouchPosition()
    {
        return touchPosition;
    }

    bool handleLaunchMode()
    {
        int vertical = 0;
        int horizontal = 0;
        if (!initialPullBackSet)
        {
            PullBackLauncher(Vector2.zero);
            initialPullBackSet = true;
        }

        //https://answers.unity.com/questions/1164022/move-a-2d-item-in-a-circle-around-a-fixed-point.html
        if (angleChangeTimer < 0.05f)
        {
            return true;
        }
        else
        {
            angleChangeTimer = 0f;
        }

        float newAngle = lastAngle;

        if (upPressed)
        {
            pullBackRadius = pullBackRadius + 0.1f > 1.6f ? pullBackRadius : pullBackRadius + 0.1f;
        }
        else if (downPressed)
        {
            pullBackRadius = pullBackRadius - 0.1f < 0f ? pullBackRadius : pullBackRadius - 0.1f;
        }
        else if (rightPressed)
        {
            // movingDir = Direction.LEFT;
            if (!initialPullBackSet)
            {
                newAngle = 180f;
                initialPullBackSet = true;
            }
            else
            {
                newAngle = lastAngle + pullBackAngle > 360 ? 0.0f : lastAngle + pullBackAngle;
            }
        }
        else if (leftPressed)
        {
            // movingDir = Direction.RIGHT;
            if (!initialPullBackSet)
            {
                newAngle = 90f;
                initialPullBackSet = true;
            }
            else
            {
                newAngle = lastAngle - pullBackAngle < 0 ? 360 - pullBackAngle : lastAngle - pullBackAngle;
            }
        }

        Vector3 newDirection = new Vector3(Mathf.Sin(newAngle), Mathf.Cos(newAngle), 0) * pullBackRadius;
        lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, transform.position + newDirection);
        MovePullLineTo(lastPullBackPosition);
        lastAngle = newAngle;

        return true;
    }

    void handleTouchHoldInput()
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 touchOffset = startTouchPosition - playerPos;
        float touchOffsetValue = 0.75f;

        if (Mathf.Abs(touchOffset.x) < touchOffsetValue && Mathf.Abs(touchOffset.y) < touchOffsetValue)
        {
            // Hit!
            Debug.Log("Touched Player.");
            inTouchLaunchMode = true;
            return;
        }

        // isMoving = false;
        // Get Direction of input
        if (touchPosition.x > transform.position.x)
        {
            movingDir = Direction.RIGHT;
            playerAnimation.UpdateFacingDir();
            DoScoochRight();
        }
        else
        {
            movingDir = Direction.LEFT;
            playerAnimation.UpdateFacingDir();
            DoScoochLeft();
        }
        touchPosition = Camera.main.ScreenToWorldPoint(controls.Touch.TouchPosition.ReadValue<Vector2>());
    }

    void handleTouchLaunch()
    {
        DoLaunch();
        inTouchLaunchMode = false;
    }

    void handleTouchLaunchMode()
    {
        Vector3 pullEndPoint = pullLine.GetEndPointTouch(transform.position);
        Debug.Log($"pullEndPoint: {pullEndPoint}");
        pullLine.PositionClouds(transform.position, pullEndPoint);
        trajectoryDots.Position(transform.position, pullEndPoint);
        MovePullLineTo(pullEndPoint);
    }

    void handleTouchPressInput()
    {
        isMoving = false;
        // Get Direction of input
        if (touchPosition.x > transform.position.x)
        {
            movingDir = Direction.RIGHT;
        }
        else
        {
            movingDir = Direction.LEFT;
        }

        playerAnimation.UpdateFacingDir();
        DoHop();
    }

    // -------------------------------------------------------------------------------------

    // Spawn Support------------------------------------------------------------------------
    void setSafeSpot()
    {
        PlayerPrefs.SetFloat("safeSpotX", transform.position.x);
        PlayerPrefs.SetFloat("safeSpotY", transform.position.y);
        safeSpotTimer = 0.0f;
    }
    // -------------------------------------------------------------------------------------

    public bool GetOnGround()
    {
        return onGround;
    }

    public void SetOnGround(bool isOnGround)
    {
        onGround = isOnGround;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public void SetIsMoving(bool moving)
    {
        isMoving = moving;
    }

    public bool GetIsStuck()
    {
        return isStuck;
    }

    public void SetIsStuck(bool stuck)
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

    public void Hop(float x, float y)
    {
        canHop = false;

        if (onGround)
        {
            SetRespawn();
        }
        Rigidbody2D rb = this.transform.GetComponent<Rigidbody2D>();
        // this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
        // this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;	
        // this.transform.GetComponent<Rigidbody2D>().AddForce ( new Vector2 ( playerAnimation.isFacingRight ? hopX : -hopX, hopY ) ); //hop!	
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(playerAnimation.isFacingRight ? x : -x, y)); //hop!	
        fartControl.PlayScoochPoot();
        launchControl.DecrementCurrentJuice(1);
    }

    public void MovePullLineTo(Vector3 direction)
    {
        pullLine.PositionClouds(transform.position, direction);
        trajectoryDots.Position(transform.position, direction);
    }

    public void PullBackLauncher(Vector3 positionModifier)
    {
        float startPos = movingDir == Direction.RIGHT ? -2f : 2f;
        if (movingDir == Direction.RIGHT)
        {
            lastAngle = 180f;
        }
        else
        {
            lastAngle = 90f;
        }
        //Vector3 direction = new Vector3((transform.position.x + startPos), transform.position.y, 0);
        //lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, direction);
        Vector3 direction = new Vector3((transform.position.x + startPos), transform.position.y - 2, 0);
        lastPullBackPosition = pullLine.GetEndPointStatic(transform.position, direction);
        MovePullLineTo(lastPullBackPosition);
        //Debug.Log("C: startPos: " + startPos + " lastPullBack: " + lastPullBackPosition + " direction: " + direction + " playerDir: " + movingDir);

        // Debug.Log("Position: " + transform.position);
        // Debug.Log("Direction: " + pullLine.GetDirection(transform.position));
        // Debug.Log("Magnitude: " + direction.magnitude);
    }

    public void RegenerateFartJuiceTo(int regenerateLevel)
    {
        if (!shouldRegenerateFartJuice) { return; }
        if (launchControl.GetCurrentJuice() >= regenerateLevel) { isRegenerating = false; return; }
        else if (launchControl.GetCurrentJuice() <= regenerationTrigger) { isRegenerating = true; }

        if (!isRegenerating) { return; }
        launchControl.IncrementCurrentJuice(.07f);
    }

    public void Scooch(int x, int y)
    {
        //added for sticky block testing
        // if (this.transform.GetComponent<Rigidbody2D>().gravityScale == 0) 
        // {
        // 	this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
        // }
        this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
        SetRespawn();
        this.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y));//scooch!
        nextScoochTimer = 0f;
        fartControl.PlayScoochPoot();
    }

    public void ScoochRight()
    {
        // //added for sticky block testing
        // if (this.transform.GetComponent<Rigidbody2D>().gravityScale == 0) 
        // {
        // 	this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
        // }			

        // SetRespawn();
        // this.transform.GetComponent<Rigidbody2D>().AddForce (new Vector2 (100, 500));//scooch!
        // fartControl.PlayScoochPoot();

        Scooch(100, 500);
    }

    public void ScoochLeft()
    {
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
        PlayerPrefs.SetFloat("deathSpotX", this.transform.position.x);
        PlayerPrefs.SetFloat("deathSpotY", this.transform.position.y);
        //Debug.Log("Respawn Spot: " + this.transform.position.x + ", " + this.transform.position.y);
    }

    public void SetLastXPos(float lastX)
    {
        lastXPos = lastX;
    }

    public Direction GetMovingDir()
    {
        return movingDir;
    }
}
