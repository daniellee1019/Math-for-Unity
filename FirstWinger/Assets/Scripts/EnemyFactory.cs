using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{

    public const string EnemyPath = "Prefabs/Enemy";

    Dictionary<string, GameObject> EnemyFileCache = new Dictionary<string, GameObject>(); // ĳ���� �̿��Ͽ� �޸𸮿� ���ҽ��� ����ȭ �ϱ� ���� ��ųʸ���
                                                                                          // string���� Key�� �޾��ְ� �̰��� GameObject�� �޾��ش�.
    // Start is called before the first frame update
    void Start()    
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject Load(string resourcePath) // GameObject�� �ε��ϱ� ���� �޼ҵ�
    {
        GameObject go = null; // �̹� �ε尡 ���� ���� �ְ� �ȵ��� ���� �ֱ� ������ null�� ����

        if (EnemyFileCache.ContainsKey(resourcePath)) // ��ǳʸ� key���� resourcePath���� ������
        {
            go = EnemyFileCache[resourcePath];
        }
        else // ���°��
        {
            go = Resources.Load<GameObject>(resourcePath); // ��� �ȿ� �ִ� GameObject�� �ҷ��´�.
            if (!go)
            {
                Debug.LogError("Load error! path = " + resourcePath); // ������ ��� ������ �˾ƾߵǱ� ������ ���������� ��θ� ����.
                return null;
            }

            EnemyFileCache.Add(resourcePath, go); // �ε尡 ���������� �Ǹ� ��ųʸ� �ȿ� key���� value���� �߰����ش�.
       
        } // �޸𸮻󿡸� ��ġ�Ǿ� �ִ� ���̱� ������ Instantiate �� ����ؼ� Scene�� ��ġ�ؾߵȴ�.

        return go;
    }
}
