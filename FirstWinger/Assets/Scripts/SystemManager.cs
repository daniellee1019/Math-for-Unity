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

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("SystemManager error! Singletone error!");
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Scene �̵����� ������� �ʵ��� ó��
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
