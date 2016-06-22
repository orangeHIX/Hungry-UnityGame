using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

public class GoodsSellerManager : GameElementManager {

    public GameObject resourceManager;
    public GameObject HRManager;
    public GameObject battleUnitManager;
    public List<GameElement> resourceList =new List<GameElement>();
    public List<GameElement> buildList = new List<GameElement>();
    public List<GameElement> battleUnitList = new List<GameElement>();
    public StorePattern storePattern {
        set {
            pattern = value;
            Notify();
        }
        get {
            return pattern;
        }
    }

    ResourceManagerScript resourceManagerScript;
    HRManagerScript HRManagerScript;
    BattleUnitManager battleUnitManagerScript;

    StorePattern pattern;

    public void Awake() {
        Good g = new Good("芯片",1,true, true,"just a box");
        g.materialList.Add(new GameElement("gold", 5));
        g.item = new GameElement("芯片", 1, true);
        resourceList.Add(g);

        g = new Good("生命药品*2", 1, false, true, "just a box");
        g.materialList.Add(new GameElement("fish", 2));
        g.materialList.Add(new GameElement("gold", 6));
        g.item = new GameElement("生命药品", 2, true);
        resourceList.Add(g);

        g = new Good("超合金", 1, true, false, "just a box");
        g.materialList.Add(new GameElement("food", 2));
        g.materialList.Add(new GameElement("iron", 5));
        g.materialList.Add(new GameElement("fish", 1));
        g.materialList.Add(new GameElement("gold", 6));
        g.item = new GameElement("超合金", 1, true);
        resourceList.Add(g);

        //=================================================================
        g = new Good("居住地",1,true,true,"简陋的工人居住的地方");
        g.materialList.Add(new GameElement("gold", 10));
        g.item = new Shelter("居住地", 1, true, 10);
        buildList.Add(g);


        g = new Good("冶炼厂", 1, false, true, "just a factory");
        g.materialList.Add(new GameElement("gold", 10));
        g.item = new ProductionBuilding("冶炼厂", 1, true, "worker");
        buildList.Add(g);

        //=================================================================
        g = new Good("陆战队", 1, true, true, "大量且廉价的战士");
        g.materialList.Add(new GameElement("gold", 5));
        g.item = new BattleUnit("陆战队", "大量且廉价的战士", true, 10f,1f,1f,1f);
        battleUnitList.Add(g);

        g = new Good("狙击手", 1, true, true, "训练有素的幽灵部队");
        g.materialList.Add(new GameElement("gold", 6));
        g.materialList.Add(new GameElement("fish", 10));
        g.item = new BattleUnit("狙击手", "训练有素的幽灵部队", true, 10f, 1f, 1f, 2f);
        battleUnitList.Add(g);

        g = new Good("医疗兵", 1, true, true, "战场天使");
        g.materialList.Add(new GameElement("gold", 7));
        g.item = new BattleUnit("医疗兵", "战场天使", true, 10f, 1f, 1f, 1f);
        battleUnitList.Add(g);


        pattern = StorePattern.resource;
    }

    public override void Start()
    {
        base.Start();
        if (resourceManager != null) {
            resourceManagerScript = resourceManager.GetComponent<ResourceManagerScript>();
        }
        if (HRManager != null) {
            HRManagerScript = HRManager.GetComponent<HRManagerScript>();
        }
        if (battleUnitManager != null) {
            battleUnitManagerScript = battleUnitManager.GetComponent<BattleUnitManager>();
        }
    }

    public override List<GameElement> GetDataList(string name)
    {
        switch (storePattern) {
            case StorePattern.resource:
                return resourceList;
            case StorePattern.battle:
                return battleUnitList;
            case StorePattern.build:
                return buildList;
        }
        return null;
    }

    public override bool TryChangeGameElementValue(string name, int change)
    {
        if (change < 0)
        {
            Good ele = GetDataList().Find(x => x.name == name) as Good;
            if (ele != null)
            {
                if (change >= 0)
                    return false;

                if (Sell(ele, change))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool Sell( Good good, int change) {

        if (good.value + change < 0)
        {
            Toast.Pop("缺少"+good.name);
            return false;
        }

        int minTimes = resourceManagerScript.GetTimesResourceListCouldbeConsumed(good.materialList);
        //Debug.Log("minTimes: " + minTimes);
        if (minTimes >= -change)
        {

            Charge(good, -change);
            Give(good, -change);
            ChangeStore(good, change);
            return true;
        }
        Toast.Pop("你的资源不足");
        return false;
    }

    void Charge( Good good, int num) {
        bool resourceManagerScriptFlag = false;
        foreach (GameElement consume in good.materialList)
        {
            if (resourceManagerScript.TryChangeGameElementValue(consume.name, -consume.value * num))
            {
                resourceManagerScriptFlag = true;
            }
        }
        if (resourceManagerScriptFlag)
        {
            resourceManagerScript.Notify();
        }
    }

    void Give(Good good, int num) {
        if (storePattern == StorePattern.resource)
        {
            if (resourceManagerScript.TrySetGameElementEnable(good.item.name, true)){
                if (resourceManagerScript.TryChangeGameElementValue(good.item.name, num * good.item.value))
                {
                    resourceManagerScript.Notify();
                    Toast.Pop("你买了" + good.item.name + " * " + num * good.item.value);
                }
            }
            else
            {
                resourceManagerScript.TrySetGameElementEnable(good.item.name, false);
            }
        }
        else if (storePattern == StorePattern.build)
        {
            if (good.item is Shelter)
            {
                if (HRManagerScript.TryAddMaxHRNum((good.item as Shelter).capacity * num))
                {
                    HRManagerScript.Notify();
                    Toast.Pop("你买了" + good.item.name + " * " + num);
                    GameEventPublisher.Publish(( (Building)good.item ).activatingBuildingInfo);
                }
            }
            if (good.item is ProductionBuilding) {
                ProductionBuilding building = good.item as ProductionBuilding;
                if (HRManagerScript.TrySetGameElementEnable(building.staffName, true)) {
                    HRManagerScript.Notify();
                    Toast.Pop("你买了" + good.item.name + " * " + num);
                    GameEventPublisher.Publish(((Building)good.item).activatingBuildingInfo);
                }
            }
        }
        else if (storePattern == StorePattern.battle)
        {
            if (good.item is BattleUnit) {
                if (battleUnitManagerScript.TryChangeMilitaryCamp(good.item.name, num ))
                {
                    battleUnitManagerScript.Notify();
                    Toast.Pop("你买了" + good.item.name + " * " + num);
                }
            }
        }
    }

    void ChangeStore(Good good, int change) {

        if (!good.isInfinite)
        {
            good.value = good.value + change;
        }

        if (good.item is Shelter) {
            (good.item as Shelter).value += 1;  //add one shelter
        }
        if (good.item is ProductionBuilding)
        {
            (good.item as ProductionBuilding).enabled = false;  //Allow only one exists
        }

    }

    //StringBuilder stringBuilder = new StringBuilder(50);
    //string GetMaterialListString( Good good)
    //{
    //    stringBuilder.Remove(0, stringBuilder.Length);
    //    string delimiter = " ";
    //    foreach (GameElement ele in good.materialList)
    //    {
    //            stringBuilder.Append(ele.name).Append(" * ");
    //            stringBuilder.Append(ele.value.ToString("+#;-#;0"));
    //            stringBuilder.Append(delimiter);

    //    }
    //    if (stringBuilder.Length > 1)
    //    {
    //        stringBuilder.Remove(stringBuilder.Length - delimiter.Length, delimiter.Length);
    //        return stringBuilder.ToString();
    //    }
    //    return "";
    //}
}
