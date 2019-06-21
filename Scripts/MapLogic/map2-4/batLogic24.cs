using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class batLogic24 : MonoBehaviour
{
    GameObject player;
    //朝向玩家的向量
   Vector3 pos;

    //蝙蝠的精灵
    SpriteRenderer spriteRenderer;
         
    //蝙蝠移动速度
   float speed = 0.5f;

    //获取玩家的光照范围
    float range;

    //range作用系数
  float des = 0.1f;

    //随机距
   int seed = 0;
    System.Random random;


    //蝙蝠只在玩家的面前移动，移动的长宽

    //float width = 10f;
    //float height = 3f;

    //只记录死亡一次
   // bool isD = false;
    
    //2-4的关卡逻辑管理器
    GameObject logicManager;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        logicManager = GameObject.Find("map2-4Logic");
       // random = new System.Random();
       // des = (float)(random.NextDouble()) + des;

    }

    // Update is called once per frame
    void Update()
    {
       
        //检索玩家
        // player = GameObject.Find("player 1(Clone)");
        player = logicManager.GetComponent<MapLogic24>().getPlayer();
        //Debug.Log(des);
        float rdes = (float)(random.NextDouble()) + des;
        range = logicManager.GetComponent<MapLogic24>().getLightRange() * rdes / 2;
        if (player == null)
        {
         //   isD = false;
        }
        else
        {
            if (this.transform.position.x < player.transform.position.x)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
            pos = player.transform.position - this.transform.position;

            //蝙蝠与玩家的距离
            float dis = pos.magnitude;
            //过近
            if(dis <= 1 )
            {
                logicManager.GetComponent<MapLogic24>().setBatCauseDeath();
              //  isD = true;
            }

            //距离大于光照范围，蝙蝠飞近
            if(range > 2.4f)
            {

                if (dis > range)
                {
                    if (range < 2.5f)
                        transform.Translate(pos * Time.deltaTime * speed * 2);
                    else
                        transform.Translate(pos * Time.deltaTime * speed);
                }
                else if (dis == range)
                {
                    //距离相等 不动
                }
                else
                {  //距离小于光照距离
                   //反向移动

                    pos.z = 0;
                        transform.Translate(-pos * Time.deltaTime * speed * 25);
                }
            }
          else
            {
                transform.Translate(pos * Time.deltaTime * speed);
            }
            if ((transform.position.x < player.transform.position.x && dis > 5) ||(transform.position.y< -4|| transform.position.y > 4))
            {
               setPosition();
            }
        }
      
    }

    //蝙蝠在玩家的后面且距离过大  重置蝙蝠位置
    void setPosition()
    {
        float x = player.transform.position.x + 10 + 10 * (float)random.NextDouble();
       
        float y = -(float)random.NextDouble() * 8 +4;
       // Debug.Log(y);
        this.transform.position = new Vector3(x, y, 0);

    }

    //设置随机数种子
    public void setSeed(int seed)
    {
        this.seed = seed;
        random = new System.Random(seed);
    }

 
}
