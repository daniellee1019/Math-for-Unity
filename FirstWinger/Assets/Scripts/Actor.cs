using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField]
    protected int MaxHP = 100;

    [SerializeField]
    protected int CurrentHP;

    [SerializeField]
    protected int Damage = 1;

    [SerializeField]
    protected int crashDamage = 100;

    [SerializeField]
    bool isDead = false;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    protected int CrashDamage
    {
        get
        {
            return crashDamage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize() // protected : 자식클래스에서만 접근 가능 / virtual : 자식클래스에서 이 함수를 재정의 가능
    {
        CurrentHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActor();
    }

    protected virtual void UpdateActor()
    {

    }

    public virtual void OnBulletHited(Actor attacker, int damage) // 총알 데미지
    {
        Debug.Log("OnBullet damage = " + attacker.name + ", damage = " + damage);
        DecreaseHP(attacker, damage);
    }

    public virtual void OnCrash(Actor attacker, int damage) // 충돌 데미지
    {
        Debug.Log("OnCrash damage = " + attacker.name + ", damage = " + damage);
        DecreaseHP(attacker, damage);
    }

    void DecreaseHP(Actor attacker, int value)
    {
        if (isDead)
            return;

        CurrentHP -= value;

        if (CurrentHP < 0)
            CurrentHP = 0;

        if (CurrentHP == 0)
        {
            OnDead(attacker);
        }

    }

    protected virtual void OnDead(Actor killer)
    {
        Debug.Log(name + "OnDead");
        isDead = true;

        SystemManager.Instance.EffectManager.GenerateEffect(1, transform.position);
    }
}
