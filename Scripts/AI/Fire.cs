using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("子弹")]
    public GameObject fire;

    [Header("最长发射间隔")]
    public int fireCD = 300;
    private int cd;
    private int timer;
    void Start()
    {
        cd = fireCD;
        timer =0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Fire(Clone)") != null||GameObject.FindGameObjectWithTag("player")==null)
            timer=0;
        timer++;
        if (timer >= cd )
        {
            cd = Random.Range(60, fireCD);
            Vector3 temp = new Vector3(transform.position.x + (cd % 40), transform.position.y, transform.transform.position.z);
            Instantiate<GameObject>(fire, temp,transform.rotation);
            
            timer = 0;
        }
    }

}
