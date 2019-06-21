using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BossState
{
    walk,
    attack,
    back
}

public class Boss : MonoBehaviour
{

    [Header("速度")]
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float backSpeed = 5;

    [Header("冲刺间隔")]
    public float runCD = 0;
    private float runTimer;

    [Header("攻击残影")]
    public GameObject shadow;
    public float shadowCD = 10;
    private float shadowTimer;

    [Header("出生点")]
    public GameObject bp;

    //玩家信息
    private GameObject player;
    private Vector3 playerPos;

    //移动方向
    private int dir;
    //当前Boss状态
    private BossState bossState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player 1(Clone)");
        runTimer = 0;
        shadowTimer = 0;
        dir = -1;
        bossState = BossState.walk;
    }

    // Update is called once per frame
    void Update()
    {
        if (!getPlayer())
            return;
        //lookAtPlayer();
        switch(bossState)
        {
            case BossState.walk:
                {
                    Walk();
                    runTimer++;
                    if (runTimer > runCD)
                    {
                        bossState = BossState.attack;
                        lookAtPlayer();
                        runTimer = 0;
                    }
                    break;
                }
            case BossState.attack:
                {
                    Attack();
                    CreateAttackShadow();
                    break;
                }
            case BossState.back:
                {
                    Back();
                    break;
                }
        }
    }

    bool getPlayer()
    {
        if(!player)
        {
            player = GameObject.Find("player 1(Clone)");
        }
        return player;
    }

    void lookAtPlayer()
    {
        playerPos = player.transform.position;
        if (playerPos.x < transform.position.x)
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        else
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    void Attack()
    {
        if (gameObject.GetComponentInChildren<Animator>() == null)
            return;
        gameObject.GetComponentInChildren<Animator>().SetBool("attack", true);
        transform.Translate(Vector3.Normalize(playerPos - transform.position)*runSpeed*Time.deltaTime,Space.World);
        if( Vector3.Distance(transform.position,playerPos) <= 0.5f)
        {
            bossState = BossState.back;
            gameObject.GetComponentInChildren<Animator>().SetBool("attack", false);
        }
    }

    void CreateAttackShadow()
    {
        if (shadowTimer % shadowCD == 0)
            Instantiate<GameObject>(shadow, transform.position, transform.rotation);
        shadowTimer++;
    }

    void Back()
    {
        if (bp.transform.position.x < transform.position.x)
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        else
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
        transform.Translate(Vector3.Normalize(bp.transform.position - transform.position) * backSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, bp.transform.position) <= 0.5f)
        {
            bossState = BossState.walk;
        }
    }

    void Walk()
    {
        this.transform.localEulerAngles = new Vector3(0, (dir != 1 ? 0 : 180), 0);
        transform.position += new Vector3(dir * walkSpeed * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag!="player")
        {
            dir *= -1;
        }

    }
}
