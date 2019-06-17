using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Vector2 speed = new Vector2(2f, 2f);

    private GameObject mObjPlayer;   //游戏对象——玩家

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //搜索玩家
        if (mObjPlayer == null)
        {
            mObjPlayer = GameObject.FindGameObjectWithTag("player");
        }
        //追逐玩家(无视地形)
        else
        {
             Persue.pursueLinear(gameObject, speed * Time.deltaTime, mObjPlayer);
        }

    }

    //触碰到玩家,玩家死亡
    void OnCollisionEnter2D(Collision2D otherCollision)
    {
        //玩家的 tag 为 Player
        if (mObjPlayer != null && otherCollision.gameObject.tag.Equals("player"))
        {
            //广播玩家死亡信号
            EventCenter.Broadcast(MyEventType.DEATH);
            Debug.Log("玩家被幽灵触碰到，玩家死亡");
        }
    }

}
