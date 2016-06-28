using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;
using System.IO;

public class HRManagerScript : GameElementManager {


    public List<GameElement> currStaffList = new List<GameElement>();
    //public List<GameElement> currHRList = new List<GameElement>();
    //public List<GameElement> currNextHarVolList = new List<GameElement>();
    public GameObject resourceManager;

    ResourceManagerScript resourceManagerScript;
    public int humanNum;
    public int humanMaxNum;


    Dictionary<string, int> harvestDict = new Dictionary<string, int>();
    List<string> harvestDictKeyList = new List<string>();

    NextHarvestManager nextHarvestManager;

    //public int HRNum
    //{
    //    get
    //    {
    //        return humanNum;
    //    }
    //}

    //public int HRMaxNum
    //{
    //    get
    //    {
    //        return humanMaxNum;
    //    }
    //}

    // Use this for initialization
    public override void Awake () {
        base.Awake();



    }

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

    public override void SetDataList(string name, List<GameElement> dataList)
    {
        currStaffList = dataList;
    }

    public override void LoadDataFirstTime()
    {
        currStaffList.Clear();

        Staff s1 = new Staff("农场", 10, true);
        s1.consumeList = new List<GameElement>();
        s1.produceList = new List<GameElement>();
        s1.produceList.Add(new GameElement("食物", 1));

        Staff s2 = new Staff("电厂", 0, true);
        s2.consumeList = new List<GameElement>();
        s2.consumeList.Add(new GameElement("食物", 1));
        s2.produceList = new List<GameElement>();
        s2.produceList.Add(new GameElement("能源", 1));

        Staff s3 = new Staff("冶炼厂", 0, false);
        s3.consumeList = new List<GameElement>();
        s3.consumeList.Add(new GameElement("食物", 1));
        s3.consumeList.Add(new GameElement("能源", 5));
        s3.produceList = new List<GameElement>();
        s3.produceList.Add(new GameElement("合金", 1));


        currStaffList.Clear();
        currStaffList.Add(s1);
        currStaffList.Add(s2);
        currStaffList.Add(s3);

        humanMaxNum = 20;
        int sum = 0;
        currStaffList.ForEach(x=>sum += x.value);
        humanNum = 20 - sum;
    }

    const string ROOT_NODE_NAME = "HR";
    const string HR_NUM_NODE_NAME = "HRNum";
    const string HR_MAX_NUM_NODE_NAME = "HRMaxNum";
    const string XML_FILE_NAME = "HRNum.xml";

    private string GetFullXMLFileName() {
        return Application.persistentDataPath + '/' + DIRECTORY_NAME + '/' + XML_FILE_NAME;
    }

    public override void SaveData()
    {
        base.SaveData();

        XmlDocument doc = new XmlDocument();
        XmlNode rootNode = doc.CreateElement(ROOT_NODE_NAME);

        XmlNode HRNumNode = doc.CreateElement(HR_NUM_NODE_NAME);
        HRNumNode.InnerText = humanNum.ToString();

        XmlNode HRMaxNumNode = doc.CreateElement(HR_MAX_NUM_NODE_NAME);
        HRMaxNumNode.InnerText = humanMaxNum.ToString();

        rootNode.AppendChild(HRNumNode);
        rootNode.AppendChild(HRMaxNumNode);

        doc.AppendChild(rootNode);
        doc.Save(GetFullXMLFileName());

    }

    public override bool LoadData()
    {
        base.LoadData();
        if (File.Exists(GetFullXMLFileName()))
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(GetFullXMLFileName());
            XmlNodeList xnl = doc.SelectSingleNode(ROOT_NODE_NAME).ChildNodes;

            humanNum = int.Parse(xnl.Item(0).InnerText);
            humanMaxNum = int.Parse(xnl.Item(1).InnerText);
            return true;
        }
        return false;
    }

}
