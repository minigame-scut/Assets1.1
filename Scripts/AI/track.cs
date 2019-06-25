using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class track : MonoBehaviour
{
    [Header("生存时间")]
    public int live = 60;
    private int timer;

    [Header("追踪速度")]
    public float trackspeed = 10;

    [Header("追踪目标名字")]
    public string objname;
    private GameObject trackobj;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        if (GameObject.Find(objname) != null)
            trackobj = GameObject.Find(objname);
    }

    // Update is called once per frame
    void Update()
    {
        if(trackobj==null)
        {
            if (GameObject.Find(objname) == null)
            {
                Destroy(gameObject);
                return;
            }  
            trackobj = GameObject.Find(objname);
        }
        if(timer >= live )
        {
            Destroy(gameObject);
        }

        Track(trackobj.transform.position);
        timer++;
    }
    void Track(Vector3 dst)
    {
        Vector3 forward = dst - transform.position;
        forward = Vector3.Normalize(forward);
        transform.Translate(forward * trackspeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "player")
        {
           
            EventCenter.Broadcast(MyEventType.DEATH);
            Destroy(gameObject);
        }
    }
}
