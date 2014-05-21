using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;

public class TestDBScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE ID = 1;";
		SqliteConnection con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();
		SqliteCommand cmd = con.CreateCommand ();

		cmd.CommandText = update;
		cmd.ExecuteNonQuery ();
		con.Close ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
