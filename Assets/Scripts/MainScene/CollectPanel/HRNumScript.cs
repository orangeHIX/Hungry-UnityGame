using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HRNumScript : MonoBehaviour, IDataChangeListener {

    public GameObject HRManager;
    HRManagerScript HRManagerScript;
    Text HRText;

    public void OnDataChange()
    {
        if (HRText != null && HRManagerScript != null) {
            HRText.text = String.Format("人力 {0}/{1}", HRManagerScript.humanNum, HRManagerScript.humanMaxNum);
        }
    }

    // Use this for initialization
    void Start () {
        HRText = GetComponent<Text>();
        if (HRManager != null)
        {
            HRManagerScript = HRManager.GetComponent<HRManagerScript>();
            if (HRManagerScript != null) {
                HRManagerScript.Attach(this);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
