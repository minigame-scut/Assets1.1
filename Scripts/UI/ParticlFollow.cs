﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlFollow : MonoBehaviour
{
    Vector3 screenPosition;//将物体从世界坐标转换为屏幕坐标
    Vector3 mousePositionOnScreen;//获取到点击屏幕的屏幕坐标
    Vector3 mousePositionInWorld;//将点击屏幕的屏幕坐标转换为世界坐标
    void Update()
    {
        Cursor.visible = false;
        MouseFollow();
    }
    void MouseFollow()
    {
        //Debug.Log("FLLOW");
        //获取鼠标在相机中（世界中）的位置，转换为屏幕坐标；
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //获取鼠标在场景中坐标
        mousePositionOnScreen = Input.mousePosition;
        //让场景中的Z=鼠标坐标的Z
        mousePositionOnScreen.z = screenPosition.z;
        //将相机中的坐标转化为世界坐标
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        //物体跟随鼠标移动
        //transform.position = mousePositionInWorld;
        //物体跟随鼠标X轴移动
        transform.position = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y, transform.position.z);
    }
}
