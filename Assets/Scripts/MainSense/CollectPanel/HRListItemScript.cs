using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HRListItemScript : AddAndMinusListItemScript
{
    public override void BindGameObjectAndScript()
    {
        managerScript =
             GameObject.Find("HRManager").GetComponent<GameElementManager>();

        unitButton = transform.FindChild("UnitButton").gameObject;
        addButton = transform.FindChild("AddButton").gameObject;
        minusButton = transform.FindChild("MinusButton").gameObject;
        valueText = transform.FindChild("HRText").gameObject;
    }
}
