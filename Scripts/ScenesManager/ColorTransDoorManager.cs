using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此类用于管理 map 1-6 的传送门
//上方四扇传送门的 name 为 transDoor (161), transDoor (162), ...
//下方四扇传送门的 name 为 transDoor (165), transDoor (166), ...
//初始状态: 绿 黄 黄 绿, 玩家死亡后门的颜色状态不会重置
//在初始状态下, 通关方法, 2--3--1--3

public class ColorTransDoorManager : MonoBehaviour
{
    private Dictionary<string, string> mapTransDoor;    //触发哪扇门，会引起哪些门的变化
    private Dictionary<string, Color> mapColors;    //颜色列表

    public GameObject transDoor_1;
    public GameObject transDoor_2;
    public GameObject transDoor_3;
    public GameObject transDoor_4;
    public bool dIsFinished = false;    //用于调试的控制

    public GameObject transDoor_0;  //本关卡最终的传送门

    public GameObject tip;           //游戏提示
    GameObject player;

    //记录玩家进入门的次数，每5次重置一次
    public int count = 0;

    //记录门重置的次数，超过五次给玩家提示
    public int recoverCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //添加传送门之间关于颜色变化的映射
        mapTransDoor = new Dictionary<string, string>();
        mapTransDoor.Add("165", "161 163");
        mapTransDoor.Add("166", "162 164");
        mapTransDoor.Add("167", "162 163");
        mapTransDoor.Add("168", "161 164");

        //初始化颜色列表
        mapColors = new Dictionary<string, Color>();
        mapColors.Add("BLUE", new Color(1.0f, 1.0f, 1.0f));
        mapColors.Add("YELLOW", new Color(1.0f, 1.0f, 0.0f));
        mapColors.Add("RED", new Color(1.0f, 0.0f, 0.0f));
        mapColors.Add("GREEN", new Color(0.0f, 1.0f, 0.0f));

        //添加 颜色传送门 的监听
        EventCenter.AddListener<GameObject>(MyEventType.COLORTRANSDOOR, responseForCOLORTRANSDOOR);

        EventCenter.AddListener(MyEventType.DEATH, recoverTransDoor);


    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("SceneManager") != null)
            player = GameObject.Find("SceneManager").GetComponent<SManager>().getGamePlayer();
    }

    //对信号 颜色传送门 的处理函数
    void responseForCOLORTRANSDOOR(GameObject colorTransDoor)
    {
        count++;
        GameObject toTransDoor; //传送目的门
        //触碰的是本关卡的最终传送门, 随机传送至上方四扇门之一
        if (colorTransDoor.Equals(transDoor_0))
        {
            toTransDoor = genRandomTransDoor();
            player.transform.position = toTransDoor.transform.position + new Vector3(0.0f, 1.0f);
            return;
        }


        //提取 颜色传送门的name 的数字
        //格式 transDoor (1), transDoor (2), ...
        string szIndex = System.Text.RegularExpressions.Regex.Replace(colorTransDoor.name, @"[^0-9]+", "");

        //不是底下的门， do noting
        if (!mapTransDoor.ContainsKey(szIndex)) return;
        Debug.Log(colorTransDoor.name + "发出信号");

        //上方四扇门全部为红色
        if (transDoor_1.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) &&
            transDoor_2.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) &&
            transDoor_3.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) &&
            transDoor_4.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) ||
            dIsFinished
            )
        {
            //传送至本关卡的最终的传送门
            player.transform.position = transDoor_0.transform.position + new Vector3(0.8f, 0.0f);
            return;   //若启用此语句, 则表示, 传送至本关卡最终传送门后, 上面四扇门的颜色不会改变(保持全是红色)
        }
        else
        {
            //传送至上方四扇门之一
            toTransDoor = genRandomTransDoor();
            player.transform.position = toTransDoor.transform.position + new Vector3(0.0f, 1.0f);
        }
        



        //寻找对应的颜色转换传送门
        string mapIndex = mapTransDoor[szIndex];    //映射序列
        string[] indexes = mapIndex.Split(' '); //分割映射序列

        //对每一扇映射的门做颜色转换
        foreach (string index in indexes)
        {
            GameObject _transDoor = GameObject.Find("transDoor (" + index + ")");   //若传送门的name的格式改变，此语句也需修改
            Color _color = _transDoor.GetComponent<SpriteRenderer>().color;

            if (_color.Equals(mapColors["BLUE"]))
            {
                _transDoor.GetComponent<SpriteRenderer>().color = mapColors["GREEN"];
            }
            else if (_color.Equals(mapColors["GREEN"]))
            {
                _transDoor.GetComponent<SpriteRenderer>().color = mapColors["RED"];
            }
            else if (_color.Equals(mapColors["RED"]))
            {
                _transDoor.GetComponent<SpriteRenderer>().color = mapColors["YELLOW"];
            }
            else if (_color.Equals(mapColors["YELLOW"]))
            {
                _transDoor.GetComponent<SpriteRenderer>().color = mapColors["BLUE"];
            }
            else
            {
                Debug.Log("此种颜色的传送门未做转换设置");
            }
        }

        if (transDoor_1.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) &&
          transDoor_2.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) &&
          transDoor_3.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) &&
          transDoor_4.GetComponent<SpriteRenderer>().color.Equals(mapColors["RED"]) ||
          dIsFinished
          )
        {
            GameObject.Find("transDoor (160)").GetComponent<SpriteRenderer>().color = Color.white;
        }
            //满了五次重置门
            if (count >= 5) {
            recoverTransDoor();
            count = 0;
        }
      
    }
    //重置传送门颜色
    void recoverTransDoor()
    {
        if (GameManager.instance.sceneName != "map1-6")
            return;

            recoverCount++;
        if(recoverCount >= 5)
        {
            if(tip != null)
                tip.SetActive(true);
        }
        GameObject.Find("transDoor (161)").GetComponent<SpriteRenderer>().color = mapColors["GREEN"];
        GameObject.Find("transDoor (162)").GetComponent<SpriteRenderer>().color = mapColors["YELLOW"];
        GameObject.Find("transDoor (163)").GetComponent<SpriteRenderer>().color = mapColors["YELLOW"];
        GameObject.Find("transDoor (164)").GetComponent<SpriteRenderer>().color = mapColors["GREEN"];
    }


    //生成一个随机的传送门(上方四扇门)
    GameObject genRandomTransDoor()
    {
        int randKey = new System.Random().Next(1, 5);
        switch (randKey)
        {
            case 1: return transDoor_1;
            case 2: return transDoor_2;
            case 3: return transDoor_3;
            case 4: return transDoor_4;
        }
        return new GameObject();
    }
}
