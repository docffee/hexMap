using System;
using System.Collections;
using System.Collections.Generic;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    public class HexNode : INode<HexNode>
    {
        private HexDirection direction;
        private ITile tile;
        private IUnit movingUnit;
        private HexControl hexControl;

        public HexNode(HexDirection direction, ITile tile, IUnit movingUnit, HexControl hexControl)
        {
            this.direction = direction;
            this.tile = tile;
            this.movingUnit = movingUnit;
            this.hexControl = hexControl;
        }

        public IEnumerator<IEdge<HexNode>> GetEnumerator()
        {
            List<IEdge<HexNode>> edges = new List<IEdge<HexNode>>();
            float terrainMod = movingUnit.GetTerrainWalkability(tile.Terrain).Modifier;
            float rotateCost = movingUnit.RotateCost * terrainMod;
            HexEdge turnLeft = new HexEdge(rotateCost, new HexNode(GetValidDirection(direction, -1), tile, movingUnit, hexControl));
            HexEdge turnRight = new HexEdge(rotateCost, new HexNode(GetValidDirection(direction, 1), tile, movingUnit, hexControl));

            HexNode back = null;
            HexNode forward = hexControl.GetNodeInFront(this);
            if (movingUnit.CanWalkBackwards)
                back = hexControl.GetNodeBehind(this);

            edges.Add(turnLeft);
            edges.Add(turnRight);
            if (forward != null)
            {
                IWalkable walkable = movingUnit.GetTerrainWalkability(forward.Tile.Terrain);
                float cost = ((terrainMod + walkable.Modifier) / 2);
                if (movingUnit.Flying && forward.Tile.AirUnitOnTile == null && walkable.Passable)
                {
                    edges.Add(new HexEdge(cost, forward));
                }
                else if (forward.Tile.UnitOnTile == null && walkable.Passable)
                {
                    edges.Add(new HexEdge(cost, forward));
                }
            }
            if (back != null)
            {
                IWalkable walkable = movingUnit.GetTerrainWalkability(back.Tile.Terrain);
                float cost = ((terrainMod + walkable.Modifier) / 1.7f);
                if (movingUnit.Flying && back.Tile.AirUnitOnTile == null && walkable.Passable)
                {
                    edges.Add(new HexEdge(cost, back));
                }
                else if (back.Tile.UnitOnTile == null && walkable.Passable)
                {
                    edges.Add(new HexEdge(cost, back));
                }
            }

            return edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

            if (direction == HexDirection.Any || node.direction == HexDirection.Any)
            {
                return tile.X == node.tile.X && tile.Z == node.tile.Z;
            }
            else
            {
                return tile.X == node.tile.X && tile.Z == node.tile.Z && direction == node.direction;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = -300798877;
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ITile>.Default.GetHashCode(tile);
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

        public ITile Tile
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

        public IUnit MovingUnit
        {
            get { return movingUnit; }
        }
    }
}
