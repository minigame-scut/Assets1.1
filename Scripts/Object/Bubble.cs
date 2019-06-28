using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator anim;
    public bool isInit;
    public bool isNearToDis;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        isInit = false;
        isNearToDis = false;
    }

    // Update is called once per frame
    void Update()
    {
        playAnimation();
    }
    void playAnimation()
    {
        if (isInit)
        {
            anim.SetBool("isInit", true);
        }
        else
        {
            anim.SetBool("isInit", false);
        }
        if (isNearToDis)
        {
            anim.SetBool("isNearToDis", true);
        }
        else
        {
            anim.SetBool("isNearToDis", false);
        }
    }
}
