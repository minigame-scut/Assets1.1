using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShadow : MonoBehaviour
{
    public GameObject shdow;
    private int timer;

    private void Start()
    {
        timer = 0;
    }
    void Update()
    {
        //if (timer > 100)
        //    return;
        if (timer % 50 == 0)
        {
            Instantiate<GameObject>(shdow, transform.position, transform.rotation);
        }
            
        timer++;
    }
}
