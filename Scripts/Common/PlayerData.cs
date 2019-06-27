public class PlayerData
{
    //玩家当前所处的地图编号
    public int mapIndex;

    //玩家的出生位置
    public float x;
    public float y;
    public float z;

    public float maxSpeed = 3;

    //冲刺速度
    public float normalRushSpeed = 11;
    public float superRushSpeed = 18;
    //跳跃速度
    public float normalJumpSpeed = 5.7f;
    public float superJumpSpeed = 8.0f;

    //冲刺计时器
    public float rushTimer = 0;
    //最大冲刺时间
    public float rushMaxTime = 0.15f;

    //人物的方向 -1左 1右
    public int dir = 1;

    //判断能否跳跃
    public bool canJump = true;
    //判断能否冲刺
    public bool canRush = true;
    //能否游泳
    public bool canSwim = false;
    //重力信号
    public int gravityTrans = 1;
    public int flagGravity = 0;
    //死亡信号
    public bool isDead = false;
    //重生信号
    public bool isBirth = false;
    //弹力buff计时器
    public float elasticTimer = 0.0f;
    //持有buff列表
    public BuffStructure buff = new BuffStructure();
    //key数目
    public int numOfKey = 0;
    //笑脸数目
    public int numOfFace = 0;
    //死亡次数
    public int numOfDeath = 0;

    public void setPlayerVector3DPositionData(double x,double y, double z)
    {
        this.x = (float)x;
        this.y = (float)y;
        this.z = (float)z;
    }

    public void initJump()
    {
        this.canJump = true;
    }
    public void initRush()
    {
        this.canRush = true;
    }
}
