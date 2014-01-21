using UnityEngine;
using System.Collections;

public class MagnetCollision : MonoBehaviour 
{	
	public Transform startMarker;
	public Vector3 endMark;
	public float speed = 2.0f;
	private float startTime;
	private float journeyLength;
	public Transform target;
	public float smooth = 5.0F;
	public bool inMagnet = false;
	public float nextMagnetPull;

    private PlayerControl playerControlRef;

	void Start() 
    {		
		//added a magnetic field randomizer just for funzies
		CircleCollider2D magnetRange = (CircleCollider2D)this.transform.collider2D;		
		magnetRange.radius += Random.Range( 5, 30 ) * .01f;
		Debug.Log( "radius: " + magnetRange.radius );
        
        playerControlRef = GameObject.Find("Player").GetComponent<PlayerControl>() as PlayerControl;
	}
	
	void Update() 
    {	
		if( inMagnet )
		{	
			float distCovered = ( Time.time - startTime ) * speed;
			float fracJourney = distCovered / journeyLength;
			target.transform.position = Vector3.Lerp( startMarker.position, endMark, fracJourney );
			Debug.Log( "Journey Length: " + journeyLength );
			if( target.position == endMark )
			{
				Stuck();
				inMagnet = false;
				nextMagnetPull = Time.time + 3;
                playerControlRef.SetIsStuck( true );
			}	
			else
            {
				Debug.Log( "Journey Length: " + journeyLength + " | endMarker.position: " + endMark.ToString() );
			}
		}
	}
	
	void OnTriggerEnter2D( Collider2D obj )
	{		
		Debug.Log( "Trigger Enter" );
		if( obj.tag.Equals( "Player" ) )
		{
            if( playerControlRef )
			{
				if( !inMagnet && ( nextMagnetPull < Time.time ) )
				{
					try
					{
						//Vector3 tempEnd = GameObject.Find ("LeftMagnetPosition").transform.position;
						Vector3 tempEnd = this.transform.position;
						tempEnd.y -= this.renderer.bounds.size.y;
						endMark = tempEnd;						
                        target = playerControlRef.transform;
						inMagnet = true;
                        startMarker = playerControlRef.transform;				
						startTime = Time.time;				
                        playerControlRef.transform.rigidbody2D.gravityScale = 0;
                        playerControlRef.transform.rigidbody2D.velocity = Vector2.zero;
						journeyLength = Vector3.Distance( startMarker.position, endMark );		
					}
					catch( UnityException ex )
					{
						Debug.LogError( "Error: " + ex );
					}
				}
			}
		}
	}
	
	/*
	void OnCollisionEnter2D(Collision2D coll)
	{		
		Debug.Log ("Collision Enter");
		if(inMagnet || nextMagnetPull > Time.time)
		{
			return;
		}
		try
		{
			Vector3 tempEnd = GameObject.Find ("LeftMagnetPosition").transform.position;
			tempEnd.x += GameObject.Find("LeftMagnetPosition").transform.parent.renderer.bounds.size.x;
			endMark = tempEnd;
			GameObject tempRef = GameObject.Find("Player");
			Debug.Log ("coll.x = " + coll.transform.position.x);
			target = tempRef.transform;
			inMagnet = true;
			startMarker = tempRef.transform;
			//endMarker = GameObject.Find("LeftMagnetPosition").transform;			
			endMarker = GameObject.Find("LeftMagnetPosition").transform.parent.transform;			
			//endMarker.position.x -= (GameObject.Find("LeftMagnePosition").transform.parent.renderer.bounds.size.x / 2);
			startTime = Time.time;
			journeyLength = Vector3.Distance(startMarker.position, endMarker.position);				
			
			//tempRef.transform.position = GameObject.Find("LeftMagnetPosition").renderer.bounds.center;	
			//tempRef.transform.position = Vector3.Lerp(tempRef.transform.position, GameObject.Find("LeftMagnetPosition").renderer.bounds.center, 1);
			//tempRef.transform.rigidbody2D.gravityScale = 0;
			//tempRef.transform.rigidbody2D.velocity = Vector2.zero;		
			
			//tempRef.setIsStuck(true);
		}
		catch(UnityException ex)
		{
			Debug.LogError("Error: " + ex);
		}
	}
	*/
	
	void Stuck()
	{								
		Debug.Log( "Message Sent" );
	}
}
