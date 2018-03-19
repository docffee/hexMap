using GraphAlgorithms;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    interface ITileEngine<N> where N : INode<N>
    {
        int BoardSizeX { get; }
        int BoardSizeZ { get; }
        ITile<N> GetTile(int x, int z);
        ITile<N>[] GetAllTiles();
        IEnumerable<IPathNode<N>> GetShortestPath(IUnit<N> unit, ITile<N> startTile, ITile<N> endTile);
        IEnumerable<IPathNode<N>> GetReachable(IUnit<N> unit, ITile<N> startTile);
        bool PlaceUnit(IUnit<N> unit, int x, int z);
        bool MoveUnit(IUnit<N> unit, int lastX, int lastZ, int newX, int newZ);
    }
}
