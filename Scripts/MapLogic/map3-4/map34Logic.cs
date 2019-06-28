using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


struct BallSlime
{
    public GameObject ballSlime;
    public Rigidbody2D flyRb;
    public float flyTime;
    public Vector3 pos;
    public int index;
}

public class map34Logic : MonoBehaviour
{
    public List<Transform> scoord;

    public List<Transform> pcoord;
    float proTimer = 0;
    float proTime = 8f;

    float ballSlimeProTimer = 2;
    float ballSlimeProTime = 9;

    List<BallSlime> BallSlimeList;


    GameObject slimePre;
    GameObject ballSlimePre;


    // Start is called before the first frame update
    void Start()
    {
        BallSlimeList = new List<BallSlime>();
        slimePre = ResourceManager.GetInstance().getGameObject("GameManagerRes/Slime3-4");
        GameObject.Instantiate(slimePre, this.transform.position, Quaternion.identity);
        ballSlimePre = ResourceManager.GetInstance().getGameObject("GameManagerRes/map3-4ball");

        BallSlime bs = new BallSlime();
        proBallSlime(bs);
      
    }

    // Update is called once per frame
    void Update()
    {
        ballSlimeIsReach();
        proTimer += Time.deltaTime;
        if(proTimer > proTime)
        {
            GameObject.Instantiate(slimePre, this.transform.position, Quaternion.identity);
            proTimer = 0;
        }
        ballSlimeProTimer += Time.deltaTime;
        if (ballSlimeProTimer > ballSlimeProTime)
        {
            //丢出新的史莱姆
            BallSlime bs = new BallSlime();
            proBallSlime(bs);
            ballSlimeProTimer = 0;

        }

    }

    //随机产生球状史莱姆
    void proBallSlime(BallSlime bs)
    {
        bs.ballSlime = GameObject.Instantiate(ballSlimePre, this.transform.position, Quaternion.identity);

        bs.flyRb = bs.ballSlime.GetComponent<Rigidbody2D>();

        System.Random rand = new System.Random();
        bs.index = rand.Next(0, pcoord.Count);
        Vector2 dis = pcoord[bs.index].position - this.transform.position;

        bs.pos = pcoord[bs.index].position;
        double t = (Math.Abs(dis.y) * 2 / -Physics2D.gravity.y);

        bs.flyTime = (float)Math.Sqrt(t);
        float flySpeedX = dis.x / bs.flyTime;
        bs.flyRb.velocity = new Vector2(flySpeedX, 0);
        BallSlimeList.Add(bs);
    }

    //检测球状史莱姆是否到达目的地
    void ballSlimeIsReach()
    {
        if (BallSlimeList == null)
        {
            Debug.Log("ERROR_BALL_LIST");
        }
        else
        {
            foreach (BallSlime bs in BallSlimeList)
            {
                if ((bs.ballSlime.transform.position - bs.pos).magnitude < 0.3f && bs.ballSlime.layer == LayerMask.NameToLayer("map3-4ball"))
                {
                    bs.ballSlime.layer = LayerMask.NameToLayer("map3-4slime");
                    bs.flyRb.velocity = Vector2.zero;
                    bs.ballSlime.SetActive(false);
                    GameObject newSlime = GameObject.Instantiate(slimePre, bs.ballSlime.transform.position, Quaternion.identity);
                    switch (bs.index)
                    {
                        case 0:
                            newSlime.GetComponent<slimeController>().setIndex(9);
                            newSlime.GetComponent<SpriteRenderer>().flipX = true;
                            break;
                        case 1:
                            newSlime.GetComponent<slimeController>().setIndex(5);
                            newSlime.GetComponent<SpriteRenderer>().flipX = false;
                            break;
                        case 2:
                            newSlime.GetComponent<slimeController>().setIndex(11);
                            newSlime.GetComponent<SpriteRenderer>().flipX = false;
                            break;
                        case 3:
                            newSlime.GetComponent<slimeController>().setIndex(14);
                            newSlime.GetComponent<SpriteRenderer>().flipX = true;
                            break;
                    }
                }
            }
        }
    }
    
}
