using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour: MonoBehaviour, IDamageable
{
    public LayerMask damageableLayerMasks;

    public Transform attackPoint;
    public float attackRange;

    public Healthbar healthbar;

    public Enemy enemy;

    private Animator animator;

    private int health;
    private bool canAttack = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        healthbar.SetMaxHealth(enemy.Health);
        healthbar.gameObject.SetActive(true);

        if (enemy)
        {
            health = enemy.Health;
        }

        canAttack = true;
    }

    #region Attack
    public void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            canAttack = false;
            StartCoroutine(Cooldown());
        } 
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(enemy.AttackRate);
        canAttack = true;
    }

    public void HitTarget() // controlled by enemy's animation event
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(attackPoint.position, attackRange, damageableLayerMasks);
        
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
            AudioManager.instance.PlayEnemyHurt();

            animator.SetTrigger("TakeDamage");

            health -= damage;

            healthbar.SetHealth(health);

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        animator.SetBool("Died", true);

        healthbar.gameObject.SetActive(false);

        StartCoroutine(DisableGameObject(1.5f));

        GameManager.instance.UpdateScore(enemy.Points);
    }

    private IEnumerator DisableGameObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
