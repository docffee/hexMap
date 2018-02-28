using GraphAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    interface ITileEngine<N> where N : INode<N>
    {
        int BoardSizeX { get; }
        int BoardSizeZ { get; }
        ITile<N> GetTile(int x, int z);
        ITile<N>[] GetAllTiles();
        IEnumerable<IPathNode<N>> MoveUnit(IUnit<N> unit, ITile<N> startTile, ITile<N> endTile);
        IEnumerable<IPathNode<N>> GetReachable(IUnit<N> unit, ITile<N> startTile);
        bool PlaceUnit(IUnit<N> unit, int x, int z);
    }
}
