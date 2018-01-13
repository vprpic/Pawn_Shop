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
	public Text answerA;
	public Text answerB;
	public Text answerC;
	public Text answerD;

	private static List<Customer> customerList;     //a list of 5 customers to be recycled
	private static int currentCustomerNumber;              //which customer is in front of the row
	private static int previousCustomerNumber;
	private Image currentCustomerImage;

	public void Init()
	{
		canTest = false;
		dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		currentCustomerImage = GameObject.Find("customerImage").GetComponent<Image>();
		customerName = GameObject.Find("NameText").GetComponent<Text>();
		writtenText = GameObject.Find("WrittenText").GetComponent<Text>();
		answerA = GameObject.Find("AnswerAText").GetComponent<Text>();
		answerB = GameObject.Find("AnswerBText").GetComponent<Text>();
		answerC = GameObject.Find("AnswerCText").GetComponent<Text>();
		answerD = GameObject.Find("AnswerDText").GetComponent<Text>();
		answerA.text = "";
		answerB.text = "";
		answerC.text = "";
		answerD.text = "";
		customerList = new List<Customer>();
		previousCustomerNumber = 3;
		currentCustomerNumber = 4;

		customerList.Add(new Customer(1, "", "", null));
		customerList.Add(new Customer(2, "", "", null));
		customerList.Add(new Customer(3, "", "", null));
		customerList.Add(new Customer(4, "", "", null));
		customerList.Add(new Customer(5, "", "", null));

		foreach (Customer customer in customerList)
		{
			GetNewCustomer(customer);
			GetRandomRequestForCustomer(customer);
			customer.IsRequest = true;
			//Debug.Log("customer1: " + customer.Name + ", request: " + customer.Request.RequestText);
		}

		//if we don't need to run the procurement send the next customer
		if (!PlayScreenManager.DoWeNeedToRunProcurement())
		{
			SetupNextCustomer();
		}
		else
		{
			Debug.Log("CustMan: we need to run procurement.");
		}
	}


	public static void EnterButtons()
	{
		dialogueManager.buttonAnimator.SetBool("IsOpen", true);
	}
	public static void ExitButtons()
	{
		dialogueManager.buttonAnimator.SetBool("IsOpen", false);
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

		int id, rarity;
		string requestText, sqlCode;
		DBManager.SelectRandomRequest(out id, out requestText, out sqlCode, out rarity);
		Request request = new Request(id, requestText, sqlCode, rarity);
		customer.Request = request;

		canTest = true;
	}

	//Calls for a random request from the database and saves it in the customer
	private static void GetRandomQuestionForCustomer(Customer customer)
	{
		canTest = false;

		int idQuestion, rarity, correctAnswerId;
		string questionText, correctAnswer;
		List<string> wrongAnswers = new List<string>();
		DBManager.SelectRandomQuestion(customer.Id, out idQuestion, out questionText, out correctAnswerId, out rarity);
		wrongAnswers = DBManager.GetWrongAnswersForQuestion(idQuestion, correctAnswerId, out correctAnswer);
		Question question = new Question(idQuestion, questionText, correctAnswerId, correctAnswer, wrongAnswers);
		customer.Question = question;

		canTest = true;
	}

	public IEnumerator CorrectAnswer()
	{
		dialogueManager.TypeTextStartCoroutine(writtenText, customerList[currentCustomerNumber].BuyingText,0.01f);
		//Debug.Log("customerList[currentCustomerNumber].name: " + customerList[currentCustomerNumber].Name);
		//Debug.Log("customerList[previousCustomerNumber].name: " + customerList[previousCustomerNumber].Name);
		yield return new WaitForSecondsRealtime(customerList[currentCustomerNumber].BuyingText.Length * 0.05f);
		dialogueManager.ExitTextBox();
		dialogueManager.ExitCustomerImage();
		yield return new WaitForSecondsRealtime(1);

		//if we don't need to run the procurement send the next customer
		if (!PlayScreenManager.DoWeNeedToRunProcurement())
		{
			SetupNextCustomer();
		}
		yield return 0;
	}

	public IEnumerator WrongAnswer()
	{

		//Debug.Log("customerList[currentCustomerNumber].name: " + customerList[currentCustomerNumber].Name);
		//Debug.Log("customerList[previousCustomerNumber].name: " + customerList[previousCustomerNumber].Name);
		dialogueManager.TypeTextStartCoroutine(writtenText, customerList[currentCustomerNumber].FailedPurchaseText, 0.01f);
		yield return new WaitForSecondsRealtime(customerList[currentCustomerNumber].FailedPurchaseText.Length * 0.05f);
		//Debug.Log("Customer is sad :(");
		dialogueManager.ExitTextBox();
		dialogueManager.ExitCustomerImage();
		yield return new WaitForSecondsRealtime(1);
		SetupNextCustomer();
		yield return 0;
	}

	//set the values for the 5th customer in line and animate the customer image and text box into the screen 
	public void SetupNextCustomer()
	{
		bool isRequest;
		previousCustomerNumber = currentCustomerNumber;
		currentCustomerNumber = (currentCustomerNumber + 1) % 5;
		
		//Debug.Log("Customer: "+customerList[currentCustomerNumber].Name+" Image: " + customerList[currentCustomerNumber].Image);
		//set the image of the next customer
		currentCustomerImage.sprite = (Sprite) customerList[currentCustomerNumber].Image;
		customerName.text = customerList[currentCustomerNumber].Name;
		string newCatchphrase = customerList[currentCustomerNumber].Catchphrase;
		
		//update the data for the previous customer to the new random customer
		GetNewCustomer(customerList[previousCustomerNumber]);
		//in 80% of cases get request
		if (Random.Range(1, 101) < 50)
		{
			GetRandomRequestForCustomer(customerList[previousCustomerNumber]);
			Debug.Log("request");
			isRequest = true;
		}
		else
		{
			GetRandomQuestionForCustomer(customerList[previousCustomerNumber]);
			Debug.Log("question");
			isRequest = false;
		}
		customerList[previousCustomerNumber].IsRequest = isRequest;

		if (customerList[currentCustomerNumber].IsRequest)
		{
			PlayScreenManager.EnterPlayerInventory();
		}
		else
		{
			PlayScreenManager.ExitPlayerInventory();
		}
		dialogueManager.EnterCustomerImage();
		dialogueManager.EnterTextBox();

		if (customerList[currentCustomerNumber].IsRequest && customerList[currentCustomerNumber].Request != null)
		{
			dialogueManager.TypeTextStartCoroutine(writtenText, newCatchphrase, 0.01f, customerList[currentCustomerNumber].Request.RequestText);
		}
		else if (customerList[currentCustomerNumber].Question != null)
		{
			AddNewAnswersAndShowButtons(customerList[currentCustomerNumber].Question.CorrectAnswerId, customerList[currentCustomerNumber].Question.CorrectAnswer, customerList[currentCustomerNumber].Question.WrongAnswers);
			dialogueManager.TypeTextStartCoroutine(writtenText, newCatchphrase, 0.01f, customerList[currentCustomerNumber].Question.QuestionText, false);
		}
		else
		{
			Debug.LogWarning("what");
		}
	}

	//instantiate and fill buttons with data from correct answer and wrong answers
	private void AddNewAnswersAndShowButtons(int correctAnswerId, string correctAnswer, List<string> wrongAnswers)
	{

		answerA.text = "";
		answerB.text = "";
		answerC.text = "";
		answerD.text = "";

		if (wrongAnswers.Count < 3)
		{
			Debug.Log("wrongAnswers.Count < 3");
		}
		else
		{
			switch(Random.Range(0, 4))
			{
				case 0:
					answerA.text = correctAnswer;
					break;
				case 1:
					answerB.text = correctAnswer;
					break;
				case 2:
					answerC.text = correctAnswer;
					break;
				case 3:
					answerD.text = correctAnswer;
					break;

			}
		}
			

		foreach (string answer in wrongAnswers)
		{
			//if the button hasn't been assigned a correct answer assign it to the button, else assign to the next button
			//show buttons - dialogue manager

			if(answerA.text == "")
			{
				answerA.text = answer;
			}
			else if (answerB.text == "")
			{
				answerB.text = answer;
			}
			else if (answerC.text == "")
			{
				answerC.text = answer;
			}
			else
			{
				answerD.text = answer;
			}
			
			//TODOFIRST: hide buttons
		}
	}


	public IEnumerator Answer()
	{
		ExitButtons();
		dialogueManager.ExitTextBox();
		dialogueManager.ExitCustomerImage();
		yield return new WaitForSecondsRealtime(1);
		SetupNextCustomer();
		yield return 0;
	}

}
