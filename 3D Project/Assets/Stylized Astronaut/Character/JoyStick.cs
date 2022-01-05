using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    public Transform Player;
    public Transform Stick;

    private Vector3 StickFirstPos; // ���̽�ƽ ó�� ��ġ
    private Vector3 JoyVec;        // ���̽�ƽ ����(����)
    private float Radius;          // ���̽�ƽ ����� ������
    public bool MoveFlag;

    // Start is called before the first frame update
    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;

        // ĵ���� ũ�⿡ ���� ������ ����
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        MoveFlag = false;
    }
    private void Update()
    {
 
    }
    public void Drag(BaseEventData _Data)
    {
        PointerEventData Data = _Data as PointerEventData;
        Vector3 Pos = Data.position;

        // ���̽�ƽ�� �̵���ų ������ ���ϱ�(����, ������, ��, �Ʒ�)
        JoyVec = (Pos - StickFirstPos).normalized;

        // ���̽�ƽ�� ó�� ��ġ�� ���� ���� ��ġ�ϰ� �ִ� ��ġ�� �Ÿ��� ���Ѵ�.
        float Dis = Vector3.Distance(Pos, StickFirstPos);

        // �Ÿ��� ��������[�� Ŀ���� ���̽�ƽ�� �������� ũ�⸸ŭ�� �̵�
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        else
            Stick.position = StickFirstPos + JoyVec * Radius;

        Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
    }


    // �巡�� ����
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // ��ƽ�� ������ ��ġ��
        JoyVec += Vector3.zero;         // ������ 0����
        MoveFlag = false;
    }
}
