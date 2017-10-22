using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DBManager : MonoBehaviour {

    private string connectionString;

	// Use this for initialization
	void Start () {
        connectionString = "URI=file:" + Application.dataPath + "/DB/DB.db";
        GetItems();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GetItems()
    {
        //when using ends it calls .dispose() which disposes of the connection
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using(IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM playerInventory";
                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log(reader.GetString(1) + " " + reader.GetInt32(2));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
    }
}
