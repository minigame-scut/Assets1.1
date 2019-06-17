using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    public float speed = 0.5f;
    public float startX;
    public float endX;
    private float x;
    // Start is called before the first frame update
    void Start()
    {
        //startX = -6.0f;
        //endX = 12.5f;
        x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        x += speed*Time.fixedDeltaTime;
        if (x > endX)
            x = startX;

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
