using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    float howLongToCelebrate = 5;
    [SerializeField]
    int numQuestions = 3;
    [SerializeField]
    bool gameStopsWhenPlayerFallsOnBarrier = false;
    [SerializeField]
    bool alwaysScrolls = false;

    [Header("Object references")]
    [SerializeField]
    TMP_Text scoreCanvas = null;
    [SerializeField]
    RewardWall rewardWall = null;
    QuestionManager questionManager = null;

    [SerializeField]
    PlayerWithCollider man = null;
    [SerializeField]
    GameObject awardTextCanvas = null;



    [SerializeField]
    LevelScroller scroller = null;
    [Header("Particle systems")]
    [SerializeField]
    ParticleSystem celebrationWhenPlayerTouchesDown = null;
    [SerializeField]
    ParticleSystem celebrationIfPlayerFinishesLevel = null;
    [SerializeField]
    ParticleSystem celebrationBoxesPassBarrier = null;

    private int levelScore = 0;

    enum GameState
    {
        Intro, ShowSplashScreen, TakingQuestions, Scrolling, WaitingAtEnd
    }
    GameState gameState = GameState.Intro;
    float timeBeforeCreatingNextQuestion = 0;
    CameraSwingAndZoom cameraSwingAndZoom;


    private void Awake()
    {
    }
    void Start()
    {
        questionManager = GetComponent<QuestionManager>();
        Debug.Assert(rewardWall != null, "requires reward wall");
        questionManager.rewardWall = rewardWall;
        rewardWall.BuildWall(9);
        levelScore = 0;
        cameraSwingAndZoom = FindObjectOfType<CameraSwingAndZoom>();
    }


    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Intro:
                {
                    SetupIntroState();
                }
                break;
            case GameState.ShowSplashScreen:
                {
                    ShowSplashScreen();
                }
                break;

            case GameState.TakingQuestions:
                if (GetComponent<QuestionManager>().numQuestionsRemaining == 0)
                {
                    timeBeforeCreatingNextQuestion = Time.time;
                    if(alwaysScrolls == false)
                        timeBeforeCreatingNextQuestion += 1.0f;
                    gameState = GameState.Scrolling;
                }
                break;
            case GameState.Scrolling:
                if (Utils.HasExpired(timeBeforeCreatingNextQuestion))
                {
                    if(scroller != null)
                        scroller.scrollingEnabled = true;
                    GetComponent<QuestionManager>().EnableQuestions(false);
                    man.initialState = false;
                }
                break;
            case GameState.WaitingAtEnd:
                if (Utils.HasExpired(timeBeforeCreatingNextQuestion))
                {
                    //isAwaitingNextQuestion = false;
                    timeBeforeCreatingNextQuestion = 0;
                    gameState = GameState.Intro;
                    //scroller.scrollingEnabled = true;
                    var boxStacker = (BoxStacker)FindObjectOfType(typeof(BoxStacker));
                    boxStacker.Reset();
                }
                break;
        }
    }

    private void SetupIntroState()
    { 
        if (scroller)
        {
            scroller.Reset();
            scroller.scrollingEnabled = alwaysScrolls;
            scroller.GetComponent<CoinManagement>().StartNewLevel();
        }
        awardTextCanvas?.SetActive(false);

        gameState = GameState.TakingQuestions;
        questionManager.EnableQuestions(true);
        questionManager.StartRounds(numQuestions);
        questionManager.Reset();
        man.initialState = true;

        gameState = GameState.ShowSplashScreen;
        timeBeforeCreatingNextQuestion = Time.time + 0.1f;
    }
    private void ShowSplashScreen()
    {
        if (Utils.HasExpired(timeBeforeCreatingNextQuestion))
        {
            gameState = GameState.TakingQuestions;
        }
    }

    internal void OnPlayerTouchesDown(string awardText)
    {
        ChangeStateToWaitingAtEnd();

        if (celebrationWhenPlayerTouchesDown != null)
            celebrationWhenPlayerTouchesDown.Play();

        if (awardTextCanvas)
        {
            awardTextCanvas.SetActive(true);
            TMP_Text[] texts = awardTextCanvas.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                GameObject go = text.gameObject;
                if (go != null)
                {
                    if (go.name == "WinText")
                        text.text = "You Win!";
                    else
                        text.text = "Coins collected: "+levelScore+"\n"+awardText;
                }
            }
        }

    }
    internal void OnScrollToEndReached()
    {
        ChangeStateToWaitingAtEnd();

        if (celebrationIfPlayerFinishesLevel != null)
            celebrationIfPlayerFinishesLevel.Play();
    }
    internal void OnPlayerHitsBarrier()
    {
        if (gameStopsWhenPlayerFallsOnBarrier == true)
        {
            ChangeStateToWaitingAtEnd();

            if (celebrationIfPlayerFinishesLevel != null)
                celebrationIfPlayerFinishesLevel.Play();

            if (awardTextCanvas)
            {
                awardTextCanvas.SetActive(true);
                TMP_Text[] texts = awardTextCanvas.GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text text in texts)
                {
                    GameObject go = text.gameObject;
                    if ( go != null)
                    {
                        if (go.name == "WinText")
                            text.text = "Fail";
                        else
                            text.text = "";
                    }
                }
                   
            }
        }
    }

    internal void OnBoxesPassBarrier()
    {
        if (celebrationBoxesPassBarrier != null)
            celebrationBoxesPassBarrier.Play();
    }
    internal void OnScoreChange(int score)
    {
        if (scoreCanvas != null)
            scoreCanvas.text = "Coins: " + score;
        levelScore = score;
    }

    void ChangeStateToWaitingAtEnd()
    {
        gameState = GameState.WaitingAtEnd;
        timeBeforeCreatingNextQuestion = Time.time + howLongToCelebrate;
        scroller.scrollingEnabled = false;

    }
}
