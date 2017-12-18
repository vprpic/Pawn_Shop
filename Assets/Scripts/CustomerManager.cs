using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*	Used to manage the 5 customers that arrive in the shop with requests and questions
 *	there are always 5 customer objects which are recycled
*/
public class CustomerManager {

	public static DialogueManager dialogueManager;
	public static bool canTest;
	public Text customerName;
	public Text writtenText;
	public Animator animator;

	private static List<Customer> customerList;		//a list of 5 customers to be recycled
	private static int currentCustomerNumber;              //which customer is in front of the row
	private Image currentCustomerImage;

	public void Init()
	{
		canTest = false;
		dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		currentCustomerImage = GameObject.Find("customerImage").GetComponent<Image>();
		customerName = GameObject.Find("NameText").GetComponent<Text>();
		writtenText = GameObject.Find("WrittenText").GetComponent<Text>();
		customerList = new List<Customer>();
		currentCustomerNumber = 4;

		customerList.Add(new Customer(1, "", "", null, "", ""));
		customerList.Add(new Customer(2, "", "", null, "", ""));
		customerList.Add(new Customer(3, "", "", null, "", ""));
		customerList.Add(new Customer(4, "", "", null, "", ""));
		customerList.Add(new Customer(5, "", "", null, "", ""));

		foreach (Customer customer in customerList)
		{
			GetNewCustomer(customer);
			GetRandomRequestForCustomer(customer);
			//Debug.Log("customer1: " + customer.Name + ", request: " + customer.Request.RequestText);
		}
		
		SetupNextCustomer();
		//TODOFIRST: set canTest to true after the request has been displayed
	}

	public static Customer GetCurrentCustomer()
	{
		return customerList[currentCustomerNumber];
	}

	//takes the number of a customer that needs to be replaced and sets it's values to a the values of a new customer
	private static void GetNewCustomer(Customer customer)
	{
		canTest = false;

		int id;
		string name;
		string catchphrase;
		Sprite image;
		string buyingText;
		string failedPurchaseText;
		DBManager.SelectRandomCustomer(out id, out name, out catchphrase, out image, out buyingText, out failedPurchaseText);
		customer.Id = id;
		customer.Name = name;
		customer.Catchphrase = catchphrase;
		customer.Image = image;
		customer.BuyingText = buyingText;
		customer.FailedPurchaseText = failedPurchaseText;

		canTest = true;
	}
	
	//Calls for a random request from the database and saves it in the customer
	private static void GetRandomRequestForCustomer(Customer customer)
	{
		canTest = false;

		int id, rarity, rowCount;
		string requestText, sqlCode;
		DBManager.SelectRandomRequest(out id, out requestText, out sqlCode, out rarity, out rowCount);
		Request request = new Request(id, requestText, sqlCode, rarity,rowCount);
		customer.Request = request;

		canTest = true;
	}

	public IEnumerator CorrectAnswer()
	{
		//TODOFIRST: change text
		dialogueManager.ExitTextBox();
		dialogueManager.ExitCustomerImage();
		yield return new WaitForSeconds(1);
		SetupNextCustomer();
		yield return 0;
	}

	//set the values for the 5th customer in line and animate the customer image and text box into the screen 
	private void SetupNextCustomer()
	{
		int previousCustomerNumber = currentCustomerNumber;
		currentCustomerNumber = (currentCustomerNumber++) % 5;
		//Debug.Log("Customer: "+customerList[currentCustomerNumber].Name+" Image: " + customerList[currentCustomerNumber].Image);
		//set the image of the next customer
		currentCustomerImage.sprite = (Sprite) customerList[currentCustomerNumber].Image;
		customerName.text = customerList[currentCustomerNumber].Name;

		string newCatchphrase = customerList[currentCustomerNumber].Catchphrase;
		string newRequest = customerList[currentCustomerNumber].Request.RequestText;
		//update the data for the previous customer to the new random customer
		GetNewCustomer(customerList[previousCustomerNumber]);
		GetRandomRequestForCustomer(customerList[previousCustomerNumber]);

		dialogueManager.EnterCustomerImage();
		dialogueManager.EnterTextBox();

		dialogueManager.TypeTextStartCoroutine(writtenText, newCatchphrase, 0.01f, newRequest);
	}
	
	//TODOFIRST: the price is not the same in the query and table!

	//TODOFIRST: wrong answer
	public IEnumerator WrongAnswer()
	{
		Debug.Log("Customer is sad :(");
		dialogueManager.ExitTextBox();
		dialogueManager.ExitCustomerImage();
		yield return new WaitForSeconds(1);
		SetupNextCustomer();
		yield return 0;
	}

	
}
