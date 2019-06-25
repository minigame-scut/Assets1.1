using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(LineRenderer))]
//[ExecuteInEditMode]  //普通的类，加上ExecuteInEditMode， 就可以在编辑器模式中运行
public class ChainLightning : MonoBehaviour
{
    public float detail = 0.2f;//1;//增加后，线条数量会减少，每个线条会更长。  
    public float displacement = 1.0f;//15;//位移量，也就是线条数值方向偏移的最大值  
    private Transform EndPostion;//链接目标  
    private Transform StartPosition;
    public Transform[] Positons;
    public float yOffset = 0;
    private LineRenderer lineRender;
    private List<Vector3> linePosList;
    private int CD = 20;
    private int timer ;
    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        linePosList = new List<Vector3>();
        timer = CD;
    }
    private void Update()
    {
        if (timer >= CD)
        {
            if (transform.GetComponent<LineRenderer>().enabled)
                transform.GetComponent<LineRenderer>().enabled = false;
            return;
        }
            
        timer++;
        //判断是否暂停，未暂停则进入分支
        if (Time.timeScale != 0)
        {
            linePosList.Clear();
            for (int k = 0; k < Positons.Length - 1; k++)
            {
                StartPosition = Positons[k];
                EndPostion = Positons[k + 1];
                Vector3 startPos = Vector3.zero;
                Vector3 endPos = Vector3.zero;
                if (EndPostion != null)
                {
                    endPos = EndPostion.position + Vector3.up * yOffset;
                }
                if (StartPosition != null)
                {
                    startPos = StartPosition.position + Vector3.up * yOffset;
                }
                //获得开始点与结束点之间的随机生成点
                CollectLinPos(startPos, endPos, displacement);
                linePosList.Add(endPos);
                //把点集合赋给LineRenderer
                lineRender.positionCount = linePosList.Count;
                for (int i = 0, n = linePosList.Count; i < n; i++)
                {
                    lineRender.SetPosition(i, linePosList[i]);
                }
            }

        }
    }
    //收集顶点，中点分形法插值抖动  
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        //递归结束的条件
        if (displace < detail)
        {
            linePosList.Add(startPos);
        }
        else
        {
            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;
            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;
            Vector3 midPos = new Vector3(midX, midY, midZ);
            //递归获得点
            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }

    public  void Play()
    {
        timer = 0;
        transform.GetComponent<LineRenderer>().enabled = true;
    }
    public bool isPlaying()
    {
        return timer < CD;
    }
}