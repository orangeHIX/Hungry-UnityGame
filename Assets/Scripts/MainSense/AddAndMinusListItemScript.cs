using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class AddAndMinusListItemScript : GameListItemScript{

    string unitName {
        get {
            if( gameElement != null)
                return gameElement.name;
            return null;
        }
    }
    int unitValue {
        get {
            if (gameElement != null)
                return gameElement.value;
            return 0;            
        }
    }

    protected GameElementManager managerScript;
    protected GameElement gameElement;
    protected GameObject unitButton;
    protected GameObject addButton;
    protected GameObject minusButton;
    protected GameObject valueText;


    public virtual void Start() {
        BindGameObjectAndScript();
    }

    public abstract void BindGameObjectAndScript();

    public override void SetGameElement(GameElement ele)
    {
        this.gameElement = ele;
        if (gameElement != null)
        {
            SetText(unitButton, true, unitName);
            SetText(valueText, false, unitValue.ToString());

            Button b = unitButton.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(ShowDescription);

            b = addButton.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(AddOne);

            b = minusButton.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(MinusOne);
        }
    }

    protected void SetText(GameObject textObject, bool isTextInChild, string str)
    {
        if (textObject != null)
        {
            Text text = null;
            if (isTextInChild)
            {
                text = textObject.GetComponentInChildren<Text>();
            }
            else
            {
                text = textObject.GetComponent<Text>();
            }
            if (text != null)
            {
                text.text = str;
            }
        }
    }

    void AddOne()
    {
        if (managerScript != null)
        {
            if(managerScript.TryChangeGameElementValue(unitName, 1)){
                managerScript.Notify();
            }
        }
    }

    void MinusOne()
    {
        if (managerScript != null)
        {
            if (managerScript.TryChangeGameElementValue(unitName, -1))
            {
                managerScript.Notify();
            }
        }
    }

    void ShowDescription() {
        InfoPanel.Pop(gameElement.description);
    }
}
