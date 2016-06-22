using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleUnitManager : GameElementManager
{
    public List<GameElement> unitList = new List<GameElement>();
    public int maxTeamNum;
    public int currTeamNum {
        get {
            int num = 0;
            unitList.ForEach(
                ele =>
                    num += ele.value
            );
            return num;
        }
    }

    //public List<GameElement> currTeamList = new List<GameElement>();
    //public List<GameElement> militaryCamp = new List<GameElement>();

    public void Awake()
    {
        //currTeamList.Add(new GameElement("soldier1", 1));
        //currTeamList.Add(new GameElement("soldier2", 2));
        //currTeamList.Add(new GameElement("soldier3", 1));
        //currTeamList.Add(new GameElement("soldier4", 0));

        //militaryCamp.Add(new GameElement("soldier1",1));
        //militaryCamp.Add(new GameElement("soldier2", 2));
        //militaryCamp.Add(new GameElement("soldier3", 1));
        //militaryCamp.Add(new GameElement("soldier4",2));
        unitList.Clear();

        BattleUnit b = new BattleUnit("陆战队", "大量且廉价的战士", true, 10f, 1f, 1f, 1f);
        unitList.Add(new RosterItem(b, 1, 1, true));

        b = new BattleUnit("狙击手", "训练有素的幽灵部队", true, 10f, 1f, 1f, 2f);
        unitList.Add(new RosterItem(b, 0, 1, true));

        b = new BattleUnit("医疗兵", "战场天使", true, 10f, 1f, 1f, 1f);
        unitList.Add(new RosterItem(b, 1, 2, true));

        maxTeamNum = 4;

        //foreach (GameElement g in currTeamList) {
        //    if (g.value > getBattleUnitNum(g.name))
        //    {
        //        throw new Exception("element "+g.name+" in currTeamList is consistent with militaryCamp data");
        //    }
        //}
    }

    public override bool TryChangeGameElementValue(string name, int change)
    {
        RosterItem ele = unitList.Find(x => x.name == name) as RosterItem;
        if (ele != null) {
            int newValue = ele.value + change;
            if (currTeamNum + change > maxTeamNum )
            {
                Toast.Pop("队伍无法容纳更多的人");
            }
            else if (newValue > ele.maxValue) {
                Toast.Pop("你缺少" + ele.name);
            } else{
                ele.value = newValue;
                //Notify();
                return true;
            }
        }
        return false;
    }

    public bool TryChangeMilitaryCamp(string name, int change) {
        RosterItem ele = unitList.Find(x => x.name == name) as RosterItem;
        if (ele != null)
        {
            int newMaxValue = ele.maxValue + change;
            if (newMaxValue >= 0)
            {
                ele.maxValue = newMaxValue;
                return true;
            }
        }
        return false;
    }


    public override List<GameElement> GetDataList(string name)
    {
        return unitList;
    }
}
