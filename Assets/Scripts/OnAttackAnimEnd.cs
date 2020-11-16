using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackAnimEnd : MonoBehaviour
{
    [SerializeField] PlayerInput player;

    public void DealDamage()
    {
        player.canAttack = true;
    }
}
