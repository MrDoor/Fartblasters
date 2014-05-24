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
		string sql = "UPDATE PickUpInfo Set PUTotal = PUTotal +" + count + "WHERE PUName = " + name + ";";
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();
		
		cmd.CommandText = update;
		cmd.ExecuteNonQuery ();
		con.Close ();
	}

	public static void updateLevelInfo()
	{
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();
		
		cmd.CommandText = update;

		SqliteDataReader current = cmd.ExecuteReader () ;
		string sql;
		con.Close ();
	}
}
