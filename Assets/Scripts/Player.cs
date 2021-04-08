using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
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
        Debug.Log("Attack");
    }
}
