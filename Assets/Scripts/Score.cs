using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text  scoreText;
    public int score;

    private void Start()
    {
        scoreText.text = ("Score: ") + score.ToString();
    }

    public void ChangeScore(int value)
    {
        score = value + score;
        scoreText.text = ("Score: ") + score.ToString();
    }
}