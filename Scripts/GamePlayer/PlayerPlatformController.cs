using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPlatformController : PhysicalObject
{
    //动画组件
    private Animator anim;
    //当前跳跃速度
    public float curJumpSpeed;
    //当前冲刺速度
    public float curRushSpeed;
    //阴影对象
    public GameObject shadowr;
    public GameObject shadowl;
    //输入计时器，用于防止出生动画未播完玩家就开始进行操作
    public float inputTimer = 0;
    //是否暂停
    public bool isPause = false;
    //受到弹力方向
    public Transform elasticTrans;
    //旋转角
    float angleX;
    float angleY;

    Vector2 move;
    
    // Start is called before the first frame update
    void Start()
    {
        move = Vector2.zero;
        anim = GetComponent<Animator>();
        curJumpSpeed = playerData.normalJumpSpeed;
        curRushSpeed = playerData.normalRushSpeed;
        //设置玩家出生动画
        if (playerData.isBirth)
        {
            anim.SetBool("isBirth", true);
            playerData.isBirth = false;
            StartCoroutine(setBirthAnimAsFalse());
        }

        angleX = (playerData.gravityTrans == -1) ? 180 : 0;
        angleY = (playerData.dir == 1) ? 0 : 180;
        
        this.transform.localEulerAngles = new Vector3(angleX, angleY, 0);
    }
    //重写PhysicalObject中计算玩家速度的函数
    protected override void playerControl()
    {
        //是否暂停
        if (isPause)
            return;

        angleX = (playerData.gravityTrans == -1) ? 180 : 0;
        angleY = (playerData.dir == 1) ? 0 : 180;
             
        //输入计时器计时
        if (inputTimer <= 1.2f)
        {
            inputTimer += Time.fixedDeltaTime;
            return;
        }
        //玩家是否死亡
        if (playerData.isDead)
        {
            Dead();
            return;
        }
        //是否重力翻转
        if (playerData.buff.contains(Buff.GRAVITY))
        {
            if (playerData.flagGravity == 0)
            {
                gravityContrary();
                playerData.flagGravity = 1;
            }
        }
        //判断是否冲刺，然后进行冲刺位移计算和阴影效果生成
        if (isRush)
        {
            //生成阴影
            createShdow();
            //冲刺移动
            rushMove();
            return;
        }
        else
        {
            move = Vector2.zero;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetAxis("Horizontal") >= -1 && Input.GetAxis("Horizontal") <= -0.3))
        {
            //左跑
            run(-1);
        }
        else if (Input.GetKey(KeyCode.D) || (Input.GetAxis("Horizontal") <= 1 && Input.GetAxis("Horizontal") >= 0.3))
        {
            //右跑
            run(1);
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetAxis("Horizontal") == 0)
        {
            isWalk = false;
        }
        //冲刺状态
        if ((Input.GetKeyDown(KeyCode.K) || Input.GetButtonDown("Rush")) && playerData.canRush)
        {
            if (playerData.buff.contains(Buff.SUPERRUSH))
            {
                superRush();
            }
            else
            {
                rush();
            }
            //空中只能冲刺一次
            playerData.canRush = false;
            //广播移除重置buff的信号
            EventCenter.Broadcast(MyEventType.INITRUSHDELETE);
            EventCenter.Broadcast(MyEventType.RUSH);
            return;
        }
        //跳跃
        if (Input.GetButtonDown("Jump") && playerData.canJump)
        {
            //再次初始化curJumpSpeed
            curJumpSpeed = playerData.normalJumpSpeed;
            if(playerData.buff.contains(Buff.SUPERJUMP))
            {
                superJump();
            }
            else
            {
                jump();
            }
            EventCenter.Broadcast(MyEventType.INITJUMPDELETE);
            EventCenter.Broadcast(MyEventType.JUMP);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            //大跳小跳
            isWalk = false;
            isJump = true;
            if (velocity.y > 0 && playerData.gravityTrans == 1)
                velocity.y = velocity.y * 0.3f;
            else if (velocity.y < 0 && playerData.gravityTrans == -1)
                velocity.y = velocity.y * 0.3f;
        }
        //判断玩家当前是否持有弹力buff
        if (playerData.buff.contains(Buff.ELASTIC))
        {
            //计算弹力buff持有的时间，当时间>=0.2s时广播信号移除该buff
            playerData.elasticTimer += Time.fixedDeltaTime;
            if (playerData.elasticTimer < 0.3f)
            {
                Debug.Log("I have ealstic");
                elasticUp();
            }
            else
            {
                EventCenter.Broadcast(MyEventType.ELASTICDELETE);
            }
        }
        targetVelocity = move * playerData.maxSpeed;
    }
    protected override void playAnimation()
    {
        if(isWalk)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
        if(isJump)
        {
            anim.SetBool("isJump", true);
        }
        else if(!isJump)
        {
            anim.SetBool("isJump", false);
        }
        if(isRush)
        {
            anim.SetBool("isDash", true);
        }
        else if(!isRush)
        {
            anim.SetBool("isDash", false);
        }
    }
    //得到玩家的数据
    public PlayerData getPlayerData()
    {
        return this.playerData;
    }
    //设置玩家的数据
    public void setPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
    }
    //设置玩家的位置数据
    public void setPlayerDataPosition(Vector3 pos)
    {
        this.playerData.setPlayerVector3DPositionData(pos.x, pos.y, pos.z);
    }
    //设置玩家所处地图的数据
    public void setPlayerDataMapIndex(int index)
    {
        this.playerData.mapIndex = index;
    }
    //奔跑
    void run(int direction)
    {
        playerData.dir = direction;
        this.transform.localEulerAngles = new Vector3(angleX, angleY, 0);
        move.x = playerData.dir;
        isWalk = true;
    }
    //跳跃
    void jump()
    {
        isWalk = false;
        isJump = true;
        curJumpSpeed = playerData.normalJumpSpeed;
        velocity.y = curJumpSpeed * playerData.gravityTrans;
    }
    //超级跳
    void superJump()
    {
        isWalk = false;
        isJump = true;
        curJumpSpeed = playerData.superJumpSpeed;
        velocity.y = curJumpSpeed * playerData.gravityTrans;
    }
    //普通冲刺,设置普通冲刺时的速度
    void rush()
    {
        isJump = false;
        isWalk = false;
        move.x = playerData.dir;
        isRush = true;
        this.curRushSpeed = playerData.normalRushSpeed;
        targetVelocity = move * curRushSpeed;
    }
    //超级冲刺， 设置超级冲刺时的速度
    void superRush()
    {
        isJump = false;
        isWalk = false;
        move.x = playerData.dir;
        isRush = true;
        this.curRushSpeed = playerData.superRushSpeed;
        targetVelocity = move * curRushSpeed;
    }
    //冲刺移动，实际计算冲刺位移的函数，并判断冲刺是否结束
    void rushMove()
    {
        //是冲刺状态
        if (playerData.rushTimer <= playerData.rushMaxTime)
        {
            playerData.rushTimer += Time.fixedDeltaTime;
            targetVelocity = move * curRushSpeed;
        }
        else  //结束了冲刺状态
        {
            playerData.rushTimer = 0;
            targetVelocity = move * playerData.maxSpeed;
            isRush = false;
        }
    }
    //生成阴影
    void createShdow()
    {
        if (playerData.dir == 1)
            Instantiate<GameObject>(shadowr, transform.position, transform.rotation);
        else if (playerData.dir == -1)
            Instantiate<GameObject>(shadowl, transform.position, transform.rotation);
    }
    //反重力
    void gravityContrary()
    {
        this.transform.localEulerAngles = new Vector3(angleX, angleY, 0);
        playerData.gravityTrans *= -1;
        velocity.y = 0;
    }
    //弹起
    void elasticUp()
    {
        velocity.y = elasticTrans.right.y * 4.4f;
        velocity.x = elasticTrans.right.x * 4.4f;
    }
    //玩家死亡
    void Dead()
    {
        velocity = Vector2.zero;
        anim.SetBool("isDeath", true);
    }
    IEnumerator setBirthAnimAsFalse()
    {
        yield return new WaitForSeconds(0.958f);
        anim.SetBool("isBirth", false);
    }
}
