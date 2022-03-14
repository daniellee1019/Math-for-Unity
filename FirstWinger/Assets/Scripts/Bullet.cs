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

    bool NeedMove = false; // �߻��ڰ� �̵��� �ʿ����� ����

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

    public void Fire(Actor owner, Vector3 fireDirection, Vector3 direction, float speed, int damage) // �߻縦 �ϱ� ���� ��������
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
        RaycastHit hitinfo; // ����ĳ��Ʈ�� "Ư�� ��ġ���� Ư�� �������� �������� ��� ���ӿ�����Ʈ�� ���� �ϴ� ��ó�� ���. out -> ��� �Ű����� ������ ��, ������ ���� ȣ��

        if (Physics.Linecast(transform.position, transform.position + moveVector, out hitinfo)) // ������ -> ���� ���̿� �̺�Ʈ�� �߻��ϴ� ���� �˾Ƴ��� ���� Linecast ������ ���ش�.
        {
            moveVector = hitinfo.point - transform.position;
            OnBulletCollision(hitinfo.collider);
        }

        return moveVector;
    }

    void OnBulletCollision(Collider collider) // bullet�� collider ����
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
        myCollider.enabled = false; // ���ʿ��� �浹���� ����.

        Hited = true;
        NeedMove = false;

        //Debug.Log("OnBulletCollision collider = " + collider.name);

        GameObject go = SystemManager.Instance.EffectManager.GenerateEffect(0, transform.position);
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        DIsapper();
    }

    private void OnTriggerEnter(Collider other) // �浹�� �̺�Ʈ �߻�
    {
        OnBulletCollision(other);
    }

    bool ProcessDisapperCondition() // �Ѿ� ���� ����.
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
