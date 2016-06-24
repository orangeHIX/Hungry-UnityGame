using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NextHarvestManager : GameElementManager {

    public List<GameElement> currNextHarVolList = new List<GameElement>();

    public override void Awake() {
        base.Awake();
        InitNextHarVolList();
    }

    public void InitNextHarVolList()
    {
        currNextHarVolList.Clear();
        currNextHarVolList.Add(new GameElement("food", 0, false));
        currNextHarVolList.Add(new GameElement("iron", 0, false));
        currNextHarVolList.Add(new GameElement("fish", 0, false));
        currNextHarVolList.Add(new GameElement("gold", 0, false));
        currNextHarVolList.ForEach(x => { x.value = 0; x.enabled = false; });
    }




    // Use this for initialization
    public override void Start () {
        base.Start();
    }
	
    public override List<GameElement> GetDataList(string name)
    {
        return currNextHarVolList;
    }

    public override void SetDataList(string name, List<GameElement> dataList)
    {
        currNextHarVolList = dataList;
    }
}
