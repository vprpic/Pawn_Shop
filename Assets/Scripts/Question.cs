using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {

	private int idQuestion;
	private string questionText = null;
	private int correctAnswerId;
	private string correctAnswer;
	private List<string> wrongAnswers;


	public Question(int newIdQuestion, string newQuestionText, int newCorrectAnswerId, string newCorrectAnswer, List<string> newWrongAnswers)
	{
		idQuestion = newIdQuestion;
		questionText = newQuestionText;
		correctAnswerId = newCorrectAnswerId;
		correctAnswer = newCorrectAnswer;
		wrongAnswers = newWrongAnswers;
	}
	public int IdQuestion
	{
		get
		{
			return idQuestion;
		}

		set
		{
			idQuestion = value;
		}
	}
	public string QuestionText
	{
		get
		{
			return questionText;
		}

		set
		{
			questionText = value;
		}
	}
	public int CorrectAnswerId
	{
		get
		{
			return correctAnswerId;
		}

		set
		{
			correctAnswerId = value;
		}
	}
	public string CorrectAnswer
	{
		get
		{
			return correctAnswer;
		}

		set
		{
			correctAnswer = value;
		}
	}
	public List<string> WrongAnswers
	{
		get
		{
			return wrongAnswers;
		}

		set
		{
			wrongAnswers = value;
		}
	}

}
