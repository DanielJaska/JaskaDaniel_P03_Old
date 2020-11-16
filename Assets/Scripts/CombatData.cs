using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatData
{
    [SerializeField] string _entityName;
    public string getEntityName { get { return _entityName; } }
    [SerializeField] int _attackValue;
    public float getAttackValue { get { return _attackValue; } }
    [SerializeField] int _defenseValue;
    public float getDefenseValue { get { return _defenseValue; } }

    [SerializeField] int _currentHp;
    public int getCurrentHp { get { return _currentHp; } }
    [SerializeField] int _maxHp;
    public int getMaxHp { get { return _maxHp; } }

    [SerializeField] int _minDamageValue;
    public int getMinDamageValue { get { return _minDamageValue; } }
    [SerializeField] int _maxDamageValue;
    public int getMaxDamageValue { get { return _maxDamageValue; } }
    [SerializeField] float _attackSpeed;
    public float getAttackSpeed { get { return _attackSpeed; } }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        if(_currentHp < 0)
        {
            _currentHp = 0;
        }
    }

}
