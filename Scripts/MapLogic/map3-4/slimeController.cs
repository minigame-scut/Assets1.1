using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeController : MonoBehaviour
{
    //当前位置索引
    int nowCoordIndex = 0;
    GameObject mapLogic;

    SpriteRenderer sr;

    //缸体
    Rigidbody2D rb;

    //速度
    public float speed = 5;

    //目标位置
    Vector3 toPos;

    //声音定时器
    float voiceTimer = 0;
    bool isVoice = false;

    //是否在空中
     bool isJump = false;
  
    // Start is called before the first frame update
    void Start()
    {
        mapLogic = GameObject.Find("map3-4Logic");
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
     
        if (sr.sprite.name == "sprite_6" && !isJump)
        {
            rb.velocity = new Vector2(0,7);
        
        }
        toPos = mapLogic.GetComponent<map34Logic>().scoord[nowCoordIndex].position;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, toPos, speed * Time.deltaTime);
        if ((this.transform.position - toPos).magnitude < 0.1f && nowCoordIndex < mapLogic.GetComponent<map34Logic>().scoord.Count - 1)
        {
            if (nowCoordIndex == 2 || nowCoordIndex == 5 || nowCoordIndex == 9 || nowCoordIndex == 12 || nowCoordIndex == 15)
                sr.flipX = !sr.flipX;

            nowCoordIndex++;
        }

        if (isVoice)
        {
            voiceTimer += Time.deltaTime;
            if (voiceTimer > 1f)
            {
                voiceTimer = 0;
                isVoice = false;
            }
               
        }
           



    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "ground" && coll.contacts[0].normal.y == 1 && coll.contacts[0].normal.x == 0)
        {
            if(isJump && !isVoice)
            {
                EventCenter.Broadcast(MyEventType.SLMJUMP);
                isVoice = true;
            }
             
            isJump = false;
        
        }
            
        if (coll.transform.tag == "trap")
            Destroy(gameObject);
    }
    

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "ground")
        {
          
            isJump = true;
            
        }
            
     
    }

    public void setIndex(int input)
    {
        this.nowCoordIndex = input;
    }

   
}
