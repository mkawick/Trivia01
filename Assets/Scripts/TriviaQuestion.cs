using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TriviaQuestionData", menuName = "ScriptableObjects/TriviaQuestion", order = 1)]
public class TriviaQuestion : ScriptableObject
{
    public string question;

    public int levelOfDifficulty;
    public int numOptions;
    public int numAnswersNeeded;
    public string[] correctAnswers;
    public string[] falseAnswers;
}