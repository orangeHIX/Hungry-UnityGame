using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

public class HRManagerScript : GameElementManager {


    public List<GameElement> currStaffList = new List<GameElement>();
    //public List<GameElement> currHRList = new List<GameElement>();
    //public List<GameElement> currNextHarVolList = new List<GameElement>();
    public GameObject resourceManager;

    ResourceManagerScript resourceManagerScript;
    private int humanNum;
    private int humanMaxNum;


    Dictionary<string, int> harvestDict = new Dictionary<string, int>();
    List<string> harvestDictKeyList = new List<string>();

    NextHarvestManager nextHarvestManager;

    public int HRNum
    {
        get
        {
            return humanNum;
        }
    }

    public int HRMaxNum
    {
        get
        {
            return humanMaxNum;
        }
    }

    // Use this for initialization
    void Awake () {

        humanMaxNum = 100;
        humanNum = 10;

        Staff s1 = new Staff("farmer",10,true);
        s1.consumeList = new List<GameElement>();
        s1.produceList = new List<GameElement>();
        s1.produceList.Add(new GameElement("food",1));

        Staff s2 = new Staff("worker",0,false);
        s2.consumeList = new List<GameElement>();
        s2.consumeList.Add(new GameElement("food", 2));
        s2.produceList = new List<GameElement>();
        s2.produceList.Add(new GameElement("iron", 1));

        Staff s3 = new Staff("fisher", 2, true);
        s3.consumeList = new List<GameElement>();
        s3.produceList = new List<GameElement>();
        s3.produceList.Add(new GameElement("fish", 5));



        currStaffList.Clear();
        currStaffList.Add(s1);
        currStaffList.Add(s2);
        currStaffList.Add(s3);

        //currHRList.Clear();
        //currHRList.Add(new GameElement("farmer", 10, true));
        //currHRList.Add(new GameElement("worker", 0, true));
        //currHRList.Add(new GameElement("fisher", 5, true));

        //InitNextHarVolList();
    }

    //public void InitNextHarVolList()
    //{
    //    currNextHarVolList.Clear();
    //    currNextHarVolList.Add(new GameElement("food", 0, false));
    //    currNextHarVolList.Add(new GameElement("iron", 0, false));
    //    currNextHarVolList.Add(new GameElement("fish", 0, false));
    //    currNextHarVolList.Add(new GameElement("gold", 0, false));
    //    currNextHarVolList.ForEach(x => { x.value = 0; x.enabled = false; });
    //    currStaffList.ForEach(x => changeRelatedNextHar(x as Staff, x.value));
    //}

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        if(resourceManager != null) {
            resourceManagerScript = resourceManager.GetComponent<ResourceManagerScript>();
            if (resourceManagerScript != null) {
                harvestDict.Clear();
                harvestDictKeyList.Clear();
                resourceManagerScript.currResList.ForEach(res => harvestDict.Add(res.name, 0));
                harvestDictKeyList.AddRange(harvestDict.Keys);
            }
        }
        nextHarvestManager = GetComponent<NextHarvestManager>();

        currStaffList.ForEach(x => ChangeRelatedNextHar(x as Staff, x.value));
    }

    bool couldChangeHRNum(int change)
    {
        if (humanNum + change < 0)
        {
            return false;
        }
        else if (humanNum + change > humanMaxNum)
        {
            return false;
        }
        else {
            return true;
        }
    }

    public void Harvest() {
        ResetHarvestDict();
        foreach (GameElement staff in currStaffList)
        {
            Staff s = staff as Staff;

            int minTimes = resourceManagerScript.GetTimesResourceListCouldbeConsumed(s.consumeList);
            
            int onJob = minTimes < s.value ? minTimes : s.value;
            if (minTimes > 0)
            {
                bool flag = false;
                foreach (GameElement consume in s.consumeList)
                {
                    if (resourceManagerScript.TryChangeGameElementValue(consume.name, -consume.value * onJob)) {
                        flag = true;
                        harvestDict[consume.name] += -consume.value * onJob;
                    }
                }
                foreach (GameElement produce in s.produceList) {
                    if (resourceManagerScript.TryChangeGameElementValue(produce.name, produce.value * onJob)) {
                        flag = true;
                        harvestDict[produce.name] += produce.value * onJob;
                    }
                }
                if (flag)
                {
                    resourceManagerScript.Notify();
                }
            }
        }
        HarvestInfo();
    }

    void ResetHarvestDict() {
        foreach (string key in harvestDictKeyList) {
            harvestDict[key] = 0;
        }
    }

    StringBuilder harvestInfoStringBuilder = new StringBuilder(50);
    void HarvestInfo() {
        harvestInfoStringBuilder.Remove(0, harvestInfoStringBuilder.Length);
        foreach (string key in harvestDictKeyList)
        {
            int value = 0;
            harvestDict.TryGetValue(key,out value);
            if (value != 0) {
                harvestInfoStringBuilder.Append(key).Append(" ");
                harvestInfoStringBuilder.Append(value.ToString("+#;-#;0"));
                harvestInfoStringBuilder.Append(" ");
            }
        }
        if (harvestInfoStringBuilder.Length > 1)
        {
            harvestInfoStringBuilder.Remove(harvestInfoStringBuilder.Length - 1, 1);
            string info = harvestInfoStringBuilder.ToString();
            Toast.Pop(info);
            GameEventPublisher.Publish(info);
        }
    }

    public bool TryAddMaxHRNum(int num) {
        if (num > 0)
        {
            humanMaxNum = humanMaxNum + num;
            return true;
        }
        return false;
    }

    public override bool TryChangeGameElementValue(string name, int change)
    {
        if (couldChangeHRNum(-change))
        {
            GameElement staff = currStaffList.Find(x => x.name == name);
            if (staff != null)
            {
                if (staff.value + change >= 0 && staff.value + change <= humanMaxNum)
                {
                    changeHRValue(staff as Staff, change);
                    //Notify();
                    return true;
                }
            }
        }
        return false;
    }

    private void changeHRValue(Staff staff, int change)
    {
        staff.value += change;
        humanNum -= change;
        ChangeRelatedNextHar(staff, change);
    }

    void ChangeRelatedNextHar(Staff staff, int change)
    {
        if (nextHarvestManager != null)
        {
            foreach (GameElement con in staff.consumeList)
            {
                nextHarvestManager.TryChangeGameElementValue(con.name, -con.value * change);
                //GameElement nextHar = currNextHarVolList.Find(x => x.name == con.name);
                //if (nextHar != null)
                //{
                //    nextHar.value -= con.value * change;
                //}
            }
            foreach (GameElement pro in staff.produceList)
            {
                nextHarvestManager.TryChangeGameElementValue(pro.name, pro.value * change);
                //GameElement nextHar = currNextHarVolList.Find(x => x.name == pro.name);
                //if (nextHar != null)
                //{
                //    nextHar.value += pro.value * change;
                //}
            }
            List<GameElement> currNextHarVolList = nextHarvestManager.GetDataList();
            currNextHarVolList.ForEach(ge => ge.enabled = ge.value != 0);
            nextHarvestManager.Notify();
        }
    }
    //private void changeRelatedNextHar(Staff staff, int change)
    //{
    //    foreach (GameElement con in staff.consumeList)
    //    {
    //        GameElement nextHar = currNextHarVolList.Find(x => x.name == con.name);
    //        if (nextHar != null)
    //        {
    //            nextHar.value -= con.value * change;
    //        }
    //    }
    //    foreach (GameElement pro in staff.produceList)
    //    {
    //        GameElement nextHar = currNextHarVolList.Find(x => x.name == pro.name);
    //        if (nextHar != null)
    //        {
    //            nextHar.value += pro.value * change;
    //        }
    //    }
    //    currNextHarVolList.ForEach(ge => ge.enabled = ge.value != 0);
    //}

    public override List<GameElement> GetDataList(string name)
    {
        return currStaffList;
    }
}
