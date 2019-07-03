using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

//游戏管理类

public class GameManager : MonoBehaviour
{
    // public string testBirthPlace;
    public int BattleIndexNum = 0;
    public int MapIndexNum = 0;
    public static GameManager instance;

    Dictionary<string, string> mapData;

    //玩家的object
    static public GameObject player;
    //持有道具数据
    public PropData propData;

    //持有的当前场景的SceneManager
    GameObject sManager;

    //持有的当前场景的UI
    GameObject UIPrefab;
    GameObject UI;
    //是否由NPC引起的暂停
    private bool NPCPause = false;
    //GameObject UIAnmationPrefab;
    //GameObject UIAnmation;


    //持有的当前场景的AudioManager
    GameObject aManagerPrefab;
    static GameObject aManager;
    private static float musicVolume;
    private static float soundVolume;

    public string sceneName;



    static string toPlace; //通往关卡的标号
    static string toTrans;  //通往的传送门
    static string toWorld;  //通往的里表世界门


    int toPlaceIndex;  //小关卡的下标
    int toBigPlaceIndex;  //大关卡的下标




    //不销毁GameManager
    // DontDestroyOnLoad(this); 
    void Awake()
    {
        propData = new PropData();
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
    }




    // Start is called before the first frame update
    void Start()
    {

        //instance = this;
        //隐藏鼠标
        Cursor.visible = false;
        //读取玩家资源
        //建议使用资源管理类
        // player = Resources.Load<GameObject>("GameManagerRes/playerTestPrefab");

        //读取开始游戏过渡动画资源
        //UIAnmationPrefab = Resources.Load<GameObject>("GameManagerRes/UIToGameAnimation");


        //初始化映射关系
        SceneMapData.getInstance().init();
        mapData = SceneMapData.getInstance().getMapData();

        //获取当前场景的name
        sceneName = SceneManager.GetActiveScene().name;

        //监听玩家关卡转换
        EventCenter.AddListener<string>(MyEventType.NEXTPLACE, toNextPlace);

        //监听传送门的转换
        EventCenter.AddListener<string>(MyEventType.TRANSDOORTOWORLD, toTransDoor);

        //监听里表世界门的转换
        EventCenter.AddListener<string>(MyEventType.INWORLDDOOR, toWorldDoor);
        EventCenter.AddListener<string>(MyEventType.OUTWORLDDOOR, toWorldDoor);

        //创建当前场景的sceneManager
        if (sceneName != "Interface")
            buildSceneManager(GameObject.Find("birthPlace" + BattleIndexNum + "-" + MapIndexNum + "-1").transform.position);
        //  buildSceneManager(new Vector3(-7.733808f, 3.064172f, 0));


        //监听进入新游戏
        EventCenter.AddListener<string>(MyEventType.UITOGAME, buildGameScene);
        //监听返回主菜单
        EventCenter.AddListener(MyEventType.GAMETOUI, buildUIScene);
        //监听继续游戏
        EventCenter.AddListener<Vector3>(MyEventType.CONTINUEGAME, buildGameScene);
        //监听进入下一关
        EventCenter.AddListener(MyEventType.NEXTMAP, gotoNextMap);
        //监听暂停游戏
        EventCenter.AddListener<bool>(MyEventType.PLAYERPAUSE, pauseGame);
        //创建当前场景的AudioManager
        buildAudioManager(new Vector3(0, 0, 0));

        //创建当前场景的UI
        buildUI();
    }

    // Update is called once per frame
    void Update()
    {
        //隐藏鼠标
        if(Cursor.visible)
           Cursor.visible = false;
        sceneName = SceneManager.GetActiveScene().name;
        //保存玩家数据
        if (Input.GetKeyUp(KeyCode.T))
        {
            SavePlayerData.SetData("Save/PlayerData.sav", sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().getPlayerData());
        }
        // Debug.Log(sceneName);
        //暂停游戏并显示UI
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu"))
        {
            //显示UI
            UI.SetActive(true);
            //暂停游戏
            //pauseGame(true);
            //Debug.Log(sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().isPause);
            sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().isPause = true;
            //Debug.Log(sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().isPause);
        }
        if (UI != null && UI.activeSelf == false && sManager.GetComponent<SManager>().getGamePlayer() != null
            && sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().isPause && !NPCPause)
        {
            sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().isPause = false;
        }
    }

    //转化关卡
    void toNextPlace(string nowPlace)
    {
        //获取通往的关卡的标号
        toPlace = mapData[nowPlace];
        toPlaceIndex = 0;  //小关卡的下标
        toBigPlaceIndex = 0;  //大关卡的下标

        try
        {
            toBigPlaceIndex = int.Parse(toPlace.Substring(toPlace.IndexOf('-') - 1, 1));
            toPlaceIndex = int.Parse(toPlace.Substring(toPlace.IndexOf('-') + 1, 1));
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }

        Debug.Log(toPlace);
        Debug.Log(toPlaceIndex);
        //播放切换场景动画
        //mapSwitcher.GetComponent<SwitchMap>().PlayCloseMap();

        //切关保存玩家的数据
        SavePlayerData.SetData("Save/PlayerData.sav", sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().getPlayerData());


        ////转移到新的场景
        SceneManager.LoadScene("map" + toBigPlaceIndex + '-' + toPlaceIndex);

        //StartCoroutine(waitForMapSwitch(toBigPlaceIndex, toPlaceIndex));

        sceneName = SceneManager.GetActiveScene().name;

      


        //切换场景 生成玩家
        StartCoroutine(waitForFindForNextPlace());

    }

    void toTransDoor(string toTransDoor)
    {
        toTrans = toTransDoor;  //目标传送门的标值
        toPlaceIndex = 0;  //小关卡的下标
        toBigPlaceIndex = 0;  //大关卡的下标


        Debug.Log("toTransDoor  " + toTransDoor);
        try
        {
            toBigPlaceIndex = int.Parse(toTrans.Substring(toTrans.IndexOf('-') - 1, 1));
            toPlaceIndex = int.Parse(toTrans.Substring(toTrans.IndexOf('-') + 1, 1));

        }
        catch (UnityException e)
        {

            Debug.Log(e.Message);
        }

        Debug.Log(toPlace);
        Debug.Log(toPlaceIndex);

        //切关保存玩家的数据
        SavePlayerData.SetData("Save/PlayerData.sav", sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().getPlayerData());
        //转移到新的场景
        SceneManager.LoadScene("map" + toBigPlaceIndex + '-' + toPlaceIndex);
       
        StartCoroutine(waitForFindForTransDoor());
    }

    void toWorldDoor(string toWorldDoor)
    {
        toWorld = mapData[toWorldDoor];  //目标传送门的标值
        toPlaceIndex = 0;  //小关卡的下标
        toBigPlaceIndex = 0;  //大关卡的下标

        //播放TransferDoor音效
        if (aManager != null)
        {
            aManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/InOuntWorldDoor");
        }

        Debug.Log("toWorldDoor  " + toWorldDoor);
        try
        {
            toBigPlaceIndex = int.Parse(toWorld.Substring(toWorld.IndexOf('-') - 1, 1));
            toPlaceIndex = int.Parse(toWorld.Substring(toWorld.IndexOf('-') + 1, 1));

        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }

        //切关保存玩家的数据
        SavePlayerData.SetData("Save/PlayerData.sav", sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().getPlayerData());
        Debug.Log(toPlace);
        Debug.Log(toPlaceIndex);
        //转移到新的场景
        SceneManager.LoadScene("map" + toBigPlaceIndex + '-' + toPlaceIndex);
       

        StartCoroutine(waitForFindForWorldDoor());
    }



    IEnumerator waitForFindForNextPlace()
    {

        yield return new WaitForSeconds(0.5f);
        Transform birthPlacePosition = GameObject.Find(toPlace).transform;
        Debug.Log("birthPlacePosition" + birthPlacePosition.position);
        //生成玩家 


        //创建sceneManager
        buildSceneManager(birthPlacePosition.position);
        //生成玩家
        //GameObject.Find("SceneManager").GetComponent<SManager>().birthPlayer();
        //创建mapSwitcher
        //  buildSceneMapSwitcher();

        //创建audioManager
        buildAudioManager(new Vector3(0, 0, 0));

        //创建UI
        buildUI();
    }

    IEnumerator waitForFindForTransDoor()
    {
        yield return new WaitForSeconds(1);
        Transform toTransPosition = GameObject.Find(toTrans).transform;
        GameObject effect = toTransPosition.Find("transInEffect").gameObject;
        if (effect != null)
        {
            //if (!effect.GetComponent<ParticleSystem>().isPlaying)
                effect.GetComponent<ParticleSystem>().Play();
        }
        effect = toTransPosition.Find("transOutEff").gameObject;
        if (effect != null)
        {
            //if (!effect.GetComponent<ParticleSystem>().isPlaying)
                effect.GetComponent<ParticleSystem>().Play();
        }
        Vector3 birthPosition = new Vector3(0, 0, 0);
        Debug.Log("toTransPosition" + toTransPosition.position);
        //生成玩家 

        switch (toTransPosition.tag)
        {
            case "transDoor":

                birthPosition = new Vector3(toTransPosition.position.x - 1, toTransPosition.position.y, 0);
                break;
            case "transDoor_r":

                birthPosition = new Vector3(toTransPosition.position.x + 1, toTransPosition.position.y, 0);
                break;
            case "transDoor_u":

                birthPosition = new Vector3(toTransPosition.position.x, toTransPosition.position.y + 1, 0);
                break;
            case "transDoor_d":

                birthPosition = new Vector3(toTransPosition.position.x, toTransPosition.position.y - 1, 0);
                break;
            default:
                break;
        }


        buildSceneManager(birthPosition);
        //生成玩家
        //GameObject.Find("SceneManager").GetComponent<SManager>().birthPlayer();
        //创建mapSwitcher
        // buildSceneMapSwitcher();
        //创建audioManager
        buildAudioManager(new Vector3(0, 0, 0));

        //创建UI
        buildUI();
    }

    IEnumerator waitForFindForWorldDoor()
    {

        yield return new WaitForSeconds(1);
        Debug.Log("toWorld  " + toWorld);
        Transform toWorldPosition = GameObject.Find(toWorld).transform;
        Debug.Log("toWorldPosition" + toWorldPosition.position);
        //生成玩家 
        Vector3 birthPosition = new Vector3(0, 0, 0);


        switch (toWorldPosition.tag)
        {
            case "inworldDoor":

                birthPosition = new Vector3(toWorldPosition.position.x - 0.8f, toWorldPosition.position.y, 0);
                break;
            case "inworldDoor_r":

                birthPosition = new Vector3(toWorldPosition.position.x + 0.8f, toWorldPosition.position.y, 0);
                break;
            case "inworldDoor_u":

                birthPosition = new Vector3(toWorldPosition.position.x, toWorldPosition.position.y + 1, 0);
                break;
            case "inworldDoor_d":

                birthPosition = new Vector3(toWorldPosition.position.x, toWorldPosition.position.y - 1, 0);
                break;
            case "outworldDoor":

                birthPosition = new Vector3(toWorldPosition.position.x - 0.8f, toWorldPosition.position.y, 0);
                break;
            case "outworldDoor_r":

                birthPosition = new Vector3(toWorldPosition.position.x + 0.8f, toWorldPosition.position.y, 0);
                break;
            case "outworldDoor_u":

                birthPosition = new Vector3(toWorldPosition.position.x, toWorldPosition.position.y + 1, 0);
                break;
            case "outworldDoor_d":

                birthPosition = new Vector3(toWorldPosition.position.x, toWorldPosition.position.y - 1, 0);
                break;
            default:
                break;
        }


        buildSceneManager(birthPosition);
        //生成玩家
        //GameObject.Find("SceneManager").GetComponent<SManager>().birthPlayer();

        //创建mapSwitcher
        //buildSceneMapSwitcher();
        //创建audioManager
        buildAudioManager(new Vector3(0, 0, 0));

        //创建UI
        buildUI();
    }


    //创建场景管理器
    void buildSceneManager(Vector3 birthPlace)
    {
        if (sceneName == "Interface")
            return;
        sManager = GameObject.Find("SceneManager");
        //当前场景没有管理器
        if (sManager == null)
        {
            sManager = new GameObject("SceneManager");
            sManager.AddComponent<SManager>();
            sManager.GetComponent<SManager>().setBirthPosition(birthPlace);
        }
        else//先销毁已有场景管理器，再生成新的
        {
            Destroy(sManager);
            sManager = new GameObject("SceneManager");
            sManager.AddComponent<SManager>();
            SManager.Instance.setBirthPosition(birthPlace);
            sManager.GetComponent<SManager>().setBirthPosition(birthPlace);
        }
    }

    void buildAudioManager(Vector3 birthPlace)
    {
        //aManagerPrefab = Resources.Load<GameObject>("GameManagerRes/AudioManager");
        aManagerPrefab = ResourceManager.GetInstance().getGameObject("GameManagerRes/AudioManager");
        aManager = GameObject.Find("AudioManager");
        ////当前场景没有声音管理器
        if (aManager == null)
        {

            aManager = GameObject.Instantiate(aManagerPrefab);

            changeMusicVolum(0.8f);
            changeSoundVolum(0.8f);
           
            if (sceneName.EndsWith("-0"))
            {
                //Debug.Log("123456");
                aManager.GetComponent<AudioManager>().PlayMusic("Music/BGM/map-0");
                return;
            }
            if(sceneName == "Interface")
            {
                aManager.GetComponent<AudioManager>().PlayMusic("Music/BGM/Interface");
                return;
            }
            //if(sceneName.StartsWith("map1"))
                aManager.GetComponent<AudioManager>().PlayMusic("Music/BGM/map1");
            //if (sceneName.StartsWith("map2"))
            //    aManager.GetComponent<AudioManager>().PlayMusic("Music/BGM/map2");
            //if (sceneName.StartsWith("map3"))
            //    aManager.GetComponent<AudioManager>().PlayMusic("Music/BGM/map3");

            return;
        }
    }

    public static void changeMusicVolum(float mv)
    {
        musicVolume = mv;
        aManager.GetComponent<AudioManager>().setMusciVolume(musicVolume);
    }

    public static void changeSoundVolum(float sv)
    {
        soundVolume = sv;
        aManager.GetComponent<AudioManager>().setSoundVolume(soundVolume);
    }

    IEnumerator waitForMapSwitch(int toBigPlaceIndex, int toPlaceIndex)
    {
        yield return 0;
        //转移到新的场景
        SceneManager.LoadScene("map" + toBigPlaceIndex + '-' + toPlaceIndex);
    }

    void buildUI()
    {
        if (sceneName == "Interface")
            return;
        //UIPrefab = Resources.Load<GameObject>("GameManagerRes/UI");
        UIPrefab = ResourceManager.GetInstance().getGameObject("GameManagerRes/UI");
        UI = GameObject.Find("UI");
        //当前场景没有声音管理器
        if (UI == null)
        {
            UI = GameObject.Instantiate(UIPrefab);
            //if (UI.GetComponentInChildren<Canvas>() != null)// = GameObject.Find("Main Camera(Clone)").GetComponent<Camera>();
            //UI.GetComponentInChildren<Canvas>();//.worldCamera
            if (GameObject.FindWithTag("MainCamera") != null && UI.GetComponentInChildren<Canvas>() != null)
                UI.GetComponentInChildren<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            UI.SetActive(false);
        }
    }

    void buildGameScene(string birthPlace)
    {
        StartCoroutine(buildGameSceneIE(birthPlace));


    }

    IEnumerator buildGameSceneIE(string bp)
    {
        yield return new WaitForSeconds(1);
        //Debug.Log(GameObject.Find("birthPlace1-0-1") == null);

        buildSceneManager(GameObject.Find(bp).transform.position);
        buildAudioManager(new Vector3(0, 0, 0));
        buildUI();
        //UIAnmation.GetComponent<Animator>().Play("UIToGame");

    }
    void buildGameScene(Vector3 birthPosition)
    {
        StartCoroutine(buildGameSceneIE(birthPosition));
    }
    IEnumerator buildGameSceneIE(Vector3 bp)
    {
        yield return new WaitForSeconds(1);
        //Debug.Log(GameObject.Find("birthPlace1-0-1") == null);

        buildSceneManager(bp);
        buildAudioManager(new Vector3(0, 0, 0));
        buildUI();
    }
    void buildUIScene()
    {
        StartCoroutine(buildUISceneIE());
    }
    IEnumerator buildUISceneIE()
    {
        yield return new WaitForSeconds(1);
        buildAudioManager(new Vector3(0, 0, 0));
    }
    void pauseGame(bool ispause)
    {
        Debug.Log("Pause:" + ispause);
        sManager.GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().isPause = ispause;
        NPCPause = ispause;
        return;
    }

    //返回持有的sceneManager
    public GameObject getSceneManager()
    {
        return this.sManager;
    }

    //去下一关
    void gotoNextMap()
    {
        SceneManager.LoadScene("");
    }
}
