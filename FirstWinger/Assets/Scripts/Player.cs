using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    Vector3 MoveVector = Vector3.zero;

    [SerializeField]
    float Speed;

    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    Transform MainBGQuadTransform;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    float BulletSpeed = 1;

    float timer = 0.0f;
    public float waitingTime = 0.1f;

    // Update is called once per frame
    protected override void UpdateActor()
    {
        UpdateMove();
        
    }
    
    void UpdateMove()
    {
        
        if(MoveVector.sqrMagnitude == 0)
            return;

        MoveVector = AdjustMoveVector(MoveVector);

        transform.position += MoveVector;
    }

    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;
    }
   
    Vector3 AdjustMoveVector (Vector3 moveVector)
    {
        Vector3 result = Vector3.zero;

        result = boxCollider.transform.position + boxCollider.center + moveVector;

        if ( result.x-boxCollider.size.x *0.5f < -MainBGQuadTransform.localScale.x * 0.5f)
            moveVector.x = 0;

        if (result.x + boxCollider.size.x * 0.5f > MainBGQuadTransform.localScale.x * 0.5f)
            moveVector.x = 0;

        if (result.y - boxCollider.size.y * 0.5f < -MainBGQuadTransform.localScale.y * 0.5f)
            moveVector.y = 0;

        if (result.y + boxCollider.size.y * 0.5f > MainBGQuadTransform.localScale.y * 0.5f)
            moveVector.y = 0;

        return moveVector;
    }
    private void OnTriggerEnter(Collider other) //3차원상 충돌체로 만들어 주어 내가 다른 오브젝트와 충돌했을 때 이벤트 발생하게 하는 메소드
    {

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
            if (!enemy.IsDead)
            {
                enemy.OnCrash(this, CrashDamage);
            }
    }
    public override void OnCrash(Actor attacker, int damage) // 상대 오브젝트에게 충돌을 했을 때 데미지를 주기위한 메소드
    {
        base.OnCrash(attacker, damage);
    }

    public void Fire() // 일정한 시간 간격으로 총알 발사
    {

        timer += Time.deltaTime;

        if (timer > waitingTime)
        {
            Bullet bullet = SystemManager.Instance.BulletManager.Generate(BulletManager.PlayerBulletIndex);
            bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed , Damage);

            timer = 0;
        }
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);
        gameObject.SetActive(false);
    }

}



