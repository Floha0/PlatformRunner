using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text  scoreText;

    private DontDestroy dontDestroy;

    private void Start()
    {
        dontDestroy = FindObjectOfType<DontDestroy>();
        scoreText.text = ("Score: ") + dontDestroy.score.ToString();
        dontDestroy.score = dontDestroy.score;
    }

    public void ChangeScore(int value)
    {
        dontDestroy.score = value + dontDestroy.score;
        scoreText.text = ("Score: ") + dontDestroy.score.ToString();
    }
}