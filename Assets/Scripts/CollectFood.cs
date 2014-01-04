using UnityEngine;
using System.Collections;

public class CollectFood : MonoBehaviour 
{
	public float pickupJuice = 0.0f;
	private float destroyTime = 0.0f;

	void OnTriggerEnter2D( Collider2D obj )
	{
		if( obj.name == "Player" )
		{
			PlayerControl pControl = obj.GetComponent<PlayerControl>();
			if( pControl )
			{
				pControl.Launch_IncCurrentJuice( pickupJuice );
				
				if( this.gameObject.name.Contains( "(Clone)" ) )
				{
					pControl.Debug_DecFoodCount();
				}
				
				checkFoodType(pControl);				
				Debug.Log( "Food Checked!" );
			}
			
			//Destroy( this.gameObject );	
		}
	}
	
	void Update()
	{
		if(destroyTime > 0 && Time.time >= destroyTime)
		{
			Destroy( this.gameObject );	
		}
	}
	
	void checkFoodType(PlayerControl pControl)
	{
		if(this.gameObject.name.Equals("Jalapeno"))
		{	
			Vector2 boost = new Vector2();
			if(pControl.Animation_GetFacingRight())
			{
				boost.x = 4000;
			}
			else
			{
				boost.x = -4000;
			}
			pControl.transform.rigidbody2D.AddForce(boost);	
			this.audio.Play();		
			this.transform.renderer.enabled = false;
			destroyTime = Time.time + audio.clip.length;
			Debug.Log( "Fart Boost! AudioClipLength: " + audio.clip.length );
		}
		else
		{			
			Debug.Log( this.gameObject.name );
			destroyTime = Time.time;
		}
	}

}
