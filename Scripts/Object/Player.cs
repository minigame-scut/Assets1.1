using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 speed = new Vector2(1.5f, 1.5f);

    public bool hasKey = false; //是否拥有当前关卡的钥匙
    public int faceCount = 0;   //收集的滑稽脸的个数
    public bool[] finalKey = new bool[9];   //最终钥匙碎片的收集状态

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(MyEventType.DEATH, playerDeath);
    
    }

    // Update is called once per frame
    void Update()
    {
        //检测输入
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed.x * inputX, speed.y * inputY);

        
    }

    //玩家死亡处理函数
    void playerDeath()
    {
        Debug.Log("玩家死亡");
    }
}
