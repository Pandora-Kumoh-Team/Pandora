using Pandora.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStatus
{
    private int _code;
    private float _maxHealth;
    private float _nowHealth;
    private float _baseDamage;
    private float _attackPower;
    private float _defencePower;
    private float _speed;
    private float _attackSpeed;
    private List<Buff> _buffs = new List<Buff>();
    private List<Buff> _attackBuffs = new List<Buff>();

    //------MOB CODE-----
    //
    // [Normal]
    // 000 : FlyingEye
    //
    // [Melee]
    // 100 : Goblin
    // 101 : Mushroom
    // 102 : Skeleton
    // 103 : Assassin Cultist
    //
    // [Test]
    // 999 : TestDummy
    //
    // [boss]
    //  300: 1StageBoss
    //  301: 2StageBoss
    //  302: 3StageBoss
    //-------------------

    public EnemyStatus(string mobName)
    {
        switch (mobName)
        {
            case "FlyingEye":
                _code = 000; _maxHealth = 20; _nowHealth = 20; _baseDamage = 2; _attackPower = 1; _defencePower = 0; _speed = 1; _attackSpeed = 1;
                break;
            case "Goblin":
                _code = 100; _maxHealth = 30; _nowHealth = 30; _baseDamage = 2; _attackPower = 2; _defencePower = 1; _speed = 1; _attackSpeed = 1;
                break;
            case "Mushroom":
                _code = 101; _maxHealth = 40; _nowHealth = 40; _baseDamage = 3; _attackPower = 2; _defencePower = 2; _speed = 1; _attackSpeed = 1;
                break;
            case "Skeleton":
                _code = 102; _maxHealth = 100; _nowHealth = 100; _baseDamage = 4; _attackPower = 2; _defencePower = 3; _speed = 1; _attackSpeed = 1;
                break;
            case "AssassinCultist":
                _code = 103; _maxHealth = 30; _nowHealth = 30; _baseDamage = 2; _attackPower = 2; _defencePower = 2; _speed = 2; _attackSpeed = 1;
                break;
            case "TwistedCultist":
                _code = 104; _maxHealth = 30; _nowHealth = 30; _baseDamage = 2; _attackPower = 2; _defencePower = 2; _speed = 2; _attackSpeed = 1;
                break;
            case "BigCultist":
                _code = 105; _maxHealth = 100; _nowHealth = 100; _baseDamage = 2; _attackPower = 2; _defencePower = 5; _speed = 1; _attackSpeed = 1;
                break;
            case "1StageBoss":
                _code = 300; _maxHealth = 500; _nowHealth = 500; _baseDamage = 10; _attackPower = 3; _defencePower = 5; _speed = 2; _attackSpeed = 8;
                break;
            case "TestDummy":
                _code = 999; _maxHealth = float.MaxValue; _nowHealth = float.MaxValue; _baseDamage = 0; _attackPower = 0; _defencePower = 0; _speed = 0; _attackSpeed = 0;
                break;
        }
    }

    public int Code
    {
        get => _code;
    }

    // 최대 체력
    public float MaxHealth
    {
        get => _maxHealth + _buffs.Sum(buff => buff.MaxHealthChange);
        set => _maxHealth = value;
    }

    public float NowHealth
    {
        get => _nowHealth + _buffs.Sum(buff => buff.NowHealthChange);
        set
        {
            if(value > MaxHealth)
            {
                _nowHealth = MaxHealth;
            }
            else if (value < 0)
            {
                _nowHealth = 0;
            }
            else
            {
                _nowHealth = value;
            }
        }
    }

    public float BaseDamage
    {
        get => _baseDamage + _buffs.Sum(buff => buff.BaseDamageChange);
        set => _baseDamage = value;
    }

    /// <summary>
    /// 공격력 기본데메지에 곱적용 됨 (1.0f = 100%)
    /// </summary>
    public float AttackPower
    {
        get => _attackPower + _buffs.Sum(buff => buff.AttackPowerChange);
        set => _attackPower = value;
    }

    /// <summary>
    /// 방어율 (0~1) ex)0.5 = 50%피해
    /// </summary>
    public float DefencePower
    {
        get => _defencePower + _buffs.Sum(buff => buff.DefencePowerChange);
        set => _defencePower = value;
    }

    public float Speed
    {
        get => _speed + _buffs.Sum(buff => buff.SpeedChange);
        set => _speed = value;
    }

    public float AttackSpeed
    {
        get => _attackSpeed + _buffs.Sum(buff => buff.AttackSpeedChange);
        set => _attackSpeed = value;
    }

    public void AddBuffs(List<Buff> buffs)
    {
        if (buffs == null) return;
        _buffs.AddRange(buffs);
    }

    public void RemoveBuff(Buff buff)
    {
        _buffs.Remove(buff);
    }

    public void AddAttackBuffs(List<Buff> buffs)
    {
        _attackBuffs.AddRange(buffs);
    }

    public void RemoveAttackBuff(Buff buff)
    {
        _attackBuffs.Remove(buff);
    }

    public List<Buff> GetAttackBuffs()
    {
        return _attackBuffs;
    }
}
