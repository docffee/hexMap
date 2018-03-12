using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    class HexEdge : IEdge<HexNode>
    {
        private HexNode end;
        private float cost;

        public HexEdge(float cost, HexNode end)
        {
            this.cost = cost;
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
