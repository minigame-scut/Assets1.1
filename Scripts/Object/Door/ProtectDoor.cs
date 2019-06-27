using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectDoor : MonoBehaviour
{
    float timer;
    bool startTimer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        startTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if(timer >= 15.0f)
            {
                timer = 0;
                startTimer = false;
                EventCenter.Broadcast(MyEventType.SWIMDELETE);
            }
            if(GameManager.instance.getSceneManager().GetComponent<SManager>().gamePlayer.GetComponent<PlayerPlatformController>().getPlayerData().isDead)
            {
                timer = 0;
                startTimer = false;
            }
        }
            
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            timer = 0;
            startTimer = true;
            EventCenter.Broadcast(MyEventType.PREPARESWIM);
            Debug.Log("protectDoor");
        }
    }
}
