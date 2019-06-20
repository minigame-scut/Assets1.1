using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMap : MonoBehaviour
{
    private Animator ani;//获得动画控制器组件

    void Start()
    {
        ani = this.GetComponent<Animator>();
        //  ani.SetBool("closemap", true);
        ani.SetBool("openmap", true);
        Debug.Log("switcher");
    }

    //播放开始动画
    public void PlayOpenMap()
    {
        ani.SetBool("openmap", true);
    }
    //停止播放开始动画
    public void StopOpenMap()
    {
        ani.SetBool("openmap", false);
    }

    //播放关闭动画
    public void PlayCloseMap()
    {
        ani.SetBool("closemap", true);
    }
    //停止播放关闭动画 
    public void StopCloseMap()
    {
        ani.SetBool("closemap", false);
    }
}
