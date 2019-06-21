using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magma : MonoBehaviour
{
    private float magmaUpSpeed;
    private bool isUp;
    private Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        isUp = true;
        initPos = this.transform.position;
        magmaUpSpeed = 2.0f;
        listener();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp)
        {
            gameObject.transform.position += new Vector3(0, magmaUpSpeed * Time.deltaTime * 1.0f);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.DEATH);
        }
    }
    void responseForDEATH()
    {
        isUp = false;
        Invoke("resetMagma", 2.2f);
    }
    void listener()
    {
        EventCenter.AddListener(MyEventType.DEATH, responseForDEATH);
    }
    void resetMagma()
    {
        this.transform.position = initPos;
        isUp = true;
    }
    void OnDestroy()
    {
        EventCenter.RemoveListenter(MyEventType.DEATH, responseForDEATH);
    }
}
