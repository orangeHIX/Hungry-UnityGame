using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NextHarVolListItemScript : GameListItemScript {


    GameElement element;
    Text[] labels;

    // Use this for initialization
    void Start()
    {
        labels = GetComponentsInChildren<Text>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public override void SetGameElement(GameElement ele)
    {
        this.element = ele;
        //Text[] texts = GetComponentsInChildren<Text>();
        if (labels != null && element != null)
        {
            labels[0].text = element.name;
            labels[1].text = element.value.ToString("+#;-#;0");
        }
    }
}
