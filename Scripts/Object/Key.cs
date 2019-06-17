using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //关卡钥匙被玩家触碰
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //玩家的 tag 为 Player
        if(otherCollider.tag.Equals("player"))
        {
            Debug.Log("collection");
            //广播销毁关卡钥匙道具的信号并传递当前gameObject对象
            EventCenter.Broadcast(MyEventType.DESTROY, this.gameObject); 
        }
    }

}
