using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;

public class DBFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void updatePickUp(string name, int count)
	{
	/*
		string sql = "UPDATE PickUpInfo Set PUTotal = PUTotal +" + count + "WHERE PUName = " + name + ";";
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();
		
		cmd.CommandText = sql;
		cmd.ExecuteNonQuery ();
		con.Close ();
	*/	
	}

	public static void updateLevelInfo(string level, bool win, int score, Time lvlTime)
	{
		int complete = 0;
		if (win) 
		{ 
			complete = 1;
		}
		string[] updates = new string[]{"UPDATE PlayerInfoByLevels SET TimesPlayed = TimesPlayed + 1, TimesComplete = TimesComplete + 1 WHERE LevelName = "+level+";",
										"UPDATE PlayerInfoByLevels SET HighScore = " + score + "WHERE LevelName = " + level + "AND HighScore < " + score + ";",
										"UPDATE PlayerInfoByLevels SET FastestTime = " + lvlTime + "WHERE LevelName = " + level + "AND FastestTIme < " + lvlTime + ";"};
/*
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();

		foreach (string update in updates) {
		
					cmd.CommandText = update;
					cmd.ExecuteNonQuery ();
				}
		con.Close ();
*/		
	}
}
