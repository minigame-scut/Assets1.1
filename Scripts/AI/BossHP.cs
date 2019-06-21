using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{

    [Header("生命")]
    public int HP = 3;

    private int hurtCD;
    private GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        Boss = transform.parent.gameObject;
        hurtCD = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP < 0)
            Boss.SetActive(false);
        if (hurtCD < 60)
            hurtCD++;
        if (GameObject.Find("player 1(Clone)") == null)
        {
            HP = 3;
            if(!Boss.activeSelf)
                Boss.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player"&&hurtCD >= 60)
        {
            HP--;
            hurtCD = 0;
            transform.parent.gameObject.GetComponentInChildren<Animator>().SetBool("hurt", true);
            Invoke("setHurtFalse",0.66f);
            Debug.Log(123);
            //setHurtFalse();
        }
           
    }
    void setHurtFalse()
    {
        transform.parent.gameObject.GetComponentInChildren<Animator>().SetBool("hurt", false);
        Invoke("setHurtFalse", 0.66f);
    }
}
