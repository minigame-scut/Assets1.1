using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于检测关卡的转换

public class NextPlace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "player")
        {
            Debug.Log(this.name);
            Debug.Log("nextPlace");
            //发送进入下一个场景的信号还有当前场景的标识
            EventCenter.Broadcast<string>(MyEventType.NEXTPLACE, this.name);
        }
    }
}
