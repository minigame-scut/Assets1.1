using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class map35Logic : MonoBehaviour
{
    //生成弹球的计时器
    float timer = 0;
    float proTime = 2f;

    //分数的计时器
    float timer2 = 0;
    float ScoreTime = 1f;
    public Text timeText;

    //史莱姆数量
    int maxSlmSum = 2;
    public int slmSum = 0;
    //关卡时间
    public int mapTime = 120;

    //是否通关
    bool isPass = false;

    //是否开始
    bool isBegin = false;
    public GameObject wall;
    //玩家是否死亡
    bool playerIsDeath = false;

    //通关的门
    public GameObject passDoor;
    GameObject player;
    GameObject ballPre;
    // Start is called before the first frame update
    void Start()
    {
        ballPre = ResourceManager.GetInstance().getGameObject("GameManagerRes/ball");
        timeText.text = mapTime.ToString()+"s";

        EventCenter.AddListener(MyEventType.DEATH, onPlayerDeath);
       
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("player");
        if (player == null)
        {
            if (playerIsDeath == true)
                playerIsDeath = false;
        }
        else
        {
            if (player.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Kinematic)
            {
                player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            }
            timer += Time.deltaTime;
            if (timer > proTime && slmSum < maxSlmSum && isBegin)
            {
                GameObject.Instantiate(ballPre, this.transform.position, Quaternion.identity);
                slmSum++;
                timer = 0;
            }

            timer2 += Time.deltaTime;
            if (timer2 > ScoreTime && !isPass && mapTime > 0 && isBegin && !playerIsDeath)
            {
                timer2 = 0;
                mapTime--;
                timeText.text = mapTime.ToString() + "s";
            }
            if (mapTime == 0 && !isPass)
            {
                //开启通关的门
                isPass = true;
                passDoor.SetActive(true);
            }

            if (isBegin && wall.activeSelf == false)
            {

                wall.SetActive(true);
            }
        }
      
    }
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.transform.tag == "player")
        {
            isBegin = true;
        }
    }

    //玩家死亡重置
    void onPlayerDeath()
    {
        playerIsDeath = true;
        mapTime = 120;
    }
}
