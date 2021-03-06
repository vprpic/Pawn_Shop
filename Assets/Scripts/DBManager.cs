﻿using System.Collections;
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
	private static List<Item> procurementItemList = new List<Item>();

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
		string sqlQuery = "SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price," +
			" p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i " +
			"JOIN playerItems as p ON i.id_item=p.id_item";
		
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
						itemList.Add(new Item(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), 
							reader.GetInt32(4), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
						//Debug.Log("Added " + reader.GetString(2) + " to itemList");
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
		Debug.Log("num of rows: "+itemList.Count);
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
	//connects to the DB database and returns the values from the playerItems table for updating the table on screen with the added where part from user input
	//TODO: if sqlite returns an error?
	public static List<Item> Level1GetItemsFromTable(string whereCode = "")
	{
		string sqlQuery = ("SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price," +
			" p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i " +
			"JOIN playerItems as p ON i.id_item=p.id_item WHERE " + whereCode).ToString();
		if (whereCode.Equals(""))
			return GetItemsForUpdateTable();
		//Debug.Log(sqlQuery);
		
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
						itemList.Add(new Item(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), 
							reader.GetInt32(4), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
						//Debug.Log("Added " + reader.GetString(2) + " to itemList");
					}

					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return itemList;
	}

	//connects to the DB database and returns the values from the items table for updating the procurement table in game
	public static List<Item> ProcurementGetItemsFromTable()
	{
		string sqlQuery = "SELECT id_item, name, price, type, image FROM items";

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
						procurementItemList.Add(new Item(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4)));
					}

					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return procurementItemList;
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

	public static void SelectRandomRequest(out int id, out string requestText, out string sqlCode, out int rarity)
	{
		string sqlQuery = "select cr.id_request as id,r.request as requestText,r.sql_code as sqlCode,cr.rarity as rarity from customers_requests as cr join requests as r on cr.id_request = r.id_request order by random() limit 1";
		int rowCount;

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
				string testCode = "SELECT count(*)," + sqlCode.Substring(6);
				//Debug.Log("testCode: " + testCode);

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
					if (rowCount < 1 && (int)UnityEngine.Random.Range(1, 101) < 90)
					{
						SelectRandomRequest(out id, out requestText, out sqlCode, out rarity);
						//Debug.LogWarning("Request has 0 items in table, calling for it again");
					}
					reader.Close();
				}
					
				dbConnection.Close();
			}
		}
	}

	//count the number of rows from the real answer and from the user input answer
	public static bool TestUserInputedQueryAgainstRequestCode(string userCode, string sqlCode)
	{
		//userCode = the where part from user input
		//sqlCode = the 
		string wholeUserCode = "SELECT count(*), p.id_player_item as id," +
			" i.id_item as id_item, i.name as name,i.price as buy_price, p.sell_price as sell_price, " +
			"i.type as type, i.description as description, i.image as image FROM items as i JOIN playerItems as p" +
			" ON i.id_item=p.id_item WHERE " + userCode;
		string unionSqlQuery = "SELECT count(*) FROM ( " + sqlCode + " UNION SELECT p.id_player_item as id," +
			" i.id_item as id_item, i.name as name,i.price as buy_price, p.sell_price as sell_price, " +
			"i.type as type, i.description as description, i.image as image FROM items as i JOIN playerItems as p" +
			" ON i.id_item=p.id_item WHERE " + userCode + " )";
		Debug.Log("testUserInput: "+unionSqlQuery);
		int realAnswerNum, userAnswerNum;

		//when 'using' ends it calls .dispose() which disposes of the connection
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{

				/////////////////////////

				//substring removes the "SELECT" part of the code
				//to see how many items there are in the table
				string testCode = "SELECT count(*)," + sqlCode.Substring(6);

				dbCmd.CommandText = testCode;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					realAnswerNum = reader.GetInt32(0);

					reader.Close();
				}

				/////////////////////////

				dbCmd.CommandText = wholeUserCode;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					userAnswerNum = reader.GetInt32(0);

					reader.Close();
				}

				/////////////////////////


				dbCmd.CommandText = unionSqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					//if the number of rows in both of the tables (user input and answer) is the same the player answered correctly
					//ofc if the tables just happen to have the same rows it will also register as correct
					int num = reader.GetInt32(0);
					reader.Close();
					dbConnection.Close();
					//Debug.Log("RealAnswerNum: " + realAnswerNum + " num: " + num + "UserAnswerNum: " + userAnswerNum);
					if (realAnswerNum == num && realAnswerNum != 0 && realAnswerNum == userAnswerNum)
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
		string sqlQuery = "SELECT p.id_player_item as id, i.id_item as id_item, i.name as name,i.price as buy_price," +
			" p.sell_price as sell_price, i.type as type, i.description as description, i.image as image FROM items as i" +
			" JOIN playerItems as p ON i.id_item=p.id_item WHERE " + inputText + " ORDER BY random() LIMIT 1";

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

	public static int ReturnFirstInt(string sqlCode) {
		int numOfItems;

		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				
				dbCmd.CommandText = sqlCode;

				using (IDataReader reader = dbCmd.ExecuteReader())
				{
					reader.Read();
					numOfItems = reader.GetInt32(0);
					
					reader.Close();
				}

				dbConnection.Close();
			}
		}
		return numOfItems;
	}

	public static void AddItemsToPlayerItems(List<string> addItemsList)
	{
		string sqlCode;
		using (IDbConnection dbConnection = new SqliteConnection(connectionString))
		{
			dbConnection.Open();
			foreach (string itemIdBuyPrice in addItemsList)
			{
				sqlCode = "INSERT INTO playerItems VALUES (null, " + itemIdBuyPrice.Split(';')[0] + ", " + (int)(int.Parse(itemIdBuyPrice.Split(';')[1]) * 1.3 + 1) + ")";
				Debug.Log(sqlCode);
				
				using (IDbCommand dbCmd = dbConnection.CreateCommand())
				{
					dbCmd.CommandText = sqlCode;
					dbCmd.ExecuteNonQuery();
					
				}
			}
			dbConnection.Close();
		}
	}

	public static void SelectRandomQuestion(int id_customer, out int id_question, out string questionText, out int correctAnswerId, out int rarity)
	{
		string sqlQuery = "SELECT q.id_question, question, correct_answer, rarity FROM questions q JOIN customers_questions cq ON q.id_question = cq.id_question WHERE cq.id_customer = " + id_customer + " order by random() limit 1";

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
					id_question = reader.GetInt32(0);
					questionText = reader.GetString(1);
					correctAnswerId = reader.GetInt32(2);
					rarity = reader.GetInt32(3);
					//Debug.Log("Added request: " + reader.GetString(1));

					reader.Close();

				}

				dbConnection.Close();
			}
		}
	}

	//returns the text of the correct answer and a list of 3 incorrect answers
	public static List<string> GetWrongAnswersForQuestion(int id_question, int id_correct_answer, out string correctAnswer, int rowCount = 3)
	{
		List<string> wrongAnswers = new List<string>();
		string sqlQuery = "SELECT ca.text as correct_answer, wa.text as wrong_answer FROM " +
			"(SELECT text FROM answers WHERE id_answer = " + id_correct_answer + ") ca JOIN " +
			"(SELECT text FROM answers a JOIN answers_questions aq ON a.id_answer = aq.id_answer " +
			"WHERE aq.id_question = " + id_question + " order by random() limit " + rowCount + ") wa ";
		Debug.Log(sqlQuery);
		string correctAnswer_ = null;

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
						correctAnswer_ = reader.GetString(0);
						//Debug.Log("correct: " + correctAnswer_ + " wrong: " + reader.GetString(1));
						wrongAnswers.Add(reader.GetString(1));
					}
					correctAnswer = correctAnswer_;

					dbConnection.Close();
					reader.Close();

					return wrongAnswers;
				}
			}
		}
	}
}
