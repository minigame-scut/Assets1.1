using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : MonoBehaviour {

    private static Dictionary<MyEventType, Delegate> m_EventTable = new Dictionary<MyEventType, Delegate>();


    private static void OnAddListener(MyEventType MyEventType, Delegate callBack) {

        if (!m_EventTable.ContainsKey(MyEventType))
        {       //没有该类型的数据
            m_EventTable.Add(MyEventType, null);

        }
        Delegate d = m_EventTable[MyEventType];
        if (d != null && d.GetType() != callBack.GetType())
        { //类型不一致
            throw new Exception(string.Format("尝试为事件{0}添加不一致的委托，当前事件所对应的委托为{1}, 添加的委托事件为{2}", MyEventType, d.GetType(), callBack.GetType()));

        }
    }


    //无参的监听
    public static void AddListener(MyEventType MyEventType, CallBack callBack) {

        OnAddListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack)m_EventTable[MyEventType] + callBack;

    }
    //一个参数
    public static void AddListener<T>(MyEventType MyEventType, CallBack<T> callBack)
    {
        OnAddListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T>)m_EventTable[MyEventType] + callBack;

    }
    //2个参数
    public static void AddListener<T, Y>(MyEventType MyEventType, CallBack<T,Y> callBack)
    {
        OnAddListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T, Y>)m_EventTable[MyEventType] + callBack;

    }
    public static void AddListener<T, Y ,X>(MyEventType MyEventType, CallBack<T, Y, X> callBack)
    {
        OnAddListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T, Y, X>)m_EventTable[MyEventType] + callBack;

    }
    public static void AddListener<T, Y, X, Z>(MyEventType MyEventType, CallBack<T, Y, X, Z> callBack)
    {
        OnAddListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T, Y, X, Z>)m_EventTable[MyEventType] + callBack;

    }


    private static void  OnRemoveListener(MyEventType MyEventType, Delegate callBack) {
        if (m_EventTable.ContainsKey(MyEventType))
        {
            Delegate d = m_EventTable[MyEventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", MyEventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前事件类型为{1}，要移除的为{2}", MyEventType, d.GetType(), callBack.GetType()));

            }

        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", MyEventType));
        }

    }

    private static void OnRemoveListenered(MyEventType MyEventType)
    {
        if (m_EventTable[MyEventType] == null)
        {
            m_EventTable.Remove(MyEventType);
        }

    }

    //无参的监听移除
    public static void RemoveListenter(MyEventType MyEventType, CallBack callBack) {
        OnRemoveListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack)m_EventTable[MyEventType] - callBack;
        OnRemoveListenered(MyEventType);

    }
    //一个参数的监听移除
    public static void RemoveListenter<T>(MyEventType MyEventType, CallBack<T> callBack)
    {
        OnRemoveListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T>)m_EventTable[MyEventType] - callBack;
        OnRemoveListenered(MyEventType);

    }
    public static void RemoveListenter<T, X>(MyEventType MyEventType, CallBack<T, X> callBack)
    {
        OnRemoveListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T, X>)m_EventTable[MyEventType] - callBack;
        OnRemoveListenered(MyEventType);

    }
    public static void RemoveListenter<T, X, Y>(MyEventType MyEventType, CallBack<T, X, Y> callBack)
    {
        OnRemoveListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T, X, Y>)m_EventTable[MyEventType] - callBack;
        OnRemoveListenered(MyEventType);

    }
    public static void RemoveListenter<T, X, Y, Z >(MyEventType MyEventType, CallBack<T, X, Y, Z> callBack)
    {
        OnRemoveListener(MyEventType, callBack);
        m_EventTable[MyEventType] = (CallBack<T, X, Y, Z>)m_EventTable[MyEventType] - callBack;
        OnRemoveListenered(MyEventType);

    }
    //广播
    public static void Broadcast(MyEventType MyEventType){
        Delegate d;
        if (m_EventTable.TryGetValue(MyEventType, out d)) {
            CallBack callBack = d as CallBack; //强制类型转换
            if (callBack != null)
            {
                callBack();
            }
            else {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", MyEventType));
            }

        }
    
    
    }
    //一个参数的广播
    public static void Broadcast<T>(MyEventType MyEventType, T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(MyEventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>; //强制类型转换
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", MyEventType));
            }
        }
    }

    public static void Broadcast<T, X>(MyEventType MyEventType, T arg, X arg2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(MyEventType, out d))
        {
            CallBack<T, X> callBack = d as CallBack<T,X>; //强制类型转换
            if (callBack != null)
            {
                callBack(arg, arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", MyEventType));
            }
        }
    }

    public static void Broadcast<T,X, Y>(MyEventType MyEventType, T arg, X arg2, Y arg3 )
    {
        Delegate d;
        if (m_EventTable.TryGetValue(MyEventType, out d))
        {
            CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>; //强制类型转换
            if (callBack != null)
            {
                callBack(arg, arg2, arg3);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", MyEventType));
            }
        }
    }

    public static void Broadcast<T, X, Y, Z>(MyEventType MyEventType, T arg, X arg2, Y arg3, Z arg4)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(MyEventType, out d))
        {
            CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>; //强制类型转换
            if (callBack != null)
            {
                callBack(arg, arg2, arg3, arg4);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", MyEventType));
            }
        }
    }
}
