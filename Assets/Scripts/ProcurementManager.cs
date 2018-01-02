using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProcurementManager : MonoBehaviour {

	private List<Item> procurementItemList;							//list of items to put into rows
	private List<GameObject> procurementInstantiatedItemsList;		//list of rows from the table
	private static List<string> itemsToInsertIntoPlayerItems;					//a list of item ids - used for adding into the playerItems table


	public GameObject procurementListedItemPrefab;					//row prefab
	public GameObject scrollView;									//parent of the instantiated rows
	public Animator procurementAnimator;

	public void EnterProcurementManagerScreen()
	{
		//TODO: check if playerInventory is open
		procurementAnimator.SetBool("IsOpen", true);
	}
	public void ExitProcurementManagerScreen()
	{
		//StopAllCoroutines();
		procurementAnimator.SetBool("IsOpen", false);
	}

	public void Start()
	{
		procurementInstantiatedItemsList = new List<GameObject>();
		itemsToInsertIntoPlayerItems = new List<string>();
		procurementItemList = DBManager.ProcurementGetItemsFromTable();
		FillProcurementInventoryTable();
	}
	
	public void RunProcurement()
	{
		EnterProcurementManagerScreen();
		//TODO: enter merchant
		
	}

	//after the button exit procurement is pressed
	public void EndProcurement()
	{
		Debug.Log("button pressed");

		if(itemsToInsertIntoPlayerItems.Count > 0)
		{
			DBManager.AddItemsToPlayerItems(itemsToInsertIntoPlayerItems);
		}

		if(DBManager.ReturnFirstInt("SELECT count(*) FROM playerItems") >= 10)
		{
			//TODOFIRST: add all the bought items to the playerItems table with the sell_price as (int) buy_price*1.3
			ExitProcurementManagerScreen();
			//TODO: exit merchant
			PlayScreenManager.ExitProcurementScreenEnterPlayerInventory();
		}

		itemsToInsertIntoPlayerItems.Clear();
	}

	//instantiate and fill rows with data from itemList
	private void FillProcurementInventoryTable()
	{
		Text itemText;
		if (procurementItemList.Count < 1)
			Debug.Log("Empty list");
		else
			foreach (Item item in procurementItemList)
			{
				procurementInstantiatedItemsList.Add((GameObject)Instantiate(procurementListedItemPrefab, new Vector3(0, 0, 0), Quaternion.identity));
				procurementInstantiatedItemsList[procurementInstantiatedItemsList.Count - 1].transform.SetParent(scrollView.transform, false);

				//set data from table into the newly created row
				for (int i = 1; i < 5; i++)
				{
					itemText = procurementInstantiatedItemsList[procurementInstantiatedItemsList.Count - 1].transform.GetChild(0).GetChild(i).GetComponent<Text>();
					switch (i)
					{
						case 0:
							//TODO: set image + for loop starts at 0
							break;
						case 1:
							itemText.text = item.IdItem.ToString();
							procurementInstantiatedItemsList[procurementInstantiatedItemsList.Count - 1].name = itemText.text + ';' + item.BuyPrice.ToString();
							break;
						case 2:
							itemText.text = item.Name;
							break;
						case 3:
							itemText.text = item.BuyPrice.ToString();
							break;
						case 4:
							itemText.text = item.Type;
							break;
					}
				}
				//Debug.Log("Item added");
			}
	}

	public void OnProcurementItemClicked()
	{
		int buyPrice;
		int.TryParse(EventSystem.current.currentSelectedGameObject.name.Split(';')[1], out buyPrice);
		Debug.Log(" buyPrice: " + buyPrice);
		if(PlayScreenManager.DecreaseCoinCounter(buyPrice))
		{
			itemsToInsertIntoPlayerItems.Add(EventSystem.current.currentSelectedGameObject.name);
		}
		//Debug.Log(itemsToInsertIntoPlayerItems.Count);
	}
}
