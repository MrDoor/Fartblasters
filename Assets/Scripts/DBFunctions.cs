using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;

public class DBFunctions : MonoBehaviour {

	public static SqliteConnection con;
	public static SqliteCommand cmd;
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

		cmd = con.CreateCommand ();
		
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
<<<<<<< HEAD


		cmd = con.CreateCommand ();
=======
/*
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();
>>>>>>> 7be1efabaa63573b3994b952f1599222fd503f86

		foreach (string update in updates) {
		
					cmd.CommandText = update;
					cmd.ExecuteNonQuery ();
				}
		con.Close ();
*/		
	}

	public static void incrementLives(int count)
	{
		connectToDB ();
		con.Close();

	}
	public static void connectToDB()
	{
		con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		}

	public static void TestDB()
	{
		string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum > 5;";
		
		connectToDB ();
		
		cmd = con.CreateCommand ();
		
		cmd.CommandText = update;
		cmd.ExecuteNonQuery ();
		cmd.CommandText = "SELECT*FROM TestTable;";
		
		
		SqliteDataReader reader = cmd.ExecuteReader();
		
		while (reader.Read()) 
		{
			Debug.Log (reader ["ID"] + ", " + reader ["testNum"]);
		}
		con.Close ();
	}

}
