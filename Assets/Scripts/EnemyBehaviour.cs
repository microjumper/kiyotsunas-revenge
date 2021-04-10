using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour: MonoBehaviour, IDamageable
{
    public AudioClip hurtClip;

    public LayerMask damageableLayerMasks;

    public Transform attackPoint;
    public float attackRange;

    public Enemy enemy;

    private Animator animator;
    private AudioSource audioSource;

    private int health;
    private bool canAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if(enemy)
        {
            health = enemy.Health;
        }
    }

    public void TakeDamage(int damage)
    {
        if(health > 0)
        {
            audioSource.Stop();
            audioSource.clip = hurtClip;
            audioSource.Play();

            animator.SetTrigger("TakeDamage");

            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;

            animator.SetTrigger("Attack");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damageableLayerMasks);
            System.Array.ForEach(colliders, collider => collider.GetComponent<IDamageable>().TakeDamage(enemy.Attack));

            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(enemy.AttackRate);
        canAttack = true;
    }

    private void Die()
    {
        animator.SetBool("Died", true);

        StartCoroutine(DisableGameObject(1.5f));
    }

    private IEnumerator DisableGameObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        gameObject.SetActive(false);
    }
}
