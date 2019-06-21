using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject player;  //主角
    private float speed = 5f;  //相机跟随速度

    //设定一个角色能看到的最远值
    public float Ahead = 1;

    //设置一个摄像机要移动到的点
    public Vector3 targetPos;

    //设置一个缓动速度插值
    public float smooth = 1;

    public Vector3 coord1;

    public Vector3 coord2;

    public float scrWidth;
    public float scrHeight;

    public float cameraWidth= 2;
    public float cameraHeight = 1;

    void Start()
    {
        coord1 = GameObject.Find("coord1").transform.position;
        coord2 = GameObject.Find("coord2").transform.position;
        scrWidth = Math.Abs(coord1.x - coord2.x);
        scrHeight = Math.Abs(coord1.y - coord2.y);
        float sizeOfCamera = this.GetComponent<Camera>().orthographicSize;
        cameraHeight = sizeOfCamera  ;
        cameraWidth= sizeOfCamera   * ((float)Screen.width / Screen.height);
    }
    void FixedUpdate()
    {
        if (player == null)
            player = GameObject.Find("player(Clone)");

        if (player != null)
            FixCameraPos();
    }

    void FixCameraPos()
    {
        targetPos = new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);

        if (player.transform.position.x - cameraWidth <= -scrWidth / 2 )
            targetPos.x =-scrWidth /2 +cameraWidth;
        else if (player.transform.position.x + cameraWidth >= scrWidth / 2)
            targetPos.x = scrWidth / 2 - cameraWidth;
        if (player.transform.position.y - cameraHeight <= -scrHeight / 2 )
            targetPos.y = -scrHeight/2 + cameraHeight;
        else if (player.transform.position.y + cameraHeight >= scrHeight / 2)
            targetPos.y = scrHeight/2 - cameraHeight;

        //插值顺滑
        if (GameManager.instance.sceneName == "map2-3")
        {
            transform.position = Vector3.Lerp(transform.position, targetPos + new Vector3(0, -2.0f, 0), Time.deltaTime * speed);
        }
        else if (GameManager.instance.sceneName == "map2-6")
        {
            transform.position = Vector3.Lerp(transform.position, targetPos + new Vector3(0, 1, 0), Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }

}
