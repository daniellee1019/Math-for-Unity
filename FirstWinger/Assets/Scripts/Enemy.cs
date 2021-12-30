using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum State : int // -1 ~ 4 까지의 int 상수값을 갖는 열거형 변수들
    {
        None = -1, // 사용전
        Ready = 0, // 준비완료
        Appear, // 등장
        Battle, // 전투중
        Dead, // 사망
        Disappear // 퇴장
    };
    [SerializeField]
    State CurrentState = State.None;

    const float MaxSpeed = 10.0f; // 자연스러운 이동을 위해 가속, 감속 사용 - 비행기의 최대 스피드
    const float MaxSpeedTime = 0.5f; // 비행기의 가속,감속을 표현해주기 위한 시간 

    [SerializeField]
    Vector3 TargetPosition; //이동하는 지점

    [SerializeField]
    float CurrentSpeed; //초기 속도

    Vector3 CurrentVelocity; //3차원에서의 이동벡터 값
    float MoveStratTime = 0.0f; // 움직이기 시작한 기준이 되는 시간
    float BattleStartTime = 0.0f; // 배틀을 시작하는 초기 시간

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
            case State.Disappear: // 플에이어가 보이지 않을 때 move or speed 메소드 실행
                UpdateMove();   
                UpdateSpeed();
                break;
            case State.Battle: // battle 모드일 때 updatebattle 메소드 실행
                UpdateBattle();
                break;
        }
      
    }

    void UpdateSpeed() //가속을 할 때 사용하기 위한 메소드
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStratTime)/MaxSpeedTime); // a,b인자값 0~1 사이의 값을 반환하는데 a에서 b까지 선형보간하면서 증가한다.
                                                                                                     // 도착위치에 가까워 질 수록 속도가 느려지고
                                                                                                     // t값을 기준으로 각 지점을 선형보간하여 목표지점까지 이동.
                                                                                                     // 자연스러운 속도로 부드러운 움직임 표현
    }

    void UpdateMove()
    {
        float distance = Vector3.Distance(TargetPosition, transform.position); // 거리
        if (distance == 0) // 도착했을 때 
        {
            Arrived();
            return;
        }

        CurrentVelocity = (TargetPosition - transform.position).normalized * CurrentSpeed; // 단위벡터 * 현재 속도 해줘 해당 거리에서의 순간적인 속도를 지정해줌 
                                     //자연스러운 움직임을 위한 메서드              // 지속가능한(실시간으로 변하는) 값으로 만들어 주기 위해 ref 값으로 만들어줌.
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, distance / CurrentSpeed, MaxSpeed); // 현재위치, 이동지점, 가속도값, 시간, 속도
                                                                                                         // 시간 = 거리 / 속도
    }

    void Arrived() // 비행기의 도착을 표현하기 위한 메소드
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

    public void Appear(Vector3 targetPos) // 나타났을 때 
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStratTime = Time.time;
    }

    void Disappear(Vector3 targetPos) // 사라졌을 때
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0.0f;

        CurrentState = State.Disappear;
        MoveStratTime = Time.time;
    }

    void UpdateBattle() // 등장하고 3초 뒤에 지정해준 x값으로 가는 메소드  
    {
        if(Time.time - BattleStartTime > 3.0f)
        {
            Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
        }
    }

    private void OnTriggerEnter(Collider other) //3차원상 충돌체로 만들어 주어 내가 다른 오브젝트와 충돌했을 때 이벤트 발생하게 하는 메소드
    {

        Player player = other.GetComponentInParent<Player>();
        if (player)
            player.OnCrash(this);
    }

    public void OnCrash(Player player) // 상대 오브젝트에게 충돌을 했을 때 데미지를 주기위한 메소드
    {
        Debug.Log("OnCrash player = " + player);
    }
}
