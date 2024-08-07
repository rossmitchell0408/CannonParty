using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    public bool isPickupable = true;

    GameObject parentObject;

    private void Start()
    {
        parentObject = transform.parent.gameObject;
    }

    private void Update()
    {
        Carry();
    }

    public void Pickup(Transform pickupPoint)
    {
        //Debug.Log(parentObject.transform.parent);
        isPickupable = false;

        parentObject.transform.position = pickupPoint.position;
        parentObject.transform.parent = pickupPoint;
    }

    public void Drop(Vector3 direction)
    {
        isPickupable = true;

        parentObject.transform.parent = null;

        //parentObject.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        parentObject.GetComponent<Rigidbody>().velocity = direction;
        //Debug.Log(direction);
    }

    private void Carry()
    {
        if (parentObject.transform.parent == null)
            return;

        parentObject.transform.position = parentObject.transform.parent.position;
        //Debug.Log("1");

    }
}
