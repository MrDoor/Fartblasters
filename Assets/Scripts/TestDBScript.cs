using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;

public class TestDBScript : MonoBehaviour {
	SqliteConnection con;
	// Use this for initialization
	void Start () {


		DBFunctions.TestDB ();
		/*string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum > 5;";


		string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum > 5;";

		connectDB ();

		SqliteCommand cmd = con.CreateCommand ();

		cmd.CommandText = update;
		cmd.ExecuteNonQuery ();
		cmd.CommandText = "SELECT*FROM TestTable;";


		SqliteDataReader reader = cmd.ExecuteReader();

		while (reader.Read()) 
		{
			Debug.Log (reader ["ID"] + ", " + reader ["testNum"]);
		}

		con.Close ();*/
	

		con.Close ();
	

	}

	public void connectDB()
	{
		con = new SqliteConnection ("Data Source=Assets/TestDB");
		con.Open ();

	}
	// Update is called once per frame
	void Update () {
	
	}
}
