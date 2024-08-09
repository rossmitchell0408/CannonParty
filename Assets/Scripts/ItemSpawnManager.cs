using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawnManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static ItemSpawnManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    class TeamSpawner
    {
        public List<ItemSpawner> itemSpawners;
        public Team team;
    }


    //[SerializeField]
    //List<ItemBehaviour> itemPrefabs;
    //List<ItemSpawner> itemSpawners;
    List<TeamSpawner> teamSpawners;
    //List<ItemBehaviour> inactiveItems;
    [SerializeField]
    int maxItems;

    [SerializeField]
    [Range(3.0f,10.0f)]
    float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        SetupSpawnerLists();
        //itemSpawners = new List<ItemSpawner>(FindObjectsOfType<ItemSpawner>());
        //inactiveItems = new List<ItemBehaviour>();

        //CreateItems();

        for(int i = 0; i < (int)Team.MAXTEAMCOUNT; i++)
        {
            StartCoroutine(SpawnDelay((Team)i));
        }
        
    }

    void SetupSpawnerLists()
    {
        teamSpawners = new List<TeamSpawner>();
        List<ItemSpawner> spawners = new List<ItemSpawner>(FindObjectsOfType<ItemSpawner>());

        for(int i = 0; i < (int)Team.MAXTEAMCOUNT; i++)
        {
            teamSpawners.Add(new TeamSpawner());
            teamSpawners[i].team = (Team)i;
            teamSpawners[i].itemSpawners = new List<ItemSpawner>();

            foreach (ItemSpawner spawner in spawners)
            {
                if (spawner.team == teamSpawners[i].team)
                {
                    teamSpawners[i].itemSpawners.Add(spawner);
                }
            }
        }
    }

    List<ItemSpawner> GetSpawnerListByTeam(Team team)
    {
        foreach (TeamSpawner t in teamSpawners)
        {
            if (team == t.team)
                return t.itemSpawners;
        }

        return null;
    }

    //void CreateItems()
    //{
    //    if (itemPrefabs.Count <= 0)
    //        return;

    //    foreach (ItemBehaviour item in itemPrefabs)
    //    {
    //        // TODO: create an item manager of sorts that knows how many of each item to create
    //        for (int i = 0; i < 2; i++)
    //        {
    //            item.gameObject.SetActive(false);
    //            inactiveItems.Add(Instantiate(item/*, transform*/));
    //        }
    //    }
    //}


    IEnumerator SpawnDelay(Team team)
    {
        yield return new WaitForSeconds(spawnDelay);

        ItemSpawner spawner = GetRandomSpawner(team);

        if (spawner == null)
        {
            Debug.Log("No spawners found");
            yield return null;
        }

        ItemBehaviour item = ItemManager.Instance.GetRandomInactiveItem(team);/*GetRandomItem()*/

        if (item == null)
        {
            Debug.Log("Could not find items to spawn");
            yield return null;
        }

        //if (inactiveItems.Contains(item))
        //{
        //    inactiveItems.Remove(item);
        //}

        spawner.SpawnItem(item);
        //Debug.Log("Spawn Cycle complete");

        StartCoroutine(SpawnDelay(team));
    }

    //public void DespawnItem(ItemBehaviour item)
    //{
    //    if (inactiveItems.Contains(item))
    //        return;

    //    inactiveItems.Add(item);
    //    item.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    item.gameObject.SetActive(false);
    //}

    ItemSpawner GetRandomSpawner(Team team)
    {
        List<ItemSpawner> itemSpawners = GetSpawnerListByTeam(team);

        if (itemSpawners == null)
            return null;

        if (itemSpawners.Count <= 0)
            return null;

        if (itemSpawners.Count == 1)
            return itemSpawners[0];

        return itemSpawners[Random.Range(0, itemSpawners.Count)];
    }

    //ItemBehaviour GetRandomItem()
    //{
    //    if (inactiveItems == null)
    //        return null;

    //    if (inactiveItems.Count == 0)
    //        return null;

    //    if (inactiveItems.Count == 1)
    //        return inactiveItems[0];

    //    return inactiveItems[Random.Range(0, inactiveItems.Count)];
    //}
}
