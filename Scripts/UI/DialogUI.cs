using UnityEngine;
using System.Xml;                  //引用XML
using UnityEngine.UI;              //引用UI
using System.Collections.Generic;  //引用集合
using UnityEngine.SceneManagement; //引用命名空间



/// <summary>
/// 枚举指令类型
/// </summary>
public enum CommandType
{
    Say, //说话
    Bgm, //背景音
    Bg   //背景
}


///// <summary>
///// 基类：指令类
///// </summary>
//public class Command
//{
//    public CommandType AllType; //定义成员变量 类型对象
//}
///// <summary>
///// 背景音指令类：继承 指令基类
///// </summary>
//public class Bgm : Command
//{
//    public string Name; //名字
//}
///// <summary>
///// 背景指令类：继承 指令基类
///// </summary>
//public class Bg : Command
//{
//    public string Name; //名字
//}


/// <summary>
/// 说话指令类：继承 指令基类
/// </summary>
public class Say /*: Command*/
{
    public CommandType AllType;
    public string Name;    //名字
    public string Image;   //图片
    public string Content; //内容
}

/// <summary>
/// 对话系统
/// </summary>
public class DialogUI : MonoBehaviour
{
    public List<Say> Commands = new List<Say>(); //声明一个 List 数组 类型为：Command
    private int _index = 0;                   //默认索引为0
    public GameObject GameImage;                      //游戏界面
    public Image BgImage;                        //背景图
    public Image HeadPortrait;                   //头像
    public Text NameText;                       //名字文本
    public Text ConttentText;                   //内容文本
    private bool _isExecute = false;             //是否执行命令：默认不执行
    //private string HeadPath = "Resources/UI";


    /// <summary>
    /// 初始化方法
    /// </summary>
    void Start()
    {
        AnalysisXml();                                                                            //调用解析XML方法
        EventCenter.AddListener(MyEventType.DIALOG, StartGame);
        if (GameImage != null)
            GameImage.SetActive(false);
    }


    /// <summary>
    /// 更新函数
    /// </summary>
    void Update()
    {
        if((Input.GetButtonDown("Skip") || Input.GetKeyDown(KeyCode.Space)) && _isExecute == true)

        {
            OneByOneExecuteCommand(); //执行对话命令函数
        }
    }


    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        Debug.Log("DIALOG");
        if(GameImage != null)
            GameImage.SetActive(true); //激活游戏界面
        _isExecute = true;         //游戏开始：可以开始执行代码
        OneByOneExecuteCommand();  //游戏页面被激活的时候，就执行一次 
        
    }


    /// <summary>
    /// 执行对话命令函数
    /// </summary>
    public void OneByOneExecuteCommand()
    {
        if (_index >= Commands.Count)
        {
            //GameObject.Find("GameImage").GetComponent<Image>().enabled = false;
            //GameObject.Find("Name").GetComponent<Text>().enabled = false;
            //GameObject.Find("Content").GetComponent<Text>().enabled = false;
            //GameObject.Find("Head Portrait").GetComponent<Image>().enabled = false;
            if (GameImage != null)
                GameImage.SetActive(false);
            _isExecute = false;                                                //关闭执行命令
            EventCenter.Broadcast(MyEventType.PLAYERPAUSE, false);
            return;
        }

        Say say = Commands[_index++]; //自增：取出一条命令
        HeadPortrait.sprite = Resources.Load<Sprite>(say.Image); //更换头像
        NameText.text = say.Name;                          //人物
        ConttentText.text = say.Content;                       //说话内容
    }

    /// <summary>
    /// 解析XML
    /// </summary>
    private void AnalysisXml()
    {
        XmlDocument document = new XmlDocument();                 //实例化一个xml文档
        if (GameManager.instance.sceneName == "Interface" || GameManager.instance.sceneName == "map1-0")
        {
            Debug.Log(1);
            document.Load(Application.dataPath + "/Data/Dialog1.xml");//加载 XML 内容
        }
        else if (GameManager.instance.sceneName == "map1-6" || GameManager.instance.sceneName == "map2-0")
        {
            Debug.Log(2);
            document.Load(Application.dataPath + "/Data/Dialog2.xml");
        }
        else if (GameManager.instance.sceneName == "map2-6" || GameManager.instance.sceneName == "map3-0")
        {
            Debug.Log(3);
            document.Load(Application.dataPath + "/Data/Dialog3.xml");
        }
            XmlElement rootEle = document.LastChild as XmlElement;    //根节点
        foreach (XmlElement ele in rootEle.ChildNodes)            //遍历根节点的所有子节点
        {
            if (ele.Name == "say")
            {
                Say say = new Say();
                say.AllType = CommandType.Say;
                say.Name = ele.ChildNodes[0].InnerText;
                say.Image = ele.ChildNodes[1].InnerText;
                say.Content = ele.ChildNodes[2].InnerText;
                Commands.Add(say);
            }
        }
    }
}