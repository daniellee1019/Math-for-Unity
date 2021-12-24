using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatrix : MonoBehaviour
{

    private Matrix4x4 WorldMat;
    public GameObject ChangeObj;

    void Start()
    {
        FirstMatrix();
        ChangeWorldMatrix();
    }

    private void FirstMatrix()
    {
        Matrix4x4 matrix = transform.localToWorldMatrix;
        Debug.Log("=== 초기행렬 ===");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(matrix.GetRow(i));
        }

    }
    void ChangeWorldMatrix()
    {
        Vector3 Pos = new Vector3(-4, 2, 3);
        Quaternion Rot = Quaternion.Euler(30, 0, 30);
        Vector3 Scal = new Vector3(10, 10, 10);
        WorldMat = Matrix4x4.TRS(Pos, Rot, Scal);
        //WorldMat = Matrix4x4.Translate(new Vector3(-4, 2, 3)) * Matrix4x4.Rotate(Rot) * Matrix4x4.Scale(new Vector3(10,10,10));

        Debug.Log("===== 회전변환을 한 행렬 ====");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(WorldMat.GetRow(i));
        }

 
        GameObject tempObj = Instantiate(ChangeObj);

        tempObj.transform.position = Pos;
        tempObj.transform.rotation = Rot;
        tempObj.transform.localScale = Scal;
        
    }
   
}
