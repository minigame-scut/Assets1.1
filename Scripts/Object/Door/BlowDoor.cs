using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//吹风门的脚本
public class BlowDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        //检测到玩家触碰
        if (collider.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.BLOWDOOR, this.transform);
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.BLOWDELETE);
        }
    }
}
