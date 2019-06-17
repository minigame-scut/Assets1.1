using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Effect1: MonoBehaviour
{
    public float maxTime = 5.0f;
    // Start is called before the first frame update

    float timer = 0;


    private float random()
    {
        var seed = Guid.NewGuid().GetHashCode();
        System.Random r = new System.Random(seed);
        int i = r.Next(0, 100000);
        if (i % 2 == 0)
            i = -i;
        return (float)i / 100000;
    }

    Vector3 pos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < maxTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            pos = new Vector3(random()*8, random()*5, 0);
            EventCenter.Broadcast(MyEventType.WAVE, pos);
            timer = 0;
        }
    }
}
