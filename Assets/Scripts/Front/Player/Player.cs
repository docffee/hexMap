using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    private int id;
    private int team;

    private Dictionary<ResourceType, int> resources;

    public void Initialize(int id, int team, int money)
    {
        resources = new Dictionary<ResourceType, int>();

        this.id = id;
        this.team = team;
        resources.Add(ResourceType.Money, money);
    }

    public bool Equals(IPlayer other)
    {
        if (other == null)
            return false;

        return id == other.Id;
    }

    public int GetResource(ResourceType type)
    {
        if (resources.ContainsKey(type))
            return resources[type];

        return 0;
    }

    public void SetResource(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
            resources[type] = amount;
    }

    public int Team
    {
        get
        {
            return team;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }
}
