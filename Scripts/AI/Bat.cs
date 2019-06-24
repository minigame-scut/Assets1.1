using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    //蝙蝠朝向,1为右，-1为左
    private int trans;
    //蝙蝠移动速度
    public float moveSpeed;
    //蝙蝠冲刺间隔计时器
    public float timer;
    //蝙蝠是否悬停
    private bool isSuspension;
    //蝙蝠是否冲刺
    private bool isRush;
    //获得的目标人物当前位置。每3s获得一次
    private Vector3 playerPos;
    //蝙蝠与目标的角度
    private float angle;
    //冲刺时长
    private float rushTime;
    //冲刺计时器
    private float rushTimer;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        trans = 1;
        timer = 0f;
        rushTime = 0f;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        setAnimation();
        setTrans();
        this.transform.localEulerAngles = new Vector3(0, (trans == 1 ? 0 : 180), 0);
        //蝙蝠没有冲刺则计时器计时
        if(!isRush)
        {
            timer += Time.deltaTime;
            isSuspension = true;
        }
        else if(isRush)
        {
            rushTimer += Time.deltaTime;
            transform.position += new Vector3(moveSpeed * Mathf.Cos(angle) * Time.deltaTime, moveSpeed * Mathf.Sin(angle) * Time.deltaTime, 0);
            if(rushTimer >= rushTime)
            {
                isRush = false;
                isSuspension = true;
                rushTimer = 0f;
            }
        }
            
        if (timer >= 3.0f)
        {
            timer = 0f;
            isSuspension = false;
            isRush = true;
            getPlayerPos();
            angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x);
            rushTime = Mathf.Sqrt(Mathf.Pow((playerPos.y - transform.position.y), 2) + Mathf.Pow((playerPos.x - transform.position.x), 2)) / (float)moveSpeed;
        }
    }
    void setAnimation()
    {
        if (isSuspension)
        {
            anim.SetBool("isSuspension", true);
        }
        else
        {
            anim.SetBool("isSuspension", false);
        }
        if (isRush)
        {
            anim.SetBool("isRush", true);
        }
        else
        {
            anim.SetBool("isRush", false);
        }
    }
    void setTrans()
    {
        if((playerPos.x-transform.position.x)>=0)
        {
            trans = 1;
        }
        else
        {
            trans = -1;
        }
    }
    void getPlayerPos()
    {
        playerPos = GameObject.FindWithTag("player").transform.position;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag=="player")
        {
            EventCenter.Broadcast(MyEventType.DEATH);
        }
    }

}
