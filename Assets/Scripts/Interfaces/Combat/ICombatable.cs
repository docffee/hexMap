
public interface ICombatable : IUnit
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; }
    int Damage { get; }
    int Range { get; }
    float AttackActionPointCost { get; }
    IPlayer Controller { get; }

    bool CanRetaliate();
    void OnAttack(ICombatable target);
    void OnDeath();
}
