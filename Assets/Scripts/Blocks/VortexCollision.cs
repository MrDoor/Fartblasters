using UnityEngine;
using System.Collections;

public class VortexCollision : MonoBehaviour 
{	
	//private Transform startMarker;
	private Vector3 startMark;
	private Vector3 endMark;
	private float speed = 3.0f;
	private float startTime;
	private float journeyLength;
	private Transform target;
	//private float smooth = 5.0F;
	private float timeToLaunch = 0.0f;
	private float timeToReLaunch = 0.0f;
	private bool canLaunch = false;
    private PlayerControl playerControlRef;
	
    void Start() 
    {	
        playerControlRef = (PlayerControl)GameObject.Find ("Player").GetComponent<PlayerControl>();
	}
	
	void Update() 
    {
		if( ( timeToLaunch <= Time.time ) && ( timeToReLaunch <= Time.time ) && canLaunch )
		{				
            Debug.Log( "Gravity Scale before: " + playerControlRef.transform.GetComponent<Rigidbody2D>().gravityScale );
			canLaunch = false;
			timeToReLaunch = Time.time + 3.0f;
            playerControlRef.transform.GetComponent<Rigidbody2D>().gravityScale = 1;			
            playerControlRef.SetInVortex( false );
			EjectFromVortex();
            Debug.Log( "Gravity Scale after: " + playerControlRef.transform.GetComponent<Rigidbody2D>().gravityScale );			
		}
        else if( playerControlRef && playerControlRef.GetInVortex() )
		{
			if( target && this.transform.position == endMark )
			{
				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				target.transform.position = Vector3.Lerp( startMark, endMark, fracJourney );
			}
		}			
	}
	
	void OnTriggerEnter2D(Collider2D obj)
	{
		//WIP -- Don't much like the animation... 
		if( timeToReLaunch <= Time.time )
		{
			if( obj.tag.Equals( "Player" ) )
			{
                if( playerControlRef )
				{
					try
					{
						Vector3 tempEnd = this.transform.position;
						tempEnd = this.GetComponent<Collider2D>().transform.position;
						endMark = tempEnd;						
                        target = playerControlRef.transform;
                        //startMarker = playerControlRef.transform;
                        startMark = playerControlRef.transform.position;				
						startTime = Time.time;				
                        playerControlRef.transform.GetComponent<Rigidbody2D>().gravityScale = 0;
                        playerControlRef.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
						journeyLength = Vector3.Distance( startMark, endMark );
                        playerControlRef.SetInVortex( true );
						timeToLaunch = Time.time + 3.0f;
						canLaunch = true;
						Debug.Log( "Animation Played" );
					}
					catch( UnityException ue )
					{
						Debug.Log( "Error: " + ue.ToString() );
					}
				}
			}
			else
			{
				Debug.Log( "obj not player. obj: " + obj.ToString() );
			}
		}
	}
	
	void EjectFromVortex()
	{
		Vector2 direction = new Vector2();
		direction.x = Random.Range( 0, 180 ) * .01f;
		direction.y = Random.Range( 0, 180 ) * .01f;
		float force = Random.Range( 5, 10 ) * 1000f;
		float sign = Random.Range( 0, 11 );
		Debug.Log ( "sign: " + sign );
		if(sign >= 5)
		{
			direction *= -1;
		}
		Debug.Log("direction: " + direction.ToString() + " force: " + force + " direction*force = " + (direction * force));
		endMark = Vector3.zero;
		startMark = Vector3.zero;
        playerControlRef.transform.GetComponent<Rigidbody2D>().AddForce(direction * force);
	}
}
