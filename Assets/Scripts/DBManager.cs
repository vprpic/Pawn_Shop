using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;


public class DBManager {

    private static string connectionString = "URI=file:" + Application.dataPath + "/DB/DB.db";
    private static List<Item> itemList = new List<Item>();
    
    //connects to the DB database and returns the values from it
    static public List<Item> GetItemsFromTable(string table)
    {
        //when 'using' ends it calls .dispose() which disposes of the connection
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using(IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM " + table;
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

}
