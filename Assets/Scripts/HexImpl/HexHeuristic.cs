using GraphAlgorithms;
using System;
using UnityEngine;

namespace Assets.Scripts.HexImpl
{
    class HexHeuristic : IHeuristic<HexNode>
    {
        public float MinDist(HexNode a, HexNode b)
        {
            Cube aV = StraightenAxes(a.X, a.Z);
            Cube bV = StraightenAxes(b.X, a.Z);
            return HexLength(SubtractHex(aV, bV));
        }

        public static int MinDistTile(ITile a, ITile b)
        {
            Cube aV = StraightenAxes(a.X, a.Z);
            Cube bV = StraightenAxes(b.X, a.Z);
            return HexLength(SubtractHex(aV, bV));
        }

        private static Cube SubtractHex(Cube a, Cube b)
        {
            Cube hex = new Cube();
            int aY = -(a.x + a.z);
            int bY = -(b.x + b.z);

            hex.x = a.x - b.x;
            hex.z = a.z - b.z;
            hex.y = aY - bY;

            return hex;
        }

        private static int HexLength(Cube hex)
        {
            return ((Mathf.Abs(hex.x) + Mathf.Abs(hex.y) + Mathf.Abs(hex.z)) / 2);
        }

        private static Cube StraightenAxes(int x, int z)
        {
            Cube cube = new Cube();
            cube.x = x - (z - (z & 1)) / 2;
            cube.z = z;
            cube.y = -x - z;
            return cube;
        }

        private class Cube
        {
            public int x;
            public int y;
            public int z;

            public Cube(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Cube() { }
        }
    }
}
