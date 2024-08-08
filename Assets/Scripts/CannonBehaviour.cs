using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform loadPoint;
    [SerializeField]
    Transform launchPoint;

    Queue<GameObject> ammo = new Queue<GameObject>();

    [SerializeField]
    Vector3 launchDirection;
    [SerializeField]
    float launchPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveToLoad(GameObject obj, float time)
    {
        Vector3 startPos = obj.transform.position;
        Vector3 endPos = loadPoint.position;

        float timeElapsed = 0f;

        while(timeElapsed < time)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        obj.SetActive(false);
        StartCoroutine(DelayBeforeLaunch(obj));
    }

    IEnumerator DelayBeforeLaunch(GameObject obj, float time = 3f)
    {
        yield return new WaitForSeconds(time);
        Launch(obj);
    }

    void Launch(GameObject obj)
    {
        // TODO: Explosion partical effect here
        obj.transform.position = launchPoint.position;
        obj.SetActive(true);

        obj.GetComponent<Rigidbody>().velocity = launchDirection * launchPower;

        if (obj.TryGetComponent<ItemBehaviour>(out ItemBehaviour item))
        {
            item.CountDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");

        //Debug.Log(other.name + " Tag: " + other.tag);


        //if(other.tag == "Item")
        //{

        //}

        if (other.gameObject.TryGetComponent<ItemBehaviour>(out ItemBehaviour item))
        {
            Debug.Log("Loading");
            ammo.Enqueue(other.gameObject);
            StartCoroutine(MoveToLoad(other.gameObject, 0.6f));
            //item.CountDown();
        }
    }
}
