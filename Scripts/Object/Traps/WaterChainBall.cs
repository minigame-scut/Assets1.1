using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterChainBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "player" && !GameManager.instance.getSceneManager().GetComponent<SManager>().getGamePlayer().GetComponent<PlayerPlatformController>().getPlayerData().isDead)
        {
            EventCenter.Broadcast(MyEventType.SHAKESCREEN);
            EventCenter.Broadcast(MyEventType.DEATH);
        }
    }
}
