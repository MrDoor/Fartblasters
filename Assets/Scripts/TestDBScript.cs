using UnityEngine;
using System.Collections;
using System.Data; 
using Mono.Data.Sqlite;

public class TestDBScript : MonoBehaviour {
	SqliteConnection con;
	// Use this for initialization
	void Start () {
<<<<<<< HEAD

		DBFunctions.TestDB ();
		/*string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum > 5;";
=======
/*
		string update = "UPDATE TestTable SET TestNum = TestNum+1 WHERE testNum > 5;";
>>>>>>> 7be1efabaa63573b3994b952f1599222fd503f86

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
<<<<<<< HEAD
		con.Close ();*/
	
=======
		con.Close ();
*/	
>>>>>>> 7be1efabaa63573b3994b952f1599222fd503f86
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
