using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopIncreaser : Building
{
    public int increaseBy;

    public override void BuildingPlaced()
    {
        GameManager.instance.IncreasePop(increaseBy);
    }
}
