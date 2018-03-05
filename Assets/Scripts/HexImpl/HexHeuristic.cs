using GraphAlgorithms;
using System;
using UnityEngine;

namespace Assets.Scripts.HexImpl
{
    class HexHeuristic : IHeuristic<HexNode>
    {
        public float MinDist(HexNode a, HexNode b)
        {
            return HexLength(SubtractHex(a, b));
        }

        public Vector3Int SubtractHex(HexNode a, HexNode b)
        {
            Vector3Int hex = new Vector3Int();
            int aY = -(a.X + a.Z);
            int bY = -(b.X + b.Z);

            hex.x = a.X - b.X;
            hex.z = a.Z - b.Z;
            hex.y = aY - bY;

            return hex;
        }

        public int HexLength(Vector3Int hex)
        {
            return ((Mathf.Abs(hex.x) + Mathf.Abs(hex.y) + Mathf.Abs(hex.z)) / 2);
        }
    }
}
