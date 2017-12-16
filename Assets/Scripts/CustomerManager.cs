using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	Used to manage the 5 customers that arrive in the shop with requests and questions
 *	there are always 5 customer objects which are recycled
*/
public class CustomerManager :MonoBehaviour{

	private static List<Customer> customerList;		//a list of 5 customers to be recycled
	private static int customerNumber;              //which customer is in front of the row
	public static bool canTest;

	public void Start()
	{
		canTest = false;
		customerList = new List<Customer>();
		customerNumber = 0;

		customerList.Add(new Customer(1, "", "", "", "", ""));
		customerList.Add(new Customer(2, "", "", "", "", ""));
		customerList.Add(new Customer(3, "", "", "", "", ""));
		customerList.Add(new Customer(4, "", "", "", "", ""));
		customerList.Add(new Customer(5, "", "", "", "", ""));
		
		foreach(Customer customer in customerList)
		{
			GetNewCustomer(customer);
			GetRandomRequestForCustomer(customer);
			//Debug.Log("customer1: " + customer.Name + ", request: " + customer.Request.RequestText);
		}
		canTest = true;
		//TODOFIRST: set canTest to true after the request has been displayed
	}

	public static Customer GetCurrentCustomer()
	{
		return customerList[customerNumber];
	}

	//takes the number of a customer that needs to be replaced and sets it's values to a the values of a new customer
	private static void GetNewCustomer(Customer customer)
	{
		canTest = false;

		int id;
		string name;
		string catchphrase;
		string image;
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
		Request request = new Request(id, requestText, sqlCode, rarity);
		customer.Request = request;

		canTest = true;
	}

}
