using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreenManager : MonoBehaviour
{
	public static int coinCounter;
	public static CustomerManager customerManager;
	public 

	// Use this for initialization
	void Start()
	{
		coinCounter = 30;
		customerManager = new CustomerManager();
		customerManager.Init();
	}

	public static void IncreaseCoinCounter(int coinsToAdd)
	{
		coinCounter += coinsToAdd;
	}
	public static void DecreaseCoinCounter(int coinsToRemove)
	{
		coinCounter -= coinsToRemove;
	}

	
}
