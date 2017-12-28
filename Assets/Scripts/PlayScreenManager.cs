using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreenManager : MonoBehaviour
{
	private static Text coinCountText;
	private static int numOfItemsInTable;

	public static int coinCounter;
	public static CustomerManager customerManager;

	
	public static int NumOfItemsInTable
	{
		get
		{
			return numOfItemsInTable;
		}

		set
		{
			numOfItemsInTable = value;
		}
	}

	// Use this for initialization
	void Start()
	{
		coinCounter = 30;
		numOfItemsInTable = DBManager.ReturnFirstInt("SELECT count(*) FROM playerItems");
		coinCountText = GameObject.Find("coinCountText").GetComponent<Text>();
		customerManager = new CustomerManager();
		coinCountText.text = coinCounter.ToString();
		customerManager.Init();
	}

	public static void IncreaseCoinCounter(int coinsToAdd)
	{
		coinCounter += coinsToAdd;
		//add coin animation
		coinCountText.text = coinCounter.ToString();
	}
	public static void DecreaseCoinCounter(int coinsToRemove)
	{
		coinCounter -= coinsToRemove;
		//remove coin animation
		coinCountText.text = coinCounter.ToString();
	}


}
