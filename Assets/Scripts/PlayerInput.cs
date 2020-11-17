using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : Entity
{
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent navAgent;
    
    private Vector3 targetDestination;
    
    public enum PlayerState
    {
        Idle,
        Attacking,
        Dead
    }

    private PlayerState playerState = PlayerState.Idle;

    

    [SerializeField] GameObject hpBar;
    [SerializeField] DamageText damageText;

    [SerializeField] float attackRange;

    public bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.Play("Base Layer.unarmed_run_forward_inPlace");
            SelectDestination();
            
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            anim.Play("Base Layer.standing_melee_attack_360_high");
            //attackTarget.TakeDamage(50, this);
            
        }

        //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        //{
        //    if (playerState == PlayerState.Idle)
        //    {
        //        anim.Play("Base Layer.unarmed_idle");
        //    }
        //    else if(playerState == PlayerState.Attacking)
        //    {
        //        anim.Play("Base Layer.standing_melee_attack_horizontal");
        //    }
        //    else if (playerState == PlayerState.Dead)
        //    {
        //        Destroy(gameObject);
        //    }

        //}

       //if (navAgent.velocity.magnitude != 0)
       // {
        //    anim.Play("Base Layer.unarmed_run_forward_inPlace");
            //stop walking
        //} else 
        if(navAgent.remainingDistance <= navAgent.stoppingDistance && playerState == PlayerState.Idle)
        {
            anim.SetBool("isWalking", false);
            anim.Play("Base Layer.unarmed_idle");
            //if (attackTarget != null)
            //{
            //    StartCoroutine(Attack());
            //}
        } else if(playerState == PlayerState.Attacking && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            anim.Play("Base Layer.standing_melee_attack_horizontal");
            anim.SetInteger("CombatState", 1);
        }

        if(attackTarget == null && playerState != PlayerState.Idle)
        {
            playerState = PlayerState.Idle;
            anim.SetInteger("CombatState", 0);
        }
    }

    private void SelectDestination()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (hit.collider.tag == "Enemy")
            {
                Vector3 dir = (hit.point - navAgent.transform.position).normalized;
                attackTarget = hit.collider.GetComponent<Entity>();
                playerState = PlayerState.Attacking;
                navAgent.transform.LookAt(hit.transform);
                targetDestination = hit.point - (dir * attackRange);
            }
            else
            {
                attackTarget = null;
                //anim.SetInteger("CombatState", 0);
                playerState = PlayerState.Idle;
                targetDestination = hit.point;
            }

            navAgent.destination = targetDestination;
            anim.SetBool("isWalking", true);
        }
    }

    public override void TakeDamage(int damageDelt, Entity enemy)
    {
        Debug.Log("Player Takes Damage");
        DamageText temp = Instantiate(damageText, anim.transform);
        if (damageDelt > 0)
        {
            combatData.TakeDamage(damageDelt);
            temp.UpdateText(damageDelt.ToString());
            hpBar.transform.localScale = new Vector3((float)((float)combatData.getCurrentHp / (float)combatData.getMaxHp), 1, 1);
            //anim.SetFloat(animVariableName, (int)EnemyState.TookDamage);
            anim.Play("Base Layer.standing_react_large_gut");
        }
        else
        {
            temp.UpdateText("Miss");
        }

        //anim.Play("Base Layer.BlendTree.Damage");
        if (combatData.getCurrentHp <= 0)
        {
            //anim.SetFloat(animVariableName, (int)EnemyState.Dead);
            //anim.Play("Base Layer.Death");
            playerState = PlayerState.Dead;
        }

        //anim.Play("Base Layer.standing_react_large_gut");
    }
}
