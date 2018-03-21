using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    public class HexControl : ITileControl<HexNode>
    {
        int sizeX, sizeZ;
        ITile[] tiles;
        IPathfinding pathfinder;

        public HexControl(int sizeX, int sizeZ, IMapGenerator generator)
        {
            this.sizeX = sizeX;
            this.sizeZ = sizeZ;
            tiles = generator.GenerateTiles(sizeX, sizeZ);
            pathfinder = new Pathfinding();
        }

        public int BoardSizeX
        {
            get { return sizeX; }
        }

        public int BoardSizeZ
        {
            get { return sizeZ; }
        }

        public ITile[] GetAllTiles()
        {
            return tiles;
        }

        public ITile GetTile(int x, int z)
        {
            int index = z * sizeX + x;
            if (index < 0 || index >= tiles.Length)
                throw new ArgumentException("Invalid tile parameters");

            return tiles[index];
        }

        public IEnumerable<IPathNode<HexNode>> GetReachable(IUnit unit, ITile startTile)
        {
            HexNode node = new HexNode((HexDirection) unit.Direction, startTile, unit, this);
            return pathfinder.GetReachableNodes(node, unit.CurrentActionPoints);
        }

        public IEnumerable<IPathNode<HexNode>> GetShortestPath(IUnit unit, ITile startTile, ITile endTile)
        {
            HexNode start = new HexNode((HexDirection) unit.Direction, startTile, unit, this);
            HexNode end = new HexNode(HexDirection.Any, endTile, unit, this);
            IEnumerable<IPathNode<HexNode>> path = pathfinder.GetShortestPath(start, end, new HexHeuristic());

            return path;
        }

        public bool PlaceUnit(IUnit unit, int x, int z)
        {
            ITile tile = GetTile(x, z);
            if (unit.Flying)
            {
                if (tile.AirUnitOnTile != null)
                    return false;

                unit.Tile = tile;
                tile.AirUnitOnTile = unit;
            }
            else
            {
                if (tile.UnitOnTile != null)
                    return false;

                unit.Tile = tile;
                tile.UnitOnTile = unit;
            }
            return true;
        }

        public bool MoveUnit(IUnit unit, int lastX, int lastZ, int newX, int newZ)
        {
            ITile endTile = GetTile(newX, newZ);
            if (unit.Flying)
            {
                if (endTile.AirUnitOnTile != null)
                    return false;

                unit.Tile.AirUnitOnTile = null;
                unit.Tile = endTile;
                endTile.AirUnitOnTile = unit;
            }
            else
            {
                if (endTile.UnitOnTile != null)
                    return false;

                unit.Tile.UnitOnTile = null;
                unit.Tile = endTile;
                endTile.UnitOnTile = unit;
            }
            return true;
        }

        public HexNode GetNodeBehind(HexNode cur)
        {
            switch ((int) cur.Direction)
            {
                case 0:
                    if ((cur.Z & 1) != 0)
                        return CreateNode(cur.X, cur.Z - 1, cur.Direction, cur);
                    else if (cur.Z > 0 && cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z - 1, cur.Direction, cur);

                    return null;
                case 1:
                    if (cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z, cur.Direction, cur);

                    return null;
                case 2:
                    if ((cur.Z & 1) != 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X, cur.Z + 1, cur.Direction, cur);
                    else if (cur.Z < BoardSizeZ - 1 && cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z + 1, cur.Direction, cur);

                    return null;
                case 3:
                    if ((cur.Z & 1) == 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X, cur.Z + 1, cur.Direction, cur);
                    else if (cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X + 1, cur.Z + 1, cur.Direction, cur);

                    return null;
                case 4:
                    if (cur.X < BoardSizeX - 1)
                        return CreateNode(cur.X + 1, cur.Z, cur.Direction, cur);

                    return null;
                case 5:
                    if ((cur.Z & 1) == 0 && cur.Z > 0)
                        return CreateNode(cur.X, cur.Z - 1, cur.Direction, cur);
                    else if (cur.X < BoardSizeX - 1 && cur.Z > 0)
                        return CreateNode(cur.X + 1, cur.Z - 1, cur.Direction, cur);

                    return null;
                default:
                    throw new ArgumentException("Node direction must be more than 0 and less than 6!");
            }
        }

        public HexNode GetNodeInFront(HexNode cur)
        {
            switch ((int)cur.Direction)
            {
                case 0:
                    if ((cur.Z & 1) == 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X, cur.Z + 1, cur.Direction, cur);
                    else if (cur.X < BoardSizeX - 1 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X + 1, cur.Z + 1, cur.Direction, cur);

                    return null;
                case 1:
                    if (cur.X < BoardSizeX - 1)
                        return CreateNode(cur.X + 1, cur.Z, cur.Direction, cur);

                    return null;
                case 2:
                    if ((cur.Z & 1) == 0 && cur.Z > 0)
                        return CreateNode(cur.X, cur.Z - 1, cur.Direction, cur);
                    else if (cur.X < BoardSizeX - 1 && cur.Z > 0)
                        return CreateNode(cur.X + 1, cur.Z - 1, cur.Direction, cur);

                    return null;
                case 3:
                    if ((cur.Z & 1) != 0)
                        return CreateNode(cur.X, cur.Z - 1, cur.Direction, cur);
                    else if (cur.X > 0 && cur.Z > 0)
                        return CreateNode(cur.X - 1, cur.Z - 1, cur.Direction, cur);

                    return null;
                case 4:
                    if (cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z, cur.Direction, cur);

                    return null;
                case 5:
                    if ((cur.Z & 1) != 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X, cur.Z + 1, cur.Direction, cur);
                    else if (cur.X > 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X - 1, cur.Z + 1, cur.Direction, cur);

                    return null;
                default:
                    throw new ArgumentException("Node direction must be more than 0 and less than 6!");
            }
        }

        private HexNode CreateNode(int x, int z, HexDirection direction, HexNode prev)
        {
            ITile tile = GetTile(x, z);
            HexNode node = new HexNode(direction, tile, prev.MovingUnit, this);
            return node;
        }
    }
}
