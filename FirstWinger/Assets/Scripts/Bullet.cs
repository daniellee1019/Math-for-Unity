using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OwnerSide : int
{
    Player = 0,
    Enemy
}
public class Bullet : MonoBehaviour
{

    OwnerSide ownerside = OwnerSide.Player; // �߻��� ����

    [SerializeField]
    Vector3 MoveDirection = Vector3.zero;

    [SerializeField]
    float Speed = 0.0f;

    bool NeedMove = false; // �߻��ڰ� �̵��� �ʿ����� ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();    
    }

    void UpdateMove()
    {

        if (!NeedMove)
            return;

        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;
        transform.position += moveVector;
    }

    public void FIre(OwnerSide FireOwner, Vector3 fireDirection, Vector3 direction, float speed) // �߻縦 �ϱ� ���� ��������
    {
        ownerside = FireOwner;
        transform.position = fireDirection;
        MoveDirection = direction;
        Speed = speed;

        NeedMove = true;
    }
}