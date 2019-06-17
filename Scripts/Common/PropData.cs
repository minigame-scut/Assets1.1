using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*用于存储每个关卡的道具的映射关系，管理对应关卡的道具的启用，
 * 方便玩家收集道具后对道具进行管理 ，对应的Vector4数值对的意义是：
 * (关卡编号，地图编号，钥匙是否启用（-1表示没有，0表示禁用，1表示启用）， 笑脸是否启用（-1表示没有，0表示禁用，1表示启用）)
 */
public class PropData
{
    public List<Vector4> collectionMap = new List<Vector4>();
    public PropData()
    {
        collectionMap.Add(new Vector4(1, 1, 1, -1));
        collectionMap.Add(new Vector4(1, 2, -1, 1));
        collectionMap.Add(new Vector4(1, 7, 1, 1));
    }
    public void setKeyTrue(int numOfBattle, int numOfMap)
    {
        for (int i = 0; i < collectionMap.Count; i++)
        {
            if (collectionMap[i].x == numOfBattle && collectionMap[i].y == numOfMap)
            {
                if(collectionMap[i].z != -1)
                {
                    collectionMap[i] = new Vector4(numOfBattle, numOfMap, 1, collectionMap[i].w);
                    break;
                }
            }
                
        }
    }
    public void setKeyFalse(int numOfBattle, int numOfMap)
    {
        for (int i = 0; i < collectionMap.Count; i++)
        {
            if (collectionMap[i].x == numOfBattle && collectionMap[i].y == numOfMap)
            {
                if (collectionMap[i].z != -1)
                {
                    collectionMap[i] = new Vector4(numOfBattle, numOfMap, 0, collectionMap[i].w);
                    break;
                }
            }
        }
    }
    public void setFaceTrue(int numOfBattle, int numOfMap)
    {
        for (int i = 0; i < collectionMap.Count; i++)
        {
            if (collectionMap[i].x == numOfBattle && collectionMap[i].y == numOfMap)
            {
                if (collectionMap[i].w != -1)
                {
                    collectionMap[i] = new Vector4(numOfBattle, numOfMap, collectionMap[i].z, 1);
                    break;
                }
            }
        }
    }
    public void setFaceFalse(int numOfBattle, int numOfMap)
    {
        for (int i = 0; i < collectionMap.Count; i++)
        {
            if (collectionMap[i].x == numOfBattle && collectionMap[i].y == numOfMap)
            {
                if (collectionMap[i].w != -1)
                {
                    collectionMap[i] = new Vector4(numOfBattle, numOfMap, collectionMap[i].z, 0);
                    break;
                }
            }
        }
    }
    
}
