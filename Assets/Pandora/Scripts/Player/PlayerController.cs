using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    
    // Tmp Status
    public float tmpSpeed = 5f;
    
    // Variables
    private Vector2 moveInput;
    private bool isOnControl;
    public bool onControlInit = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isOnControl = onControlInit;
    }

    void Update()
    {
        if(isOnControl)
        {
            // move by velocity
            rb.velocity = moveInput * tmpSpeed;
        }
        else
        {
            // stop
            rb.velocity = Vector2.zero;
            // TODO : move by AI
        }
    }
    
    // input system event OnMove
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    // input system event OnTag
    public void OnTag(InputValue value)
    {
        isOnControl = !isOnControl;
    }
}
