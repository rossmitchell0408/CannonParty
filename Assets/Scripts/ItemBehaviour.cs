using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountDown()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        Debug.Log("Begin Self Destruction of " + name);

        yield return new WaitForSeconds(3);
        Debug.Log("Self Destructing");
        ItemSpawnManager.Instance.DespawnItem(this);
    }
}
