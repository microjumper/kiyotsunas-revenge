using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeBehavior : MonoBehaviour, IDamageable
{
    public LayerMask damageableLayerMasks;

    public Enemy enemy;

    private Animator animator;

    private int health;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (enemy)
        {
            health = enemy.Health;
        }
    }

    #region Attack
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void HitTarget() // controlled by enemy's animation event
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, enemy.AttackRange, damageableLayerMasks);

        if (playerCollider)
        {
            playerCollider.GetComponent<IDamageable>().TakeDamage(enemy.Attack);
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        if(health > 0)
        {
            health -= damage;

            AudioManager.instance.PlayFlyingEyeHurt();

            animator.SetTrigger("TakeHit");
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("Dead", true);

        StartCoroutine(DisableGameObject(0.75f));

        GameManager.instance.UpdateScore(enemy.Points);
    }

    private IEnumerator DisableGameObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        gameObject.SetActive(false);
    }
}
