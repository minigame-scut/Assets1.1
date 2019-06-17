using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStructure
{
    private List<Buff> buffList = new List<Buff>();
    public void add(Buff newBuff)
    {
        if (buffList.Contains(newBuff))
            return;
        else
            buffList.Add(newBuff);
    }
    public void remove(Buff newBuff)
    {
        if (buffList.Contains(newBuff))
            buffList.Remove(newBuff);
    }
    public bool contains(Buff newBuff)
    {
        return buffList.Contains(newBuff);
    }
}
