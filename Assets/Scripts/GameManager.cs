﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    TMP_Text text;
    public Button questionButton;
    public Button submitButton;
    public Button[] buttons;
    public Color newColor;
    private Dictionary<int, TriviaQuestion> questionMap;
    int currentQuestion = -1;

    private void Awake()
    {
        LoadQuestions();
        submitButton.onClick.AddListener(SubmitAnswerOnClick);
    }
    void Start()
    {
        NextQuestion();
    }

    public int GetQuestion(int random)
    {
        return questionMap.ElementAt(random).Key;
    }

    private void LoadQuestions()
    {
        questionMap = new Dictionary<int, TriviaQuestion>();
        var questions = Resources.LoadAll("Data", typeof(TriviaQuestion));
        for (int i = 0; i < questions.Length; i++)
        {
            questionMap.Add(questions[i].GetInstanceID(), questions[i] as TriviaQuestion);
        }
    }

    void NextQuestion()
    {
        if (questionMap == null || questionMap.Count < 3)
            LoadQuestions();
        int selection = Random.Range(0, questionMap.Count);

        currentQuestion = GetQuestion(selection);

        TriviaQuestion tq = questionMap[currentQuestion];
        questionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tq.question;


        var buttonList = PutAllButtonsInAList(tq.numOptions);
        var correctAnswers = PutOptionsInList(tq.correctAnswers);
        var falseAnswers = PutOptionsInList(tq.falseAnswers);
        // todo: hide unused buttons
        SelectAnswersForButtons(tq.numAnswersNeeded, buttonList, correctAnswers);
        int numFalseAnswersNeeded = tq.numOptions - tq.numAnswersNeeded;
        SelectAnswersForButtons(numFalseAnswersNeeded, buttonList, falseAnswers);
    }

    void SelectAnswersForButtons(int numToSelect, List<Button> buttonList, List<string> answers)
    {
        for (int i = 0; i < numToSelect; i++)
        {
            Button b = buttonList[Random.Range(0, buttonList.Count)];
            string answer = answers[Random.Range(0, answers.Count)];

            b.GetComponentInChildren<AnswerButton>().Answer = answer;

            buttonList.Remove(b);
            answers.Remove(answer);
        }
    }

    List<Button> PutAllButtonsInAList(int num)
    {
        if (num == buttons.Length)
        {
            return buttons.ToList();
        }
        else
        {
            List<Button> tempList = new List<Button>();
            for (int i = 0; i < num; i++)
            {
                tempList.Add(buttons[i]);
            }
            return tempList;
        }
    }
    List<string> PutOptionsInList(string[] options)
    {
        return options.ToList();
    }
    // Update is called once per frame
    void Update()
    {

    }
    void SubmitAnswerOnClick()
    {
        TriviaQuestion tq = questionMap[currentQuestion];
        int numNeeded = tq.numAnswersNeeded;
        int numCorrect = 0, numFalse = 0;
        foreach (var button in buttons)
        {
            if(button.GetComponent<AnswerButton>().isSelected == true)
            {
                string answer = button.GetComponent<AnswerButton>().Answer;
                if (IsInList(answer, tq.correctAnswers))
                    numCorrect++;
                else if (IsInList(answer, tq.falseAnswers) == true)
                    numFalse++;
            }
        }
        if(numFalse > 0)
        {
            Debug.Log("wrong answers");
        }
        if(numCorrect == numNeeded)
        {
            Debug.Log("all correct answers");
        }
    }
    bool IsInList(string needle, string[] answers)
    {
        foreach(var answer in answers)
        {
            if (needle == answer)
                return true;
        }
        return false;
    }
    void ResetAllButtons()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<AnswerButton>().Reset();
        }
    }
}
