using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Xml;

public interface ISubject
{
    void Attach(IDataChangeListener listener);
    void Dettach(IDataChangeListener listener);
    void DettachAll();
    void Notify();
}

public interface IPersist
{
    void SaveData();
    void LoadData();
    void LoadDataFirstTime();
    string GetFileName();
}


public abstract class GameElementManager : PersistModel, ISubject
{

    //public static bool LOAD_ORIGINAL_DATA = false;
    public const string DEFAULT_DATA_LIST_NAME = "default";

    List<IDataChangeListener> listenerList = new List<IDataChangeListener>();
    bool isFirstUpdate;

    public void Attach(IDataChangeListener listener)
    {
        if (listener != null)
        {
            listenerList.Add(listener);
        }
        
    }

    public void Dettach(IDataChangeListener listener)
    {
        listenerList.Remove(listener);
    }


    public void DettachAll()
    {
        listenerList.Clear();
    }

    public void Notify()
    {
        foreach (IDataChangeListener listener in listenerList)
        {
            listener.OnDataChange();
        }
    }

    //public virtual void Awake()
    //{
    //    string s = GetType().Name + "_FirstAwake";
    //    int firstWake =  PlayerPrefs.GetInt(s, 0);
    //    if (firstWake == 0 || LOAD_ORIGINAL_DATA)
    //    {
    //        LoadDataFirstTime();
    //        PlayerPrefs.SetInt(s, 1);
    //        PlayerPrefs.Save();
    //    }
    //    else {
    //        LoadData();
    //    }
    //}


    public virtual void Start()
    {
        //Debug.Log("GameElementManager Start");
        isFirstUpdate = true;
        //Notify();
    }

    public virtual void Update()
    {
        if (isFirstUpdate)
        {
            Notify();
            isFirstUpdate = false;
        }
    }

    //public virtual void OnDestroy()
    //{
    //    SaveData();
    //}

    public virtual bool CouldChangeGameElementValue(GameElement ele, int change)
    {
        if (ele != null)
        {
            return true;
        }
        return false;
    }

    public virtual bool TryChangeGameElementValue(string name, int change)
    {

        GameElement ele = GetDataList().Find(x => x.name == name);
        if (ele != null)
        {
            if (CouldChangeGameElementValue(ele, change))
            {
                ele.value += change;
                //Notify();
                return true;
            }
        }
        return false;
    }

    public virtual bool TrySetGameElementEnable(string name, bool enable)
    {
        GameElement ele = GetDataList().Find(x => x.name == name);
        if (ele != null)
        {
            if (ele.enabled == enabled)
            {
                return true;
            }
            else
            {
                ele.enabled = enabled;
                //Notify();
                return true;
            }
        }
        return false;
    }

    public List<GameElement> GetDataList()
    {
        return GetDataList(DEFAULT_DATA_LIST_NAME);
    }

    public void SetDataList(List<GameElement> dataList)
    {
        SetDataList(DEFAULT_DATA_LIST_NAME, dataList);
    }

    public abstract List<GameElement> GetDataList(string name);

    public abstract void SetDataList(string name, List<GameElement> dataList);


    public const string DIRECTORY_NAME = "Saves";
    public const string SUFFIXE = ".binary";

    public override string GetFileName()
    {
        return DIRECTORY_NAME +"/" + GetType().Name + SUFFIXE;
    }

    protected BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器

    public override void SaveData()
    {
        if (!Directory.Exists(DIRECTORY_NAME))
            Directory.CreateDirectory(DIRECTORY_NAME);
        using (FileStream fs = File.Create(GetFileName()))
        {
            binFormat.Serialize(fs, GetDataList());
        }
    }

    public override void LoadData()
    {
        string path = GetFileName();
        if (File.Exists(path))
        {
            using (FileStream fs = File.OpenRead(path))
            {
                List<GameElement> dataList = (List<GameElement>)binFormat.Deserialize(fs);
                SetDataList(dataList);
            }
        }
        else
        {
            Debug.LogError("File " + path + " doesn't exisit");
        }
    }

    public override abstract void LoadDataFirstTime();

}
