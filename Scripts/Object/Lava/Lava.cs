using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    List<Transform> children = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        listener();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.INPOOL);
            foreach (Transform child in children)
            {
                Color tempColor = child.gameObject.GetComponent<SpriteRenderer>().color;
                tempColor.a = 0.3f;
                child.gameObject.GetComponent<SpriteRenderer>().color = tempColor;
                //child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "player")
        {
            EventCenter.Broadcast(MyEventType.OUTPOOL);
            foreach (Transform child in children)
            {
                Color tempColor = child.gameObject.GetComponent<SpriteRenderer>().color;
                tempColor.a = 1.0f;
                child.gameObject.GetComponent<SpriteRenderer>().color = tempColor;
                //child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }
    void responseForDEATH()
    {
        StartCoroutine("initLava");
    }
    void listener()
    {
        EventCenter.AddListener(MyEventType.DEATH, responseForDEATH);
    }
    IEnumerator initLava()
    {
        yield return new WaitForSeconds(3.0f);
        foreach (Transform child in children)
        {
            Color tempColor = child.gameObject.GetComponent<SpriteRenderer>().color;
            tempColor.a = 1.0f;
            child.gameObject.GetComponent<SpriteRenderer>().color = tempColor;
            //child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }
    void OnDestroy()
    {
        EventCenter.RemoveListenter(MyEventType.DEATH, responseForDEATH);
    }
}
