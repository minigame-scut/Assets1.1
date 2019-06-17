using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    bool isFirst = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player" && isFirst)
        {
            isFirst = false;
            EventCenter.Broadcast(MyEventType.DIALOG);
            EventCenter.Broadcast(MyEventType.PLAYERPAUSE, true);
        }
        return;
    }
}
