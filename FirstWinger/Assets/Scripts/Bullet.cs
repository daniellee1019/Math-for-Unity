using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    const float LifeTime = 15.0f;

    [SerializeField]
    Vector3 MoveDirection = Vector3.zero;

    [SerializeField]
    float Speed = 0.0f;

    bool NeedMove = false; // 발사자가 이동이 필요한지 여부

    float FiredTime;
    bool Hited = false;

    [SerializeField]
    int Damage = 1;

    Actor Owner;
    // Update is called once per frame
    void Update()
    {
        if (ProcessDisapperCondition())
            return;
        UpdateMove();    
    }

    void UpdateMove()
    {

        if (!NeedMove)
            return;

        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;
        moveVector = AdjustMove(moveVector);
        transform.position += moveVector;
    }

    public void Fire(Actor owner, Vector3 fireDirection, Vector3 direction, float speed, int damage) // 발사를 하기 위한 변수지정
    {
        Owner = owner;
        transform.position = fireDirection;
        MoveDirection = direction;
        Speed = speed;
        Damage = damage;

        NeedMove = true;
        FiredTime = Time.time;
    }

    Vector3 AdjustMove(Vector3 moveVector)
    {
        RaycastHit hitinfo; // 레이캐스트는 "특정 위치에서 특정 방향으로 레이저를 쏘아 게임오브젝트를 감지 하는 것처럼 사용. out -> 출력 매개변수 한정자 즉, 참조에 의한 호출

        if (Physics.Linecast(transform.position, transform.position + moveVector, out hitinfo)) // 시작점 -> 끝점 사이에 이벤트가 발생하는 것을 알아내기 위해 Linecast 구문을 써준다.
        {
            moveVector = hitinfo.point - transform.position;
            OnBulletCollision(hitinfo.collider);
        }

        return moveVector;
    }

    void OnBulletCollision(Collider collider) // bullet의 collider 감지
    {
        if (Hited)
            return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")
            || collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            return;
        }

        Actor actor = collider.GetComponentInParent<Actor>();
        if (actor && actor.IsDead)
            return;

        actor.OnBulletHited(Owner, Damage);

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false; // 불필요한 충돌부하 제거.

        Hited = true;
        NeedMove = false;

        //Debug.Log("OnBulletCollision collider = " + collider.name);

        GameObject go = SystemManager.Instance.EffectManager.GenerateEffect(0, transform.position);
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        DIsapper();
    }

    private void OnTriggerEnter(Collider other) // 충돌시 이벤트 발생
    {
        OnBulletCollision(other);
    }

    bool ProcessDisapperCondition() // 총알 제거 조건.
    {
        if(transform.position.x > 15.0f || transform.position.x < -15.0f
            || transform.position.y > 15.0f || transform.position.y < -15.0f)
        {
            DIsapper();
            return true;
        }
        else if(Time.time - FiredTime > LifeTime)
        {
            DIsapper();
            return true;
        }


        return false;
    }

    void DIsapper()
    {
        Destroy(gameObject);
    }
}
