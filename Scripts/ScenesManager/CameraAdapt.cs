using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdapt : MonoBehaviour
{
    public float width = 1920f;
    public float height = 1080f;

    // Start is called before the first frame update
    void Awake()
    {
        float orthorgraphicSize = GetComponent<Camera>().orthographicSize;
        float orthorgraphicRate = (Screen.height / (float)Screen.width) / (height / width);
        this.GetComponent<Camera>().orthographicSize = orthorgraphicSize * orthorgraphicRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
