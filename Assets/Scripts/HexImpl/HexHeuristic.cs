using GraphAlgorithms;
using System;
using UnityEngine;

namespace Assets.Scripts.HexImpl
{
    class HexHeuristic : IHeuristic<HexNode>
    {
        public float MinDist(HexNode a, HexNode b)
        {
            int x = Mathf.Abs(a.X - b.X);
            int z = Mathf.Abs(a.Z - b.Z);
            int y = Mathf.Abs(-(a.X + a.Z) - -(b.X + b.Z));

            return Mathf.Max(x, z, y);
        }

        public int GetCubeCoordinateFromAxial(int x, int z)
        {
            return -x - z;
        }
    }
}
