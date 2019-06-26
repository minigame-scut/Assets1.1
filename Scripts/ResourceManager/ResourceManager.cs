using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//创建管理整个游戏资源的资源容器
//在asset菜单中显示创建这个文件的按钮
//[CreateAssetMenu(menuName ="CreateManagerContainer")]
public class ResourceManager : MonoBehaviour
{
    public AudioClip backgroundMusic1;
    public AudioClip backgroundMusic2;
    static ResourceManager instance;

    public Dictionary<string, AudioClip> musicDictionary;
    public Dictionary<string, GameObject> gameObjectDictionary;
    public Dictionary<string, Material> materialDictionary;
    public static ResourceManager GetInstance()
    {
            return instance;
    }
    
    void Awake()
    {

        if (instance == null)
        {
            // 判定 null 是保证场景跳转时不会出现重复的 GlobalScript 实例 (主要是跳转回上一个场景)
            // 在没有 GlobalScript 实例时才创建 GlobalScript 实例
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // 保证场景中只有唯一的 GlobalScript 实例，如果有多余的则销毁
            Destroy(gameObject);
        }
        musicDictionary = new Dictionary<string, AudioClip>();
        gameObjectDictionary = new Dictionary<string, GameObject>();
        materialDictionary = new Dictionary<string, Material>();
        init();
    }



    void Start()
    {

    }
   
    void init()
    {
        musicDictionary.Add("Music/BGM/map1", Resources.Load<AudioClip>("Music/BGM/map1"));
        musicDictionary.Add("Music/BGM/map1-0", Resources.Load<AudioClip>("Music/BGM/map1-0"));
        musicDictionary.Add("Music/BGM/Interface", Resources.Load<AudioClip>("Music/BGM/Interface"));
        musicDictionary.Add("Music/BGM/map2", Resources.Load<AudioClip>("Music/BGM/map2"));
        musicDictionary.Add("Music/Sounds/Walk", Resources.Load<AudioClip>("Music/Sounds/Walk"));
        musicDictionary.Add("Music/Sounds/Jump", Resources.Load<AudioClip>("Music/Sounds/Jump"));
        musicDictionary.Add("Music/Sounds/Rush", Resources.Load<AudioClip>("Music/Sounds/Rush"));
        musicDictionary.Add("Music/Sounds/Death", Resources.Load<AudioClip>("Music/Sounds/Death"));
        musicDictionary.Add("Music/Sounds/UpSpeedDoor", Resources.Load<AudioClip>("Music/Sounds/UpSpeedDoor"));
        musicDictionary.Add("Music/Sounds/TransferDoor", Resources.Load<AudioClip>("Music/Sounds/TransferDoor"));
        musicDictionary.Add("Music/Sounds/GravityDoor", Resources.Load<AudioClip>("Music/Sounds/GravityDoor"));
        musicDictionary.Add("Music/Sounds/BounceDoor", Resources.Load<AudioClip>("Music/Sounds/BounceDoor"));
        musicDictionary.Add("Music/Sounds/InOutWorldDoor", Resources.Load<AudioClip>("Music/Sounds/InOutWorldDoor"));
        musicDictionary.Add("Music/Sounds/LevelDoor", Resources.Load<AudioClip>("Music/Sounds/LevelDoor"));
        musicDictionary.Add("Music/Sounds/GetProp", Resources.Load<AudioClip>("Music/Sounds/GetProp"));
        musicDictionary.Add("Music/Sounds/MagicalDoor", Resources.Load<AudioClip>("Music/Sounds/MagicalDoor"));

        musicDictionary.Add("Music/Boss/Attack", Resources.Load<AudioClip>("Music/Boss/Attack"));
        musicDictionary.Add("Music/Boss/Bats", Resources.Load<AudioClip>("Music/Boss/Bats"));
        musicDictionary.Add("Music/Boss/Hurt", Resources.Load<AudioClip>("Music/Boss/Hurt"));
        musicDictionary.Add("Music/Boss/Wing", Resources.Load<AudioClip>("Music/Boss/Wing"));

        gameObjectDictionary.Add("GameManagerRes/player", Resources.Load<GameObject>("GameManagerRes/player"));
        gameObjectDictionary.Add("GameManagerRes/bat", Resources.Load<GameObject>("GameManagerRes/bat"));
        gameObjectDictionary.Add("GameManagerRes/AudioManager", Resources.Load<GameObject>("GameManagerRes/AudioManager"));
        gameObjectDictionary.Add("GameManagerRes/UI", Resources.Load<GameObject>("GameManagerRes/UI"));
        gameObjectDictionary.Add("GameManagerRes/bat24", Resources.Load<GameObject>("GameManagerRes/bat24"));
        materialDictionary.Add("Materials/lightM", Resources.Load<Material>("Materials/lightM"));
    }
    public AudioClip getClip(string name)
    {
        if(musicDictionary.ContainsKey(name))
            return musicDictionary[name];
        return null;
    }
    public GameObject getGameObject(string name)
    {
        if (gameObjectDictionary.ContainsKey(name))
            return gameObjectDictionary[name];
        return null;
    }
    public Material getMaterial(string name)
    {
        if (materialDictionary.ContainsKey(name))
            return materialDictionary[name];
        return null;
    }
}
