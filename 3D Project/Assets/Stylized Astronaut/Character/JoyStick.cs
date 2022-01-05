using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    public Transform Player;
    public Transform Stick;

    private Vector3 StickFirstPos; // 조이스틱 처음 위치
    private Vector3 JoyVec;        // 조이스틱 벡터(방향)
    private float Radius;          // 조이스틱 배경의 반지름
    public bool MoveFlag;

    // Start is called before the first frame update
    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;

        // 캔버스 크기에 대한 반지름 조절
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

        // 조이스틱을 이동시킬 방향을 구하기(왼쪽, 오른쪽, 위, 아래)
        JoyVec = (Pos - StickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고 있는 위치의 거리를 구한다.
        float Dis = Vector3.Distance(Pos, StickFirstPos);

        // 거리가 반지름보[다 커지면 조이스틱을 반지름의 크기만큼만 이동
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        else
            Stick.position = StickFirstPos + JoyVec * Radius;

        Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
    }


    // 드래그 종료
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로
        JoyVec += Vector3.zero;         // 방향을 0으로
        MoveFlag = false;
    }
}
