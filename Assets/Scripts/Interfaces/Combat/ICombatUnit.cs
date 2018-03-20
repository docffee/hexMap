using GraphAlgorithms;

public interface ICombatUnit<N> : IUnit<N> where N : INode<N>
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; }
    int Damage { get; }
    int Range { get; }
}
