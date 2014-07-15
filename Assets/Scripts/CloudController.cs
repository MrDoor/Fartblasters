using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	public bool goRight;
	public float speed;
	// Use this for initialization
	void Start () {
		speed = speed / 100;
	}
	
	// Update is called once per frame
	void Update () {
		if ( goRight )
		{
			Vector2 temp = this.transform.position;
			temp.x += speed;
			this.transform.position = temp;
		}
	}
}
