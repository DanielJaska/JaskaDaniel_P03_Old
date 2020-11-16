using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Animator anim;
    public NavMeshAgent navAgent;
    

    private Vector3 targetDestination;

    private string walkingBoolName = "isMoving";
    public enum PlayerState
    {
        Idle,
        Attacking,
        Dead
    }

    private PlayerState playerState = PlayerState.Idle;

    private Enemy attackTarget;

    public PlayerData playerData;

    [SerializeField] GameObject hpBar;
    [SerializeField] DamageText damageText;

    public bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                //Debug.Log("destination is " + targetDestination);
                if (hit.collider.tag == "Enemy")
                {
                    Vector3 dir = (hit.point - navAgent.transform.position).normalized;
                    attackTarget = hit.collider.GetComponent<Enemy>();
                    navAgent.transform.LookAt(hit.transform);
                    targetDestination = hit.point - (dir * attackTarget.range);
                } else
                {
                    attackTarget = null;
                    targetDestination = hit.point;
                }

                
                navAgent.destination = targetDestination;
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            anim.Play("Base Layer.standing_melee_attack_360_high");
            
        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        {
            if (playerState == PlayerState.Idle)
            {
                anim.Play("Base Layer.unarmed_idle");
            }
            else if(playerState == PlayerState.Attacking)
            {
                anim.Play("Base Layer.standing_melee_attack_horizontal");
            }
            else if (playerState == PlayerState.Dead)
            {
                Destroy(gameObject);
            }

        }

        if (navAgent.velocity.magnitude != 0 && anim.GetBool(walkingBoolName) == false)
        {
            anim.SetBool(walkingBoolName, true);
        //stop walking
        } else if (anim.GetBool(walkingBoolName) == true && navAgent.velocity.magnitude <= 0)
        {
            anim.SetBool(walkingBoolName, false);
            if(attackTarget != null)
            {
                StartCoroutine(Attack());
            }
        }
    }

    public void TakeDamage(int damageDelt)
    {
        playerData.TakeDamage(damageDelt);
        DamageText temp = Instantiate(damageText, anim.transform);
        if (damageDelt > 0)
        {
            temp.UpdateText(damageDelt.ToString());
            hpBar.transform.localScale = new Vector3((float)((float)playerData.getCurrentHp / (float)playerData.getMaxHp), 1, 1);
            //anim.SetFloat(animVariableName, (int)EnemyState.TookDamage);
            anim.Play("Base Layer.Damage");
        }
        else
        {
            //temp.UpdateText("Miss");
        }

        //anim.Play("Base Layer.BlendTree.Damage");
        if (playerData.getCurrentHp <= 0)
        {
            //anim.SetFloat(animVariableName, (int)EnemyState.Dead);
            anim.Play("Base Layer.Death");
            playerState = PlayerState.Dead;
        }

        anim.Play("Base Layer.standing_react_large_gut");
    }

    public IEnumerator Attack()
    {
        playerState = PlayerState.Attacking;

        while (attackTarget.enemyData.getCurrentHp > 0)
        {
            //anim.Play("Base Layer.standing_melee_attack_horizontal");

            yield return new WaitUntil(() => canAttack == true);

            canAttack = false;

            float hitChance = playerData.getAttackValue / attackTarget.enemyData.getDefenseValue;

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
                int damageToDeal = Random.Range(playerData.getMinDamageValue, playerData.getMaxDamageValue + 1);
                attackTarget.TakeDamage(damageToDeal, this);
            }
            else
            {
                attackTarget.TakeDamage(-1, this);
            }
        }
        playerState = PlayerState.Idle;
        //anim.Play("Base Layer.unarmed_idle");
    }
}
