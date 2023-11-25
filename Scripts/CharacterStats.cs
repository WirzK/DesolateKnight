using UnityEngine;
//需要破甲才能造成火焰伤害？为什么？

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stats")]
    public Stat strength;//1 point increase damage by 1 and crit.power by 1 percent
    public Stat agility;// 1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelligence;// 1 point increace magic damage by 1 and magic resistance by 3 
    public Stat vitality;//1 point increace health by 3 or 5 points

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;//暴击率
    public Stat critPower; //暴击伤害 default value 150%

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited; //does damage over time
    public bool isChilled; //reduce armor by 20%
    public bool isShocked; //reduce accuracy by 20%, or reduce strength?


    [SerializeField] private float ailmentsDuration = 4;//元素效果持续时间
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCoolDown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    
    public int currentHealth;

    public System.Action onHealthChanged;
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
  

    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;

        if (igniteDamageTimer < 0 && isIgnited)//点燃效果造成伤害，单独结算
        {
            Debug.Log("Take burn damage" + igniteDamage);

            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0)
                Die();
            igniteDamageTimer = igniteDamageCoolDown;
        }

    }
    
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        if(_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgnitedFxFor(ailmentsDuration);
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;
            float slowPercentage = 0.2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = ailmentsDuration;

            fx.ShockedFxFor(ailmentsDuration);
        }

    }

    public void SetUpIgniteDamage(int _damage) => igniteDamage = _damage; 
    public virtual void TakeDamage(int _damage)//造成伤害使用该函数
    {
        DecreaseHealthBy(_damage);
        Debug.Log(_damage);
        if (currentHealth < 0)
            Die();

        onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }//造成伤害，且检测是否有事件
    public virtual void DoMagicalDamage(CharacterStats _targetStats)//造成魔法伤害，也许该写在子弹或是刀刃中
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;
        
        while (!canApplyChill && !canApplyIgnite && !canApplyShock)
        {
            if(Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("is chilled");
                return;
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        if (canApplyIgnite)
            _targetStats.SetUpIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));//将点燃伤害设置为火焰伤害的五分之一
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private  int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)//检查法抗
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);


        }
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        //_targetStats.TakeDamage(totalDamage);此行应当造成物理伤害
        DoMagicalDamage(_targetStats);
    }


    protected virtual void Die()
    {
        Debug.Log("u kill one");

        //throw new NotImplementedException();
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)//冻结状态下护甲降低
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else    
            totalDamage -= _targetStats.armor.GetValue();
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }//检测护甲
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)//测试闪避是否成功
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (_targetStats.isShocked)
            totalEvasion -= 20;//被震慑时降低闪避率，但这是否有必要？不如修改为降低攻击力？

        if (Random.Range(0, 100) <= totalEvasion)//测试是否闪避成功
        {
            return true;
        }
        return false; 
    }

    private bool CanCrit() //检测是否暴击
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0,100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }//计算暴击伤害

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;



}
