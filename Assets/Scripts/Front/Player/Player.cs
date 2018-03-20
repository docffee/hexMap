using System;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    private int id;
    private int money;
    private int team;

    public void Initialize(int id, int team, int money)
    {
        this.id = id;
        this.team = team;
        this.money = money;
    }

    public bool Equals(IPlayer other)
    {
        if (other == null)
            return false;

        return id == other.Id;
    }

    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            money = value;
        }
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
