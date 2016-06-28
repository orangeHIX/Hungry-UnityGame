using UnityEngine;
using System.Collections;
using System;

public abstract class PersistModel : MonoBehaviour, IPersist
{

    public static bool ALWAYS_LOAD_ORIGINAL_DATA = false;

    // Use this for initialization
    public virtual void Awake() {
        if (needPersistence())
        {
            //string s = GetType().Name + "_FirstAwake";
            //int firstWake = PlayerPrefs.GetInt(s, 0);
            //if (firstWake == 0 || ALWAYS_LOAD_ORIGINAL_DATA)
            //{
            //    LoadDataFirstTime();
            //    PlayerPrefs.SetInt(s, 1);
            //    PlayerPrefs.Save();
            //}
            //else
            //{
            //    LoadData();
            //}
            if (ALWAYS_LOAD_ORIGINAL_DATA)
            {
                LoadDataFirstTime();
            }
            else if ( LoadData())
            {
                Debug.Log(GetType().Name + " Load Data");
            }
            else {
                LoadDataFirstTime();
            }
        }
        else {
            LoadDataFirstTime();
        }
    }

    public virtual void OnDestroy()
    {
        if (needPersistence())
        {
            SaveData();
        }
    }

    public virtual bool needPersistence() {
        return true;
    }


    public abstract string GetFileName();

    public abstract bool LoadData();

    public abstract void LoadDataFirstTime();

    public abstract void SaveData();
}
