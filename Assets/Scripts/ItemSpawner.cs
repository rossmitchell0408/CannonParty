using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;

    public bool occupied = false;

    public Team team;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(ItemBehaviour item)
    {
        if (item == null)
            return;

        item.gameObject.SetActive(true);
        item.transform.position = spawnPoint.position/* - new Vector3(20, 0, 0)*/;
        Debug.Log(item.transform.localPosition);
    }
}
