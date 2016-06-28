using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NextHarvestManager : GameElementManager {

    public List<GameElement> currNextHarVolList = new List<GameElement>();

    public override List<GameElement> GetDataList(string name)
    {
        return currNextHarVolList;
    }

    public override void SetDataList(string name, List<GameElement> dataList)
    {
        currNextHarVolList = dataList;
    }

    public override void LoadDataFirstTime()
    {
        currNextHarVolList.Clear();
        currNextHarVolList.Add(new GameElement("资金", 0, false));
        currNextHarVolList.Add(new GameElement("食物", 0, false));
        currNextHarVolList.Add(new GameElement("能源", 0, false));
        currNextHarVolList.Add(new GameElement("合金", 0, false));
        currNextHarVolList.ForEach(x => { x.value = 0; x.enabled = false; });
    }

    public override bool needPersistence()
    {
        return false;
    }
}
