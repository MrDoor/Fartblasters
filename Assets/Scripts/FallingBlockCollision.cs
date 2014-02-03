using UnityEngine;
using System.Collections;

public class FallingBlockCollision : MonoBehaviour 
{	
	public float fallTime 		= 2.5f;
	public float totalBlinkTime_= 1.0f;	
	private float blinkTime 	= 0.1f;	
	
	void Start() 
    {		
	}
	
	void Update() 
    {	
	}
	
	void OnCollisionEnter2D( Collision2D coll )
	{
		if( coll.gameObject.tag.Equals( "Player" ) )
		{
			StartCoroutine( Fall() );			
		}
		else
		{	
			if( !coll.gameObject.tag.Equals( "TrajectoryDot" ))
			{
				StartCoroutine( FadeOut() );
			}
		}	
	}	
	
	IEnumerator Fall()
	{
		Debug.Log( "Fall Start: " + Time.time + " | " + ( fallTime - totalBlinkTime_ ) + " | " + ( totalBlinkTime_ - fallTime ) );
		
		yield return new WaitForSeconds( fallTime - totalBlinkTime_ );
		Debug.Log( "Fall End: " + Time.time );
		
		int blinkCount = (int)( totalBlinkTime_ / blinkTime );
		for( int blinkIndex = 0; blinkIndex < blinkCount; blinkIndex++)
		{
			this.renderer.enabled = !this.renderer.enabled;//I love this :)
			yield return new WaitForSeconds( blinkTime );
		}
		
		this.renderer.enabled = true;
		this.gameObject.AddComponent<Rigidbody2D>();
		this.transform.rigidbody2D.gravityScale = 1;
		this.transform.rigidbody2D.mass = 10;
	}	
	
	IEnumerator FadeOut()
	{
		int blinkCount = (int)( totalBlinkTime_ / blinkTime );
		for( int blinkIndex = 0; blinkIndex < blinkCount; blinkIndex++)
		{
			this.renderer.enabled = !this.renderer.enabled;
			yield return new WaitForSeconds( blinkTime / 2 );
		}
		
		Destroy( this.gameObject );
	}	
	
}
