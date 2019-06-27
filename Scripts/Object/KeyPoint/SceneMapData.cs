using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//地图之间的映射关系保存

public class SceneMapData
{
    public static SceneMapData instance;
    //存储映射关系的键值对容器
    private static Dictionary<string, string> mapData;

    private SceneMapData()
    {

    }

    public static SceneMapData getInstance()
    {
        if (instance == null)
        {
            instance = new SceneMapData();
            return instance;
        }
        else
        {
            return instance;
        }
    }

    public void init()
    {
        //if(instance != null)
        //  {
        mapData = new Dictionary<string, string>();

        //关卡映射
        mapData.Add("nextPlace1-1-2", "birthPlace1-2-1");
        mapData.Add("nextPlace1-0-1", "birthPlace1-1-1");
        mapData.Add("nextPlace1-1-3", "birthPlace1-2-2");
        mapData.Add("nextPlace1-2-1", "birthPlace1-1-2");
        mapData.Add("nextPlace1-2-2", "birthPlace1-1-3");
        mapData.Add("nextPlace1-2-3", "birthPlace1-4-1");
        mapData.Add("nextPlace1-2-4", "birthPlace1-3-1");
        mapData.Add("nextPlace1-3-1", "birthPlace1-2-4");
        mapData.Add("nextPlace1-4-1", "birthPlace1-2-3");
        mapData.Add("nextPlace1-3-2", "birthPlace1-6-1");
        mapData.Add("nextPlace1-6-1", "birthPlace1-3-2");
        mapData.Add("nextPlace1-4-2", "birthPlace1-5-1");
        mapData.Add("nextPlace1-6-2", "birthPlace2-0-1");
        mapData.Add("nextPlace2-0-1", "birthPlace2-1-1");
        mapData.Add("nextPlace2-1-1", "birthPlace2-2-1");
        mapData.Add("nextPlace2-2-1", "birthPlace2-3-1");
        mapData.Add("nextPlace2-3-1", "birthPlace2-4-1");
        mapData.Add("nextPlace2-4-1", "birthPlace2-5-1");

        //传送门映射
        mapData.Add("transDoor1-1-1-1", "transDoor1-1-1-2");
        mapData.Add("transDoor1-1-1-2", "transDoor1-1-1-1");
        mapData.Add("transDoor1-1-2-2", "transDoor1-5-2-1");
        mapData.Add("transDoor1-5-2-1", "transDoor1-1-2-2");
        mapData.Add("transDoor1-3-3-2", "transDoor1-7-3-1");
        mapData.Add("transDoor1-7-3-1", "transDoor1-3-3-2");
        mapData.Add("transDoor2-5-1-2", "transDoor2-5-1-1");
        mapData.Add("transDoor2-5-1-1", "transDoor2-5-1-2");


        //里表世界门映射
        mapData.Add("inworldDoor1-3-1", "outworldDoor1-7-1");
        mapData.Add("outworldDoor1-7-1", "inworldDoor1-3-1");
        mapData.Add("inworldDoor1-3-2", "outworldDoor1-7-2");
        mapData.Add("outworldDoor1-7-2", "inworldDoor1-3-2");
        mapData.Add("inworldDoor1-3-3", "outworldDoor1-7-3");
        mapData.Add("outworldDoor1-7-3", "inworldDoor1-3-3");
        mapData.Add("inworldDoor2-3-1", "outworldDoor2-6-1");
        mapData.Add("outworldDoor2-6-1", "inworldDoor2-3-1");
        mapData.Add("inworldDoor2-3-2", "outworldDoor2-6-2");
        mapData.Add("outworldDoor2-6-2", "inworldDoor2-3-2");


        //地图编号与build编号的映射
        mapData.Add("map1-0", "1");
        mapData.Add("map1-1", "2");
        mapData.Add("map1-2", "3");
        mapData.Add("map1-3", "4");
        mapData.Add("map1-4", "5");
        mapData.Add("map1-5", "6");
        mapData.Add("map1-6", "7");
        mapData.Add("map1-7", "8");
        mapData.Add("map2-0", "9");
        mapData.Add("map2-1", "10");
        mapData.Add("map2-2", "11");
        mapData.Add("map2-3", "12");
        mapData.Add("map2-4", "13");
        mapData.Add("map2-5", "14");
        mapData.Add("map2-6", "15");
        mapData.Add("map3-0", "16");
        mapData.Add("map3-1", "17");
        mapData.Add("map3-2", "18");
        mapData.Add("map3-3", "19");
        mapData.Add("map3-4", "20");
        mapData.Add("map3-5", "21");
        mapData.Add("map3-6", "22");


    }
    //获得这个映射数据
    public Dictionary<string, string> getMapData()
    {
        return mapData;
    }
    //修改映射关系
    public void addMapData(string key, string value)
    {
        //已有这个key
        if (mapData.ContainsKey(key))
        {
            mapData[key] = value;
        }
        else//没有这个key，新建一个key
        {
            mapData.Add(key, value);
        }
    }

    //删除映射关系
    public void removeMapData(string key)
    {
        //已有这个key
        if (mapData.ContainsKey(key))
        {
            mapData.Remove(key);
        }
        else
        {
            Debug.Log("error_noSuchKeyAndValue");
        }
    }
    public string getTransName(string name)
    {
        if (mapData.ContainsKey(name))
            return mapData[name];
        return null;
    }



}
