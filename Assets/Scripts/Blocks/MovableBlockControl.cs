using UnityEngine;
using System.Collections;

public class MovableBlockControl : MonoBehaviour {

	public int force = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//Vector3 temp = new Vector3 ();
		Vector2 temp = new Vector2 (.9f, .0f);
		this.transform.GetComponent<Rigidbody2D>().gravityScale = 1;
		if (coll.gameObject.tag.Equals ("Player"))
		{
			this.transform.GetComponent<Rigidbody2D>().AddForce (temp * force);
			Debug.Log (coll.gameObject.tag);
		} 
		else 
		{
				}
	}

	/*void OnTriggerEnter2D( Collider2D obj )
	{
		Vector2 temp = new Vector2 (.9f, .0f);
		this.transform.rigidbody2D.gravityScale = 1;
		if (obj.gameObject.tag.Equals ("Player"))
		{
			this.transform.rigidbody2D.AddForce (temp * force);
			Debug.Log (obj.gameObject.tag);
		} 
		else 
		{
		}
	}*/
}
