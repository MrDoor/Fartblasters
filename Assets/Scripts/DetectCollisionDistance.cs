using UnityEngine;
using System.Collections;

public class DetectCollisionDistance : MonoBehaviour {

	private PlayerControl pControl;
	private Transform wallTransform;
	
	// Use this for initialization
	void Start () {
		pControl = Util.SafePlayerControlFind();
	}
	
	// Update is called once per frame
	void Update () {
	
		if( wallTransform != null )
		{
			this.GetComponent<Animator>().SetFloat( "distFromWall",  Mathf.Abs ( pControl.transform.position.x - wallTransform.position.x ) );	
			if( Mathf.Abs( pControl.transform.position.x - wallTransform.position.x ) > 3 )
			{
				this.GetComponent<Animator>().SetBool ( "nearWall" , false );
			}
			else
			{
				this.GetComponent<Animator>().SetBool ( "nearWall" , true );
			}
		}	
	}
	
	void OnTriggerEnter2D ( Collider2D coll )
	{	
		if( coll.gameObject.name.Equals ( "Wall" ) )
		{	
			wallTransform = coll.transform;
		}
	}
	
}
