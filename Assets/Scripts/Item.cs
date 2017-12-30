using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item  {

	private int id;
	private int idItem;
	private string name;
	private int buyPrice;
	private int sellPrice;
	private string type;
	private string image;
	private string description;

	public Item(int newId, int newIdItem, string newName, int newBuyPrice, int newSellPrice, string newType, string newDescription, string newImage)
	{
		id = newId;
		idItem = newIdItem;
		name = newName;
		buyPrice = newBuyPrice;
		sellPrice = newSellPrice;
		type = newType;
		image = newImage;
		description = newDescription;
	}

	public Item(int newIdItem, string newName, int newBuyPrice, string newType)
	{
		idItem = newIdItem;
		name = newName;
		buyPrice = newBuyPrice;
		type = newType;
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

	public int IdItem
	{
		get
		{
			return idItem;
		}

		set
		{
			idItem = value;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}

		set
		{
			name = value;
		}
	}

	public int BuyPrice
	{
		get
		{
			return buyPrice;
		}

		set
		{
			buyPrice = value;
		}
	}

	public int SellPrice
	{
		get
		{
			return sellPrice;
		}

		set
		{
			sellPrice = value;
		}
	}

	public string Type
	{
		get
		{
			return type;
		}

		set
		{
			type = value;
		}
	}

	public string Image
	{
		get
		{
			return image;
		}

		set
		{
			image = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}

		set
		{
			description = value;
		}
	}
}
