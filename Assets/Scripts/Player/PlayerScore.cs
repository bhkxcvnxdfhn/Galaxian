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
            OnScoreChange?.Invoke(score, bestScore);
        }
    }

    private int bestScore = 0;

    public event Action<int, int> OnScoreChange;

    private void OnEnable()
    {
        OnScoreChange += scoreUI.UpdateScoreUI;
    }

    private void OnDestroy()
    {
        OnScoreChange -= scoreUI.UpdateScoreUI;
    }

    private void Awake()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        scoreUI.UpdateScoreUI(Score, bestScore);
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
