using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
    float BattleStartTime = 0.0f; // ��Ʋ�� �����ϴ� �ʱ� �ð�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStratTime)/MaxSpeedTime); // a,b���ڰ� 0~1 ������ ���� ��ȯ�ϴµ� a���� b���� ���������ϸ鼭 �����Ѵ�.
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
        if(CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            BattleStartTime = Time.time;
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

    void UpdateBattle() // �����ϰ� 3�� �ڿ� �������� x������ ���� �޼ҵ�  
    {
        if(Time.time - BattleStartTime > 3.0f)
        {
            Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
        }
    }

    private void OnTriggerEnter(Collider other) //3������ �浹ü�� ����� �־� ���� �ٸ� ������Ʈ�� �浹���� �� �̺�Ʈ �߻��ϰ� �ϴ� �޼ҵ�
    {

        Player player = other.GetComponentInParent<Player>();
        if (player)
            player.OnCrash(this);
    }

    public void OnCrash(Player player) // ��� ������Ʈ���� �浹�� ���� �� �������� �ֱ����� �޼ҵ�
    {
        Debug.Log("OnCrash player = " + player);
    }
}
