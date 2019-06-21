using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Chainball : MonoBehaviour
{
    public Vector3 point;

    //旋转速度
    public  float speed = 5f;

    float speedRate = 10;
    float angle = 0;
    //围绕点距离中心多远

    float limitAngle = 62f;
    // Start is called before the first frame update
    void Start()
    {
        //获得围绕的点的位置
        point = GetComponentInChildren<Transform>().position;
        angle = 10 * Time.deltaTime * speed;

        //float newAngle = (transform.localEulerAngles.z >= 180) ? (transform.localEulerAngles.z - 360) : (transform.localEulerAngles.z);
        //Debug.Log(newAngle);
        //point.x = point.x - dis * (float)Math.Sin(newAngle / (2*Math.PI));
        //point.y = point.y + dis * (float)Math.Cos(newAngle / (2*Math.PI));
        //Debug.Log(point);
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameObject.Find("player 1(Clone)") == null)
        //{
        //    speed = 5;
        //}

        float increase = 0f;
        //反向
        float newAngle = (transform.localEulerAngles.z >= 180) ? (transform.localEulerAngles.z - 360) : (transform.localEulerAngles.z);

        //计算速度
         speed = speed< 0? -((1-Math.Abs(newAngle) / limitAngle) * speedRate + 2): ((1-Math.Abs(newAngle) / limitAngle) * speedRate + 2);
       
        if (newAngle > limitAngle || newAngle < -limitAngle)
        {
            speed *= -1;
            if (speed > 0)
                increase = 1;
            else
                increase = -1;
        }

        //  transform.Rotate(Vector3.forward, 10 * Time.deltaTime * speed);
        angle = 10 * Time.deltaTime * speed;
        //transform.RotateAround(point, Vector3.forward, angle);
        transform.Rotate(Vector3.forward, angle + increase);
    }

    //玩家触碰死亡
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "player" && !GameManager.instance.getSceneManager().GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().getPlayerData().isDead)
        {
            EventCenter.Broadcast(MyEventType.DEATH);
            speed *= -1;
            Debug.Log(speed);
        }
    }
}
