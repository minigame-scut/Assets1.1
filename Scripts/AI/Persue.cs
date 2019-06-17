using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//追逐
public class Persue : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //追逐者(chaser) 和 被追逐者(prey)
    //简单的线性追逐
    public static void pursueLinear(GameObject chaser, Vector2 speed, GameObject prey)
    {
        //两者的 Position
        Vector3 chaserPos = chaser.transform.localPosition;
        Vector3 preyPos = prey.transform.localPosition;

        //两者的 方向向量
        Vector2 direction = new Vector2(
            preyPos.x - chaserPos.x,
            preyPos.y - chaserPos.y
            );
        //归一化
        direction.Normalize();

        //向右追赶，沿X轴翻转
        if(direction.x > 0)
        {
            chaser.GetComponent<SpriteRenderer>().flipX = true;
        }
        //向左追赶，沿X轴不翻转
        else
        {
            chaser.GetComponent<SpriteRenderer>().flipX = false;
        }

        //追逐者的位移
        Vector2 offset = new Vector2(
            direction.x * speed.x,
            direction.y * speed.y
            );

        //更新追逐者的 Position
        chaser.transform.Translate(offset.x, offset.y, 0);


    }
}
