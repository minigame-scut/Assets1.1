using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadow : MonoBehaviour
{
    [Header("残影存在时间")]
    public int CD = 10;

    private int timer;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > CD)
            Destroy(gameObject);
        timer++;
        Color color = sr.color;
        color.a = (CD - timer) * 1.0f / CD;
        sr.color = color;
    }
}
