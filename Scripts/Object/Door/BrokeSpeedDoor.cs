﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//检测弹力门的触发
public class BrokeSpeedDoor : MonoBehaviour
{
    public float BiggestTriggerTime = 1.0f;   //一个门在最大triggerTime时间内能够触发的次数
    private float deltaTime = 0;       //定时器
    private Transform elasticTrans;
    // Start is called before the first frame update
    void Start()
    {
        elasticTrans = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
       
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //检测到玩家触碰
        if (collision.transform.tag == "player")
        {
            if (deltaTime > BiggestTriggerTime)  //触发时间间隔大于一秒
            {

                Debug.Log("BorkeDoor");//测试
                EventCenter.Broadcast(MyEventType.WAVE, this.transform.position);
                EventCenter.Broadcast(MyEventType.BROKESPEEDDOOR, elasticTrans, transform.Find("bounceEffect") != null);   //广播弹力门触碰信号
                deltaTime = 0;  //重置间隔定时器
                if(transform.Find("bounceEffect")!=null)
                {
                    GameObject effect = transform.Find("bounceEffect").gameObject;
                    if (effect != null)
                    {
                        //if (!effect.GetComponent<ParticleSystem>().isPlaying)
                        effect.GetComponent<ParticleSystem>().Play();
                    }
                }

            }
        }
    }
}
