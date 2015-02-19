using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour 
{
    public AudioSource[] audioSources = new AudioSource[2];

    public void PlayClip(AudioClip clip)
    {
        foreach(AudioSource source in audioSources)
        {
            if(!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                break;
            }
        }
    }
}
