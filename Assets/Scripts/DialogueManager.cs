using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Animator textBoxAnimator;
	public Animator customerImageAnimator;

	public void EnterTextBox()
	{
		textBoxAnimator.SetBool("IsOpen", true);
	}
	public void ExitTextBox()
	{
		StopAllCoroutines();
		textBoxAnimator.SetBool("IsOpen", false);
	}

	public void EnterCustomerImage()
	{
		customerImageAnimator.SetBool("IsOpen", true);
	}
	public void ExitCustomerImage()
	{
		customerImageAnimator.SetBool("IsOpen", false);
	}

	public void TypeTextStartCoroutine(Text writtenText, string catchphrase, float lettersPause, string request = "")
	{
		StopAllCoroutines();
		StartCoroutine(TypeText(writtenText, catchphrase, lettersPause, request));
	}
	private static IEnumerator TypeText(Text writtenText, string text1, float lettersPause, string text2 = "")
	{
		CustomerManager.canTest = false;
		int i = 0;
		writtenText.text = "";
		while (i < text1.Length)
		{
			writtenText.text += text1[i++];
			yield return new WaitForSeconds(lettersPause);
		}
		yield return new WaitForSeconds(3f);
		if (text2.Length != 0)
		{
			i = 0;
			writtenText.text = "";
			while (i < text2.Length)
			{
				writtenText.text += text2[i++];
				yield return new WaitForSeconds(lettersPause);
			}
		}
		CustomerManager.canTest = true;
	}
}
