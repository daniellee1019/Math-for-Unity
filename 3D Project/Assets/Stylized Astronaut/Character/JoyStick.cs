using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10,150)]
    private float leverRange; // 레버를 일정 범위 이상으로 못 넘어가게 하기 위한 변수

    private Vector2 inputDirection;
    private bool isInput;
    
    [SerializeField]
    private Player controller;


    public enum JoystickType { Move, Rotate }
    public JoystickType joystickType;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (isInput)
        {
            InputControlVector();
        }
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그를 시작할 때
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData) // 드래그 중일때 
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그가 끝날을 때
    {
        lever.anchoredPosition = Vector2.zero; // 드래그가 끝나면 처음 위치로 돌아감
        isInput = false;

        switch (joystickType)
        {
            case JoystickType.Move:
                controller.Move(Vector2.zero);
                break;
            // 조이스틱 2개 쓰기위한 switch문
            //case JoystickType.Rotate:
                //break;
        }
        
    }
    private void ControlJoystickLever(PointerEventData eventData) // 레버의 움직임 구현 함수
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition; // 터치 위치를 반환해줌
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        //레버의 위치 길이가 일정 범위보다 작으면 그대로 반환하고, 길면 위치 길이를 정규화 하여 일정 범위를 곱해줘 레버가 일정 범위 밖으로 나가지 못하도록 만들어줌
        lever.anchoredPosition = inputVector;

        inputDirection = inputVector / leverRange; // inputVector는 해상도를 기반으로 만들어진 값이라 유동적이고 너무 큰 값을 갖고있어 캐릭터의 이동속도로 쓰기엔 너무 빠름
                                                   // 따라서 값을 0 ~ 1 사이의 값으로 만들어 주어 정규화된 값으로 캐릭터에 전달해야 되기 때문에 나누어줌
    }

    public void InputControlVector() // 캐릭터에게 입력벡터를 전달하는 함수
    {

        switch (joystickType)
        {
            case JoystickType.Move:
                controller.Move(inputDirection);
                break;

            // 조이스틱 2개 쓰기위한 switch문
            //case JoystickType.Rotate:
                //controller.LookAround(inputDirection);
                //break;
        }
        
        Debug.Log(inputDirection.x + " / " + inputDirection.y);
    }
}
