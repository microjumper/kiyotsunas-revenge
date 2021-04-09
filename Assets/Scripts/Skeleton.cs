using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Animator animator;

    private int health;

    private void OnEnable()
    {
        health = 80;
    }

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

    public void TakeHit(int damage)
    {
        if(health > 0)
        {
            animator.SetTrigger("TakeHit");

            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        animator.SetBool("Died", true);

        StartCoroutine(DisableGameObject());
    }

    private IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}