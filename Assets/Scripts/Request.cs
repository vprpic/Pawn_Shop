﻿using System.Collections;
using System.Collections.Generic;

public class Request  {

	private int id;
	private string requestText;
	private string sqlCode;
	private int rarity;
	private int rowCount;

	public Request(int newId, string newRequestText, string newSqlCode, int newRarity)
	{
		id = newId;
		requestText = newRequestText;
		sqlCode = newSqlCode;
		rarity = newRarity;
	}
	public Request(int newId, string newRequestText, string newSqlCode, int newRarity, int newRowCount)
	{
		id = newId;
		requestText = newRequestText;
		sqlCode = newSqlCode;
		rarity = newRarity;
		rowCount = newRowCount;
	}
	public int Id
	{
		get
		{
			return id;
		}

		set
		{
			id = value;
		}
	}
	public string RequestText
	{
		get
		{
			return requestText;
		}

		set
		{
			requestText = value;
		}
	}
	public string SqlCode
	{
		get
		{
			return sqlCode;
		}

		set
		{
			sqlCode = value;
		}
	}
	public int Rarity
	{
		get
		{
			return rarity;
		}

		set
		{
			rarity = value;
		}
	}
	public int RowCount
	{
		get
		{
			return rowCount;
		}

		set
		{
			rowCount = value;
		}
	}

}
