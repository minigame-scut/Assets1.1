using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endToDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast<string>(MyEventType.NEXTPLACE, this.name);
        }
    }
}
