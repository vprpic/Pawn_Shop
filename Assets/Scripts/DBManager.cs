using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;


public class DBManager {

	//used for saving the list of attributes connected to a table
	private class Table
	{
		string name { get; set; }
		List<string> attributes { get; set; }
	}


	private static string connectionString = "URI=file:" + Application.dataPath + "/DB/DB.db";	//path to the database in the Assets
	private static List<Item> itemList = new List<Item>();										//list of items in the table
	private List<Table> tables;
	
	//connects to the DB database and returns the values from it
	static public List<Item> GetItemsFromTable(string table, string whereCode = "")
	{
		string sqlQuery = ("SELECT * FROM " + table).ToString();
		if (!whereCode.Equals(""))
			sqlQuery = (sqlQuery + " WHERE " + whereCode).ToString();
		Debug.Log(sqlQuery);

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using(IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					while (reader.Read())
					{
						itemList.Add(new Item(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4)));
						//Debug.Log("Added " + reader.GetString(1) + " to itemList");
					}

					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return itemList;
	}

	//takes the complete SQL code and runs it in the db - meant for updates, deletes and other, not for selects (doesn't return anything)
	static public void ExecuteSQLCode(String code)
	{
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = code;
				dbConnection.Close();
			}
		}
	}
}
