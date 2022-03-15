using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance = null;
    // Start is called before the first frame update

    public static SystemManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    Player player;

    public Player Hero
    {
        get
        {
            if (!player)
            {
                Debug.LogError("Main Player is not setted!");
            }

            return player;
        }
    }

    GamePointAccumulator gamePointAccumulator = new GamePointAccumulator();

    public GamePointAccumulator GamePointAccumulator
    {
        get
        {
            return gamePointAccumulator;
        }
    }

    [SerializeField]
    EffectManager effectManager;

    public EffectManager EffectManager
    {
        get
        {
            return effectManager;
        }
    }

    [SerializeField]
    EnemyMamager enemyMamager;

    public EnemyMamager EnemyMamager
    {
        get
        {
            return enemyMamager;
        }
    }

    [SerializeField]
    BulletManager bulletManager;
    
    public BulletManager BulletManager
    {
        get
        {
            return bulletManager;
        }
    }

    PrefabCacheSystem enemyCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem EnemyCacheSystem
    {
        get
        {
            return enemyCacheSystem;
        }
    }

    PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem EffectCacheSystem
    {
        get
        {
            return effectCacheSystem;
        }
    }

    PrefabCacheSystem bulletCacheSystem = new PrefabCacheSystem();

    public PrefabCacheSystem BulletCacheSystem
    {
        get
        {
            return bulletCacheSystem;
        }
    }

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("SystemManager error! Singletone error!");
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Scene 이동간에 사라지지 않도록 처리
        DontDestroyOnLoad(gameObject);
    }

}
