using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInventoryManager : MonoBehaviour {

    private List<Item> itemList;                        //list of items to put into rows
    private List<GameObject> instantiatedItemsList;     //list of rows from the table

    public GameObject listedItem;                       //row prefab
    public GameObject scrollView;                       //parent of the instantiated rows

    // Use this for initialization
    void Start () {
        instantiatedItemsList = new List<GameObject>();
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
}
