using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicForMapSeven : MonoBehaviour
{
    public float ghostBirthTimer;
    public GameObject ghost;
    private Vector3 ghostPos;
    private GameObject key;
    public GameObject transDoor;
    // Start is called before the first frame update
    void Start()
    {
        listener();
        ghostBirthTimer = 0.0f;
        key = GameObject.Find("Key");
        ghostPos = ghost.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ghostBirthTimer += Time.fixedDeltaTime;
        if(ghost != null)
        {
            if (ghostBirthTimer > 3.0f)
            {
                
                ghost.SetActive(true);
                ghostBirthTimer = 0;
            }
        }
        if(key != null && transDoor != null)
        {
            if (!key.activeInHierarchy)
                transDoor.SetActive(true);
            else
                transDoor.SetActive(false);
        }
    }
    void responseForDEATH()
    {
        if (ghost != null)
        {
            ghost.transform.position = ghostPos;
            ghostBirthTimer = 0;
            ghost.SetActive(false);
        }
        return;
    }
    void listener()
    {
        EventCenter.AddListener(MyEventType.DEATH, responseForDEATH);
    }
}
