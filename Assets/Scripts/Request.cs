using System.Collections;
using System.Collections.Generic;

public class Request  {

	private int id;
	private string requestText = null;
	private string sqlCode;
	private int rarity;

	public Request(int newId, string newRequestText, string newSqlCode, int newRarity)
	{
		id = newId;
		requestText = newRequestText;
		sqlCode = newSqlCode;
		rarity = newRarity;
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

}
