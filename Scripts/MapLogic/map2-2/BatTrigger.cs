using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.ENABLEBAT);
        }
    }
}
