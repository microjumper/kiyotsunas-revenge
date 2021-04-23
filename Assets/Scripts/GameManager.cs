using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public Text scoreText;
    public GameObject gameOverPanel;

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
        audioManager.music.pitch = 0.5f;
        Time.timeScale = 0.5f;
        IsGameOver = true;
        gameOverPanel.SetActive(true);

        System.Array.ForEach(spawners, spawner => spawner.CancelInvoke());
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
        IsGameOver = false;
        audioManager.music.pitch =1;
        Time.timeScale = 1;
    }
}
