using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text bestScoreText;

    public void UpdateScoreUI(int score, int bestscore)
    {
        playerScoreText.text = score.ToString();

        bestScoreText.text = bestscore.ToString();
    }
}
