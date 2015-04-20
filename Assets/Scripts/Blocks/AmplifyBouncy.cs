using UnityEngine;
using System.Collections;

public class AmplifyBouncy : MonoBehaviour 
{
	public static float bounceForce;
    public AudioSource audioSource;
    public AudioClip[] boingClips = new AudioClip[4];

    private float forceIncAmount;

    private static readonly int maxBounceCount          = 4;
    private static readonly float startForce            = 70f;
    private static readonly float startForceIncAmount   = 3f;
    private static readonly float forceReductPercent    = 0.7f;
    private static readonly float minVelocityMag        = 4f;
    private static readonly float maxVelocityMag        = 15f;
    private static readonly float minSqrMag             = 12f;
    private static readonly float maxSqrMag             = 225f;

	void Start () 
    {
        bounceForce     = startForce;
        forceIncAmount  = startForceIncAmount;
	}
		
	void OnCollisionEnter2D(Collision2D coll)
	{
        if( IsBounceable( coll.gameObject ) )
		{		
            ContactPoint2D hit = coll.contacts[0];     // the first contact point
            Vector3 hitNormal = new Vector3( hit.normal.x, hit.normal.y, 0f );
            Vector3 velocity = Vector3.Reflect( -coll.relativeVelocity, -hitNormal );

            if( velocity.Equals(Vector3.zero) )
            {
                velocity = hitNormal;
                velocity *= minVelocityMag;
            }
            else if( velocity.sqrMagnitude < minSqrMag )
            {
                velocity.Normalize();
                velocity *= minVelocityMag;
            }
            else if( velocity.sqrMagnitude > maxSqrMag )
            {
                velocity.Normalize();
                velocity *= maxVelocityMag;
            }

            coll.collider.rigidbody2D.velocity = Vector2.zero;
            coll.collider.rigidbody2D.AddForce( new Vector2( velocity.x, velocity.y ) * bounceForce * coll.collider.rigidbody2D.mass );   
        
            // Only the player gets the amplifying effect    
            PlayerControl pControl = coll.gameObject.GetComponent<PlayerControl>();

            if( pControl )
            {
                if( pControl.amplifyBounceCount < maxBounceCount )
                {
                    if( pControl.amplifyBounceCount == 0 )
                    {
                        bounceForce = startForce;
                        forceIncAmount = startForceIncAmount;
                    }
                    else
                    {
                        forceIncAmount *= forceReductPercent;
                        bounceForce += forceIncAmount;
                    }
                }

                pControl.amplifyBounceCount++;
            }

            if(audioSource != null)
            {
                int boingIndex = Random.Range(0, boingClips.Length - 1);
                if(boingClips[boingIndex] != null)
                {
                    audioSource.clip = boingClips[boingIndex];
                }
                audioSource.audio.Play();
            }
		}
	}

    private bool IsBounceable(GameObject obj)
    {
        return true;//obj.CompareTag( "Player" );
    }
}
