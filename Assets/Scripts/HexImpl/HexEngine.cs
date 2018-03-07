using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    class HexEngine : ITileEngine<HexNode>
    {
        int sizeX, sizeZ;
        ITile<HexNode>[] tiles;
        IPathfinding pathfinder;

        private static HexEngine singleton;

        public HexEngine(int sizeX, int sizeZ, IMapGenerator<HexNode> generator)
        {
            this.sizeX = sizeX;
            this.sizeZ = sizeZ;
            tiles = generator.GenerateTiles(sizeX, sizeZ);
            pathfinder = new Pathfinding();

            singleton = this;
        }

        public int BoardSizeX
        {
            get { return sizeX; }
        }

        public int BoardSizeZ
        {
            get { return sizeZ; }
        }

        public ITile<HexNode>[] GetAllTiles()
        {
            return tiles;
        }

        public ITile<HexNode> GetTile(int x, int z)
        {
            int index = z * sizeX + x;
            if (index < 0 || index >= tiles.Length)
                throw new ArgumentException("Invalid tile parameters");

            return tiles[index];
        }

        public IEnumerable<IPathNode<HexNode>> GetReachable(IUnit<HexNode> unit, ITile<HexNode> startTile)
        {
            HexNode node = new HexNode((HexDirection) unit.Direction, startTile, unit);
            return pathfinder.GetReachableNodes(node, unit.CurrentMovePoints);
        }

        public IEnumerable<IPathNode<HexNode>> GetShortestPath(IUnit<HexNode> unit, ITile<HexNode> startTile, ITile<HexNode> endTile)
        {
            HexNode start = new HexNode((HexDirection) unit.Direction, startTile, unit);
            HexNode end = new HexNode(HexDirection.Any, endTile, unit);
            IEnumerable<IPathNode<HexNode>> path = pathfinder.GetShortestPath(start, end, new HexHeuristic());

            return path;
        }

        public bool PlaceUnit(IUnit<HexNode> unit, int x, int z)
        {
            ITile<HexNode> tile = GetTile(x, z);
            if (tile.UnitOnTile != null)
                return false;

            unit.Tile = tile;
            tile.UnitOnTile = unit;
            return true;
        }

        public bool MoveUnit(IUnit<HexNode> unit, int lastX, int lastZ, int newX, int newZ)
        {
            ITile<HexNode> endTile = GetTile(newX, newZ);
            if (endTile.UnitOnTile != null)
                return false;

            ITile<HexNode> currentTile = unit.Tile;
            currentTile.UnitOnTile = null;
            endTile.UnitOnTile = unit;
            unit.Tile = endTile;
            return true;
        }

        public HexNode GetNodeBehind(HexNode cur)
        {
            switch ((int) cur.Direction)
            {
                case 0:
                    if ((cur.Z & 1) != 0)
                        return CreateNode(cur.X, cur.Z - 1, cur.Direction.OppositeDirection(), cur);
                    else if (cur.Z > 0 && cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z - 1, cur.Direction.OppositeDirection(), cur);

                    return null;
                case 1:
                    if (cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z, cur.Direction.OppositeDirection(), cur);

                    return null;
                case 2:
                    if ((cur.Z & 1) != 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X, cur.Z + 1, cur.Direction.OppositeDirection(), cur);
                    else if (cur.Z < BoardSizeZ - 1 && cur.X > 0)
                        return CreateNode(cur.X - 1, cur.Z + 1, cur.Direction.OppositeDirection(), cur);

                    return null;
                case 3:
                    if ((cur.Z & 1) == 0 && cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X, cur.Z + 1, cur.Direction.OppositeDirection(), cur);
                    else if (cur.Z < BoardSizeZ - 1)
                        return CreateNode(cur.X + 1, cur.Z + 1, cur.Direction.OppositeDirection(), cur);

                    return null;
                case 4:
                    if (cur.X < BoardSizeX - 1)
                        return CreateNode(cur.X + 1, cur.Z, cur.Direction.OppositeDirection(), cur);

                    return null;
                case 5:
                    if ((cur.Z & 1) == 0 && cur.Z > 0)
                        return CreateNode(cur.X, cur.Z - 1, cur.Direction.OppositeDirection(), cur);
                    else if (cur.X < BoardSizeX - 1 && cur.Z > 0)
                        return CreateNode(cur.X + 1, cur.Z - 1, cur.Direction.OppositeDirection(), cur);

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
            ITile<HexNode> tile = GetTile(x, z);
            HexNode node = new HexNode(direction, tile, prev.MovingUnit);
            return node;
        }

        public static HexEngine Singleton
        {
            get { return singleton; }
        }
    }
}
