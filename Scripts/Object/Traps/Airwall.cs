using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airwall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //地刺被玩家触碰
    void OnCollisionEnter2D(Collision2D otherCollision)
    {
        //玩家的 tag 为 Player
        if (otherCollision.gameObject.tag.Equals("player"))
        {
            //广播玩家死亡信号
            EventCenter.Broadcast(MyEventType.DEATH);
        }
    }
}