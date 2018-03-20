using GraphAlgorithms;

public interface ITile
{
    int X { get; }
    int Y { get; }
    int Z { get; }

    float WorldPosX { get; }
    float WorldPosY { get; }
    float WorldPosZ { get; }

    ITerrain Terrain { get; }
    IUnit UnitOnTile { get; set; }
    IUnit AirUnitOnTile { get; set; }
}