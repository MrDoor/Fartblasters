using UnityEngine;
using System.Collections;

public class BreakBlockCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*oid OnCollisionEnter2D( Collision2D coll )
    {
    	
        if( coll.gameObject.tag.Equals( "Player" ) )
        {
            this.renderer.enabled = false;
            this.gameObject.collider2D.enabled = false;
        }
    }*/
	

    void OnTriggerEnter2D( Collider2D obj )
    {
        if (obj.name == "Player")
        {
			PlayerControl pControl = obj.GetComponent<PlayerControl>();
			if( pControl )
			{
				this.transform.renderer.enabled = false;
				this.gameObject.collider2D.enabled = false;
				pControl.transform.rigidbody2D.AddForce( pControl.transform.rigidbody2D.velocity.normalized * -3000 );
			}
        }
    }
}
