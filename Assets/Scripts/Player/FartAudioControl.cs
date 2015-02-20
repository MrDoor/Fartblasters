using UnityEngine;
using System.Collections;

public class FartAudioControl : MonoBehaviour 
{
    public AudioHandler audioHandler;

    public AudioClip[] scoochPootClips      = new AudioClip[1];
    public AudioClip[] pullLineFartClips    = new AudioClip[12];   // Right now assuming all farts are in order of weakest to strongest
    public AudioClip[] powerUpFartClips     = new AudioClip[1];

	
    private void PlayClip(AudioClip clip)
    {
        audioHandler.PlayClip(clip);
    }
    
    public void PlayScoochPoot()
    {   
        audioHandler.PlayClip(scoochPootClips[0]); 
    }
    
    public void PlayFartByPullPercent( float pullPercent )
    {
        DebugUtils.Assert(pullLineFartClips.Length > 0);

        int index = Mathf.FloorToInt((pullLineFartClips.Length - 1) * pullPercent);
        audioHandler.PlayClip(pullLineFartClips[index]);
    }
}
