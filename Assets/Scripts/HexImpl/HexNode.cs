using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    class HexNode : INode<HexNode>
    {
        public int direction;
        private HexCell tile;

        public HexNode(int direction, HexCell tile)
        {
            this.direction = direction;
            this.tile = tile;
        }

        public IEnumerator<IEdge<HexNode>> GetEnumerator()
        {
            List<IEdge<HexNode>> edges = new List<IEdge<HexNode>>();
            HexEdge turnLeft = new HexEdge(1, new HexNode(direction - 1, tile));
            HexEdge turnRight = new HexEdge(1, new HexNode(direction + 1, tile));

            return edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            var node = obj as HexNode;
            if (node == null)
                return false;

            return tile.X == node.tile.X && tile.Z == node.tile.Z && Direction == node.Direction;
        }

        public override int GetHashCode()
        {
            var hashCode = -300798877;
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<HexCell>.Default.GetHashCode(tile);
            return hashCode;
        }

        private int getValidDirection(int increment)
        {
            if (Direction + increment > 5)
                return 0;
            if (Direction + increment < 0)
                return 5;

            return Direction + increment;
        }

        public int Direction
        {
            get { return direction; }
        }
    }
}
