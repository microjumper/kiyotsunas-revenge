using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;

    public Spawner[] spawners;

    public static GameManager instance;

    public static bool IsGameOver { get; private set; } = false;

    private int score;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        score = 0;
        UpdateScore(score);
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = $"{score}";
    }

    public void GameOver()
    {
        IsGameOver = true;

        System.Array.ForEach(spawners, spawner => spawner.CancelInvoke());
    }
}
