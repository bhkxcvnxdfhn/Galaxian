using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScore : MonoBehaviourSingleton<PlayerScore>
{
    [SerializeField] private PlayerScoreUI scoreUI;

    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            if(score > bestScore)
                bestScore = score;
            scoreUI.UpdateScoreUI(Score, bestScore);
        }
    }

    private int bestScore = 0;

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        Initialization();
    }

    public void Initialization()
    {
        Score = 0;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
    }
}
