using UnityEngine;

public class waveEffect : PostEffectBase
{

    //距离系数
    public float distanceFactor = 60.0f;
    //时间系数
    public float timeFactor = -30.0f;
    //sin函数结果系数
    public float totalFactor = 1.0f;

    //波纹宽度
    public float waveWidth = 0.3f;
    //波纹扩散的速度
    public float waveSpeed = 0.3f;

    private float waveStartTime;
    private Vector4 startPos = new Vector4(0.5f, 0.5f, 0, 0);

    //产生波纹的位置
    private Vector3 pos;

    //产生波纹的标志
    private bool isWave = false;


    void Start()
    {
        EventCenter.AddListener<Vector3>(MyEventType.WAVE, setWave);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //计算波纹移动的距离，根据enable到目前的时间*速度求解
        float curWaveDistance = (Time.time - waveStartTime) * waveSpeed;
        //设置一系列参数
        _Material.SetFloat("_distanceFactor", distanceFactor);
        _Material.SetFloat("_timeFactor", timeFactor);
        _Material.SetFloat("_totalFactor", totalFactor);
        _Material.SetFloat("_waveWidth", waveWidth);
        _Material.SetFloat("_curWaveDis", curWaveDistance);
        _Material.SetVector("_startPos", startPos);
        Graphics.Blit(source, destination, _Material);
    }

    void Update()
    {
        if (isWave)
        {
            //将mousePos转化为（0，1）区间
            startPos = new Vector4(Camera.main.WorldToScreenPoint(pos).x / Screen.width, Camera.main.WorldToScreenPoint(pos).y / Screen.height, 0, 0);
            waveStartTime = Time.time;
            isWave = false;
           // Debug.Log("Wave");
        }
        if (Input.GetMouseButton(0))
        {
            
            Vector2 mousePos = Input.mousePosition;
            //将mousePos转化为（0，1）区间
            startPos = new Vector4(mousePos.x / Screen.width, mousePos.y / Screen.height, 0, 0);
       
            waveStartTime = Time.time;
        }

    }

    void setWave(Vector3 pos)
    {
        this.pos = pos;
        isWave = true;
    }
}
