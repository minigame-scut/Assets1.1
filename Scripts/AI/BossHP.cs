using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{

    [Header("生命")]
    public int HP = 3;

    private int hurtCD;
    private GameObject Boss;
    private int hp;
    // Start is called before the first frame update
    void Start()
    {
        Boss = transform.parent.gameObject;
        hurtCD = 0;
        hp = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP < 0)
        {
            Boss.transform.Find("BossBody").gameObject.SetActive(false);
            if(!Boss.transform.Find("Particle System").GetComponent<ParticleSystem>().isPlaying)
                Boss.transform.Find("Particle System").GetComponent<ParticleSystem>().Play();
            dieBoss();
            GameObject.Find("KeyPoints").transform.Find("nextPlace2-5-2").gameObject.SetActive(true);
        }
            
        if (hurtCD < 60)
            hurtCD++;
        if (GameObject.Find("player 1(Clone)") == null)
        {
            HP = hp;
            if(!Boss.activeSelf)
            {
                Boss.SetActive(true);
                Boss.transform.Find("BossBody").gameObject.SetActive(true);
            }

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
        if (transform.parent.gameObject.GetComponentInChildren<Animator>() == null)
            return;
        transform.parent.gameObject.GetComponentInChildren<Animator>().SetBool("hurt", false);
        Invoke("setHurtFalse", 0.66f);
    }

    void dieBoss()
    {
        StartCoroutine(dieBossIE());
    }
    IEnumerator dieBossIE()
    {
        yield return new WaitForSeconds(1.5f);
        Boss.SetActive(false);
    }
}
