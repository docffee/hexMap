using UnityEngine;

public class FireFight : IFireFight
{
    public void Fight(ICombatUnit attacker, ICombatUnit defender)
    {
        int damage = CalculateAttackerDamage(attacker, defender);
        attacker.OnAttack(defender);
        attacker.CurrentActionPoints -= attacker.AttackActionPointCost;
        defender.CurrentHealth -= damage;

        if (defender.CurrentHealth <= 0)
        {
            defender.OnDeath();
        }
        else if (defender.CanRetaliate())
        {
            damage = CalculateDefenderDamage(defender);
            defender.OnAttack(attacker);
            attacker.CurrentHealth -= damage;
        }
    }

    private int CalculateAttackerDamage(ICombatUnit attacker, ICombatUnit defender)
    {
        return attacker.Damage * BackstabBonus(attacker.Direction, defender.Direction);
    }

    private int CalculateDefenderDamage(ICombatUnit defender)
    {
        return Mathf.CeilToInt((float)defender.Damage / 2);
    }

    private int BackstabBonus(int aDir, int bDir)
    {
        int dif = Mathf.Abs(aDir - bDir);

        if (dif < 2)
            return 2;

        return 1;
    }
}
