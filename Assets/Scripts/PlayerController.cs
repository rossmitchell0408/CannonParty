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


    //[SerializeField]
    List<PickupableObject> pickupsDetected = new List<PickupableObject>();
    //[SerializeField]
    PickupableObject pickup;
    [SerializeField]
    Transform pickupPoint;
    [SerializeField]
    float throwPower;

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
            return;

        if (!context.performed)
            return;

        rBod.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        //Debug.Log(jumpForce);
        
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundPoint.position, 0.1f, groundLayer);
    }

    public void PickupItem(InputAction.CallbackContext context)
    {
        if (pickup != null)
            return;

        if (!context.performed)
            return;

        if (pickupsDetected.Count <= 0)
            return;


        //PickupableObject pickup = null;
        float distance = float.PositiveInfinity;

        if (pickupsDetected.Count == 1)
        {
            pickup = pickupsDetected[0];
        }

        foreach (PickupableObject pick in pickupsDetected)
        {
            if (!pick.isPickupable)
                continue;

            float dist = Vector3.Distance(transform.position, pick.transform.position);

            if (dist < distance)
            {
                distance = dist;
                pickup = pick;
            }
        }

        if (pickup == null)
            return;

        pickup.Pickup(pickupPoint);
    }

    //void Pickup(PickupableObject pick)
    //{
    //    pick.Pickup(pickupPoint);
    //}

    public void Throw(InputAction.CallbackContext context)
    {
        if (pickup == null)
            return;

        if (!context.performed)
            return;

        // TODO: GET FORWARD DIRECTION
        pickup.Drop(new Vector3(1.5f, 1f, 0f) * throwPower);

        pickup = null;
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (pickup == null)
            return;

        if (!context.performed)
            return;

        // TODO: GET FORWARD DIRECTION
        pickup.Drop(new Vector3(-1.5f, 1.5f, 0f));

        pickup = null;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            if (pickupsDetected.Contains(other.GetComponent<PickupableObject>()))
                return;

            pickupsDetected.Add(other.GetComponent<PickupableObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pickup")
        {
            if (!pickupsDetected.Contains(other.GetComponent<PickupableObject>()))
                return;

            pickupsDetected.Remove(other.GetComponent<PickupableObject>());
        }
    }

}
