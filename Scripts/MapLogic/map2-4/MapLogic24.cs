using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapLogic24 : MonoBehaviour
{
    //玩家灯光
    public GameObject playerLight;

    //玩家初始灯光范围大小
    float lightRange;

    //玩家
    GameObject player;

    //蝙蝠的列表
    List<GameObject> batList;

    //蝙蝠的数量
    int batNum = 200;

    //蝙蝠的prefab
    GameObject batPrefab;
    
    //地图半长宽
    float mapX;
    float mapY;

    //蝙蝠是否已经生成
    public bool isInit = false;

    //是否重新激活
    public bool isReset = false;
    //蝙蝠过于靠近玩家 玩家死亡
    float deathNum = 100;
    public float deathCount = 0;
    public float lastCount = 0;


    //10秒之内死亡值未改变就重置
    float timer = 0;
    float maxTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        mapX = Math.Abs(GameObject.Find("coord1").transform.position.x - GameObject.Find("coord2").transform.position.x) / 2;
        mapY = Math.Abs(GameObject.Find("coord1").transform.position.y - GameObject.Find("coord2").transform.position.y) / 2;

        batPrefab = ResourceManager.GetInstance().getGameObject("GameManagerRes/bat24");

        batList = new List<GameObject>();

        lightRange = playerLight.GetComponent<Light>().range;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //定时器重置玩家触碰蝙蝠次数
        if(timer > maxTime )
        {
            if(lastCount == deathCount)
            {
                deathCount = 0;
                lastCount = 0;
            }
            timer = 0;
        }
        else
        {
            if (timer < 0.1f)
                lastCount = deathCount;
        }
        //过多蝙蝠过近 死亡
        if (deathCount >= deathNum)
        {
            EventCenter.Broadcast(MyEventType.DEATH);
            deathCount = 0;
            lastCount = 0;
        }
        player = GameObject.FindWithTag("player");
        if (player == null)
        { //没找到玩家 禁用光照
            playerLight.SetActive(false);
            //激活过，玩家死亡后禁用蝙蝠
            if(isInit && batList.Count != 0)
            setBatsActive(false);
            isReset = false;
            //玩家死亡重置
            deathCount = 0;
            playerLight.GetComponent<Light>().range = lightRange;
        }
        else
        {
            playerLight.SetActive(true);
            playerLight.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, playerLight.transform.position.z);
            //未激活过，初始化
            if (!isInit && batList.Count == 0)
            {
                //初始化蝙蝠
                initBats();
                //设置玩家材质
                player.GetComponent<SpriteRenderer>().material = ResourceManager.GetInstance().getMaterial("Materials/lightM"); 
            }   
            else if (batList.Count > 0 && !isReset)    //已激活过 重新启用
            {
                setBatsActive(true);
                player.GetComponent<SpriteRenderer>().material = ResourceManager.GetInstance().getMaterial("Materials/lightM");
            }
              

            //已重新激活
            isReset = true;
        }

    }
    //设置蝙蝠激活等
    void setBatsActive(bool active)
    {

        Debug.Log("reset");
        //重置蝙蝠的位置
        if (active)
        {

            System.Random random = new System.Random(5);
            foreach (GameObject bat in batList)
            {
            
             
                Vector3 batPos = new Vector3(random.Next(-(int)mapX, (int)mapX), random.Next(-(int)mapY, (int)mapY), 0);
                //防止蝙蝠生成过近
                if ((batPos - player.transform.position).magnitude < 5)
                {
                    do
                    {
                        batPos.x += 2;
                        batPos.y += 2;
                    } while ((batPos - player.transform.position).magnitude < 5);

                }

                bat.transform.position = batPos;
                bat.SetActive(active);
            }

        }
        else
        {
            foreach (GameObject bat in batList)
            {
                bat.SetActive(active);
            }
        }
    }

    //随机生成蝙蝠
    void initBats()
    {
        System.Random random = new System.Random(6);
        //随机蝙蝠出生位置
        for (int i = 0; i < batNum; i++) {
            if(batPrefab == null)
            {
                Debug.Log("null_bat_prefab_error!!");
                return;
            }
           
            //生成位置
            Vector3 batPos = new Vector3(random.Next(-(int)mapX, (int)mapX), random.Next(-(int)mapY, (int)mapY), 0);
            //防止蝙蝠生成过近
            if ((batPos - player.transform.position).magnitude < 5)
            {
                do
                {
                    batPos.x += 2;
                    batPos.y += 2;
                } while ((batPos - player.transform.position).magnitude < 5);
            }

            //生成蝙蝠
            GameObject bat;
            bat = GameObject.Instantiate(batPrefab, batPos,Quaternion.identity);
            //设置蝙蝠的随机距离种子
            bat.GetComponent<batLogic24>().setSeed(i);
            batList.Add(bat);
        
        }
        //只生成一次
        isInit = true;
    }



    //返回光照的范围
    public float getLightRange()
    {
        return playerLight.GetComponent<Light>().range;

    }
    //返回玩家实例
    public GameObject getPlayer()
    {
        return player;
    }

    public void setBatCauseDeath()
    {
        //Debug.Log("----------------!");
        deathCount++;
    }
}
