using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour
{

    Ray2D ray;

    public Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y - 0.3f), Vector2.down);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue,0.1f);//起点，方向，颜色（可选）
        RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction,0.1f);
        if(info.collider!= null)
        {

            if (info.transform.gameObject.tag == "trap")
            {
                Debug.Log("awdwad");
                rb2d.AddForce(new Vector2(0,1000));
            }
        }

    }
}
