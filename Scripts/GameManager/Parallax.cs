using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;
    public float parallaxScale;
    public float parallaxReduxtionFactor;
    public float smoothing;


    private Transform cam;
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = cam.position;
    }

    // Update is called once per frame
    void Update()
    {
        UseParallax();
        previousCamPos = cam.position;
    }
    void UseParallax()
    {
        float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScale;
        float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScale;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float backgroundTargetPosX = backgrounds[i].position.x + parallaxX * (i * parallaxReduxtionFactor + 1);
            float backgroundTargetPosY = backgrounds[i].position.y + parallaxY * (i * parallaxReduxtionFactor + 1);
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing);
        }
    }
}
