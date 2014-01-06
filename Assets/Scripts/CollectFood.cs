using UnityEngine;
using System.Collections;

public class CollectFood : MonoBehaviour 
{
	public float pickupJuice	= 0.0f;
	private bool isDebugFood	= false;

	void Start()
	{
		isDebugFood = Util.IsSpaceBarSpawnedFood( this.gameObject );
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
			this.audio.Play();		
			this.transform.renderer.enabled = false;
			destroyTime = audio.clip.length;
			Debug.Log( "Fart Boost! AudioClipLength: " + audio.clip.length );
		}
		else
		{			
			Debug.Log( this.gameObject.name );
		}
		Destroy( this.gameObject, destroyTime );
	}

}
