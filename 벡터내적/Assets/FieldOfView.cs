using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public GameObject[] enemyObjectArray;

    public float objectSpeed = 3.5f;
    public float viewAngle = 60f;
    public float viewDistance = 5f;

    void Update()
    {
        MovePlayer();
        FindEnemy();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(x, y) * (objectSpeed * Time.deltaTime));
    }

    void FindEnemy()
    {
        int i = 0;
        foreach (var enemy in enemyObjectArray)
        {
            Vector3 distanceVec = enemy.transform.position - transform.position;
            if(distanceVec.magnitude < viewDistance)
            {
                Vector3 dirVec = distanceVec.normalized;

                if (Vector3.Dot(transform.up, dirVec) > Mathf.Cos(viewAngle * Mathf.Deg2Rad))
                {
                    i++;
                    Debug.DrawRay(transform.position, distanceVec.magnitude * dirVec, Color.red);
                }
                else
                {
                    Debug.DrawRay(transform.position, distanceVec.magnitude * dirVec, Color.cyan);
                }
            }
        }

        Debug.Log(i + "명의 적을 발견했습니다!");
    }
}
