using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum dir{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class ballController : MonoBehaviour
{

    Ray2D ray;

    public  float timer = 0f;
    float maxStayTime = 2f;
    public bool isMove = true;

    SpriteRenderer spriteRenderer;

    dir collDir;

    public Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
       
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

        ballJump();

        //ray = new Ray2D(new Vector2(transform.position.x, transform.position.y), Vector2.down);
        //Debug.DrawRay(ray.origin, ray.direction, Color.blue,0.3f);//起点，方向，颜色（可选）
        //RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction,0.3f);
        //if(info.collider!= null)
        //{

        //    if (info.collider.transform.tag == "trap")
        //    {
        //        Debug.Log("awdwad");
        //        rb2d.AddForce(new Vector2(0,1000));
        //    }
        //}

    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "trap")
        {
            timer = 0;
            isMove = true;
        }
    }

    void ballJump()
    {
        if (!isMove)
        {
            timer += Time.fixedDeltaTime;
            if (timer > maxStayTime)
            {
                System.Random random = new System.Random();
                float xF = random.Next(100, 500);
                float yF = random.Next(100, 500);
                if (random.Next() % 2 == 0)
                    xF *= -1;
                if (random.Next() % 2 == 0)
                    yF *= -1;

                rb2d.AddForce(new Vector2(xF, yF));
            }
        }
       
    }



private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "trap")
        {
            System.Random random = new System.Random();
            float xF = random.Next(100, 1000);
            float yF = random.Next(100, 1000);
           
            
            //判定球撞击方向
            if (collision.contacts[0].normal.y == -1)//从上方碰撞
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


            StartCoroutine(Coll(new Vector2(xF, yF), collDir));
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

