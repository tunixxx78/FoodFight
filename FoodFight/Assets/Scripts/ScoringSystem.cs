using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
    public static int theScore;
    public static int theHighScore;
    [SerializeField] TMP_Text scoreText, highScoreText, yourScore;

    public int score;
    public int highScore;

    private void Start()
    {
        theScore = 0;
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void Update()
    {
        score = theScore;
        highScore = theHighScore;

        if(theScore>PlayerPrefs.GetInt("TheScore", 0))
        {
            PlayerPrefs.SetInt("TheScore", theScore);
        }
        if(theHighScore>PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = highScore.ToString();
        }

        scoreText.text = theScore.ToString();
        
        yourScore.text = theScore.ToString();
    }
}
