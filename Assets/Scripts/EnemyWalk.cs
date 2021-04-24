using UnityEngine;

public class EnemyWalk : StateMachineBehaviour
{
    private Player player;
    private Rigidbody2D rigidbody;
    private EnemyBehaviour enemyBehaviour;

    //  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Player>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        enemyBehaviour = animator.GetComponent<EnemyBehaviour>();
    }

    //  OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TargetInRange(player.transform, enemyBehaviour.enemy.AttackRange))
        {
            enemyBehaviour.Attack();
        }
        else
        {
            CheckDistanceFromOtherEnemies(animator.gameObject);
            Move();

            animator.gameObject.transform.position = new Vector2(animator.gameObject.transform.position.x, Mathf.Clamp(animator.gameObject.transform.position.y, Constraint.BOTTOM, Constraint.TOP));

            if (GameManager.IsGameOver)
            {
                animator.SetBool("Walking", false);
            }
        }
    }

    //  OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Walking", false);
    }

    #region Movement
    private void Move()
    {
        FaceTarget(player.gameObject.transform);
        ChaseTarget(player.gameObject.transform);
    }

    private void CheckDistanceFromOtherEnemies(GameObject current)
    {
        EnemyBehaviour[] enemies = FindObjectsOfType<EnemyBehaviour>();
        System.Array.ForEach(enemies, enemy =>
        {
            GameObject go = enemy.gameObject;
            if(go != current)
            {
                if (Vector2.Distance(go.transform.position, current.transform.position) < 1)
                {
                    Vector2 direction = current.transform.position - go.transform.position;
                    current.gameObject.transform.Translate(direction * Time.deltaTime);
                }
            }
        });
    }

    private bool TargetInRange(Transform target, float range)
    {
        return Vector2.Distance(rigidbody.position, target.position) <= range;
    }

    private void FaceTarget(Transform target)
    {
        if (rigidbody.position.x > target.position.x)
        {
            rigidbody.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else
        {
            rigidbody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void ChaseTarget(Transform target)
    {
        rigidbody.position = Vector2.MoveTowards(rigidbody.position, target.position, enemyBehaviour.enemy.Speed * Time.deltaTime);
    }
    #endregion
}