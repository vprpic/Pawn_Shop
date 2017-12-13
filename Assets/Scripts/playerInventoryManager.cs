using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour {

	private List<Item> itemList;                        //list of items to put into rows
	private List<GameObject> instantiatedItemsList;     //list of rows from the table
	

	public GameObject listedItem;						//row prefab
	public GameObject scrollView;						//parent of the instantiated rows
	public InputField sfwInputField;					//where the user inputs the 'where' for the sql query

	// Use this for initialization
	void Start () {
		instantiatedItemsList = new List<GameObject>();
		//DBManager.ExecuteSQLCode("INSERT INTO playerInventory VALUES(null, 'Wooden Axe', 2, 5, 'Weapon')");
		itemList = DBManager.GetItemsFromTable("playerInventory");
		FillPlayerInventoryTable();
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
				instantiatedItemsList.Add((GameObject)Instantiate(listedItem, new Vector3(0, 0, 0), Quaternion.identity));
				instantiatedItemsList[instantiatedItemsList.Count - 1].transform.SetParent(scrollView.transform, false);
				//Debug.Log("child number: " + instantiatedItemsList[instantiatedItemsList.Count - 1].transform.GetChild(0).childCount);

				//set data from table into the newly created row
				for(int i = 1; i < 5; i++)
				{
					itemText = instantiatedItemsList[instantiatedItemsList.Count - 1].transform.GetChild(0).GetChild(i).GetComponent<Text>();
					switch (i)
					{
						case 1:
							itemText.text = item.Id.ToString();
							break;
						case 2:
							itemText.text = item.Name;
							break;
						case 3:
							itemText.text = item.BuyPrice.ToString();
							break;
						case 4:
							itemText.text = item.SellPrice.ToString();
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
		itemList = DBManager.GetItemsFromTable("playerInventory");
		FillPlayerInventoryTable();
	}

	//called when the player stops inputing in the 'whereInputField'
	//runs the inputted sql query and returns the values
	public void OnSFWEndEdit()
	{
		KillAllPlayerInventoryChildren();
		itemList = DBManager.GetItemsFromTable("playerInventory", sfwInputField.text);
		FillPlayerInventoryTable();
	}
}
