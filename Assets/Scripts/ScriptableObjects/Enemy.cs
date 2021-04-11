using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField]
    private int points;
    public int Points { get { return points; } }

    [SerializeField]
    private int health;
    public int Health {get { return health; } }

    [SerializeField]
    private int attack;
    public int Attack { get { return attack; } }

    [SerializeField]
    private int attackRate;
    public int AttackRate { get { return attackRate; } }

    [SerializeField]
    private int speed;
    public int Speed { get { return speed; } }
}