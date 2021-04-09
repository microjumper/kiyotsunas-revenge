using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Transform attackPoint;

    private new Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audioSource;

    private readonly float attackRange = 1.25f;
    private readonly float speed = 8f;
    private readonly float attackRate = 0.25f;

    private bool canAttack = true;
    private Vector2 inputDirection = Vector2.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, Constraint.LEFT, Constraint.RIGHT), Mathf.Clamp(transform.position.y, Constraint.BOTTOM, Constraint.TOP));
    }

    private void FixedUpdate()
    {
        if(canAttack)
        {
            animator.SetBool("Running", !inputDirection.Equals(Vector2.zero));

            rigidbody.MovePosition((Vector2)transform.position + inputDirection * speed * Time.fixedDeltaTime);
        }
    }

    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();

        if (inputDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnAttack()
    {
        if(canAttack)
        {
            Attack();
            canAttack = false;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        System.Array.ForEach(enemies, enemy => {
            enemy.GetComponent<Enemy>().TakeHit(10);
        });

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    public void TakeHit(int damage)
    {
        animator.SetTrigger("TakeHit");
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint!=null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
