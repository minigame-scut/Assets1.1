using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{
    private Animator anim;

    public float speed = 3;

    private int state = 0;

    public float runTimer = 0;
    public float maxTime = 2.5f;

    private float dir = 1;

    public float jumpTimer = 0;
    public float jumpTime = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("change", state);
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(state);
        if (runTimer < maxTime)
        {
            transform.Translate(speed * Time.deltaTime * dir, 0, 0);
            runTimer += Time.deltaTime;
        }
        else
        {
            if (state == 0)
            {
                state = 1;
              
            }    
            else if(state == 2)
                state = 3;

            anim.SetInteger("change", state);
            if (jumpTimer < jumpTime / 2)
            {
                jumpUp();
            }
            else if (jumpTimer < jumpTime)
            {
                jumpDown();
            }
            else
            {
                dir = -dir;
                if (state == 1)
                    state = 2;
                else if(state == 3)
                    state = 0;
                anim.SetInteger("change", state);
                runTimer = 0;
                jumpTimer = 0;
            }
           
        }


    }

    void jumpUp()
    {
        transform.Translate(0, speed * 0.5f * Time.deltaTime, 0);
        jumpTimer += Time.deltaTime;
    }

    void jumpDown()
    {
        transform.Translate(0, -speed * 0.5f * Time.deltaTime, 0);
        jumpTimer += Time.deltaTime;
    }
}
