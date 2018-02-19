using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    private int x, z;

    public Hex(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public int X
    {
        get { return x; }
    }

    public int Z
    {
        get { return z; }
    }
}
