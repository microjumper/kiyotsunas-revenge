using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeFlight : StateMachineBehaviour
{
    private Player player;
    private Rigidbody2D rigidbody;
    private FlyingEyeBehavior flyingeyeBehaviour;

    //  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Player>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        flyingeyeBehaviour = animator.GetComponent<FlyingEyeBehavior>();
    }
    //  OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TargetInRange(player.transform, flyingeyeBehaviour.enemy.AttackRange))
        {
            flyingeyeBehaviour.Attack();
        }
        else
        {
            FaceTarget(player.transform);
            rigidbody.position = Vector2.MoveTowards(rigidbody.position, player.transform.position, flyingeyeBehaviour.enemy.Speed * Time.deltaTime);
        }
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
}
