using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField] float range;

    [SerializeField] GameObject hpBar;
    [SerializeField] Animator anim;

    [SerializeField] DamageText damageText;
    

    public enum EnemyState
    {
        Idle,
        Attacking,
    }

    private NavMeshAgent navAgent;

    public EnemyState enemyState = EnemyState.Idle;

    //Coroutine coroutine = null;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(attackTarget != null && Vector3.Distance(transform.position, attackTarget.transform.position) >= range && combatData.getCurrentHp > 0)
        {
            anim.Play("Base Layer.Walk");

            Vector3 dir = (attackTarget.transform.position - navAgent.transform.position).normalized;
            navAgent.transform.LookAt(attackTarget.transform);
            Vector3 targetDestination = attackTarget.transform.position - (dir * range);

            navAgent.destination = targetDestination;
        }
    }

    public override void TakeDamage(int damageDelt, Entity player)
    {
        if(combatData.getCurrentHp > 0)
        {
            combatData.TakeDamage(damageDelt);

            DamageText temp = Instantiate(damageText, transform);

            if (damageDelt > 0)
            {
                temp.UpdateText(damageDelt.ToString());
                hpBar.transform.localScale = new Vector3((float)((float)combatData.getCurrentHp / (float)combatData.getMaxHp), 1, 1);
                anim.Play("Base Layer.Damage");
                //anim.Play("Base Layer.Attack");
            }
            else
            {
                temp.UpdateText("Miss");
            }

            if (combatData.getCurrentHp <= 0)
            {
                anim.Play("Base Layer.Death");
                Destroy(gameObject, 2.05f);
            }

            if (attackTarget == null)
            {
                attackTarget = player;
                enemyState = EnemyState.Attacking;
                anim.SetInteger("CombatState", 1);
                anim.Play("Base Layer.Attack");
            }
        }
        
    }
}
