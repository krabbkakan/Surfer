using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public int score;

    public Text scoreText;

    public GameObject titleScreen;


	// Use this for initialization
	void Start () {
        StartCountingPoints();
	}
	
    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }

    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
    }

    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
    }

    public void StartCountingPoints() {
        InvokeRepeating("UpdateScore", 5.0f, 5.0f);
    }

    public void StopCountingPoints()
    {
        CancelInvoke("UpdateScore");
    }

}
