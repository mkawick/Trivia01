using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    GameObject canvas = null;
    public Button questionButton;
    public Button submitButton;
    public Button[] buttons;
    public Color buttonHighlightColor;
    public Color correctAnswerHighlightColor;
    public Color incorrectAnswerHighlightColor;
    private Dictionary<int, TriviaQuestion> questionMap;
    const int invalidQuestion = -1;
    int currentQuestion = invalidQuestion;

    [SerializeField]
    float howLongToCelebrate = 5;
    [SerializeField]
    bool autoComplete = false;
    //[SerializeField]
    internal RewardWall rewardWall;
    internal BoxStacker boxStacker;

    bool isAwaitingNextQuestion = true;
    float timeBeforeCreatingNextQuestion = 0;
    bool allowingQuestions = false;
    internal int numQuestionsRemaining = 0;

    [SerializeField]
    ParticleSystem singleQuestionCelebration = null;

    private void Awake()
    {
        LoadQuestions();
        if(submitButton != null)
            submitButton.onClick.AddListener(SubmitAnswerOnClick);
        boxStacker = (BoxStacker)FindObjectOfType(typeof(BoxStacker));
    }
    void Start()
    {
        if (buttons != null)
        {
            foreach (var button in buttons)
            {
                if (button == null)
                    continue;

                var ab = button.GetComponent<AnswerButton>();
                ab.highlightColor = buttonHighlightColor;
                ab.correctAnswerHighlightColor = correctAnswerHighlightColor;
                ab.incorrectAnswerHighlightColor = incorrectAnswerHighlightColor;
            }
        }
    }

    internal void EnableQuestions(bool enable)
    {
        allowingQuestions = enable;
        if(canvas != null)
        {
            canvas.SetActive(enable);
        }
    }
    internal void StartRounds(int numRounds)
    {
        numQuestionsRemaining = numRounds;
        currentQuestion = invalidQuestion;
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
        int selection = UnityEngine.Random.Range(0, questionMap.Count);

        currentQuestion = GetQuestion(selection);

        TriviaQuestion tq = questionMap[currentQuestion];
        if(questionButton != null)
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
            Button b = buttonList[UnityEngine.Random.Range(0, buttonList.Count)];
            if (b == null)
                continue;
            string answer = answers[UnityEngine.Random.Range(0, answers.Count)];

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
        if (allowingQuestions == true)
        {
            if (isAwaitingNextQuestion == true)
            {
                if (Utils.HasExpired(timeBeforeCreatingNextQuestion))
                {
                    isAwaitingNextQuestion = false;
                    timeBeforeCreatingNextQuestion = 0;
                    Reset();
                }
            }
        }
        if(autoComplete)
        {
            submitButton?.gameObject.SetActive(false);
            CheckAutoComplete();
        }
        else
        {
            submitButton?.gameObject.SetActive(true);
        }

     /*   if (Input.GetKeyUp(KeyCode.B) == true)
        {
            rewardWall.BuildWall(6);
        }*/
    }

    internal void Reset()
    {
        ResetAllButtons();
        NextQuestion();
    }



    private void CheckAutoComplete()
    {
        if (currentQuestion == invalidQuestion)
            return;

        TriviaQuestion tq = questionMap[currentQuestion];
        int count = 0;
        foreach (var button in buttons)
        {
            AnswerButton ab = button.GetComponent<AnswerButton>();
            if (ab.isSelected == true)
            {
                count++;
            }
        }
        if(count >= tq.numAnswersNeeded)
        {
            FinishRound();
        }
    }

    void FinishRound()
    {
        int numCorrect = CountCorrectItems(currentQuestion);
        questionMap.Remove(currentQuestion);
        timeBeforeCreatingNextQuestion = Time.time + howLongToCelebrate;
        isAwaitingNextQuestion = true;

        boxStacker.AddBox(numCorrect);
        numQuestionsRemaining--;
        if (numQuestionsRemaining < 0)
            numQuestionsRemaining = 0;
        currentQuestion = invalidQuestion;

        if (singleQuestionCelebration != null)
            singleQuestionCelebration.Play();
    }

    int CountCorrectItems(int currentQuestion)
    {
        TriviaQuestion tq = questionMap[currentQuestion];
        //int numNeeded = tq.numAnswersNeeded;
        int numCorrect = 0, numFalse = 0;
        foreach (var button in buttons)
        {
            AnswerButton ab = button.GetComponent<AnswerButton>();
            string answer = ab.Answer;
            if (ab.isSelected == true)
            {
                if (Utils.IsInList(answer, tq.correctAnswers))
                {
                    numCorrect++;
                }
                else if (Utils.IsInList(answer, tq.falseAnswers) == true)
                {
                    numFalse++;
                    ab.ShowIncorrectOverlay();
                }
            }
            else
            {
                /// if is correct answer but missed...
                if (Utils.IsInList(answer, tq.correctAnswers))
                {
                    ab.ShowCorrectOverlay();
                }
            }
        }
        return numCorrect;
    }

    void SubmitAnswerOnClick()
    {
        FinishRound();
    }
    void ResetAllButtons()
    {
        if (buttons != null)
        {
            foreach (var button in buttons)
            {
                button?.GetComponent<AnswerButton>().Reset();
            }
        }
    }
}
