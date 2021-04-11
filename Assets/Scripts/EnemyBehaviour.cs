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

    private readonly float targetRange = 2f;    // enemy reaches the target if it's in this range

    private Animator animator;

    private Player player;

    private int health;
    private bool canAttack;
    private bool isTakingDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        player = FindObjectOfType<Player>();
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
        isTakingDamage = false;
    }

    private void Update()
    {
        if (health > 0)
        {
            bool targetInRange = TargetInRange(player.gameObject.transform, targetRange);

            if (targetInRange)
            {
                Attack();
            }
            else
            {
                Move();
            }
        }
    }

    #region Movement
    private void Move()
    {
        if(isTakingDamage || !canAttack)
        {
            return;
        }

        FaceTarget(player.gameObject.transform);

        animator.SetBool("Walking", true);

        ChaseTarget(player.gameObject.transform);
    }

    private bool TargetInRange(Transform target, float range)
    {
        return Vector2.Distance(transform.position, target.position) <= range;
    }

    private void FaceTarget(Transform target)
    {
        if (transform.position.x < target.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ChaseTarget(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemy.Speed * Time.deltaTime);
    }
    #endregion

    #region Attack
    private void Attack()
    {
        if (canAttack)
        {
            animator.SetBool("Walking", false);

            canAttack = false;

            animator.SetTrigger("Attack");

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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damageableLayerMasks);

        if (colliders.Length > 0)
        {
            System.Array.ForEach(colliders, damageable =>
            {
                damageable.GetComponent<IDamageable>().TakeDamage(enemy.Attack);
            });
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        if(health > 0)
        {
            AudioManager.instance.PlayEnemyHurt();

            animator.SetTrigger("TakeDamage");

            isTakingDamage = true;

            health -= damage;

            healthbar.SetHealth(health);

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void EndTakingDamageAnimation()
    {
        isTakingDamage = false;
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
