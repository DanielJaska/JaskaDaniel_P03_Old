using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackAnimEnd : MonoBehaviour
{
    [SerializeField] Entity entity;

    public void DealDamage()
    {
        //player.canAttack = true;
        entity.Attack(entity, entity.attackTarget);
    }
}
