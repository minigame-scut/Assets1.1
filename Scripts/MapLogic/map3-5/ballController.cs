using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum dir{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NO,
}

public class ballController : MonoBehaviour
{

    Ray2D ray;

    public Vector2 moveDir;
    //上一时刻的位置
    Vector2 prePos;

    public float forceFactor = 2f;

    public  float timer = 0f;
    float maxStayTime = 2f;
    public bool isMove = true;

    SpriteRenderer spriteRenderer;

    dir collDir;

    public Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        collDir = dir.NO;
        prePos = transform.position;
        moveDir = Vector2.zero;
        System.Random random = new System.Random();
        float xF = random.Next(-300, 400);
        float yF = random.Next(-300, 400);
        rb2d.AddForce(new Vector2(xF,yF));
    }
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        moveDir.x = transform.position.x - prePos.x;
        moveDir.y = transform.position.y - prePos.y;
        moveDir = moveDir.normalized;
        ballJump();


    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "ground")
        {
            timer = 0;
            isMove = true;
            prePos = transform.position;
        }
    }

    void ballJump()
    {
        if (!isMove)
        {
            timer += Time.fixedDeltaTime;
            //原地不动就变为史莱姆
            if (timer > maxStayTime / 4) {
                spriteRenderer.sprite = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_6") as Sprite;
                if (this.transform.position.x < GameManager.instance.getSceneManager().GetComponent<SManager>().getGamePlayer().transform.position.x)
                    spriteRenderer.flipX = true;
            }
            if (timer > maxStayTime)
            {
                System.Random random = new System.Random();
                float xF = random.Next(100, 500);
                float yF = random.Next(100, 500);
                if (random.Next() % 2 == 0)
                    xF *= -1;
                if (random.Next() % 2 == 0)
                    yF *= -1;
                spriteRenderer.sprite = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_2") as Sprite;
                rb2d.AddForce(new Vector2(xF, yF));
            }
        }
       
    }



private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "ground")
        {
            System.Random random = new System.Random();
            float xF = random.Next(300, 400) * forceFactor;
            float yF = random.Next(300, 400) * forceFactor;

            Vector2 force = new Vector2(xF, yF);
            //force *= moveDir;
            if (moveDir.x < 0)
                force.x *= -1;
            if (moveDir.y < 0)
                force.y *= -1;
            //Debug.Log(collision.contacts[0].normal.x + "  " + collision.contacts[0].normal.y);
            //判定球撞击方向
            if (collision.contacts[0].normal.y == -1 && collision.contacts[0].normal.x== 0)//从上方碰撞
            {
               
                collDir = dir.UP;
                force.y *= -1;
            }
            else if (collision.contacts[0].normal.y == 1 && collision.contacts[0].normal.x == 0)//从下方碰撞
            {
                force.y *= -1;
                collDir = dir.DOWN;
            }
            else if (collision.contacts[0].normal.x >0 && collision.contacts[0].normal.y == 0)//左边碰撞
            {
                force.x *= -1;
                collDir = dir.LEFT;
            }
            else if (collision.contacts[0].normal.x < 0 && collision.contacts[0].normal.y == 0)//右边碰撞
            {
                force.x *= -1;
                // Debug.Log("右");
                collDir = dir.RIGHT;
            }


            StartCoroutine(Coll(force, collDir));
        }
    }

    IEnumerator Coll(Vector2 force, dir direction)
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        switch (direction)
        {
           case dir.UP:
           spriteRenderer.sprite = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_5") as Sprite;
           break;
           case dir.DOWN:
                spriteRenderer.sprite = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_0") as Sprite;
                break;
            case dir.LEFT:
                spriteRenderer.sprite = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_4") as Sprite;
                break;
            case dir.RIGHT:
                spriteRenderer.sprite = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_3") as Sprite;
                break;
            default:
                Debug.Log("ERROR_DIR");
                break;
        }
       
        yield return new WaitForSeconds(0.1f);
        Sprite sp = ResourceManager.GetInstance().getSptite("Image/Roles/shilaimu/sprite_2") as Sprite;
        if (sp == null)
            Debug.Log("none sp");
        spriteRenderer.sprite = sp;
        isMove = false;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2d.AddForce(force);
    }
}

/*
///
///   if (collision.contacts[0].normal.y == -1)//从上方碰撞
            {
                if (random.Next() % 2 == 0)
                    xF *= -1;
                yF *= -1;
                //Debug.Log("上");
                collDir = dir.UP;
            }
            else if (collision.contacts[0].normal.y == 1)//从下方碰撞
            {
                if (random.Next() % 2 == 0)
                    xF *= -1;
                //Debug.Log("下");
                collDir = dir.DOWN;
            }
            else if (collision.contacts[0].normal.x == 1)//左边碰撞
            {
                if (random.Next() % 2 == 0)
                   yF *= -1;
             
                //Debug.Log("左");
                collDir = dir.LEFT;
            }
            else if (collision.contacts[0].normal.x == -1)//右边碰撞
            {
                if (random.Next() % 2 == 0)
                    yF *= -1;
                xF *= -1;
               // Debug.Log("右");
                collDir = dir.RIGHT;
            }
///*/