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
    RewardWall rewardWall = null;
    [SerializeField]
    QuestionManager questionManager = null;

    [SerializeField]
    PlayerWithCollider man = null;

    float timeBeforeCreatingNextQuestion = 0;

    [SerializeField]
    LevelScroller scroller = null;
    [SerializeField]
    ParticleSystem celebrationWhenPlayerTouchesDown = null;
    [SerializeField]
    ParticleSystem celebrationIfPlayerFinishesLevel = null;

    [SerializeField]
    bool alwaysScrolls = false;

    enum GameState
    {
        Intro, ShowSplashScreen, TakingQuestions, Scrolling, WaitingAtEnd
    }
    GameState gameState = GameState.Intro;

    private void Awake()
    {
    }
    void Start()
    {
        Debug.Assert(rewardWall != null, "requires reward wall");
        questionManager.rewardWall = rewardWall;
        rewardWall.BuildWall(9);
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
                    // paused need to happen here.. new game state
                    gameState = GameState.Scrolling;
                    scroller.scrollingEnabled = true;
                    GetComponent<QuestionManager>().EnableQuestions(false);
                    man.initialState = false;
                }
                break;
            case GameState.Scrolling:
                break;
            case GameState.WaitingAtEnd:
                if (timeBeforeCreatingNextQuestion < Time.time)
                {
                    //isAwaitingNextQuestion = false;
                    timeBeforeCreatingNextQuestion = 0;
                    gameState = GameState.Intro;
                    //scroller.scrollingEnabled = true;
                    var boxStacker = (BoxStacker)FindObjectOfType(typeof(BoxStacker));
                    boxStacker.Reset();
                    //GetComponent<QuestionManager>()
                }
                break;
        }
    }

    private void SetupIntroState()
    {
        scroller.Reset();
        scroller.scrollingEnabled = alwaysScrolls;

        gameState = GameState.TakingQuestions;
        GetComponent<QuestionManager>().EnableQuestions(true);
        GetComponent<QuestionManager>().StartRounds(numQuestions);
        man.initialState = true;

        gameState = GameState.ShowSplashScreen;
        timeBeforeCreatingNextQuestion = Time.time + 0.1f;
    }
    private void ShowSplashScreen()
    {
        if (timeBeforeCreatingNextQuestion < Time.time)
        {
            gameState = GameState.TakingQuestions;
        }
    }

    internal void OnPlayerTouchesDown()
    {
        ChangeStateToWaitingAtEnd();

        if (celebrationWhenPlayerTouchesDown != null)
            celebrationWhenPlayerTouchesDown.Play();
    }
    internal void OnScrollToEndReached()
    {
        ChangeStateToWaitingAtEnd();

        if (celebrationIfPlayerFinishesLevel != null)
            celebrationIfPlayerFinishesLevel.Play();
    }
    internal void OnPlayerHitsBarrier()
    {
        ChangeStateToWaitingAtEnd();

        if (celebrationIfPlayerFinishesLevel != null)
            celebrationIfPlayerFinishesLevel.Play();
    }

    void ChangeStateToWaitingAtEnd()
    {
        gameState = GameState.WaitingAtEnd;
        timeBeforeCreatingNextQuestion = Time.time + howLongToCelebrate;
        scroller.scrollingEnabled = false;
        // Todo: Outro hook

    }
}
