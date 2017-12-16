using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

//holds all the regular expressions, handles the testing of the user input and forwards the code to the DBManager class
public class RegexManager
{

	/* example used
	 * SELECT price.item,  name,type, id_item
	 * FROM playerItems
	 * WHERE price < 20
	 */

	/* (?<words>[\w\._]+)		selects all words and numbers and captures them into a group named words
	 * 
	 * (?<words>[\w\._]+)		name the groups (1 or more bc +) of characters [\w+\._]+
	 * [\w+\._]+		a letter or a number or a . or a _ (one or more) -> word
	 * 
	 */


	static public void TestUserInputSelect(string code)
	{
		string pattern = @"(?<words>[\w\._]+)";
		foreach (Match match in Regex.Matches(code, pattern))
			Debug.Log("Found " + match.ToString());
	}

}
