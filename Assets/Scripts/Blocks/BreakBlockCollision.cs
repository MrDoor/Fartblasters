using UnityEngine;
using System.Collections;

public class BreakBlockCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D( Collider2D obj )
    {
        if (obj.name == "Player")
        {
			PlayerControl pControl = obj.GetComponent<PlayerControl>();
			if( pControl )
			{
				this.transform.GetComponent<Renderer>().enabled = false;
				this.gameObject.GetComponent<Collider2D>().enabled = false;
				pControl.transform.GetComponent<Rigidbody2D>().AddForce( pControl.transform.GetComponent<Rigidbody2D>().velocity.normalized * -3000 );
				Debug.Log ("Break Block");
			}
        }
    }
}
