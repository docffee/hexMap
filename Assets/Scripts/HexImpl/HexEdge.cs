using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    class HexEdge : IEdge<HexNode>
    {
        private HexNode end;
        private float cost;

        public HexEdge(float prevCost, HexNode end)
        {
            cost = prevCost;
            this.end = end;
        }

        public float GetCost()
        {
            return cost;
        }

        public HexNode GetEnd()
        {
            return end;
        }
    }
}
