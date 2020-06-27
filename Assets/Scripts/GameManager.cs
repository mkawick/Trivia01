﻿using System.Collections.Generic;
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

    bool isAwaitingNextQuestion = true;
    float timeBeforeCreatingNextQuestion = 0;

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
        if (isAwaitingNextQuestion == true)
        {
            if(timeBeforeCreatingNextQuestion < Time.time)
            {
                isAwaitingNextQuestion = false;
                timeBeforeCreatingNextQuestion = 0;
            }
        }
    }
    
   
}
