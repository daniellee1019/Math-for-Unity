using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    public const int PlayerBulletIndex = 0;
    public const int EnemyBulletIndex = 1;

    [SerializeField]
    PrefabCacheDatae[] bulletFiles;

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

    public GameObject Load(string resourcePath) // GameObject�� �ε��ϱ� ���� �޼ҵ�
    {
        GameObject go = null; // �̹� �ε尡 ���� ���� �ְ� �ȵ��� ���� �ֱ� ������ null�� ����

        if (FileCache.ContainsKey(resourcePath)) // ��ǳʸ� key���� resourcePath���� ������
        {
            go = FileCache[resourcePath];
        }
        else // ���°��
        {
            go = Resources.Load<GameObject>(resourcePath); // ��� �ȿ� �ִ� GameObject�� �ҷ��´�.
            if (!go)
            {
                Debug.LogError("Load error! path = " + resourcePath); // ������ ��� ������ �˾ƾߵǱ� ������ ���������� ��θ� ����.
                return null;
            }

            
            FileCache.Add(resourcePath, go); // �ε尡 ���������� �Ǹ� ��ųʸ� �ȿ� key���� value���� �߰����ش�.

        } // �޸𸮻󿡸� ��ġ�Ǿ� �ִ� ���̱� ������ Instantiate �� ����ؼ� Scene�� ��ġ�ؾߵȴ�.

        return go;
    }

    public void Prepare()
    {
        for (int i = 0; i < bulletFiles.Length; i++)
        {
            GameObject go = Load(bulletFiles[i].filePath);
            SystemManager.Instance.BulletCacheSystem.GenerateCache(bulletFiles[i].filePath, go, bulletFiles[i].cacheCount);
        }
    }

    public Bullet Generate(int index)
    {
        string filePath = bulletFiles[index].filePath;
        GameObject go = SystemManager.Instance.BulletCacheSystem.Archive(filePath);

        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Filepath = filePath;

        return bullet;
    }

    public bool Remove(Bullet bullet)
    {
        SystemManager.Instance.BulletCacheSystem.Restore(bullet.Filepath, bullet.gameObject);
        return true;
    }
}
