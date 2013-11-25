using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private Transform myTransform;
	//private Vector3 spawnPoint;
	
	void Awake()
	{
		//myTransform = transform;
	}
	
	void Start() 
	{
		//spawnPoint = myTransform.position;		
	}
	
	void Update () 
	{

	}
	
	void OnCollisionEnter( Collision other )
	{
		
	}	
}
