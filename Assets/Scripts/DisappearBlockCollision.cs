using UnityEngine;
using System.Collections;

public class DisappearBlockCollision : MonoBehaviour 
{
	public float disappearTime	= 2.5f;
	public float totalBlinkTime	= 1.0f;
	public bool reappears		= true;
	public float reappearTime	= 1.0f;

	private float blinkTime		= 0.1f;
	private bool startDisappear	= false;

	void Start() 
	{	
		reappearTime += totalBlinkTime;
	}

	void Update() 
	{		
	}
	
	void OnCollisionEnter2D( Collision2D coll )
	{
		if( coll.gameObject.tag.Equals( "Player" ) )
		{
			if( !startDisappear )
			{
				StartCoroutine( Disappear() );
				startDisappear = true;
			}
		}
	}

	IEnumerator Disappear()
	{
		yield return new WaitForSeconds( disappearTime - totalBlinkTime );

		int blinkCount = (int)( totalBlinkTime / blinkTime );
		for( int blinkIndex = 0; blinkIndex < blinkCount; blinkIndex++ )
		{
			this.renderer.enabled = !this.renderer.enabled;
			yield return new WaitForSeconds( blinkTime );
		}
		this.renderer.enabled = false;
		this.gameObject.collider2D.enabled = false;

		if( reappears )
		{
			StartCoroutine( Reappear() );
		}
	}

	IEnumerator Reappear()
	{
		yield return new WaitForSeconds( reappearTime - totalBlinkTime );

		int blinkCount = (int)( totalBlinkTime / blinkTime );
		for( int blinkIndex = 0; blinkIndex < blinkCount; blinkIndex++ )
		{
			this.renderer.enabled = !this.renderer.enabled;
			yield return new WaitForSeconds( blinkTime );
		}

		this.renderer.enabled = true;		
		this.gameObject.collider2D.enabled = true;
		startDisappear = false;
	}
}
