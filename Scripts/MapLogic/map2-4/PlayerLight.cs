using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    Light plight;
    //光的范围
    float range;
    //衰减系数
    float dec = 0.4f;

    bool isDes = true;
    void Start()
    {
        plight = GetComponent<Light>();
        range = plight.range;
        //监听魔法门信号
        EventCenter.AddListener(MyEventType.MAGICALDOOR, resetLightRange);
    }

    void Update()
    {
        if (isDes) 
        plight.range -= Time.deltaTime * dec; 
    }

    //穿过魔法门重置玩家光照范围
    void resetLightRange()
    {
        plight.range = range;
    }
}
