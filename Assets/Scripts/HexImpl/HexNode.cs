using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    public class HexNode : INode<HexNode>
    {
        private HexDirection direction;
        private ITile<HexNode> tile;

        public HexNode(HexDirection direction, ITile<HexNode> tile)
        {
            this.direction = direction;
            this.tile = tile;
        }

        public IEnumerator<IEdge<HexNode>> GetEnumerator()
        {
            List<IEdge<HexNode>> edges = new List<IEdge<HexNode>>();
            HexEdge turnLeft = new HexEdge(0.2f, new HexNode(GetValidDirection(direction, -1), tile));
            HexEdge turnRight = new HexEdge(0.2f, new HexNode(GetValidDirection(direction, 1), tile));

            HexEngine engine = HexEngine.Singleton;
            HexNode back = engine.GetNodeBehind(this);
            HexNode forward = engine.GetNodeInFront(this);

            edges.Add(turnLeft);
            edges.Add(turnRight);
            if (back != null)
                edges.Add(new HexEdge(1.3f, back));
            if (forward != null)
                edges.Add(new HexEdge(1, forward));
            return edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public HexDirection GetValidDirection(HexDirection direction, int increment)
        {
            if ((int)direction + increment > 5)
                return HexDirection.NE;
            if ((int)direction + increment < 0)
                return HexDirection.NW;

            return direction + increment;
        }

        public override bool Equals(object obj)
        {
            var node = obj as HexNode;
            if (node == null)
                return false;

            return tile.X == node.tile.X && tile.Z == node.tile.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = -300798877;
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ITile<HexNode>>.Default.GetHashCode(tile);
            return hashCode;
        }

        public override string ToString()
        {
            return X + ", " + Z + " | Direction = " + direction.ToString();
        }

        public HexDirection Direction
        {
            get { return direction; }
        }

        public ITile<HexNode> Tile
        {
            get { return tile; }
        }

        public int X
        {
            get { return tile.X; }
        }

        public int Z
        {
            get { return tile.Z; }
        }
    }
}
