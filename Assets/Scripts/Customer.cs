﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer {

	private int id;
	private string name;
	private string catchphrase;
	private Sprite image;
	private string buyingText;
	private string failedPurchaseText;
	private Request request;

	public Customer(int newId, string newName, string newCatchphrase, Sprite newImage, string newBuyingText, string newFailedPurchaseText)
	{
		id = newId;
		name = newName;
		catchphrase = newCatchphrase;
		image = newImage;
		buyingText = newBuyingText;
		failedPurchaseText = newFailedPurchaseText;
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
	public string Catchphrase
	{
		get
		{
			return catchphrase;
		}

		set
		{
			catchphrase = value;
		}
	}
	public Sprite Image
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
	public string BuyingText
	{
		get
		{
			return buyingText;
		}

		set
		{
			buyingText = value;
		}
	}
	public string FailedPurchaseText
	{
		get
		{
			return failedPurchaseText;
		}

		set
		{
			failedPurchaseText = value;
		}
	}
	public Request Request
	{
		get
		{
			return request;
		}

		set
		{
			request = value;
		}
	}

}