using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    float howLongToCelebrate = 5;
    [SerializeField]
    bool autoComplete = false;
    [SerializeField]
    bool questionsAreRandomized = true;
    [SerializeField]
    GameObject questionsCanvas = null;
    [SerializeField]
    ParticleSystem singleQuestionCelebration = null;

    [Header("UI")]
    public Button questionButton;
    public Button submitButton;
    public Button[] buttons;

    [Header("Button colors")]
    public Color buttonHighlightColor;
    public Color correctAnswerHighlightColor;
    public Color incorrectAnswerHighlightColor;

    private Dictionary<int, TriviaQuestion> questionMap;
    const int invalidQuestion = -1;
    int currentQuestion = invalidQuestion;

    
    //[SerializeField]
    internal RewardWall rewardWall;
    internal BoxStacker boxStacker;

    bool isAwaitingNextQuestion = true;
    float timeBeforeCreatingNextQuestion = 0;
    bool allowingQuestions = false;
    internal int numQuestionsRemaining = 0;

    PlayerAnimController playAnimController;

    private void Awake()
    {
        LoadQuestions();
        if(submitButton != null)
            submitButton.onClick.AddListener(SubmitAnswerOnClick);
        boxStacker = (BoxStacker)FindObjectOfType(typeof(BoxStacker));
        playAnimController = (PlayerAnimController)FindObjectOfType(typeof(PlayerAnimController));
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
        if(questionsCanvas != null)
        {
            questionsCanvas.SetActive(enable);
        }
    }
    internal void StartRounds(int numRounds)
    {
        numQuestionsRemaining = numRounds;
        currentQuestion = invalidQuestion;
        playAnimController.StartRunningState(true);
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
        if (questionMap == null || questionMap.Count < 1)//Capi : Changed from count < 3 to count < 1
            LoadQuestions();
        int selection = 0; 

        if(questionsAreRandomized == true)
        {
            selection = UnityEngine.Random.Range(0, questionMap.Count);
        }
        currentQuestion = GetQuestion(selection);

        TriviaQuestion tq = questionMap[currentQuestion];
        if(questionButton != null)
            questionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tq.question;


        var buttonList = PutAllButtonsInAList(tq.numOptions);
        var correctAnswers = PutOptionsInList(tq.correctAnswers);
        var falseAnswers = PutOptionsInList(tq.falseAnswers);

        var correctSprites = PutOptionsInList(tq.correctSprites);
        var falseSprites = PutOptionsInList(tq.falseSprites);

        // todo: hide unused buttons
        SelectAnswersForButtons(tq.numAnswersNeeded, buttonList, correctAnswers, correctSprites);
        int numFalseAnswersNeeded = tq.numOptions - tq.numAnswersNeeded;
        SelectAnswersForButtons(numFalseAnswersNeeded, buttonList, falseAnswers, falseSprites);
    }

    void SelectAnswersForButtons(int numToSelect, List<Button> buttonList, List<string> answers, List<Sprite> sprites)
    {
        for (int i = 0; i < numToSelect; i++)
        {
            Button b = buttonList[UnityEngine.Random.Range(0, buttonList.Count)];
            if (b == null)
                continue;
            int which = UnityEngine.Random.Range(0, answers.Count);
            string answer = answers[which];

            // this is a total hack. We will need to select from multiple icons.
            Sprite spriteOption = null;
            if (sprites.Count > 0)
                spriteOption = sprites[0];

            b.GetComponentInChildren<AnswerButton>().Answer = answer;
            b.GetComponentInChildren<AnswerButton>().pic = spriteOption;

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
    List<Sprite> PutOptionsInList(Sprite[] options)
    {
        if (options.Length > 0)
            return options.ToList();
        return new List<Sprite>();
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

        if (numCorrect > 0)
        {
            playAnimController.StartRunningState(false);
            playAnimController.Celebrate();
        }
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
