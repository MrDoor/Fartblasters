using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;
using System;

public class DBFunctions : MonoBehaviour {

	public static SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
	public static SqliteCommand cmd;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//Connect to database build command object.
	public static void connectToDB()
	{
		try
		{
			con = new SqliteConnection ("Data Source=Assets/TestDB");
			con.Open ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "Could NOT connect to database");
			con.Close ();
		}
	}
	//***********************************UPDATE TABLES*************************************//

	///////////////////////////Player Info Table

	public static void incrementLives(int count)
	{
		try{
			connectToDB ();
			
			SqliteCommand cmd = con.CreateCommand ();
			string lives = "UPDATE PlayerInfo SET CurrentLives =  CurrentLives + "+count+";";
			cmd.CommandText = lives;
			cmd.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally
		{
			con.Close();
		}
	}

	public static void incrementTokens(int count)
	{
		string tokens = "UPDATE PlayerInfo SET CurrentTokens =  CurrentTokens + "+count+";";
		try
		{
			connectToDB ();
			SqliteCommand cmd = con.CreateCommand ();
			
			cmd.CommandText = tokens;
			cmd.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally
		{
			con.Close();
		}
	}

	public static void updateLevelProgress(int levelIndex)
	{
		//temporary check for index out of bounds
		
		if (levelIndex >= Application.levelCount)
			levelIndex = Application.levelCount-1;
		
		string update = "UPDATE PlayerInfo SET levelProgress = " + levelIndex + " WHERE levelProgress < " + levelIndex + ";";
		try{
			connectToDB ();
			
			SqliteCommand cmd2 = con.CreateCommand ();
			cmd2.CommandText = update;
			cmd2.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
		}
		finally
		{
			con.Close ();
		}
	}

	public static void updateTimesDied(int deaths)
	{
		string update = "UPDATE PlayerInfo SET TimesDied = TimesDied + " + deaths + ";";

		try
		{
			connectToDB ();
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = update;
			cmd.ExecuteNonQuery();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
		}
		finally 
		{
			con.Close ();
		}
	}

	public static void updateLastLevel(int levelIndex)
	{
				
		if (levelIndex >= Application.levelCount)
						
			levelIndex = Application.levelCount - 1;
		
				
		string update = "UPDATE PlayerInfo SET levelProgress = " + levelIndex + " WHERE levelProgress < " + levelIndex + ";";
				
		try {
						
			connectToDB ();

			SqliteCommand cmd2 = con.CreateCommand ();
			cmd2.CommandText = update;
			cmd2.ExecuteNonQuery ();
		} catch (SqliteException e) {
				Debug.Log ("DATABASE error: " + e);
		} finally {
			con.Close ();
		}
	}

	//////////////////LevelInfo Table

	public static void updateLevelComplete(int levelIndex)
	{
		string update = "UPDATE LevelInfo SET complete = 0 WHERE LevelIndex = " + levelIndex + ";";

		try
		{
			connectToDB ();
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = update;
			cmd.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally{
			con.Close ();
		}
	}

	//PickUpTable

	public static void updatePickUp(string name, int count)
	{
		try{

			connectToDB ();
			string sql = "UPDATE PickUpInfo Set PUTotal = PUTotal +" + count + "WHERE PUName = " + name + ";";

			SqliteCommand cmd = con.CreateCommand ();
			
			cmd.CommandText = sql;
			cmd.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally{
			con.Close ();
		}
	}

	//PlayerInfoByLevel//
	public static void updateLevelInfo(int levelIndex, bool win, int score, int lvlTime, bool secret, int pickUps)
	{
		int complete = 0;
		if (win) 
		{ 
			complete = 1;
		}
		int sec;

		if(secret)
			sec = 1;
		else 
			sec = 0;

		float percent = calcPickUpPercentage(levelIndex, pickUps);
		string[] updates = new string[]{"UPDATE PlayerInfoByLevels SET TimesPlayed = TimesPlayed + 1, TimesComplete = TimesComplete + 1 WHERE levelIndex = "+levelIndex+";",
			"UPDATE PlayerInfoByLevels SET HighScore = " + score + " WHERE levelIndex = " + levelIndex + " AND HighScore < " + score + ";",
			"UPDATE PlayerInfoByLevels SET FastestTime = " + lvlTime + " WHERE levelIndex = " + levelIndex + " AND FastestTIme < " + lvlTime + ";",
			"UPDATE PlayerInfoByLevels SET SecretExitFound = "+ sec + " WHERE levelIndex = " + levelIndex + ";",
			"UPDATE PlayerInfoByLevels SET HighestPercentPickUps = " + percent + " WHERE levelIndex = " + levelIndex + " AND HighestPercentPickUps < " + percent +";" };



		try{

			connectToDB ();
			SqliteCommand cmd = con.CreateCommand ();
			foreach (string update in updates) {
			
						cmd.CommandText = update;
						cmd.ExecuteNonQuery ();
					}
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally
		{
			con.Close ();
		}
	}

	public static void incrementTimesPlayed(int levelIndex, int increment)
	{
		string update = "UPDATE PlayerInfoByLevels SET TimesPlayed = TimesPlayed + " + increment + " WHERE LevelIndex = " + levelIndex + ";";
		try{
			
			connectToDB ();
			SqliteCommand cmd = con.CreateCommand ();
				
			cmd.CommandText = update;
			cmd.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally
		{
			con.Close ();
		}
		
	}
	public static void incrementTimesComplete(int levelIndex, int increment)
	{
		string update = "UPDATE PlayerInfoByLevels SET TimesPlayed = TimesComplete+ " + increment + " WHERE LevelIndex = " + levelIndex + ";";
		try{
			
			connectToDB ();
			SqliteCommand cmd = con.CreateCommand ();
			
			cmd.CommandText = update;
			cmd.ExecuteNonQuery ();
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE ERROR: " + e + "\nDataBase NOT UPDATED");
		}
		finally
		{
			con.Close ();
		}
		
	}
	
	//********************Get Values from Tables**********************************//

	//Level Info Table

	public static int getTotalPickUps (int levelIndex)
	{
		string query = "SELECT TotalPickUps FROM LevelInfo WHERE levelIndex = " + levelIndex + ";";
		try 
		{
			connectToDB ();
			
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = query;
			
			SqliteDataReader reader = cmd.ExecuteReader ();
			reader.Read ();
			int perc = reader.GetInt32(0);
			reader.Close ();
			return perc;
		} 
		catch (SqliteException e) 
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		} 
		finally 
		{
			con.Close ();
		} 
	}

	public static bool getLevelComplete(int levelIndex)
	{
		string query = "SELECT Complete FROM LevelInfo WHERE levelIndex = " + levelIndex + ";";
		try 
		{
			connectToDB ();
			
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = query;
			
			SqliteDataReader reader = cmd.ExecuteReader ();
			reader.Read ();
			bool complete = reader.GetBoolean(0);
			reader.Close ();
			return complete;
		} 
		catch (SqliteException e) 
		{
			Debug.Log ("DATABASE error: " + e);
			return false;
		} 
		finally 
		{
			con.Close ();
		} 
	}

	public static bool getSecretEnding(int levelIndex)
	{
		string query = "SELECT SecretExit FROM LevelInfo WHERE levelIndex = " + levelIndex + ";";
		try 
		{
			connectToDB ();
			
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = query;
			
			SqliteDataReader reader = cmd.ExecuteReader ();
			reader.Read ();
			bool exit = reader.GetBoolean(0);
			reader.Close ();
			return exit;
		} 
		catch (SqliteException e) 
		{
			Debug.Log ("DATABASE error: " + e);
			return false;
		} 
		finally 
		{
			con.Close ();
		} 
	}

	public static int getPossibleHighScore(int levelIndex)
	{
		string query = "SELECT HighScorePossible FROM LevelInfo WHERE levelIndex = " + levelIndex + ";";
		try 
		{
			connectToDB ();
			
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = query;
			
			SqliteDataReader reader = cmd.ExecuteReader ();
			reader.Read ();
			int score = reader.GetInt32 (0);
			reader.Close ();
			return score;
		} 
		catch (SqliteException e) 
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		} 
		finally 
		{
			con.Close ();
		} 
	}

	//PickUP Info Table

	public static int getFartJuice(string name)
	{
		string query = "SELECT PUFartJuice FROM PickUpInfo WHERE PUName = " + name + ";";
		try 
		{
			connectToDB ();
			
			SqliteCommand cmd = con.CreateCommand ();
			cmd.CommandText = query;
			
			SqliteDataReader reader = cmd.ExecuteReader ();
			reader.Read ();
			int juice = reader.GetInt32 (0);
			reader.Close ();
			return juice;
		} 
		catch (SqliteException e) 
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		} 
		finally 
		{
			con.Close ();
		} 
	}


	public static int getLives ()
	{
		try{
			connectToDB ();
			string query = "SELECT CurrentLives FROM PlayerInfo";

			SqliteCommand cmd3 = con.CreateCommand ();

			cmd3.CommandText = query;
			SqliteDataReader read = cmd3.ExecuteReader();
			read.Read ();
			int num = read.GetInt32 (0);
			read.Close ();
			return num;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		}
		finally
		{
			con.Close ();
		}
	}

	public static int getTokens ()
	{
		try{
			connectToDB ();
			string query = "SELECT CurrentTokens FROM PlayerInfo";
			
			SqliteCommand cmd3 = con.CreateCommand ();
			
			cmd3.CommandText = query;
			SqliteDataReader read = cmd3.ExecuteReader();
			read.Read ();
			int num = read.GetInt32 (0);
			read.Close ();
			return num;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		}
		finally
		{
			con.Close ();
		}
	}

	public static int getLevelProgress()
	{
		try{
			connectToDB ();
		
			SqliteCommand cmd1 = con.CreateCommand ();
			string query = "SELECT levelProgress FROM PlayerInfo";
			cmd1.CommandText = query;
			SqliteDataReader level = cmd1.ExecuteReader ();
			level.Read ();
			int index = level.GetInt32 (0);
			level.Close ();
			return index;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		}
		finally
		{
			con.Close ();
		} 
	}

	public static int getLastLevel()
	{
		try{
			connectToDB ();
			
			SqliteCommand cmd1 = con.CreateCommand ();
			string query = "SELECT lastLevelBeaten FROM PlayerInfo";
			cmd1.CommandText = query;
			SqliteDataReader level = cmd1.ExecuteReader ();
			level.Read ();
			int index = level.GetInt32 (0);
			level.Close ();
			return index;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		}
		finally
		{
			con.Close ();
		} 
	}

	public static int getTimesDied()
	{
		try{
			connectToDB ();
			
			SqliteCommand cmd1 = con.CreateCommand ();
			string query = "SELECT timesDied FROM PlayerInfo";
			cmd1.CommandText = query;
			SqliteDataReader level = cmd1.ExecuteReader ();
			level.Read ();
			int deaths = level.GetInt32 (0);
			level.Close ();
			return deaths;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		}
		finally
		{
			con.Close ();
		} 
	}

	//PlayerInfoByLevel Table

	public static int getHighScore (int levelIndex)
	{
		try{
			connectToDB ();
			string query = "SELECT HighScore FROM PlayerInfoByLevels WHERE levelIndex = " + levelIndex+";";
			
			SqliteCommand cmd3 = con.CreateCommand ();
			
			cmd3.CommandText = query;
			SqliteDataReader read = cmd3.ExecuteReader();
			read.Read ();
			int num = read.GetInt32 (0);
			read.Close ();
			return num;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1;
		}
		finally
		{
			con.Close ();
		}
	}

	public static TimeSpan getFastestTime (int levelIndex)
	{
		try{
			connectToDB ();
			string query = "SELECT HighScore FROM PlayerInfoByLevels WHERE levelIndex = " + levelIndex+";";
			
			SqliteCommand cmd3 = con.CreateCommand ();
			
			cmd.CommandText = query;
			SqliteDataReader read = cmd.ExecuteReader();
			read.Read ();
			TimeSpan levelTime = new TimeSpan(read.GetInt64(0));
			read.Close ();
			return levelTime;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return TimeSpan.Zero ;
		}
		finally
		{
			con.Close ();
		}
	}

	public static int getHighestPUPercent (int levelIndex)
	{
		try{
			connectToDB ();
			string query = "SELECT HighestPercentPickUps FROM PlayerInfoByLevels WHERE levelIndex = " + levelIndex+";";
			
			SqliteCommand cmd3 = con.CreateCommand ();
			
			cmd.CommandText = query;
			SqliteDataReader read = cmd.ExecuteReader();
			read.Read ();
			int perc = read.GetInt32(0);
			read.Close ();
			return perc;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1 ;
		}
		finally
		{
			con.Close ();
		}
	}

	public static int getTimesPlayed (int levelIndex)
	{
		try{
			connectToDB ();
			string query = "SELECT TimesPlayed FROM PlayerInfoByLevels WHERE levelIndex = " + levelIndex+";";
			
			SqliteCommand cmd = con.CreateCommand ();
			
			cmd.CommandText = query;
			SqliteDataReader read = cmd.ExecuteReader();
			read.Read ();
			int times = read.GetInt32(0);
			read.Close ();
			return times;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1 ;
		}
		finally
		{
			con.Close ();
		}
	}

	public static int getTimesCompleted (int levelIndex)
	{
		try{
			connectToDB ();
			string query = "SELECT TimesCompleted FROM PlayerInfoByLevels WHERE levelIndex = " + levelIndex+";";
			
			SqliteCommand cmd = con.CreateCommand ();
			
			cmd.CommandText = query;
			SqliteDataReader read = cmd.ExecuteReader();
			read.Read ();
			int times = read.GetInt32(0);
			read.Close ();
			return times;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return -1 ;
		}
		finally
		{
			con.Close ();
		}
	}

	public static bool getSecretExitFound (int levelIndex)
	{
		try{
			connectToDB ();
			string query = "SELECT SecretExitFound FROM PlayerInfoByLevels WHERE levelIndex = " + levelIndex+";";
			
			SqliteCommand cmd = con.CreateCommand ();
			
			cmd.CommandText = query;
			SqliteDataReader read = cmd.ExecuteReader();
			read.Read ();
			bool times = read.GetBoolean(0);
			read.Close ();
			return times;
		}
		catch(SqliteException e)
		{
			Debug.Log ("DATABASE error: " + e);
			return false ;
		}
		finally
		{
			con.Close ();
		}
	}

	

	public static float calcPickUpPercentage(int levelIndex, int PUCount)
	{
		float total = getTotalPickUps (levelIndex);
		if ( total == -1) 
		{
			Debug.Log ("Database error could not calculate total");
			return 0;
		} 
		else 
		{
			return (PUCount/total)*100;
		}
	}

	public static void TestDB()
	{
		/*string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum > 5;";
		
		//Debug.Log ("Connection: " + con.State ());
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();

		cmd.CommandText = update;
		cmd.ExecuteNonQuery ();
		cmd.CommandText = "SELECT*FROM TestTable;";
		
		
		SqliteDataReader reader = cmd.ExecuteReader();
		
		while (reader.Read()) 
		{
			Debug.Log (reader ["ID"] + ", " + reader ["testNum"]);
		}

		reader.Close ();
		con.Close ();*/
		//Debug.Log ("Connection: " + con.State ());
		int num = getLives ();
		Debug.Log ("Lives: " + num);
	}

}
