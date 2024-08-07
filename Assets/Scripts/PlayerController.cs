using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rBod;
    [SerializeField]
    Transform groundPoint;
    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Vector3 moveDirection;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        rBod = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        //Debug.Log(moveDirection);
    }

    private void CalculateMovement()
    {
        if (moveDirection != Vector3.zero)
        {
            float xVelocity = moveDirection.x * moveSpeed;
            float zVelocity = moveDirection.z * moveSpeed;

            rBod.velocity = new Vector3(xVelocity, rBod.velocity.y, zVelocity);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!IsGrounded())
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }

        rBod.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        Debug.Log(jumpForce);
        
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundPoint.position, 0.1f, groundLayer);
    }
}
