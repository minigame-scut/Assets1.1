using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhysicalObject : MonoBehaviour
{
    //PlayerData实例化对象
    public PlayerData playerData;
    //判定是否在地上的阈值
    public float minGroundNormalY = 0.65f;
    protected float gravityModifier = 0.68f;
    protected float swimModifier = 0.4f;
    //是否在rush
    public bool isRush = false;
    //是否在walk
    public bool isWalk = false;
    //是否在jump
    public bool isJump = false;
    //是否在空中
    public bool isGround = true;
    //是否下坠
    public bool isDrop = false;
    //是否游泳
    public bool isSwim = false;
    //是否在水中上升
    public bool isUp = false;


    protected Vector2 targetVelocity;

    
    protected Vector2 groundNormal;

    protected Rigidbody2D rb2d;
    protected Vector2 velocity;

    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    
    //最小移动距离
    protected const float minMoveDistance = 0.001f;
    //检测半径，人物距离碰撞体边缘的检测距离
    protected const float shellRadius = 0.01f;
    //实例化PlayerData对象
    void Awake()
    {
        playerData = new PlayerData();
    }
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        playerControl();
        playAnimation();

        //广播移动信号
        if (velocity != Vector2.zero && isGround)
        {
            if(isSwim)
                EventCenter.Broadcast(MyEventType.SWIMMING);
            else
                EventCenter.Broadcast(MyEventType.WALK);
        }
            
    }
    protected virtual void playerControl() { }

    protected virtual void playAnimation() { }

    void FixedUpdate()
    {
        Debug.Log(isSwim);
        //Physics2D.gravity = (0,-9.8f)  模拟重力加速
        if (!(playerData.buff.contains(Buff.SWIM) && playerData.buff.contains(Buff.CANSWIM)))
        {
            velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime * playerData.gravityTrans;
            isSwim = false;
        }  
        else
        {
            isSwim = true;
            if (velocity.y <= 3.0f || velocity.y >= -3.0f)
                velocity += gravityModifier * swimModifier * Physics2D.gravity * Time.fixedDeltaTime * playerData.gravityTrans;
        }
        //赋予移动方向
        velocity.x = targetVelocity.x;
        isGround = false;
        if (isRush)
        {
            velocity.y = 0;
        }
        if(playerData.buff.contains(Buff.INITRUSH))
        {
            playerData.initRush();
        }
        if(playerData.buff.contains(Buff.INITJUMP))
        {
            playerData.initJump();
        }
        else
        {
            playerData.canJump = false;
        }

        if(playerData.buff.contains(Buff.CANSWIM) && playerData.buff.contains(Buff.SWIM))
        {
            playerData.canJump = false;
            playerData.canRush = false;
            playerData.canSwim = true;
        }
        else
        {
            playerData.canSwim = false;
        }

        Vector2 deltaPosition = velocity * Time.deltaTime;

        //人物前进要和地面法向量垂直，人物前进方向
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        //计算x轴移动向量
        Vector2 move = moveAlongGround * deltaPosition.x;

        //水平移动
        Movement(move, false);

        //垂直移动
        //计算y轴移动向量
        move = Vector2.up * deltaPosition.y;
        
        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        //移动向量的模，移动的距离
        float distance = move.magnitude;

        if(distance > minMoveDistance)
        {
            //cast发出射线检测
            //move 目标投影的方向向量  contactFilter过滤器，用于layer的过滤，筛选返回结果   hitbuffer存储结果   distance检测的最大距离
            //count为射线碰撞检测到的数量
            int count =  rb2d.Cast(move, contactFilter, hitBuffer, distance +shellRadius);
            hitBufferList.Clear();
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                //碰到的表面的法向量
                Vector2 currentNormal = hitBufferList[i].normal;

                if(playerData.gravityTrans == 1)
                {
                    if (currentNormal == new Vector2(0, 1))
                    {
                        if (hitBufferList[i].transform.tag == "ground")
                        {
                            playerData.canRush = true;
                        }

                        //判断玩家是否在地上
                        if (currentNormal.y > minGroundNormalY)
                        {
                            //在地上
                            isGround = true;
                            isJump = false;
                            playerData.canJump = true;
                            //在y轴上移动
                            if (yMovement)
                            {
                                groundNormal = currentNormal;
                                currentNormal.x = 0;
                            }
                        }
                        //y轴移动距离
                        float projection = Vector2.Dot(velocity, currentNormal);

                        if (projection < 0)
                        {
                            //x轴速度计算
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                }
                else if(playerData.gravityTrans == -1)
                {
                    if (currentNormal == new Vector2(0, -1))
                    {
                        if (hitBufferList[i].transform.tag == "ground")
                        {
                            playerData.canRush = true;
                        }

                        //判断玩家是否在地上
                        float yCurrentNormal = currentNormal.y > 0 ? currentNormal.y : (-currentNormal.y);
                        if (yCurrentNormal > minGroundNormalY)
                        {
                            //在地上
                            isGround = true;
                            isJump = false;
                            playerData.canJump = true;
                            //在y轴上移动
                            if (yMovement)
                            {
                                groundNormal = currentNormal * playerData.gravityTrans;
                                currentNormal.x = 0;
                            }
                        }
                        //y轴移动距离
                        float projection = Vector2.Dot(velocity, currentNormal);

                        if (projection < 0)
                        {
                            //x轴速度计算
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                }
                //防止抖动 取小值
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }                  
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
