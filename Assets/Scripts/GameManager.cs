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
    RewardWall rewardWall = null;
    [SerializeField]
    QuestionManager questionManager = null;

    [SerializeField]
    PlayerWithCollider man = null;

    float timeBeforeCreatingNextQuestion = 0;

    [SerializeField]
    LevelScroller scroller = null;

    enum GameState
    {
        Intro, TakingQuestions, Scrolling, WaitingAtEnd
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
                scroller.scrollingEnabled = false;
                scroller.Reset();
                gameState = GameState.TakingQuestions;
                GetComponent<QuestionManager>().EnableQuestions(true);
                GetComponent<QuestionManager>().StartRounds(3); // << magic number
                man.initialState = true;
                gameState = GameState.TakingQuestions;
                
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
                }
                break;
        }
    }
    
    internal void OnPlayerTouchesDown()
    {
        ChangeStateToWaitingAtEnd();
        // todo, add celebration here
    }
    internal void OnScrollToEndReached()
    {
        ChangeStateToWaitingAtEnd();
        // todo, add celebration here

    }

    void ChangeStateToWaitingAtEnd()
    {
        gameState = GameState.WaitingAtEnd;
        timeBeforeCreatingNextQuestion = Time.time + howLongToCelebrate;
        scroller.scrollingEnabled = false;
    }
}
