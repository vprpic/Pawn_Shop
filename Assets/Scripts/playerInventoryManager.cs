using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour {

	private List<Item> itemList;                        //list of items to put into rows
	private List<GameObject> instantiatedItemsList;     //list of rows from the table
	

	public GameObject listedItemPrefab;						//row prefab
	public GameObject scrollView;						//parent of the instantiated rows
	public InputField sfwInputField;                    //where the user inputs the 'where' for the sql query
	public Animator playerInventory;

	public void EnterPlayerInventoryScreen()
	{
		playerInventory.SetBool("IsOpen", true);
	}
	public void ExitPlayerInventoryScreen()
	{
		StopAllCoroutines();
		playerInventory.SetBool("IsOpen", false);
	}

	public void Start() {
		instantiatedItemsList = new List<GameObject>();
		DBManager.addTablesDictionary();
		//DBManager.ExecuteSQLCode("INSERT INTO playerItems VALUES(null, 'Wooden Axe', 2, 5, 'Weapon')");
		itemList = DBManager.GetItemsForUpdateTable();
		FillPlayerInventoryTable();
		//Debug.Log("Run insert into sales_log");
		//TODO: DBManager.ExecuteSQLCode("INSERT INTO sales_log(id_item,entry_date,sell_price) VALUES(1, datetime('now'), 5)");
	}

	//instantiate and fill rows with data from itemList
	private void FillPlayerInventoryTable()
	{
		Text itemText;
		if (itemList.Count < 1)
			Debug.Log("Empty list");
		else
			foreach (Item item in itemList)
			{
				instantiatedItemsList.Add((GameObject)Instantiate(listedItemPrefab, new Vector3(0, 0, 0), Quaternion.identity));
				instantiatedItemsList[instantiatedItemsList.Count - 1].transform.SetParent(scrollView.transform, false);
				//Debug.Log("child number: " + instantiatedItemsList[instantiatedItemsList.Count - 1].transform.GetChild(0).childCount);

				//set data from table into the newly created row
				for(int i = 1; i < 6; i++)
				{
					itemText = instantiatedItemsList[instantiatedItemsList.Count - 1].transform.GetChild(0).GetChild(i).GetComponent<Text>();
					switch (i)
					{
						case 0:
							//TODO: set image + for loop starts at 0
							break;
						case 1:
							itemText.text = item.Id.ToString();
							break;
						case 2:
							itemText.text = item.IdItem.ToString();
							break;
						case 3:
							itemText.text = item.Name;
							break;
						case 4:
							itemText.text = item.SellPrice.ToString();
							break;
						case 5:
							itemText.text = item.Type;
							break;
					}
				}
				//Debug.Log("Item added");
			}
	}

	//destroys each instantiated row in the table
	private void KillAllPlayerInventoryChildren()
	{
		foreach(GameObject row in instantiatedItemsList)
		{
			GameObject.Destroy(row);
		}
		instantiatedItemsList.Clear();
		itemList.Clear();
	}

	//when the 'updateTableButton' is pressed this empties the instantiated table and fills it with real values from the db table
	public void OnUpdateTableButton()
	{
		KillAllPlayerInventoryChildren();
		itemList = DBManager.GetItemsForUpdateTable();
		FillPlayerInventoryTable();
	}

	//called when the player presses enter or clicks out of the box in the 'whereInputField'
	//runs the inputted sql query and returns the values
	public void OnSFWEndEdit()
	{
		string inputText = sfwInputField.text;
		KillAllPlayerInventoryChildren();
		itemList = DBManager.Level1GetItemsFromTable(inputText);
		FillPlayerInventoryTable();
	}

	//called when the player presses the check button while it's the first level
	//
	public void OnCheckButtonLevel1()
	{
		if (CustomerManager.canTest)
		{
			string inputText = sfwInputField.text;
			KillAllPlayerInventoryChildren();
			itemList = DBManager.Level1GetItemsFromTable(inputText);
			FillPlayerInventoryTable();
			//test if the user input is correct
			//Debug.Log("is this the current customer? "+CustomerManager.GetCurrentCustomer().Name);
			Debug.Log("real answer: "+CustomerManager.GetCurrentCustomer().Request.SqlCode);
			bool userInputTest = DBManager.TestUserInputedQueryAgainstRequestCode(inputText, CustomerManager.GetCurrentCustomer().Request.SqlCode);
			Debug.Log("INPUT: "+userInputTest);
			if (userInputTest) //if the player's answer was correct remove random item from table, increase coins by the price and replace the customer
			{
				int priceOfItem = DBManager.SelectAndRemoveRandomItemFromPlayerItems(inputText);
				PlayScreenManager.IncreaseCoinCounter(priceOfItem);
				StartCoroutine(PlayScreenManager.customerManager.CorrectAnswer());
			}
			else //if the player was wrong animate the customer, show the failed purchase text and replace the customer
			{
				PlayScreenManager.DecreaseCoinCounter(1);
				StartCoroutine(PlayScreenManager.customerManager.WrongAnswer());
			}
		}
	}
}

/* TODO: Define the structure for the levels
 * TODO: enter the data into the tables
 * TODO: purchasing
 * TODO: questonnaires
 * EXTRAIDEAS: finish this plz
 */
