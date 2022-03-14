using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{

    public const string EnemyPath = "Prefabs/Enemy";

    Dictionary<string, GameObject> EnemyFileCache = new Dictionary<string, GameObject>(); // 캐싱을 이용하여 메모리와 리소스를 최적화 하기 위해 딕셔너리로
                                                                                          // string형의 Key로 받아주고 이것을 GameObject로 받아준다.
    // Start is called before the first frame update
    void Start()    
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject Load(string resourcePath) // GameObject를 로드하기 위한 메소드
    {
        GameObject go = null; // 이미 로드가 됐을 수도 있고 안됐을 수도 있기 때문에 null로 생성

        if (EnemyFileCache.ContainsKey(resourcePath)) // 딕션너리 key값에 resourcePath값이 있으면
        {
            go = EnemyFileCache[resourcePath];
        }
        else // 없는경우
        {
            go = Resources.Load<GameObject>(resourcePath); // 경로 안에 있는 GameObject를 불러온다.
            if (!go)
            {
                Debug.LogError("Load error! path = " + resourcePath); // 파일이 어디에 없는지 알아야되기 때문에 에러문구와 경로를 띄운다.
                return null;
            }

            EnemyFileCache.Add(resourcePath, go); // 로드가 정상적으로 되면 딕셔너리 안에 key값과 value값을 추가해준다.
       
        } // 메모리상에만 배치되어 있는 것이기 때문에 Instantiate 를 사용해서 Scene상에 배치해야된다.

        return go;
    }
}
