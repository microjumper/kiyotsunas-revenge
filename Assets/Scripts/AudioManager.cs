using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource playerAttack;
    public AudioSource playerHurt;

    public AudioSource enemyHurt;
    public AudioSource flyingEyeHurt;

    public static AudioManager instance;

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

    public void PlayPlayerAttack()
    {
        playerAttack.Play();
    }

    public void PlayPlayerHurt()
    {
        playerHurt.Play();
    }

    public void PlayEnemyHurt()
    {
        enemyHurt.Play();
    }

    public void PlayFlyingEyeHurt()
    {
        flyingEyeHurt.Play();
    }
}
