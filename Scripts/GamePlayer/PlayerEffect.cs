using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public Animator anim;

    //暂停计时器
    public float animMaxTime;

    //暂停时间
    public float animTimer;

    //是否暂停
    public bool isAnimPause;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = this.GetComponent<Animator>();
        animTimer = .0f;
        animMaxTime = .5f;
        isAnimPause = false;
        EventCenter.AddListener(MyEventType.ANIMPAUSE, setAnim);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimPause)
        {
            if(animMaxTime > animTimer)
            {
                anim.speed = 0;
                animTimer += Time.deltaTime;
            }
            else
            {
                anim.speed = 1;
                isAnimPause = false;
            }
        }
            
    }
    
    void setAnim()
    {
        isAnimPause = true;
    }

}
