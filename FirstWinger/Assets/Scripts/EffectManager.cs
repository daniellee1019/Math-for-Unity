using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public const int BulletDisappearFxIndex = 0;
    public const int ActorDeadFxIndex = 1;


    [SerializeField]
    PrefabCacheDatae[] effectFiles;

    Dictionary<string, GameObject> FileCache = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Prepare();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GenerateEffect(int index, Vector3 position)
    {
        if (index < 0 || index >= effectFiles.Length)
        {
            Debug.LogError("GenerateEffect error! out of range! index = " + index);
            return null;
        }

        string filePath = effectFiles[index].filePath;
        GameObject go = SystemManager.Instance.EffectCacheSystem.Archive(filePath);
        go.transform.position = position;

        AutoCachableEffect effect = go.GetComponent<AutoCachableEffect>();
        effect.FilePath = filePath;

        return go;
    }

    public GameObject Load(string resourcePath) // GameObject를 로드하기 위한 메소드
    {
        GameObject go = null; // 이미 로드가 됐을 수도 있고 안됐을 수도 있기 때문에 null로 생성

        if (FileCache.ContainsKey(resourcePath)) // 딕션너리 key값에 resourcePath값이 있으면
        {
            go = FileCache[resourcePath];
        }
        else // 없는경우
        {
            go = Resources.Load<GameObject>(resourcePath); // 경로 안에 있는 GameObject를 불러온다.
            if (!go)
            {
                Debug.LogError("Load error! path = " + resourcePath); // 파일이 어디에 없는지 알아야되기 때문에 에러문구와 경로를 띄운다.
                return null;
            }


            FileCache.Add(resourcePath, go); // 로드가 정상적으로 되면 딕셔너리 안에 key값과 value값을 추가해준다.

        } // 메모리상에만 배치되어 있는 것이기 때문에 Instantiate 를 사용해서 Scene상에 배치해야된다.

        return go;
    }

    public void Prepare()
    {
        for (int i = 0; i < effectFiles.Length; i++)
        {
            GameObject go = Load(effectFiles[i].filePath);
            SystemManager.Instance.EffectCacheSystem.GenerateCache(effectFiles[i].filePath, go, effectFiles[i].cacheCount);
        }
    }

    public bool RemoveEffect(AutoCachableEffect effect)
    {
        SystemManager.Instance.EffectCacheSystem.Restore(effect.FilePath, effect.gameObject);
        return true;
    }

}
