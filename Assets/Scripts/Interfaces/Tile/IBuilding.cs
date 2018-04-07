public interface IBuilding{
    int CurrentHealth { get; set; }
    int MaxHealth { get; }

    ITile Tile { get; set; }

    float PosX { get; }
    float PosY { get; }
    float PosZ { get; }
}
