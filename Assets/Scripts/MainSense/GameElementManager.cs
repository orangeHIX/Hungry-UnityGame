using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    string GetFileName();
}

public abstract class GameElementManager : MonoBehaviour, ISubject, IPersist
{

    public const string DEFAULT_DATA_LIST_NAME = "default";

    List<IDataChangeListener> listenerList = new List<IDataChangeListener>();
    bool isFirst;

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

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        //Debug.Log("GameElementManager Start");
        isFirst = true;
    }

    public virtual void Update()
    {
        if (isFirst)
        {
            Notify();
            isFirst = false;
        }
    }

    public virtual void OnDestroy()
    {
        SaveData();
    }

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

    public virtual string GetFileName()
    {
        return DIRECTORY_NAME +"/" + GetType().Name + SUFFIXE;
    }

    protected BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器

    public virtual void SaveData()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");
        using (FileStream fs = File.Create(GetFileName()))
        {
            binFormat.Serialize(fs, GetDataList());
        }
    }

    public virtual void LoadData()
    {
        using (FileStream fs = File.OpenRead(GetFileName()))
        {
            List<GameElement> dataList = (List<GameElement>)binFormat.Deserialize(fs);
            SetDataList(dataList);
        }
    }
}
