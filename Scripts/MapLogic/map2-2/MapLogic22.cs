using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLogic22 : MonoBehaviour
{
    public GameObject bat;
    private GameObject batPrefab;
    private Vector3 initPos;
    void Awake()
    {
        batPrefab = ResourceManager.GetInstance().getGameObject("GameManagerRes/bat");
    }
    // Start is called before the first frame update
    void Start()
    {
        initPos = new Vector3(0, 12.0f);
        bat = GameObject.Instantiate(batPrefab, initPos, Quaternion.identity);
        listener();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void responseForDEATH()
    {
        Invoke("destroyBat", 2.0f);
        Invoke("initBat", 4.0f);
    }
    void responseForENABLEBAT()
    {
        if (!bat.activeInHierarchy)
            bat.SetActive(true);
    }
    void listener()
    {
        EventCenter.AddListener(MyEventType.DEATH, responseForDEATH);
        EventCenter.AddListener(MyEventType.ENABLEBAT, responseForENABLEBAT);
    }
    void destroyBat()
    {
        Destroy(bat);
    }
    void initBat()
    {
        if(bat == null)
        {
            bat = GameObject.Instantiate(batPrefab, initPos, Quaternion.identity);
        }
    }
    void OnDestroy()
    {
        EventCenter.RemoveListenter(MyEventType.DEATH, responseForDEATH);
        EventCenter.RemoveListenter(MyEventType.ENABLEBAT, responseForENABLEBAT);
    }
}
