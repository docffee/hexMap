
public interface IUnit
{
    float MaxActionPoints { get; set; }
    float CurrentActionPoints { get; set; }
    int Direction { get; }
    bool Flying { get; }
    bool CanWalkBackwards { get; }

    float RotateCost { get; }
    IWalkable GetTerrainWalkability(ITerrain terrain);

    ITile Tile { get; set; }
    bool PerformingAction();
}