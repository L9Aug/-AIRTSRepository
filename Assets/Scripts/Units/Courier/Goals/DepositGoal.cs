using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class DepositGoal : GOAPGoal
{
    new Courier unit;

    public override void Initialise(BaseUnit Unit)
    {
        unit = (Courier)Unit;
        SetupPrecons();
        SetupUtility();
    }

    public override void SetupPrecons()
    {
        Preconditions.Add(new GOAPState("Has Target", new List<object> { true }));
        Preconditions.Add(new GOAPState("At Destination", new List<object> { true }));
        Preconditions.Add(new GOAPState("Has Destination", new List<object> { true }));
        Preconditions.Add(new GOAPState("Must Deposit Products", new List<object> { true }));
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration(), new UtilityConsideration(), new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = SendInventoryCount;
        UtilAction.Considerations[0].p = 3;
        UtilAction.Considerations[0].k = 0.2f;
        UtilAction.Considerations[1].GetInput = InventoryAtMax;
        UtilAction.Considerations[2].GetInput = DistanceToTarget;
    }

    float SendInventoryCount()
    {
        return unit.inventory.Count / unit.inventorySpace;
    }

    float InventoryAtMax()
    {
        if (unit.inventory.Count >= unit.inventorySpace)
            return 1;
        else
            return 0;
    }

    float DistanceToTarget()
    {
        return (float)HexTransform.CalcHexManhattanDist(MapGenerator.Map[(int)unit.hexTransform.RowColumn.x, (int)unit.hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)unit.DestinationBuilding.hexTransform.RowColumn.x, (int)unit.DestinationBuilding.hexTransform.RowColumn.y].ASI);
    }

}
