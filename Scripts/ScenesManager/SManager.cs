using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//sceneManager负责接收门触碰发出的信号，然后使用该信号来操作玩家做出相应的改变。
public class SManager : MonoBehaviour
{
    public Vector3 birthPosition;//当前场景的出生点

    //当前玩家的实例
    public GameObject gamePlayer;
    //玩家prefab
    GameObject player;
    //保存玩家存档数据
    PlayerData saveData;
    //是否收集
    bool isCollectKey = false;
    bool isCollectFace = false;
    //道具
    GameObject key;
    GameObject face;
    GameObject audioManager;

    private static SManager instance = null;

    public static SManager Instance
    {
        get
        {
            return SManager.instance;
        }

    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //player = Resources.Load<GameObject>("GameManagerRes/player");
        player = ResourceManager.GetInstance().getGameObject("GameManagerRes/player");
        if (player == null)
            Debug.Log("dont find player object");
    }

    // Start is called before the first frame update
    void Start()
    {
        key = GameObject.Find("Key");
        face = GameObject.Find("Face");
        init();
        birthPlayer();
        listener();
        saveData = (PlayerData)SavePlayerData.GetData("Save/PlayerData.sav", typeof(PlayerData));
        audioManager = GameObject.Find("AudioManager(Clone)");
    }

    void init()
    {
        int count = GameManager.instance.propData.collectionMap.Count;
        for (int i = 0; i < count; i++)
        {
            if (getNumOfMap(GameManager.instance.sceneName).x == GameManager.instance.propData.collectionMap[i].x &&
                getNumOfMap(GameManager.instance.sceneName).y == GameManager.instance.propData.collectionMap[i].y)
            {
                if (GameManager.instance.propData.collectionMap[i].z == 1 && key != null)
                    key.SetActive(true);
                else if (GameManager.instance.propData.collectionMap[i].z == 0 && key != null)
                    key.SetActive(false);
                if (GameManager.instance.propData.collectionMap[i].w == 1 && face != null)
                    face.SetActive(true);
                else if (GameManager.instance.propData.collectionMap[i].w == 0 && face != null)
                {
                    face.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gamePlayer != null && saveData != null)
        {
            //如果当前场景收集过道具，重新设置道具状态
            if (gamePlayer.GetComponent<PlayerPlatformController>().playerData.numOfFace == saveData.numOfFace && isCollectFace && face != null)
            {
                GameManager.instance.propData.setFaceTrue(getNumOfMap(GameManager.instance.sceneName).x, getNumOfMap(GameManager.instance.sceneName).y);
                face.SetActive(true);
                isCollectFace = false;
            }

            if (gamePlayer.GetComponent<PlayerPlatformController>().playerData.numOfKey == saveData.numOfKey && isCollectKey && key != null)
            {
                GameManager.instance.propData.setKeyTrue(getNumOfMap(GameManager.instance.sceneName).x, getNumOfMap(GameManager.instance.sceneName).y);
                key.SetActive(true);
                isCollectKey = false;
            }

        }
    }
    //获取当前场景出生点位置的函数
    public void setBirthPosition(Vector3 newPosition)
    {
        this.birthPosition = newPosition;
    }

    //返回一个玩家的实例
    public GameObject getGamePlayer()
    {
        return this.gamePlayer;
    }


    //生成玩家
    public void birthPlayer()
    {
        //如果找到玩家物体，则销毁当前玩家物体
        if (GameObject.Find("player(Clone)"))
        {
            Destroy(GameObject.Find("player(Clone)"));
            //return;
        }
        //加载player预制体，在出生位置创建玩家
        //GameObject player = Resources.Load<GameObject>("GameManagerRes/player");
        gamePlayer = GameObject.Instantiate(player, birthPosition, Quaternion.identity);


        //在进入场景的时候应该读取修改保存一个玩家的数据
        try
        {
            PlayerData playerData = (PlayerData)SavePlayerData.GetData("Save/PlayerData.sav", typeof(PlayerData));
            //为玩家的生成位置赋值
            if (playerData != null)
            {
                playerData.setPlayerVector3DPositionData(this.birthPosition.x, this.birthPosition.y, this.birthPosition.z);
                playerData.mapIndex = int.Parse(SceneMapData.getInstance().getMapData()[GameManager.instance.sceneName]);
                //修改
                gamePlayer.GetComponent<PlayerPlatformController>().setPlayerData(playerData);

                //保存    
                SavePlayerData.SetData("Save/PlayerData.sav", gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData());

            }
            else
            {
                gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().setPlayerVector3DPositionData(this.birthPosition.x, this.birthPosition.y, this.birthPosition.z);
                gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().mapIndex = int.Parse(SceneMapData.getInstance().getMapData()[GameManager.instance.sceneName]);
                //保存
                SavePlayerData.SetData("Save/PlayerData.sav", gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData());
            }

            //读取 修改位置地图信息 再保存
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }



    }


    //委托的方法
    //玩家通过门重置状态，玩家死亡，玩家重置位置，玩家经过门之后的效果
    private void responseForSignalBROKESPEEDDOOR(Transform transform)
    {
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.add(Buff.ELASTIC);
            gamePlayer.GetComponent<PlayerPlatformController>().elasticTrans = transform;
            //根据弹力门的方向设置玩家刚体该方向速度为0
            //gamePlayer.GetComponent<PlayerPlatformController>().rig.velocity *= (transform.right.x == 0) ? new Vector2(0, 1) : new Vector2(1, 0);
        }
        //播放BrokeSpeedDoor音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/BounceDoor");
        }
    }
    private void responseForSignalDEATHDOOR()
    {

    }
    private void responseForSignalGDOOR()
    {
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().flagGravity = 0;
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.add(Buff.GRAVITY);
        }
        //播放GravityDoor音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/GravityDoor");
        }
    }
    private void responseForMAGICALDOOR()
    {
        //Magic
    }
    private void responseForTRANSDOOR(Vector3 newPosition, string curTag)
    {
        if (gamePlayer != null)
        {
            newPosition += (curTag == "transDoor_r") ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
            gamePlayer.GetComponent<PlayerPlatformController>().transform.position = newPosition;
        }
        //播放TransferDoor音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/TransferDoor");
        }
    }
    private void responseForUPSPEEDDOOR()
    {
        if (gamePlayer != null)
        {
             gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.add(Buff.SUPERJUMP);
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.add(Buff.SUPERRUSH);
        }
        //播放UpSpeedDoor音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/UpSpeedDoor");
        }
    }
    private void responseForINITDOOR()
    {
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.add(Buff.INITRUSH);
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.add(Buff.INITJUMP);																						
        }
    }
    private void responseForWALK()
    {

        //播放Walk音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/Walk");
        }
    }

    private void responseForDEATH()
    {
        //为人物设置死亡状态

        //销毁人物，3s延迟后销毁
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().isDead = true;
            Destroy(gamePlayer, 1.67f);
            StartCoroutine(createNewPlayerInBirthPlaceAfterDeath());
        }

        //播放Death音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/Death");
        }

        //创建对应prefeb
        //Quaternion newQuaternion = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation
        // GameObject.Instantiate(prefeb, birthPosition, newQuaternion);
        //GameObject.Instantiate(/*prefeb*/);
    }
    private void responseForREBIRTH()
    {
        if (gamePlayer != null)
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().isBirth = true;
    }
    private void responseForJUMP()
    {
        //Debug.Log("LISTENJUMP");
        //播放JUMP音效
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/Jump");
        }
		//遍历Player中的buffList列表，如果在接收到JUMP信号时玩家buff状态为SUPER，则移除该buff状态
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.remove(Buff.SUPERJUMP);
        }
    }
    private void responseForRUSH()
    {
        
        if (audioManager != null)
        {
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/Rush");
        }
        //播放RUSH音效
        //AudioManager.getInstance().PlaySound("Rush");
         //遍历Player中的buffList列表，如果在接收到RUSH信号时玩家buff状态为SUPER，则移除该buff状态
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.remove(Buff.SUPERRUSH);
        }

    }
    private void responseForELASTICDELETE()
    {
        //接受到ELASTICDELETE信号后设置弹力计时器时间为0，并移除弹力buff
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().elasticTimer = 0.0f;
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.remove(Buff.ELASTIC);
        }
    }

    private void responseForINITJUMPDELETE()
    {
        //接受到INITDELETE信号后移除该buff
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.remove(Buff.INITJUMP);
        }
    }
    private void responseForINITRUSHDELETE()
    {
        //接受到INITDELETE信号后移除该buff
        if (gamePlayer != null)
        {
            gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().buff.remove(Buff.INITRUSH);
        }
    }
    private void responseForDESTROY(GameObject other)
    {
        if (gamePlayer != null)
        {
            //销毁当前场景物品的prefeb
            if (other.tag == "collection")
            {
                gamePlayer.GetComponent<PlayerPlatformController>().playerData.numOfFace++;
                GameManager.instance.propData.setFaceFalse(getNumOfMap(GameManager.instance.sceneName).x, getNumOfMap(GameManager.instance.sceneName).y);
                other.SetActive(false);
                isCollectFace = true;
            }
            if (other.tag == "collection_key")
            {
                gamePlayer.GetComponent<PlayerPlatformController>().playerData.numOfKey++;
                GameManager.instance.propData.setKeyFalse(getNumOfMap(GameManager.instance.sceneName).x, getNumOfMap(GameManager.instance.sceneName).y);
                other.SetActive(false);
                isCollectKey = true;
            }
            //播放获得道具音效
            if (audioManager != null)
            {
                audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/GetProp");
            }
        }
    }
    private void responseForLEVELDOOR()
    {
        if (audioManager != null)
        {
            Debug.Log("Level");
            audioManager.GetComponent<AudioManager>().PlaySound("Music/Sounds/LevelDoor");
        }
    }
    //获取当前场景的编号
    private Vector2Int getNumOfMap(string sceneName)
    {
        int toBigPlaceIndex = int.Parse(sceneName.Substring(sceneName.IndexOf('-') - 1, 1));
        int toPlaceIndex = int.Parse(sceneName.Substring(sceneName.IndexOf('-') + 1, 1));
        return new Vector2Int(toBigPlaceIndex, toPlaceIndex);
    }
    //添加监听器
    private void listener()
    {
        //监听门信号
        EventCenter.AddListener<Transform>(MyEventType.BROKESPEEDDOOR, responseForSignalBROKESPEEDDOOR);
        EventCenter.AddListener(MyEventType.DEATHDOOR, responseForSignalDEATHDOOR);
        EventCenter.AddListener(MyEventType.GDOOR, responseForSignalGDOOR);
        EventCenter.AddListener(MyEventType.MAGICALDOOR, responseForMAGICALDOOR);
        EventCenter.AddListener<Vector3, string>(MyEventType.TRANSDOOR, responseForTRANSDOOR);
        EventCenter.AddListener(MyEventType.UPSPEEDDOOR, responseForUPSPEEDDOOR);
        EventCenter.AddListener(MyEventType.INITDOOR, responseForINITDOOR);
        EventCenter.AddListener(MyEventType.LEVELDOOR, responseForLEVELDOOR);
        //监听玩家信号
        EventCenter.AddListener(MyEventType.WALK, responseForWALK);
        EventCenter.AddListener(MyEventType.DEATH, responseForDEATH);
        EventCenter.AddListener(MyEventType.JUMP, responseForJUMP);
        EventCenter.AddListener(MyEventType.RUSH, responseForRUSH);
        EventCenter.AddListener(MyEventType.ELASTICDELETE, responseForELASTICDELETE);
        EventCenter.AddListener(MyEventType.REBIRTH, responseForREBIRTH);
         EventCenter.AddListener(MyEventType.INITJUMPDELETE, responseForINITJUMPDELETE);
        EventCenter.AddListener(MyEventType.INITRUSHDELETE, responseForINITRUSHDELETE);
        //
        EventCenter.AddListener<GameObject>(MyEventType.DESTROY, responseForDESTROY);
    }

    IEnumerator createNewPlayerInBirthPlaceAfterDeath()
    {
        yield return new WaitForSeconds(3.0f);
        if (gamePlayer == null)
        {
            birthPlayer();
            EventCenter.Broadcast(MyEventType.REBIRTH);
        }

    }
}
