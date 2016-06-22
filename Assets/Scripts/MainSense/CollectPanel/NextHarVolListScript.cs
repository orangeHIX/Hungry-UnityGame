using UnityEngine;
using System.Collections.Generic;
using System;

public class NextHarVolListScript : GameListScript
{
    public override GameElementManager GetGameElementManager()
    {
        return gameElementManagerObject.GetComponent<NextHarvestManager>();
    }
}
