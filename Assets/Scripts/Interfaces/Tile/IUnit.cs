﻿
public interface IUnit
{
    float MaxActionPoints { get; set; }
    float CurrentActionPoints { get; set; }
    int Direction { get; }
    bool Flying { get; }
    bool CanWalkBackwards { get; }

    float RotateCost { get; }
    IWalkable GetTerrainWalkability(ITerrain terrain);
    bool IsTilePassable(ITile tile);

    float PosX { get; }
    float PosY { get; }
    float PosZ { get; }

    ITile Tile { get; set; }
    IAction[] Actions { get; }
}