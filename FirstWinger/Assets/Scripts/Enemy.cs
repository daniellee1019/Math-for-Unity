using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{

    public enum State : int // -1 ~ 4 ������ int ������� ���� ������ ������
    {
        None = -1, // �����
        Ready = 0, // �غ�Ϸ�
        Appear, // ����
        Battle, // ������
        Dead, // ���
        Disappear // ����
    };
    [SerializeField]
    State CurrentState = State.None;

    const float MaxSpeed = 10.0f; // �ڿ������� �̵��� ���� ����, ���� ��� - ������� �ִ� ���ǵ�
    const float MaxSpeedTime = 0.5f; // ������� ����,������ ǥ�����ֱ� ���� �ð� 

    [SerializeField]
    Vector3 TargetPosition; //�̵��ϴ� ����

    [SerializeField]
    float CurrentSpeed; //�ʱ� �ӵ�

    Vector3 CurrentVelocity; //3���������� �̵����� ��
    float MoveStratTime = 0.0f; // �����̱� ������ ������ �Ǵ� �ð�
    

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    float BulletSpeed = 1;

    float LastBattleUpdateTime = 0.0f;

    [SerializeField]
    int FireRemainCount = 1; // Enemy�� �� �Ѿ��� ����

    [SerializeField]
    int GamePoint = 10;

    // Update is called once per frame
    protected override void UpdateActor()
    {

        switch (CurrentState)
        {
            case State.None:
            case State.Ready:
                break;
            case State.Dead:
                break;
            case State.Appear:
            case State.Disappear: // �ÿ��̾ ������ ���� �� move or speed �޼ҵ� ����
                UpdateMove();
                UpdateSpeed();
                break;
            case State.Battle: // battle ����� �� updatebattle �޼ҵ� ����
                UpdateBattle();
                break;
        }

    }

    void UpdateSpeed() //������ �� �� ����ϱ� ���� �޼ҵ�
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStratTime) / MaxSpeedTime); // a,b���ڰ� 0~1 ������ ���� ��ȯ�ϴµ� a���� b���� ���������ϸ鼭 �����Ѵ�.
                                                                                                       // ������ġ�� ����� �� ���� �ӵ��� ��������
                                                                                                       // t���� �������� �� ������ ���������Ͽ� ��ǥ�������� �̵�.
                                                                                                       // �ڿ������� �ӵ��� �ε巯�� ������ ǥ��
    }

    void UpdateMove()
    {
        float distance = Vector3.Distance(TargetPosition, transform.position); // �Ÿ�
        if (distance == 0) // �������� �� 
        {
            Arrived();
            return;
        }

        CurrentVelocity = (TargetPosition - transform.position).normalized * CurrentSpeed; // �������� * ���� �ӵ� ���� �ش� �Ÿ������� �������� �ӵ��� �������� 
                                                                                           //�ڿ������� �������� ���� �޼���              // ���Ӱ�����(�ǽð����� ���ϴ�) ������ ����� �ֱ� ���� ref ������ �������.
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, distance / CurrentSpeed, MaxSpeed); // ������ġ, �̵�����, ���ӵ���, �ð�, �ӵ�
                                                                                                                                             // �ð� = �Ÿ� / �ӵ�
    }

    void Arrived() // ������� ������ ǥ���ϱ� ���� �޼ҵ�
    {
        CurrentSpeed = 0.0f;
        if (CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            LastBattleUpdateTime = Time.time;
        }
        else //if(CurrentState == State.Disappear)
        {
            CurrentState = State.None;
        }
    }

    public void Appear(Vector3 targetPos) // ��Ÿ���� �� 
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStratTime = Time.time;
    }

    void Disappear(Vector3 targetPos) // ������� ��
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0.0f;

        CurrentState = State.Disappear;
        MoveStratTime = Time.time;
    }

    void UpdateBattle() // �����ϰ� ��� �Ѿ��� �� ��� �������� x������ ���� �޼ҵ�  
    {
        if (Time.time - LastBattleUpdateTime > 1.0f)
        {
            if(FireRemainCount > 0)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
            }

            LastBattleUpdateTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other) //3������ �浹ü�� ����� �־� ���� �ٸ� ������Ʈ�� �浹���� �� �̺�Ʈ �߻��ϰ� �ϴ� �޼ҵ�
    {

        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            if (!player.IsDead)
                player.OnCrash(this, CrashDamage);
        }
            
    }

    public override void OnCrash(Actor attacker, int damage) // ��� ������Ʈ���� �浹�� ���� �� �������� �ֱ����� �޼ҵ�
    {
        base.OnCrash(attacker, damage);
    }

    public void Fire() 
    {
            GameObject go = Instantiate(Bullet);
            Bullet bullet = go.GetComponent<Bullet>();

            bullet.Fire(this, FireTransform.position, -FireTransform.right, BulletSpeed, Damage);
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);

        SystemManager.Instance.GamePointAccumulator.Accumulate(GamePoint);

        CurrentState = State.Dead;
        Destroy(gameObject);
    }
}