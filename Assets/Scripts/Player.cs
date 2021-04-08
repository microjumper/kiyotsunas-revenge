using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Transform attackPoint;

    private Animator animator;
    
    [SerializeField]
    private float attackRange = 1.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputValue value)
    {
        Debug.Log("Move");
        Debug.Log(value.Get<Vector2>());
    }

    public void OnAttack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        System.Array.ForEach<Collider2D>(enemies, (enemy) => Debug.Log("Colpito"));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
