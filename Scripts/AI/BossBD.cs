using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBD : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
            EventCenter.Broadcast(MyEventType.DEATH);
    }
}
