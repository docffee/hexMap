using GraphAlgorithms;
using System;
using UnityEngine;

namespace Assets.Scripts.HexImpl
{
    class HexHeuristic : IHeuristic<HexNode>
    {
        public float MinDist(HexNode a, HexNode b)
        {
            int x = Mathf.Abs(a.X) - Math.Abs(b.X);
            int z = Mathf.Abs(a.Z) - Math.Abs(b.Z);
            int y = Mathf.Abs(-(a.X + a.Z)) - Math.Abs(-(b.X + b.Z));

            return Mathf.Max(x, z, y);
        }
    }
}
