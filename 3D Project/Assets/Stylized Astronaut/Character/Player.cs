using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[SerializeField]
	private Transform characterBody;
	[SerializeField]
	private Transform cameraArm;

    
	private Animator anim;
	private CharacterController controller;
    public float gravity = 20.0f;



    void Start () 
	{
		controller = GetComponent<CharacterController>();
		anim = characterBody.GetComponent<Animator>();
	}

	public void Update()
	{

		

	}
    public void Move(Vector2 inputDirection)
    {
        // �̵� ���� ���ϱ� 1
        //Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);

        // �̵� ���� ���ϱ� 2
        //Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        // �̵� ����Ű �Է� �� ��������
        Vector2 moveInput = inputDirection;
        // �̵� ����Ű �Է� ���� : �̵� ���� ���Ͱ� 0���� ũ�� �Է��� �߻��ϰ� �ִ� ��
        bool isMove = moveInput.magnitude != 0;
        // �Է��� �߻��ϴ� ���̶�� �̵� �ִϸ��̼� ���
        
        if (isMove)
        {
            anim.SetInteger("AnimationPar", 1);
            // ī�޶� �ٶ󺸴� ����
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            // ī�޶��� ������ ����
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            // �̵� ����
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // �̵��� �� ī�޶� ���� ���� �ٶ󺸱�
            //characterBody.forward = lookForward;
            // �̵��� �� �̵� ���� �ٶ󺸱�
            characterBody.forward = moveDir;
            // �̵�
            controller.Move(moveDir * Time.deltaTime * 6.5f);
            moveDir.y -= gravity * Time.deltaTime;

        }
        else
            anim.SetInteger("AnimationPar", 0);
    }
    //  �̰� ���̽�ƽ �ΰ� �� ���� �����ϱ� ������ �����ǵ�
    //  ������ ���̽�ƽ���� �����̰� ���� ���̽�ƽ���� ȭ�� �����̴� �ڵ���
    //public void LookAround(Vector3 inputDirection)
    //{
    //    // ���콺 �̵� �� ����
    //    Vector2 mouseDelta = inputDirection;
    //    // ī�޶��� ���� ������ ���Ϸ� ������ ����
    //    Vector3 camAngle = cameraArm.rotation.eulerAngles;
    //    // ī�޶��� ��ġ �� ���
    //    float x = camAngle.x - mouseDelta.y;

    //    // ī�޶� ��ġ ���� �������� 70�� �Ʒ������� 25�� �̻� �������� ���ϰ� ����
    //    if (x < 180f)
    //    {
    //        x = Mathf.Clamp(x, -1f, 70f);
    //    }
    //    else
    //    {
    //        x = Mathf.Clamp(x, 335f, 361f);
    //    }

    //    // ī�޶� �� ȸ�� ��Ű��
    //    cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    //}
}
