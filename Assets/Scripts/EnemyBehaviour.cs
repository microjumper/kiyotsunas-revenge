using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour: MonoBehaviour, IDamageable
{
    public AudioClip hurtClip;
    public AudioClip attackClip;

    public LayerMask damageableLayerMasks;

    public Transform attackPoint;
    public float attackRange;

    public Enemy enemy;

    private Animator animator;
    private AudioSource audioSource;

    private Player player;

    private int health;

    private bool canAttack;
    private bool isTakingDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        if(enemy)
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
            bool walking = !TargetInRange()  && canAttack && !isTakingDamage;

            animator.SetBool("Walking", walking);

            if (walking)
            {
                if (transform.position.x < player.gameObject.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                transform.position = Vector2.MoveTowards(transform.position, player.gameObject.transform.position, enemy.Speed * Time.deltaTime);

            }
            else
            {
                Attack();
            }
        }
    }

    private bool TargetInRange()
    {
        return Vector2.Distance(transform.position, player.gameObject.transform.position) <= 2;
    }

    public void TakeDamage(int damage)
    {
        if(health > 0)
        {
            PlayClip(hurtClip);

            animator.SetTrigger("TakeDamage");

            isTakingDamage = true;

            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void HitTarget()
    {
        PlayClip(attackClip);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damageableLayerMasks);
        if(colliders.Length > 0)
        {
            System.Array.ForEach(colliders, damageable => 
            {
                damageable.GetComponent<IDamageable>().TakeDamage(enemy.Attack);
            });
        }
    }

    public void EndTakingDamageAnimation()
    {
        isTakingDamage = false;
    }

    private void Attack()
    {
        if(health > 0)
        {
            if (canAttack && TargetInRange())
            {
                canAttack = false;

                animator.SetTrigger("Attack");

                StartCoroutine(Cooldown());
            }
            else
            {
                StopCoroutine(Cooldown());
            }
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(enemy.AttackRate);
        canAttack = true;
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
