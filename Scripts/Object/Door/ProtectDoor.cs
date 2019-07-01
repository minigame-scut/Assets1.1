using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectDoor : MonoBehaviour
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
        if (other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.PREPARESWIM);
            GameObject effect = transform.Find("snowEffect").gameObject;
            if (effect != null)
            {
                //if(!effect.GetComponent<ParticleSystem>().isPlaying)
                effect.GetComponent<ParticleSystem>().Play();
                //Debug.Log("protectDoor");
            }
        }
    }
}
