﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.INPOOL);
            Debug.Log("Inpool");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.OUTPOOL);
            Debug.Log("Outpool");
        }
    }
}