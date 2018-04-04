using GraphAlgorithms;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface ITileControl<N> where N : INode<N>
    {
        int BoardSizeX { get; }
        int BoardSizeZ { get; }
        ITile GetTile(int x, int z);
        ITile[] GetAllTiles();
        IEnumerable<IPathNode<N>> GetShortestPath(IUnit unit, ITile startTile, ITile endTile);
        IEnumerable<IPathNode<N>> GetShortestPath(IUnit unit, ITile startTile, ITile endTile, int endDirection);
        IEnumerable<IPathNode<N>> GetReachable(IUnit unit, ITile startTile);
        bool PlaceUnit(IUnit unit, int x, int z);
        bool MoveUnit(IUnit unit, int lastX, int lastZ, int newX, int newZ);
    }
}
