using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DBManager {
	//a dictionary of string and dictionary - holds the names of all tables and their attributes and the real names of the attributes
	private static Dictionary<string, Dictionary<string,string>> allTables = new Dictionary<string, Dictionary<string,string>>();
	private static string connectionString = "URI=file:" + Application.dataPath + "/DB/DB.db";		//path to the database in the Assets
	private static List<Item> itemList = new List<Item>();                                          //list of items in the table

	//where we write all the names of the tables and of their attributes and their real names from the database
	//to be used for translating user input from game tables to real tables
	//TODO: check table names
	public static void addTablesDictionary()
	{
		allTables.Add("playerItems", new Dictionary<string, string> {{ "id_item", "playerItems.id_item"},
																	{ "name","items.name"},
																	{ "price" ,"playerItems.sell_price"},
																	{ "type","items.type"}});
		/*
		 * allTables.Values.ToList();	//so we can access all values from the dictionary
		 * allTables.SelectMany(??)		//need to research
		 * TODO: dohvatimo ime tablice is user inputa - atribute te tablice (dictionary) spremimo u listu (dictionaryja) možemo lakše pretraživati sve atribute i njihova stvarna imena
		*/
	}

	//connects to the DB database and returns the values from the playerItems list for updating the table on screen
	public static List<Item> GetItemsForUpdateTable()
	{
		string sqlQuery = "SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price, p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i JOIN playerItems as p ON i.id_item=p.id_item";
		//Debug.Log("GetItemsForUpdateTable");

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
						itemList.Add(new Item(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
						//Debug.Log("Added " + reader.GetString(2) + " to itemList");
					}

					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return itemList;
	}

	//takes the complete SQL code and runs it in the db - meant for updates, deletes and other, not for selects (doesn't return anything)
	//not to be used for player input but for updates to the tables e.g. removing an item from the playerItems table when a customer buys it
	public static void ExecuteSQLCode(String code)
	{
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = code;
				dbCmd.ExecuteNonQuery();
				dbConnection.Close();
			}
		}
	}

	//Level 1 - there is a predefined select and from part
	//connects to the DB database and returns the values from the playerItems list for updating the table on screen with the added where part from user input
	//TODO: if sqlite returns an error?
	public static List<Item> Level1GetItemsFromTable(string whereCode = "")
	{
		string sqlQuery = ("SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price, p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i JOIN playerItems as p ON i.id_item=p.id_item WHERE " + whereCode).ToString();
		if (whereCode.Equals(""))
			return GetItemsForUpdateTable();
		//Debug.Log(sqlQuery);

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					while (reader.Read())
					{
						itemList.Add(new Item(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
						//Debug.Log("Added " + reader.GetString(2) + " to itemList");
					}

					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return itemList;
	}

	public static void SelectRandomCustomer(out int id, out string name, out string catchphrase, out Sprite image, out string buyingText, out string failedPurchaseText)
	{
		string sqlQuery = "select * from customers order by random() limit 1";
		//Debug.Log(sqlQuery);

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					id = reader.GetInt32(0);
					name = reader.GetString(1);
					catchphrase = reader.GetString(2);
					image = Resources.Load<Sprite>(reader.GetString(3));
					buyingText = reader.GetString(4);
					failedPurchaseText = reader.GetString(5);
					//Debug.Log("Added customer: " + reader.GetString(1) + "image: "+ reader.GetString(3));
					
					dbConnection.Close();
					reader.Close();
				}
			}
		}
	}

	public static void SelectRandomRequest(out int id, out string requestText, out string sqlCode, out int rarity, out int rowCount)
	{
		string sqlQuery = "select cr.id_request as id,r.request as requestText,r.sql_code as sqlCode,cr.rarity as rarity from customers_requests as cr join requests as r on cr.id_request = r.id_request order by random() limit 1";
		//Debug.Log(sqlQuery);

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					id = reader.GetInt32(0);
					requestText = reader.GetString(1);
					sqlCode = reader.GetString(2);
					rarity = reader.GetInt32(3);
					//Debug.Log("Added request: " + reader.GetString(1));

					reader.Close();
				}
					//substring removes the "SELECT" part of the code
					//if the count of the rows in the table is less than 1 we call the function again to reset the request to something we need
					string testCode ="SELECT count(*),"+sqlCode.Substring(6);

					dbCmd.CommandText = testCode;
					

					using (IDataReader reader = dbCmd.ExecuteReader())
					{
						reader.Read();
						rowCount = reader.GetInt32(0);
						//Debug.Log("ROWCOUNT: " + rowCount);
						//Debug.Log("Entered");

						//test if the request has any items in the table but not every time, only if the random number is < 70
						//so that the player doesn't run into empty tables too often
						//EXTRAIDEAS: increase the number if you get too many tables with no items ( && (int)UnityEngine.Random.Range(1, 101) < 70)
						if (rowCount<1 && (int)UnityEngine.Random.Range(1, 101) < 90)
							{
								SelectRandomRequest(out id, out requestText, out sqlCode, out rarity, out rowCount);
								//Debug.LogWarning("Request has 0 items in table, calling for it again");
							}

						reader.Close();
					}
					dbConnection.Close();
			}
		}
	}

	//
	public static bool TestUserInputedQueryAgainstRequestCode(int rowCount, string userCode, string sqlCode)
	{
		string sqlQuery = "SELECT count(*) FROM ( " + sqlCode + " UNION SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price, p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i JOIN playerItems as p ON i.id_item=p.id_item WHERE " + userCode + " )";
		Debug.Log(sqlQuery);

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					//if the number of rows in both of the tables (user input and answer) is the same the player answered correctly
					//ofc if the tables just happen to have the same rows it will also register as correct
					int num = reader.GetInt32(0);
					reader.Close();
					dbConnection.Close();
					//Debug.Log("RowCount: " + rowCount + " num: " + num);
					if (rowCount == num && rowCount != 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}
	}

	//used after the player has correctly answered the customer request
	//returns the sellPrice which is used for reducing the number of coins
	public static int SelectAndRemoveRandomItemFromPlayerItems(string inputText)
	{
		int sellPrice, idPlayerItem;
		bool empty = false;
		string sqlQuery = "SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price, p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i JOIN playerItems as p ON i.id_item=p.id_item WHERE " + inputText + " ORDER BY random() LIMIT 1";
		Debug.Log("delete rand: "+sqlQuery);

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					try
					{
						idPlayerItem = reader.GetInt32(0);
						sellPrice = reader.GetInt32(4);
					}catch(Exception e)
					{
						empty = true;
						idPlayerItem = 0;
						sellPrice = 0;
						Debug.LogWarning(e);
					}

					reader.Close();
				}
				if (!empty)
				{
					dbCmd.CommandText = "DELETE FROM playerItems WHERE id_player_item = " + idPlayerItem;
					dbCmd.ExecuteNonQuery();
					Debug.Log("DELETING: " + idPlayerItem);
				}
				dbConnection.Close();
			}
		}
		return sellPrice;
	}
}
