using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public interface IDataChangeListener {
    void OnDataChange();
}

public abstract class GameListScript: MonoBehaviour,IDataChangeListener {

    public GameObject gameElementManagerObject;
    public GameObject listItemPrefab;

    GameElementManager gameElementManager;
    //List<GameElement> eleList;
    List<GameObject> listItems =new List<GameObject>();

    //private LayoutElement itemLayoutMemo;

    //public abstract List<GameElement> GetDataList();

    public virtual void Start()
    {
        gameElementManager = GetGameElementManager();//gameElementManagerObject.GetComponent<GameElementManager>();

        if (gameElementManager != null)
        {
            //eleList = GetEleList();

            gameElementManager.Attach(this); //register subject

            InitListItem();
        }
    }

    /// <summary>
    /// By default, return the first GameElementManager attached to GameElementManagerObject
    /// </summary>
    /// <returns> </returns>
    public virtual GameElementManager GetGameElementManager() {
        return gameElementManagerObject.GetComponent<GameElementManager>();
    }

    void InitListItem() {

        DestroyAllChildren();
        listItems.Clear();
        List<GameElement> eleList = gameElementManager.GetDataList();
        for (int i = listItems.Count; i < eleList.Count; i++)
        {
            AddInvisibelListItem();
        }
        //i = transform.childCount;
    }

    void AddInvisibelListItem() {
        GameObject item = Instantiate(listItemPrefab);
        item.transform.SetParent(transform);
        listItems.Add(item);
        //SetItemVisible(item, false);
    }

    void DestroyAllChildren() {
        List<Transform> lst = new List<Transform>();
        foreach (Transform child in transform)
        {
            lst.Add(child);
            //Debug.Log(child.gameObject.name);
        }
        for (int i = 0; i < lst.Count; i++)
        {
            Destroy(lst[i].gameObject);
        }
    }


    protected virtual void SetItemVisible(GameObject item, bool visible)
    {
        item.SetActive(visible);
        //LayoutElement ele = item.GetComponent<LayoutElement>();
        //if (ele != null && item != null)
        //{
        //    if (visible)
        //    {
        //        ele.preferredHeight = itemLayoutMemo.preferredHeight;
        //    }
        //    else
        //    {
        //        ele.preferredHeight = 0;
        //    }
        //}
    }

    public void OnDataChange()
    {
        //GameListItemScript[] items = fin
        //    GetComponentsInChildren<GameListItemScript>();
        List<GameElement> eleList = gameElementManager.GetDataList();
        if (listItems != null )
        {
            for (int i = 0; i < eleList.Count; i++)
            {
                if (eleList[i].enabled)
                {
                    SetItemVisible(listItems[i], true);
                    //Debug.Log("SetGameElement" + eleList[i].name);
                    listItems[i].GetComponent<GameListItemScript>().SetGameElement(eleList[i]);
                }
                else
                {
                    SetItemVisible(listItems[i], false);
                }
            }
            for (int i = eleList.Count; i < listItems.Count; i++) {
                SetItemVisible(listItems[i], false);
            }
        }
    }
}
