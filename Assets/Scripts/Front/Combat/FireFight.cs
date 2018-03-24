using Assets.Scripts.HexImpl;
using UnityEngine;

public class FireFight : IFireFight
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
        else if (defender.CanRetaliate())
        {
            int dist = HexHeuristic.MinDistTile(defender.Tile, attacker.Tile);
            if (dist > defender.Range)
                return;

            damage = CalculateDefenderDamage(defender);
            defender.OnAttack(attacker);
            attacker.CurrentHealth -= damage;
            if(attacker.CurrentHealth <= 0)
            {
                attacker.OnDeath();
            }
        }
    }

    private int CalculateAttackerDamage(ICombatable attacker, ICombatable defender)
    {
        return attacker.Damage * BackstabBonus(attacker.Direction, defender.Direction);
    }

    private int CalculateDefenderDamage(ICombatable defender)
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
