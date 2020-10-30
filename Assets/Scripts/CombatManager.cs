using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerData player;
    [SerializeField] EnemyData enemy;

    [SerializeField] Coroutine playerCoroutine;
    [SerializeField] Coroutine enemyCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        playerCoroutine = StartCoroutine(BeginCombat(player, enemy));
        enemyCoroutine = StartCoroutine(BeginCombat(enemy, player));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BeginCombat(CombatData entityAttacker, CombatData entityDefender)
    {
        while(entityDefender.getCurrentHp > 0)
        {
            float checkHit = entityAttacker.getAttackValue / entityDefender.getDefenseValue;

            float rand = Random.Range(0f, 1f);

            if (checkHit > .9f)
            {
                checkHit = .9f;
            }
            else if (checkHit < .05f)
            {
                checkHit = .05f;
            }

            if (checkHit >= rand)
            {
                entityDefender.TakeDamage(Random.Range(entityAttacker.getMinDamageValue, entityAttacker.getMaxDamageValue));
                Debug.Log(entityDefender.getEntityName + " HP: " + entityDefender.getCurrentHp + " / " + entityDefender.getMaxHp + " and took ");

                if(entityDefender.getCurrentHp <= 0)
                {
                    Debug.Log(entityAttacker.getEntityName + " wins!");
                    if(entityAttacker.getEntityName == player.getEntityName)
                    {
                        StopCoroutine(enemyCoroutine);
                        StopCoroutine(playerCoroutine);
                    }
                    else
                    {
                        StopCoroutine(playerCoroutine);
                        StopCoroutine(enemyCoroutine);
                    }
                }
            }
            else
            {
                Debug.Log(entityAttacker.getEntityName + " missed.");
            }

            yield return new WaitForSeconds(entityAttacker.getAttackSpeed);
        }
        
    }
}
