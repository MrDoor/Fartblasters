using UnityEngine;
using System.Collections;

public class TeleportCollision : MonoBehaviour 
{	
    private PlayerControl playerControlRef;
    private Transform teleportBlock1;
    private Transform teleportBlock2;
	public static float nextTeleport;
    public GameObject exitObject;

	void Start() 
    {
        playerControlRef = Util.SafePlayerControlFind();
        teleportBlock1 = Util.SafeGameObjectFind( "TeleportBlock1" ).transform;
        teleportBlock2 = Util.SafeGameObjectFind( "TeleportBlock2" ).transform;
	}
	
	void Update() 
    {	
	}
	
	void OnCollisionEnter2D( Collision2D coll )
	{
		//This only works if there is one of each block in the level... got it to work but will need to be modified to handle more than 2 teleport objects
		if( coll.gameObject.tag.Equals( "Player" ) )
		{
            if( playerControlRef )
			{				
				if( nextTeleport <= Time.time )
				{	
					try
					{
                        if( exitObject != null )
                        {
                            Teleport( coll, exitObject.transform.position );
                        }
						else if( this.gameObject.name.EndsWith( "1" ) )
						{
                            Teleport( coll, teleportBlock2.position );			
						}
						else
						{
                            Teleport( coll, teleportBlock1.position );
						}
					}
					catch( UnityException ex )
					{
						Debug.Log( "Error: " + ex.Message );
					}	
				}
			}
		}		
	}	

    public void Teleport( Collision2D coll, Vector3 newPos )
    {
        Vector2 tempVelocity = playerControlRef.transform.GetComponent<Rigidbody2D>().velocity;
        Vector3 tempPosition = newPos;
        if( coll.transform.position.x > this.transform.position.x )
        {
            tempPosition.x -= this.GetComponent<Renderer>().bounds.size.x;
        }
        else
        {
            tempPosition.x += this.GetComponent<Renderer>().bounds.size.x;
        }
        playerControlRef.transform.position = tempPosition;
        playerControlRef.transform.GetComponent<Rigidbody2D>().AddForce( tempVelocity );
        nextTeleport = Time.time + 2;
        Debug.Log( "Teleport! " + nextTeleport );   
    }
}
