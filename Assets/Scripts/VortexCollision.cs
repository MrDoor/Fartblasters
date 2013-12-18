using UnityEngine;
using System.Collections;

public class VortexCollision : MonoBehaviour {
	
	float timeToLaunch = 0.0f;
	float timeToReLaunch = 0.0f;
	bool canLaunch = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(timeToLaunch <= Time.time && timeToReLaunch <= Time.time && canLaunch)
		{				
			Debug.Log("Gravity Scale before: " + GameObject.Find ("Player").transform.rigidbody2D.gravityScale);
			canLaunch = false;
			timeToReLaunch = Time.time + 3.0f;
			GameObject.Find ("Player").transform.rigidbody2D.gravityScale = 1;			
			GameObject.Find("Player").SendMessage("SetInVortex", false);
			EjectFromVortex();
			Debug.Log("Gravity Scale after: " + GameObject.Find ("Player").transform.rigidbody2D.gravityScale);
		}	
	}
	
	void OnTriggerEnter2D(Collider2D obj)
	{
		//WIP -- Don't much like the animation... Also I think we should change this to work more like the magnet with the smooth transition
		if(timeToReLaunch <= Time.time)
		{
			if(obj.name.Equals("Player"))
			{
				try
				{
					obj.transform.position = this.transform.position;
					obj.transform.rigidbody2D.velocity = Vector2.zero;
					obj.transform.rigidbody2D.gravityScale = 0;
					obj.SendMessage("SetInVortex", true);
					timeToLaunch = Time.time + 3.0f;
					canLaunch = true;
					Debug.Log ("Animation Played");
				}
				catch(UnityException ue)
				{
					Debug.Log("Error: " + ue.ToString());
				}
			}
			else
			{
				Debug.Log ("obj not player. obj: " + obj.ToString());
			}
		}
	}
	
	void EjectFromVortex()
	{
		Vector2 direction = new Vector2();
		direction.x = Random.Range(1, 90) * .01f;
		direction.y = Random.Range(1, 90) * .01f;
		float force = Random.Range(3, 10) * 1000f;
		float sign = Random.Range (0, 3);
		Debug.Log ("sign: " + sign);
		if(sign >= 1)
		{
			direction *= -1;
		}
		Debug.Log("direction: " + direction.ToString() + " force: " + force + " direction*force = " + (direction * force));
		GameObject.Find ("Player").transform.rigidbody2D.AddForce(direction * force);
	}
}
