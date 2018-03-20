using GraphAlgorithms;

public interface ICombatUnit : IUnit
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; }
    int Damage { get; }
    int Range { get; }
}
