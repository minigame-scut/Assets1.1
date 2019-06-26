using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测加速门的触发
public class TransDoor : MonoBehaviour
{
    public float BiggestTriggerTime = 1.0f;   //一个门在最大triggerTime时间内能够触发的次数

    private float deltaTime = 0;       //定时器
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //检测到玩家触碰
        if (collision.transform.tag == "player")
        {
            if (deltaTime > BiggestTriggerTime)  //触发时间间隔大于一秒
            {
                if(SceneMapData.instance.getMapData().ContainsKey(gameObject.name))
                {
                    //这个传送门对应的传送门
                    EventCenter.Broadcast(MyEventType.WAVE, this.transform.position);
                    string mapTransDoorName = SceneMapData.instance.getMapData()[gameObject.name];
                    GameObject mapTransDoor = GameObject.Find(mapTransDoorName);

                    //这个对应的门在该scene中
                    if (mapTransDoor != null)
                    {
                        EventCenter.Broadcast(MyEventType.TRANSDOOR, mapTransDoor.transform.position, gameObject.tag);   //广播传送门门触碰信号  一个scene内的传送交给sceneManager来处理
                    }
                    else//这个门在别的scene中
                    {
                        EventCenter.Broadcast(MyEventType.TRANSDOORTOWORLD, mapTransDoorName);   //广播传送门门触碰信号  不同scene内的传送交给GameManager来处理
                    }
                }
                else  //用于 map1-6 的传送门的
                {
                    EventCenter.Broadcast<GameObject>(MyEventType.COLORTRANSDOOR, gameObject);
                }

                deltaTime = 0;  //重置间隔定时器
                GameObject effect = transform.Find("transInEffect").gameObject;
                if (effect != null)
                {
                    //if (!effect.GetComponent<ParticleSystem>().isPlaying)
                        effect.GetComponent<ParticleSystem>().Play();
                }
                effect = transform.Find("transOutEff").gameObject;
                if (effect != null)
                {
                    //if (!effect.GetComponent<ParticleSystem>().isPlaying)
                        effect.GetComponent<ParticleSystem>().Play();
                }
                string transName = SceneMapData.getInstance().getTransName(transform.name);
                GameObject transobj = GameObject.Find(transName);
                if(transobj!=null)
                {
                    effect = transobj.transform.Find("transInEffect").gameObject;
                    if (effect != null)
                    {
                        //if (!effect.GetComponent<ParticleSystem>().isPlaying)
                            effect.GetComponent<ParticleSystem>().Play();
                    }
                    effect = transobj.transform.Find("transOutEff").gameObject;
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
