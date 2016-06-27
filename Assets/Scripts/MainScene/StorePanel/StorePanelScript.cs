using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum StorePattern {
    resource,battle,build

}

public class StorePanelScript : MonoBehaviour {

    public Text titleText;
    public GameObject goodseller;


    GoodsSellerManager goodsellerManager;
    public void SetGood() {
            goodsellerManager.storePattern = StorePattern.resource;
            titleText.text = "贸易站";
    }
    public void SetBattle()
    {
        goodsellerManager.storePattern = StorePattern.battle;
        titleText.text = "兵工厂";
    }
    public void SetBuild()
    {
        goodsellerManager.storePattern = StorePattern.build;
        titleText.text = "建筑";
    }

    // Use this for initialization
    void Start () {
        goodsellerManager = goodseller.GetComponent<GoodsSellerManager>();
    }
	
}
