using UnityEngine;
using System.Collections;

public class PlayerZoom : MonoBehaviour 
{    
    public bool zoomOn  = false;
    public int maxZoom  = 5;
    public int minZoom  = 4;


    private void OnMouseUp()
    {      
        if( zoomOn )
        {
            StopCoroutine( ZoomOut() );
            StartCoroutine( ZoomIn() );       
        }
    }
    
    private void OnMouseDown()
    {
        if( zoomOn )
        {   
            StopCoroutine( ZoomIn() );                
            StartCoroutine( ZoomOut() );
        }       
    }

    private IEnumerator ZoomOut()
    {       
        while(Camera.main.orthographicSize <= maxZoom)
        {
            Camera.main.orthographicSize = Mathf.Lerp( Camera.main.orthographicSize, maxZoom + 1, Time.deltaTime * 1.25f );                        
            yield return new WaitForSeconds( 0.025f );
        }       
        yield break;        
    }
    
    private IEnumerator ZoomIn()
    {
        yield return new WaitForSeconds( 1.25f );
        while( Camera.main.orthographicSize > minZoom )
        {
            Camera.main.orthographicSize = Mathf.Lerp( Camera.main.orthographicSize,minZoom, Time.deltaTime * 0.2f );
            //Could not figure this out so I just added this crap.  Needs to be redone.
            if(Camera.main.orthographicSize <= 0.31f)
            {
                Camera.main.orthographicSize = minZoom;
                break;
            }
            yield return new WaitForSeconds(.025f);
        }
        
        yield break;
    }
}
