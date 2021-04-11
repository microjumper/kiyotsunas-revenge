using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 1.25f;

    public Healthbar healthbar;

    private new Rigidbody2D rigidbody;
    private Animator animator;

    private readonly int attack = 10;
    private readonly float speed = 8f;
    private readonly float attackRate = 0.3f;
    private int health = 80;
    private bool isTakingDamage = false;

    private bool canAttack = true;
    private Vector2 inputDirection = Vector2.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        healthbar.SetMaxHealth(health);
    }

    private void Update()
    {
        if (!GameManager.IsGameOver)
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, Constraint.LEFT, Constraint.RIGHT), 
                Mathf.Clamp(transform.position.y, Constraint.BOTTOM, Constraint.TOP));
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsGameOver)
        {
            if (canAttack && !isTakingDamage)
            {
                animator.SetBool("Running", !inputDirection.Equals(Vector2.zero));
                rigidbody.MovePosition((Vector2)transform.position + inputDirection * speed * Time.fixedDeltaTime);
            }
        }
    }

    public void OnMove(InputValue value)
    {
        if (!GameManager.IsGameOver)
        {
            inputDirection = value.Get<Vector2>();

            if (inputDirection.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (inputDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void OnAttack()
    {
        if (!GameManager.IsGameOver)
        {
            if (canAttack && !isTakingDamage)
            {
                canAttack = false;
                Attack();
            }
        }
    }

    private void Attack()
    {
        AudioManager.instance.PlayPlayerAttack();

        animator.SetTrigger("Attack");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        System.Array.ForEach(enemies, enemy => {
            enemy.GetComponent<IDamageable>().TakeDamage(attack);
        });

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    public void EndTakingDamageAnimation()
    {
        isTakingDamage = false;
    }

    public void TakeDamage(int damage)
    {
        if (!GameManager.IsGameOver)
        {
            isTakingDamage = true;

            AudioManager.instance.PlayPlayerHurt();

            animator.SetTrigger("TakeHit");

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
        GameManager.instance.GameOver();

        animator.SetBool("Died", true);

        StopCoroutine(Cooldown());
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint!=null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
