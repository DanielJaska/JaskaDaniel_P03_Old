using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Enemy : Entity
{
    public float range;
    public EnemyData enemyData;

    [SerializeField] GameObject hpBar;
    [SerializeField] Animator anim;

    [SerializeField] DamageText damageText;

    private PlayerInput aggroTarget;

    public enum EnemyState
    {
        Idle = 1,
        Attacking = 2,
        Moving = 3,
        Dead = 4,
        TookDamage = 5,
    }

    private string animVariableName = "AnimationState";

    public EnemyState enemyState = EnemyState.Idle;

    Coroutine coroutine = null;

    private void Update()
    {
        
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        {
            if (enemyState == EnemyState.Idle)
            {
                anim.Play("Base Layer.Idle");
            } else if (enemyState == EnemyState.Attacking)
            {
                if(aggroTarget == null)
                {
                    enemyState = EnemyState.Idle;
                }
                anim.Play("Base Layer.Attack");
                
                if(coroutine == null)
                {
                    coroutine = StartCoroutine(Attacking());
                }
                
            }
            else if (enemyState == EnemyState.Dead)
            {
                Destroy(gameObject);
            }
            
        }
    }

    IEnumerator Attacking()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            float hitChance = enemyData.getAttackValue / aggroTarget.playerData.getDefenseValue;

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
                int damageToDeal = Random.Range(enemyData.getMinDamageValue, enemyData.getMaxDamageValue + 1);
                aggroTarget.TakeDamage(damageToDeal);
            }
            else
            {
                aggroTarget.TakeDamage(-1);
            }
        }
        
       // aggroTarget.TakeDamage(1);
    }

    private void Idle()
    {
        //anim.SetFloat(animVariableName, (int)EnemyState.Idle);
    }

    public override void LeftClick()
    {
    }

    public void TakeDamage(int damageDelt, PlayerInput player)
    {
        enemyData.TakeDamage(damageDelt);
        DamageText temp = Instantiate(damageText, transform);
        if(damageDelt > 0)
        {
            temp.UpdateText(damageDelt.ToString());
            hpBar.transform.localScale = new Vector3((float)((float)enemyData.getCurrentHp / (float)enemyData.getMaxHp), 1, 1);
            //anim.SetFloat(animVariableName, (int)EnemyState.TookDamage);
            anim.Play("Base Layer.Damage");
        }
        else
        {
            temp.UpdateText("Miss");
        }
        
        //anim.Play("Base Layer.BlendTree.Damage");
        if (enemyData.getCurrentHp <= 0)
        {
            //anim.SetFloat(animVariableName, (int)EnemyState.Dead);
            anim.Play("Base Layer.Death");
            enemyState = EnemyState.Dead;
        }

        if(aggroTarget == null)
        {
            aggroTarget = player;
            enemyState = EnemyState.Attacking;
        }
        
    }
}
