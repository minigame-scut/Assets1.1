using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoLogic : MonoBehaviour
{
    public GameObject gamePlayer;
    // Start is called before the first frame update
    void Start()
    {
        listener();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePlayer == null)
            gamePlayer = GameObject.Find("player(Clone)");
    }
    void responseForHELLODEMO(Vector3 pos)
    {
        if(gamePlayer != null)
        {
            
            if (pos.y <= 9f && pos.y >= 8f)
            {
                Debug.Log("123");
                gamePlayer.transform.position = new Vector3(gamePlayer.transform.position.x, -7f, 0);
            }
                
            else if (pos.y >= -9f && pos.y <= -8f)
            {
                Debug.Log("123");
                gamePlayer.transform.position = new Vector3(gamePlayer.transform.position.x, 7f, 0);
            }
                
            else if (pos.x >= -15f && pos.x <= -14.0f)
            {
                Debug.Log("123");
                gamePlayer.transform.position = new Vector3(12.8f, gamePlayer.transform.position.y, 0);
            }
                
            else if (pos.x <= 14.5f && pos.x >= 13.5f)
            {
                Debug.Log("123");
                gamePlayer.transform.position = new Vector3(-13.4f, gamePlayer.transform.position.y, 0);
            }
                
        }
    }
    void listener()
    {
        EventCenter.AddListener<Vector3>(MyEventType.HELLODEMO, responseForHELLODEMO);
    }
}
