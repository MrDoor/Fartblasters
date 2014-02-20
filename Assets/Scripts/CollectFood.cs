using UnityEngine;
using System.Collections;

public class CollectFood : MonoBehaviour 
{
	public float pickupJuice	= 5.0f;
	private bool isDebugFood	= false;
	//private Animator foodAnimator;

	void Start()
	{
        isDebugFood = Util.IsObjectDebug( this.gameObject );
		//foodAnimator = GameObject.Find("PickUp_Animator").GetComponent<Animator>();
	}

	void OnTriggerEnter2D( Collider2D obj )
	{
		if( obj.name == "Player" )
		{
			PlayerControl pControl = obj.GetComponent<PlayerControl>();
			if( pControl )
			{
				pControl.Launch_IncCurrentJuice( pickupJuice );
				
				if( isDebugFood )
				{
					pControl.Debug_DecFoodCount();
				}
				
				checkFoodType(pControl);				
				Debug.Log( "Food Checked!" );
			}
		}
	}
	
	void Update()
	{
	}
	
	void checkFoodType( PlayerControl pControl )
	{
		float destroyTime = 0.0f;
		if( this.gameObject.name.Equals("Jalapeno") )
		{	
			pControl.transform.rigidbody2D.AddForce( pControl.transform.rigidbody2D.velocity.normalized * 4000 );	
			AudioSource[] sounds = this.gameObject.GetComponents<AudioSource>();
			sounds[0].Play();		
			sounds[1].PlayDelayed(sounds[0].clip.length);
			this.transform.renderer.enabled = false;
			destroyTime = sounds[1].clip.length;
			//Debug.Log( "Fart Boost! AudioClipLength: " + audio.clip.length );
		}
		else
		{
			if(this.gameObject.tag.Equals("Health"))
			{
				pControl.Health_IncHealth(pickupJuice);
				Debug.Log ( "Health inc by " + pickupJuice );
			}			
			Debug.Log( this.gameObject.name + ":" + this.gameObject.tag);
			this.audio.Play();
			destroyTime = this.audio.clip.length;
			this.transform.renderer.enabled = false;
		}
		pControl.SetIsEating ( true );
		Destroy( this.gameObject, destroyTime );
		StartCoroutine ( "StopEating", pControl );
	}
	
	IEnumerator StopEating( PlayerControl pControl )
	{
		yield return new WaitForSeconds ( .24f );
		pControl.SetIsEating ( false );
	}
}
