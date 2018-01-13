using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayScreenManager : MonoBehaviour
{
	private static Text coinCountText;
	private static int numOfItemsInTable;

	public static int coinCounter;
	public static CustomerManager customerManager;
	public static PlayerInventoryManager playerInventoryManager;
	public static ProcurementManager procurementManager;


	public static void IncreaseCoinCounter(int coinsToAdd)
	{
		coinCounter += coinsToAdd;
		//add coin animation
		coinCountText.text = coinCounter.ToString();
	}
	public static bool DecreaseCoinCounter(int coinsToRemove)
	{
		if (coinCounter - coinsToRemove < 0)
		{
			return false;
		}
		coinCounter -= coinsToRemove;
		//remove coin animation
		coinCountText.text = coinCounter.ToString();
		return true;
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
		if (!DoWeNeedToRunProcurement())
		{
			playerInventoryManager.EnterPlayerInventoryScreen();
		}
	}

	//checks if the number of items in the table is less than 10, so that we can start the procurement
	//return true if starting procurement
	public static bool DoWeNeedToRunProcurement()
	{
		numOfItemsInTable = DBManager.ReturnFirstInt("SELECT count(*) FROM playerItems");
		if (numOfItemsInTable < 10)
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

	public static void ExitProcurementScreenEnterPlayerInventory()
	{
		playerInventoryManager.EnterPlayerInventoryScreen();
		customerManager.SetupNextCustomer();
	}

	public static void ExitPlayerInventory()
	{
		playerInventoryManager.ExitPlayerInventoryScreen();
	}
	public static void EnterPlayerInventory()
	{
		playerInventoryManager.EnterPlayerInventoryScreen();
	}

	public void AnswerButtonPressed()
	{

		string answeredText = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
		
		Debug.Log(" AnswerText: " + answeredText);

		if (answeredText == CustomerManager.GetCurrentCustomer().Question.CorrectAnswer)
		{
			Debug.Log("correct answer");
			PlayScreenManager.IncreaseCoinCounter(2);
		}
		else
		{
			Debug.Log("incorrect answer");
			PlayScreenManager.DecreaseCoinCounter(2);
		}
		StartCoroutine(customerManager.Answer());
	}
}
