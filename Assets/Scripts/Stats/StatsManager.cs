using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers; 

public class StatsManager : MonoBehaviour 
{
    //DB counts
    public int pickUpCount {get; private set;}
    public int playTime {get; private set;}
    public Timer levelTime {get; private set;}
    public IDictionary<string, int> pickUps = new Dictionary<string, int>();

    
    private static StatsManager _instance;
    
    public static StatsManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<StatsManager>();

                DontDestroyOnLoad(_instance.gameObject);
            }
            
            return _instance;
        }
    }
    
    void Awake() 
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Init()
    {
        ResetCount();

        levelTime = new Timer(1000);
        levelTime.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        levelTime.Enabled = true;
        levelTime.Start();
    }

    private void OnTimedEvent(System.Object source, ElapsedEventArgs e)
    {
        playTime++;
    }

    public void ResetCount()
    {   
        pickUpCount = 0;
        playTime = 0;
        pickUps.Clear();
    }

    public int[] GetPlayTime()
    {
        int[] lvlTimes = new int[2];
        lvlTimes[0] = (playTime / 60);
        lvlTimes[1] = (playTime % 60);
        
        return lvlTimes;
    }

    public void StopLevelTime()
    {
        levelTime.Stop();
    }

    public void AddPickUp(string name)
    {
        pickUpCount++;
        
        if( pickUps.ContainsKey( name ) )
        {
            pickUps[name] += 1;
        }
        else
        {
            pickUps.Add( name, 1 );
        }
    }

    public void EndLevelUpdate(int num)
    {   
        StopLevelTime();
        DBFunctions.updateLevelInfo(num, true, 5000, playTime, false, pickUpCount);
        foreach(KeyValuePair<string, int> pU in pickUps) 
        {
            DBFunctions.updatePickUp(pU.Key, pU.Value);
        }
    }
}
