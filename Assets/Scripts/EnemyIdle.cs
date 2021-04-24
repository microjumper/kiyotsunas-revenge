using UnityEngine;

public class EnemyIdle : StateMachineBehaviour
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

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!GameManager.IsGameOver)
        {
            if (Vector2.Distance(rigidbody.position, player.transform.position) > enemyBehaviour.attackRange)
            {
                animator.SetBool("Walking", true);
            }
            else
            {
                enemyBehaviour.Attack();
            } 
        }
    }
}
