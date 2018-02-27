using GraphAlgorithms;

public interface ITile<N> where N : INode<N>
{
    int X { get; }
    int Y { get; }
    int Z { get; }

    float WorldPosX { get; }
    float WorldPosY { get; }
    float WorldPosZ { get; }

    ITerrain Terrain { get; }
    IUnit<N> UnitOnTile { get; }
}