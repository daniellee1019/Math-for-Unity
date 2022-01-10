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
        // 이동 방향 구하기 1
        //Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);

        // 이동 방향 구하기 2
        //Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        // 이동 방향키 입력 값 가져오기
        Vector2 moveInput = inputDirection;
        // 이동 방향키 입력 판정 : 이동 방향 벡터가 0보다 크면 입력이 발생하고 있는 중
        bool isMove = moveInput.magnitude != 0;
        // 입력이 발생하는 중이라면 이동 애니메이션 재생
        
        if (isMove)
        {
            anim.SetInteger("AnimationPar", 1);
            // 카메라가 바라보는 방향
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            // 카메라의 오른쪽 방향
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            // 이동 방향
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            // 이동할 때 카메라가 보는 방향 바라보기
            //characterBody.forward = lookForward;
            // 이동할 때 이동 방향 바라보기
            characterBody.forward = moveDir;
            // 이동
            controller.Move(moveDir * Time.deltaTime * 6.5f);
            moveDir.y -= gravity * Time.deltaTime;

        }
        else
            anim.SetInteger("AnimationPar", 0);
    }
    //  이건 조이스틱 두개 쓸 수도 있으니까 구현해 놓은건데
    //  오른쪽 조이스틱으로 움직이고 왼쪽 조이스틱으로 화면 움직이는 코드임
    //public void LookAround(Vector3 inputDirection)
    //{
    //    // 마우스 이동 값 검출
    //    Vector2 mouseDelta = inputDirection;
    //    // 카메라의 원래 각도를 오일러 각으로 저장
    //    Vector3 camAngle = cameraArm.rotation.eulerAngles;
    //    // 카메라의 피치 값 계산
    //    float x = camAngle.x - mouseDelta.y;

    //    // 카메라 피치 값을 위쪽으로 70도 아래쪽으로 25도 이상 움직이지 못하게 제한
    //    if (x < 180f)
    //    {
    //        x = Mathf.Clamp(x, -1f, 70f);
    //    }
    //    else
    //    {
    //        x = Mathf.Clamp(x, 335f, 361f);
    //    }

    //    // 카메라 암 회전 시키기
    //    cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    //}
}
