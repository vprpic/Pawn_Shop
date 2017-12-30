using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcurementManager : MonoBehaviour {

	private List<Item> procurementItemList;                        //list of items to put into rows
	private List<GameObject> instantiatedItemsList;     //list of rows from the table


	public GameObject procurementListedItemPrefab;                     //row prefab
	public GameObject scrollView;                       //parent of the instantiated rows
	public Animator procurementAnimator;

	public void EnterProcurementManagerScreen()
	{
		procurementAnimator.SetBool("IsOpen", true);
	}
	public void ExitProcurementManagerScreen()
	{
		StopAllCoroutines();
		procurementAnimator.SetBool("IsOpen", false);
	}

	public void Start()
	{

	}
	
	public void RunProcurement()
	{
		EnterProcurementManagerScreen();
		//TODO: enter merchant


	}

	public void EndProcurement()
	{

		//TODO: exit merchant
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
				instantiatedItemsList.Add((GameObject)Instantiate(procurementListedItemPrefab, new Vector3(0, 0, 0), Quaternion.identity));
				instantiatedItemsList[instantiatedItemsList.Count - 1].transform.SetParent(scrollView.transform, false);
				//Debug.Log("child number: " + instantiatedItemsList[instantiatedItemsList.Count - 1].transform.GetChild(0).childCount);

				//set data from table into the newly created row
				for (int i = 1; i < 6; i++)
				{
					itemText = instantiatedItemsList[instantiatedItemsList.Count - 1].transform.GetChild(0).GetChild(i).GetComponent<Text>();
					switch (i)
					{
						case 0:
							//TODO: set image + for loop starts at 0
							break;
						case 1:
							itemText.text = item.IdItem.ToString();
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
}
