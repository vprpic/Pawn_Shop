﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreenManager : MonoBehaviour
{
	private static Text coinCountText;
	private static int numOfItemsInTable;

	public static int coinCounter;
	public static CustomerManager customerManager;
	public static PlayerInventoryManager playerInventoryManager;
	public static ProcurementManager procurementManager;
	
	//checks if the number of items in the table is less than 10, so that we can start the procurement
	//return true if starting procurement
	public static bool DoWeNeedToRunProcurement()
	{
		numOfItemsInTable = DBManager.ReturnFirstInt("SELECT count(*) FROM playerItems");
		if(numOfItemsInTable < 10)
		{
			//start procurement
			playerInventoryManager.ExitPlayerInventoryScreen();
			procurementManager.RunProcurement();
			return true;
		}
		else
		{
			return false;
		}
	}

	// Use this for initialization
	void Start()
	{
		coinCounter = 30;
		numOfItemsInTable = DBManager.ReturnFirstInt("SELECT count(*) FROM playerItems");
		coinCountText = GameObject.Find("coinCountText").GetComponent<Text>();
		customerManager = new CustomerManager();
		playerInventoryManager = GameObject.FindObjectOfType<PlayerInventoryManager>();
		procurementManager = GameObject.FindObjectOfType<ProcurementManager>();
		coinCountText.text = coinCounter.ToString();
		customerManager.Init();
		playerInventoryManager.EnterPlayerInventoryScreen();
	}

	public static void IncreaseCoinCounter(int coinsToAdd)
	{
		coinCounter += coinsToAdd;
		//add coin animation
		coinCountText.text = coinCounter.ToString();
	}
	public static void DecreaseCoinCounter(int coinsToRemove)
	{
		//TODO: if coinCount <= 0 end game
		coinCounter -= coinsToRemove;
		//remove coin animation
		coinCountText.text = coinCounter.ToString();
	}


}