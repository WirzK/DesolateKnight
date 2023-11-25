using UnityEngine;
//��Ҫ�Ƽײ�����ɻ����˺���Ϊʲô��

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
    public Stat critChance;//������
    public Stat critPower; //�����˺� default value 150%

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


    [SerializeField] private float ailmentsDuration = 4;//Ԫ��Ч������ʱ��
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

        if (igniteDamageTimer < 0 && isIgnited)//��ȼЧ������˺�����������
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
    public virtual void TakeDamage(int _damage)//����˺�ʹ�øú���
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
    }//����˺����Ҽ���Ƿ����¼�
    public virtual void DoMagicalDamage(CharacterStats _targetStats)//���ħ���˺���Ҳ���д���ӵ����ǵ�����
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
            _targetStats.SetUpIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));//����ȼ�˺�����Ϊ�����˺������֮һ
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private  int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)//��鷨��
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
        //_targetStats.TakeDamage(totalDamage);����Ӧ����������˺�
        DoMagicalDamage(_targetStats);
    }


    protected virtual void Die()
    {
        Debug.Log("u kill one");

        //throw new NotImplementedException();
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)//����״̬�»��׽���
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else    
            totalDamage -= _targetStats.armor.GetValue();
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }//��⻤��
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)//���������Ƿ�ɹ�
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (_targetStats.isShocked)
            totalEvasion -= 20;//������ʱ���������ʣ������Ƿ��б�Ҫ�������޸�Ϊ���͹�������

        if (Random.Range(0, 100) <= totalEvasion)//�����Ƿ����ܳɹ�
        {
            return true;
        }
        return false; 
    }

    private bool CanCrit() //����Ƿ񱩻�
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
    }//���㱩���˺�

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;



}
