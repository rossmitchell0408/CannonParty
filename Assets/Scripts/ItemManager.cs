using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*******
TODO:
Create Save Files with lists of teams items
Read from file when creating each teams item list
Only instantiate items in teams file
Build the list when game begins and remove all items when game ends or just before new game
*******/

public class ItemManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static ItemManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    // TODO: Get all these from Resources
    [SerializeField]
    List<ItemBehaviour> itemPrefabs;

    [System.Serializable]
    class TeamItems
    {
        public List<ItemBehaviour> items;
        public Team team;
    }

    [SerializeField]
    List<TeamItems> teamItems;


    //public List<ItemBehaviour> team1Items;
    int maxItemsPerTeam = 10;

    private void Start()
    {
        CreateTeamItems();
        
    }

    void CreateTeamItems()
    {
        teamItems = new List<TeamItems>();

        for(int i = 0; i < (int)Team.MAXTEAMCOUNT; i++)
        {
            teamItems.Add(new TeamItems());
            teamItems[i].team = (Team)i;

            teamItems[i].items = new List<ItemBehaviour>();
            teamItems[i].team = Team.LEFT;

            for (int j = 0; j < maxItemsPerTeam; j++)
            {
                // TODO: Include function that checks which prefab to instantiate
                teamItems[i].items.Add(Instantiate(itemPrefabs[0]));
                teamItems[i].items[j].gameObject.SetActive(false);
            }
        }
    }

    List<ItemBehaviour> GetItemListFromTeam(Team team)
    {
        foreach (TeamItems t in teamItems)
        {
            if (team == t.team)
                return t.items;
        }

        return null;
    }

    public ItemBehaviour GetRandomInactiveItem(Team team)
    {
        List <ItemBehaviour> itemList = GetItemListFromTeam(team);

        if (itemList == null)
            return null;

        List<ItemBehaviour> inactiveItems = new List<ItemBehaviour>();
        foreach(ItemBehaviour item in itemList)
        {
            if (!item.gameObject.activeSelf)
            {
                inactiveItems.Add(item);
            }
        }

        if (inactiveItems.Count == 0)
            return null;

        if (inactiveItems.Count == 1)
            return inactiveItems[0];

        return inactiveItems[Random.Range(0, inactiveItems.Count)];
    }

    public void DespawnItem(ItemBehaviour item)
    {
        
        //if (items.Contains(item))
        //    return;

        //items.Add(item);
        item.GetComponent<Rigidbody>().velocity = Vector3.zero;
        item.gameObject.SetActive(false);
    }

}
