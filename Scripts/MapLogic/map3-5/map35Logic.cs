using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map35Logic : MonoBehaviour
{
    //生成弹球的计时器
    float timer = 0;
    float proTime = 2f;

    //分数的计时器
    float timer2 = 0;
    float ScoreTime = 1f;
    public TextMesh timeText;

    //关卡时间
    public int mapTime = 120;
    GameObject ballPre;
    // Start is called before the first frame update
    void Start()
    {
        ballPre = ResourceManager.GetInstance().getGameObject("GameManagerRes/ball");
        timeText.text = mapTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > proTime)
        {
            GameObject.Instantiate(ballPre);
            timer = 0;
        }

        timer2 += Time.deltaTime;
        if(timer2 > ScoreTime)
        {
            timer2 = 0;
            mapTime--;
            timeText.text = mapTime.ToString();
        }

    }
}
