using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    TMP_Text text;
    public Button questionButton;
    public Button[] buttons;
    public Color newColor;
    private Dictionary<int, TriviaQuestion> questionMap;

    private void Awake()
    {
        questionMap = new Dictionary<int, TriviaQuestion>();
        var questions = Resources.FindObjectsOfTypeAll<TriviaQuestion>();
        for(int i=0; i< questions.Length; i++)
        {
            questionMap.Add(questions[i].GetInstanceID(), questions[i]);
        }

        /*TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
        textmeshPro.color = new Color32(255, 128, 0, 255);*/
    }

    public int GetQuestion(int random)
    {
        return questionMap.ElementAt(random).Key;
    }
    void Start()
    {
        int selection = Random.Range(0, questionMap.Count);
        int whichQuestion = GetQuestion(selection);

        TriviaQuestion tq = questionMap[whichQuestion];
        questionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tq.question;

        
        var buttonList = PutAllButtonsInAList(tq.numOptions);
        var correctAnswers = PutOptionsInList(tq.correctAnswers);
        var falseAnswers = PutOptionsInList(tq.falseAnswers);
        // todo: hide unused buttons
        for (int i= 0; i< tq.numAnswersNeeded; i++)
        {
            Button b = buttonList[Random.Range(0, buttonList.Count)];
            string answer = correctAnswers[Random.Range(0, correctAnswers.Count)];

            b.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = answer;

            buttonList.Remove(b);
            correctAnswers.Remove(answer);
        }
        int numFalseAnswersNeeded = tq.numOptions - tq.numAnswersNeeded;
        for (int i = 0; i < numFalseAnswersNeeded; i++)
        {
            Button b = buttonList[Random.Range(0, buttonList.Count)];
            string answer = falseAnswers[Random.Range(0, falseAnswers.Count)];

            b.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = answer;

            buttonList.Remove(b);
            falseAnswers.Remove(answer);
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
            for(int i=0; i<num; i++)
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
}
