using UnityEngine;
using System.Collections.Generic;
using System;

public interface ISubject
{
    void Attach(IDataChangeListener listener);
    void Dettach(IDataChangeListener listener);
    void DettachAll();
    void Notify();
}

public abstract class GameElementManager : MonoBehaviour, ISubject
{

    public const string DefaultDataListName = "_default_";

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

    public virtual bool CouldChangeGameElementValue(GameElement ele, int change)
    {
        if (ele != null)
        {
            return true;
        }
        return false;
    }

    public virtual bool TryChangeGameElementValue(string name, int change) {
        
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

    public virtual bool TrySetGameElementEnable(string name, bool enable) {
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
    public List<GameElement> GetDataList() {
        return GetDataList(DefaultDataListName);
    }

    public abstract List<GameElement> GetDataList(string name);

}
