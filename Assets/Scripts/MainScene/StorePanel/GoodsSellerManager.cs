using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

public class GoodsSellerManager : GameElementManager {

    public GameObject resourceManager;
    public GameObject HRManager;
    public GameObject battleUnitManager;

    Dictionary<StorePattern, List<GameElement>> dict = new Dictionary<StorePattern, List<GameElement>>();

    public List<GameElement> resourceList {

        get {
            return dict[StorePattern.resource];
        }
    }// = new List<GameElement>();
    public List<GameElement> buildList {
        get {
            return dict[StorePattern.build];
        }
    }// = new List<GameElement>();
    public List<GameElement> battleUnitList {
        get {
            return dict[StorePattern.battle];
        }
    } //= new List<GameElement>();

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

    public override void Awake() {
        base.Awake();

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

    public override List<GameElement> GetDataList(string name)
    {
        if (name == DEFAULT_DATA_LIST_NAME)
        {
            return dict[storePattern];
        }
        else {
            StorePattern p = (StorePattern)Enum.Parse(typeof(StorePattern), name);
            return dict[p];
        }
    }

    public override void SetDataList(string name, List<GameElement> dataList)
    {
        if (name == DEFAULT_DATA_LIST_NAME)
        {
            if (dict.ContainsKey(storePattern))
            {
                dict.Remove(storePattern);
            }
            dict.Add(storePattern, dataList);
        }
        else {
            StorePattern p = (StorePattern)Enum.Parse(typeof(StorePattern), name);
            if (dict.ContainsKey(p))
            {
                dict.Remove(p);
            }
            dict.Add(p, dataList);
        }
    }

    public override void SaveData() {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        foreach (StorePattern p in Enum.GetValues(typeof(StorePattern)))
        {
            using (FileStream fs = File.Create(GetFileName(p)))
            {
                binFormat.Serialize(fs, GetDataList(p.ToString()));
            }
        }
    }

    public override void LoadData()
    {
        foreach (StorePattern p in Enum.GetValues(typeof(StorePattern)))
        {
            string path = GetFileName(p);
            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    List<GameElement> dataList = (List<GameElement>)binFormat.Deserialize(fs);
                    SetDataList(p.ToString(), dataList);
                }
            }
            else {
                Debug.LogError("File " + path +" doesn't exisit");
            }
        }
    }

    private string GetFileName(StorePattern p) {
        return DIRECTORY_NAME + "/" + GetType().Name +"_"+ p.ToString()+ SUFFIXE;
    }

    public override void LoadDataFirstTime()
    {
        dict.Clear();
        dict.Add(StorePattern.resource, new List<GameElement>());
        dict.Add(StorePattern.build, new List<GameElement>());
        dict.Add(StorePattern.battle, new List<GameElement>());

        Good g = new Good("芯片", 1, true, true, "just a box");
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
        g = new Good("居住地", 1, true, true, "简陋的工人居住的地方");
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
        g.item = new BattleUnit("陆战队", "大量且廉价的战士", true, 10f, 1f, 1f, 1f);
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
    }
}
