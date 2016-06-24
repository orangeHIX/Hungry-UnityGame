using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GoodListItemScript : GameListItemScript {

    GameObject goodSeller;

    const int MaterialTextNum = 3;

    Good good;
    Button goodButton;
    Text goodButtonText;
    Text[] materialTexts;
    Button infoButton;

    GoodsSellerManager goodSellerManagerScript;

    GameObject checkImage;
    GameObject leftButtomModifier;
    Text modifierText;

    float materialTextColorA;

    //Text infoButtonText;
    public void Start() {
        Transform tran = transform.FindChild("GoodButton");
        if (tran != null) {
            goodButton = tran.GetComponent<Button>();
            goodButtonText = tran.GetComponentInChildren<Text>();
            goodButton.onClick.RemoveAllListeners();
            goodButton.onClick.AddListener(BuyGood);

            leftButtomModifier = tran.FindChild("LeftButtomModifier").gameObject;
            if (leftButtomModifier != null) {
                modifierText = leftButtomModifier.GetComponentInChildren<Text>();
                leftButtomModifier.SetActive(false);
            }

        }
        materialTexts = new Text[MaterialTextNum];
        for (int i = 0; i < MaterialTextNum; i++) {
            tran = transform.FindChild( String.Format("MaterialText ({0})",i));
            if (tran != null) {
                materialTexts[i] = tran.GetComponent<Text>();
            }
        }
        if (materialTexts[0] != null) {
            materialTextColorA = materialTexts[0].color.a;
        }
        tran = transform.FindChild("InfoButton");
        if (tran != null)
        {
            infoButton = tran.GetComponent<Button>();
            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(delegate { InfoPanel.Pop(good.name + " "+ good.description); });
            //infoButtonText = tran.GetComponentInChildren<Text>();
        }
        checkImage = transform.FindChild("CheckImage").gameObject;
        if (checkImage != null) {
            checkImage.SetActive(false);
        }

        goodSeller = GameObject.Find("GoodsSeller");
        if(goodSeller != null)
        {
            goodSellerManagerScript = goodSeller.GetComponent<GoodsSellerManager>();
        }

    }

    public override void SetGameElement(GameElement ele)
    {
        if (ele is Good) {
            good = ele as Good;
            goodButtonText.text = good.name;
            for (int i = 0; i < MaterialTextNum; i++)
            {
                materialTexts[i].text = GetMaterialTextString(i);
            }

            AdjustLeftButtomModifier();

            AdjustCheckImage();
        }
    }

    void AdjustLeftButtomModifier() {
        if (good.item is Shelter)
        {
            leftButtomModifier.SetActive(true);
            modifierText.text = good.item.value.ToString();
        }
        else
        {
            leftButtomModifier.SetActive(false);
        }
    }

    void AdjustCheckImage() {
        if (good.item is ProductionBuilding && (good.item.enabled) == false)
        {
            checkImage.SetActive(true);
            SetMaterialTextVisiable(false);
            goodButton.interactable = false;
        }
        else
        {
            checkImage.SetActive(false);
            SetMaterialTextVisiable(true);
            goodButton.interactable = true;
        }
    }

    void SetMaterialTextVisiable(bool visible) {
        foreach (Text ma in materialTexts)
        {
            Color col = ma.color;
            col.a = visible ? materialTextColorA : 0;
            ma.color = col;
        }
    }

    string GetMaterialTextString(int index) {
        List<GameElement> materialList = good.materialList;
        if (index >= 0 && index < good.materialList.Count) {
            return String.Format("{0} * {1}", materialList[index].name, materialList[index].value);
        }
        return null;
    }

    void BuyGood()
    {
        if (goodSellerManagerScript != null) {
            if (goodSellerManagerScript.TryChangeGameElementValue(good.name, -1))
            {
                goodSellerManagerScript.Notify();
            }
        }
    }
}
