using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Entity attackTarget;

    public CombatData combatData;

    public void Attack(Entity attacker, Entity defender)
    {
        float hitChance = attacker.combatData.getAttackValue / defender.combatData.getDefenseValue;

        if (hitChance > .9f)
        {
            hitChance = .9f;
        }
        else if (hitChance < .05f)
        {
            hitChance = .05f;
        }

        float rand = Random.Range(0f, 1f);

        if (hitChance > rand)
        {
            int damageToDeal = Random.Range(attacker.combatData.getMinDamageValue, attacker.combatData.getMaxDamageValue + 1);
            defender.TakeDamage(damageToDeal, attacker); // for main class
        }
        else
        {
            defender.TakeDamage(-1, attacker);// for main class
        }

        if (defender.combatData.getCurrentHp <= 0)
        {
            attacker.attackTarget = null;
        }
    }

    public virtual void TakeDamage(int damageDelt, Entity entity)
    {

    }
}
