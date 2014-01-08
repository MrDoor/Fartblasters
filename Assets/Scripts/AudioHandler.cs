using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

	public AudioClip[] farts = new AudioClip[12];
	// Use this for initialization
	void Start () {

		this.audio.clip = farts[Random.Range(0, farts.Length)];
		Debug.Log ("Playing Test Fart");
		this.audio.Play();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
