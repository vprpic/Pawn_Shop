using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Animator textBoxAnimator;
	public Animator customerImageAnimator;
	public Animator buttonAnimator;

	public void EnterTextBox()
	{
		//Debug.Log("Enter Text Box");
		textBoxAnimator.SetBool("IsOpen", true);
	}
	public void ExitTextBox()
	{
		//Debug.Log("Exit Text Box");
		StopAllCoroutines();
		textBoxAnimator.SetBool("IsOpen", false);
	}

	public void EnterButtons()
	{
		//Debug.Log("Enter Text Box");
		buttonAnimator.SetBool("IsOpen", true);
	}
	public void ExitButtons()
	{
		//Debug.Log("Exit Text Box");
		StopAllCoroutines();
		buttonAnimator.SetBool("IsOpen", false);
	}

	public void EnterCustomerImage()
	{
		customerImageAnimator.SetBool("IsOpen", true);
	}
	public void ExitCustomerImage()
	{
		customerImageAnimator.SetBool("IsOpen", false);
	}

	public void TypeTextStartCoroutine(Text writtenText, string catchphrase, float lettersPause, string request = "", bool isRequest = true)
	{
		StopAllCoroutines();
		StartCoroutine(TypeText(writtenText, catchphrase, lettersPause, request, isRequest));
	}

	private static IEnumerator TypeText(Text writtenText, string text1, float lettersPause, string text2 = "", bool isRequest = true)
	{
		CustomerManager.canTest = false;
		int i = 0;
		writtenText.text = "";
		while (i < text1.Length)
		{
			writtenText.text += text1[i++];
			yield return new WaitForSecondsRealtime(lettersPause);
		}
		yield return new WaitForSecondsRealtime(3f);
		if (text2.Length != 0)
		{
			i = 0;
			writtenText.text = "";
			while (i < text2.Length)
			{
				writtenText.text += text2[i++];
				yield return new WaitForSecondsRealtime(lettersPause);
			}
		}

		//TODOFIRST: buttons appear
		//TODOFIRST: text added to buttons
		//if it's a question show the answers
		if (!isRequest)
		{
			//show buttons
			CustomerManager.EnterButtons();
		}

		CustomerManager.canTest = true;
	}
}
