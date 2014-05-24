using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;

public class TestDBScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum < 5;";
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
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
		con.Close ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
