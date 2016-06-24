using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HRListScript :GameListScript {


    //public override List<GameElement> GetEleList()
    //{
    //    HRManagerScript HRManagerScript
    //        = gameElementManagerObject.GetComponent<HRManagerScript>();
    //    return HRManagerScript.currStaffList;
    //}

    public override GameElementManager GetGameElementManager()
    {
        return gameElementManagerObject.GetComponent<HRManagerScript>();
    }
}
