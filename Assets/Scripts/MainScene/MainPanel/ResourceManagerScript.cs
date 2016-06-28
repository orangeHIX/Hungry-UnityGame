using UnityEngine;
using System.Collections.Generic;
using System;

public class ResourceManagerScript : GameElementManager
{

    public List<GameElement> currResList = new List<GameElement>();


    // Awake is called when the script instance is being loaded
    public override void Awake()
    {
        base.Awake();
    }
    //public bool CouldChangeResource(string name, int change)
    //{
    //    GameElement ge = currResList.Find(res => res.name == name);
    //    if (ge != null)
    //    {
    //        if (ge.value + change >= 0)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public int GetTimesResourceListCouldbeConsumed(List<GameElement> consumeList) {

        int minTimes = Int32.MaxValue;
        foreach (GameElement consume in consumeList)
        {
            int times = GetTimesResourceCouldbeConsumed(consume.name, consume.value);
            if (times < minTimes)
            {
                minTimes = times;
            }
        }
        return minTimes;
    }

    public int GetTimesResourceCouldbeConsumed(string name, int consume) {
        GameElement ge = currResList.Find(res => res.name == name);
        if (ge == null)
            return 0;
        if (consume > 0)
        {
            int times = ge.value / consume;
            if (times >= 0)
            {
                return times;
            }
            return 0;
        }
        else
        {
            return Int32.MaxValue;
        }

    }

    //public bool TryChangeGameResource(string name, int change)
    //{
    //    return TryChangeGameElementValue(name, change);
    //}

    public void AddGameResource(string name)
    {
        if (TryChangeGameElementValue(name, 1)) {
            Notify();
        }
    }

    //public override bool TryChangeGameElementValue(string name, int change)
    //{
    //    GameElement ge = currResList.Find(x => x.name == name);
    //    if (ge != null)
    //    {
    //        if (CouldChangeGameElementValue(ge, change))
    //        {
    //            ge.value += change;
    //            Notify();
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public override List<GameElement> GetDataList(string name)
    {
        return currResList;
    }

    public override void SetDataList(string name, List<GameElement> dataList)
    {
        currResList = dataList;
    }

    public override void LoadDataFirstTime()
    {
        currResList.Clear();
        currResList.Add(new GameElement("资金", 100, true));
        currResList.Add(new GameElement("食物", 55, true));
        currResList.Add(new GameElement("能源", 33, true));
        currResList.Add(new GameElement("合金", 22, true));

        currResList.Add(new GameElement("芯片", 0, false));
        currResList.Add(new GameElement("生命药品", 0, false));
        currResList.Add(new GameElement("超合金", 0, false));
    }
}
