using Assets.Scripts.HexImpl;
using UnityEngine;

public class FireFightNoRetaliation : IFireFight
{
    public void Fight(ICombatable attacker, ICombatable defender)
    {
        int damage = CalculateAttackerDamage(attacker, defender);
        attacker.OnAttack(defender);
        attacker.CurrentActionPoints -= attacker.AttackActionPointCost;
        defender.CurrentHealth -= damage;

        if (defender.CurrentHealth <= 0)
        {
            defender.OnDeath();
        }
    }

    private int CalculateAttackerDamage(ICombatable attacker, ICombatable defender)
    {
        return attacker.Damage * BackstabBonus(attacker.Direction, defender.Direction);
    }

    private int BackstabBonus(int aDir, int bDir)
    {
        int dif = Mathf.Abs(aDir - bDir);

        if (dif < 2)
            return 2;

        return 1;
    }
}
