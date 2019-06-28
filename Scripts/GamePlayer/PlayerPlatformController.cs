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
    //游泳速度
    private float swimSpeed = 2.0f;
    //游泳计时器
    private float swimTimer = 0;
    //阴影对象
    public GameObject shadow;
    //输入计时器，用于防止出生动画未播完玩家就开始进行操作
    public float inputTimer = 0;
    //是否暂停
    public bool isPause = false;
    //受到弹力方向
    public Transform elasticTrans;
    //受到风力方向
    public Transform blowTrans;
    //精灵
    public SpriteRenderer spriteRenderer;
    //地面碰撞子物体
    public GameObject groundCollider;
    //游泳碰撞子物体
    public GameObject swimCollider;
    //地面冰门气罩
    public GameObject groundBubble;
    //水中冰门气罩
    public GameObject swimBubble;

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

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //重写PhysicalObject中计算玩家速度的函数
    protected override void playerControl()
    {
        //是否暂停
        if (isPause)
            return;

        //游泳计时器
        if (playerData.buff.contains(Buff.CANSWIM))
        {
            swimTimer += Time.fixedDeltaTime;
            if (swimTimer >= 0.7f)
            {
                if (groundBubble.activeInHierarchy)
                {
                    groundBubble.GetComponent<Bubble>().isInit = false;
                    groundBubble.GetComponent<Bubble>().isNearToDis = false;
                }
                else if (swimBubble.activeInHierarchy)
                {
                    swimBubble.GetComponent<Bubble>().isInit = false;
                    swimBubble.GetComponent<Bubble>().isNearToDis = false;
                }
            }
            if (swimTimer >= 15.0f)
            {
                if(groundBubble.activeInHierarchy)
                {
                    groundBubble.GetComponent<Bubble>().isNearToDis = true;
                }
                else if(swimBubble.activeInHierarchy)
                {
                    swimBubble.GetComponent<Bubble>().isNearToDis = true;
                }
            }
            if(swimTimer >= 20.0f)
            {
                swimTimer = 0;
                EventCenter.Broadcast(MyEventType.SWIMDELETE);
                if (groundBubble.activeInHierarchy)
                {
                    groundBubble.GetComponent<Bubble>().isNearToDis = false;
                    groundBubble.SetActive(false);
                }
                else if (swimBubble.activeInHierarchy)
                {
                    swimBubble.GetComponent<Bubble>().isNearToDis = false;
                    swimBubble.SetActive(false);
                }
            }
        }
        //判断重力方向，翻转玩家的碰撞体
        if (playerData.gravityTrans == -1)
        {
            groundCollider.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            groundCollider.transform.localEulerAngles = Vector3.zero;
        }
        //判断人物方向，翻转swim子物体的碰撞体
        if(playerData.dir == -1)
        {
            swimCollider.transform.localEulerAngles = new Vector3(0, 180, 90);
        }
        else
        {
            swimCollider.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        //判断玩家是否持有SWIM的buff，如果有则将玩家碰撞体改为游泳状态的碰撞体
        if (playerData.buff.contains(Buff.SWIM))
        {
            if (swimCollider != null && groundCollider != null)
            {
                swimCollider.SetActive(true);
                groundCollider.SetActive(false);
            }
        }
        else
        {
            if (swimCollider != null && groundCollider != null)
            {
                swimCollider.SetActive(false);
                groundCollider.SetActive(true);
            }
        }
        //判断玩家是否同时持有CANSWIM和SWIM的buff，如果持有SWIM但没有CANSWIM则玩家死亡
        if (playerData.buff.contains(Buff.SWIM) && (!playerData.buff.contains(Buff.CANSWIM)) && (!playerData.isDead))
        {
            EventCenter.Broadcast(MyEventType.SHAKESCREEN);
            EventCenter.Broadcast(MyEventType.DEATH);
        }

        spriteRenderer.flipX = (playerData.dir == 1) ? false : true;
        spriteRenderer.flipY = (playerData.gravityTrans == -1) ? true : false;

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
        //判断是否在下坠
        if (velocity.y * playerData.gravityTrans < 0)
        {
            isDrop = true;
        }
        else
        {
            isDrop = false; 
        }
        if(isGround)
        {
            isDrop = false;
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
        //游泳
        if(Input.GetButtonDown("Jump") && playerData.canSwim)
        {
            swim();
        }
        else if(Input.GetButtonUp("Jump") && playerData.canSwim)
        {
            isUp = false;
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
                elasticUp();
            }
            else
            {
                EventCenter.Broadcast(MyEventType.ELASTICDELETE);
            }
        }
        //判断玩家当前是否持有风力buff
        if(playerData.buff.contains(Buff.BLOW))
        {
            blow();
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
        if (isDrop)
        {
            anim.SetBool("isDrop", true);
        }
        else
        {
            anim.SetBool("isDrop", false);
        }
        if(isSwim)
        {
            anim.SetBool("isSwim", true);
        }
        else
        {
            anim.SetBool("isSwim", false);
        }
        if(isUp)
        {
            anim.SetBool("isUp", true);
        }
        else
        {
            anim.SetBool("isUp", false);
        }
        if (isRush)
        {
            anim.SetBool("isRush", true);
        }
        else if(!isRush)
        {
            anim.SetBool("isRush", false);
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
        //this.transform.localEulerAngles = new Vector3(angleX, angleY, 0);
        move.x = playerData.dir;
        isWalk = true;
    }
    //游泳
    void swim()
    {
        isSwim = true;
        isJump = false;
        isRush = false;
        isUp = true;
        velocity.y = playerData.gravityTrans * swimSpeed;
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
        Instantiate<GameObject>(shadow, transform.position, transform.rotation);
    }
    //反重力
    void gravityContrary()
    {
        //this.transform.localEulerAngles = new Vector3(angleX, angleY, 0);
        playerData.gravityTrans *= -1;
        velocity.y = 0;
    }
    //弹起
    void elasticUp()
    {
        velocity.y = elasticTrans.right.y * 4.4f;
        move.x += elasticTrans.right.x * 4.4f;
        //velocity.x = elasticTrans.right.x * 4.4f;
    }
    //风吹效果
    void blow()
    {
        velocity.y = blowTrans.right.y * -8.0f;
        //targetVelocity.x = blowTrans.right.x * 10.0f;
        move.x += blowTrans.right.x * 6.0f;
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
    //判断是否碰撞到冰冻门，如果碰到则刷新swimTimer
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "coldDoor")
        {
            swimTimer = 0;
            if(groundCollider.activeInHierarchy)
            {
                groundBubble.SetActive(true);
                groundBubble.GetComponent<Bubble>().isInit = true;
                Debug.Log(groundBubble.GetComponent<Bubble>().isInit);
            }
            else
            {
                swimBubble.SetActive(true);
                swimBubble.GetComponent<Bubble>().isInit = true;
            }
        }
    }
}
