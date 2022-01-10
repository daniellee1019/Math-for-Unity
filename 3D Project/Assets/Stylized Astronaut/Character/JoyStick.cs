using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ�� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ����

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10,150)]
    private float leverRange; // ������ ���� ���� �̻����� �� �Ѿ�� �ϱ� ���� ����

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

    public void OnBeginDrag(PointerEventData eventData) // �巡�׸� ������ ��
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData) // �巡�� ���϶� 
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) // �巡�װ� ������ ��
    {
        lever.anchoredPosition = Vector2.zero; // �巡�װ� ������ ó�� ��ġ�� ���ư�
        isInput = false;

        switch (joystickType)
        {
            case JoystickType.Move:
                controller.Move(Vector2.zero);
                break;
            // ���̽�ƽ 2�� �������� switch��
            //case JoystickType.Rotate:
                //break;
        }
        
    }
    private void ControlJoystickLever(PointerEventData eventData) // ������ ������ ���� �Լ�
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition; // ��ġ ��ġ�� ��ȯ����
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        //������ ��ġ ���̰� ���� �������� ������ �״�� ��ȯ�ϰ�, ��� ��ġ ���̸� ����ȭ �Ͽ� ���� ������ ������ ������ ���� ���� ������ ������ ���ϵ��� �������
        lever.anchoredPosition = inputVector;

        inputDirection = inputVector / leverRange; // inputVector�� �ػ󵵸� ������� ������� ���̶� �������̰� �ʹ� ū ���� �����־� ĳ������ �̵��ӵ��� ���⿣ �ʹ� ����
                                                   // ���� ���� 0 ~ 1 ������ ������ ����� �־� ����ȭ�� ������ ĳ���Ϳ� �����ؾ� �Ǳ� ������ ��������
    }

    public void InputControlVector() // ĳ���Ϳ��� �Էº��͸� �����ϴ� �Լ�
    {

        switch (joystickType)
        {
            case JoystickType.Move:
                controller.Move(inputDirection);
                break;

            // ���̽�ƽ 2�� �������� switch��
            //case JoystickType.Rotate:
                //controller.LookAround(inputDirection);
                //break;
        }
        
        Debug.Log(inputDirection.x + " / " + inputDirection.y);
    }
}
