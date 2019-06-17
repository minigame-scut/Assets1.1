using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public int CD = 10;

    private int timer;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        //CD = 10;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (timer > CD)
            Destroy(gameObject);
        Color color = sr.color;
        color.a = (CD - timer)*1.0f / CD;
        sr.color = color;
    }
}
