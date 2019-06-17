using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //滑稽脸被玩家触碰
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //玩家的 tag 为 Player
        if (otherCollider.tag.Equals("player"))
        {     
            //广播销毁关卡滑稽脸的信号并传递当前gameObject对象
            GameObject gameObject = this.gameObject;
            EventCenter.Broadcast(MyEventType.DESTROY, gameObject); 
        }
    }
}
