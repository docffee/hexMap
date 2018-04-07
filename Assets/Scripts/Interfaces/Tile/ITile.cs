
public interface ITile
{
    int X { get; }
    int Y { get; }
    int Z { get; }

    float PosX { get; }
    float PosY { get; }
    float PosZ { get; }

    ITerrain Terrain { get; }
    IUnit UnitOnTile { get; set; }
    IUnit AirUnitOnTile { get; set; }
    IResource resourceOnTile { get; }
    IBuilding BuildingOnTile { get; set; }
}