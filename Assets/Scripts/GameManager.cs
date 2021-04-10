using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public void GameOver()
    {
        IsGameOver = true;

        System.Array.ForEach(spawners, spawner => spawner.CancelInvoke());
    }
}
