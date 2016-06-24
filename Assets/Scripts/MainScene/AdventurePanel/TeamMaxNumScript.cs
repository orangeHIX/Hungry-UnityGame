using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TeamMaxNumScript : MonoBehaviour, IDataChangeListener {

    BattleUnitManager manager;

    Text teamNumText;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("BattleUnitManager").GetComponent<BattleUnitManager>();
        if (manager != null) {
            manager.Attach(this);
        }
        teamNumText = transform.GetComponent<Text>();

    }


    public void OnDataChange()
    {
        if (teamNumText != null && manager != null) { 
            teamNumText.text = string.Format(
            "当前队伍人数\n{0}/{1}",
            manager.currTeamNum,
            manager.maxTeamNum);
        }
    }
}
