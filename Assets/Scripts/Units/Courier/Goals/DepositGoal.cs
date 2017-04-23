using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class DepositGoal : GOAPGoal
{
    int maxInventory;
    int inventoryCount;
    BaseBuilding targetBuilding;
    new Courier unit;

    

    void Start()
    {
        targetBuilding = unit.DestinationBuilding;
    }


    public override void SetupPrecons()
    {
        Preconditions.Add(new GOAPState("Has Target", true));
        Preconditions.Add(new GOAPState("At Destination", true));
        Preconditions.Add(new GOAPState("Has Destination", true));
        Preconditions.Add(new GOAPState("Must Deposit Products", true));
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

    public void SetUp(int InventorySize, BaseBuilding target, BaseUnit unit)
    {
        targetBuilding = target;
        maxInventory = InventorySize;
    }

    public void SetTargetBuilding(BaseBuilding target)
    {
        targetBuilding = target;
    }
    
    float SendInventoryCount()
    {
        return unit.inventory.Count / unit.inventorySpace;
    }

    float InventoryAtMax()
    {
        if (inventoryCount >= maxInventory)
            return 1;
        else
            return 0;
    }

    float DistanceToTarget()
    {
        return (float)HexTransform.CalcHexManhattanDist(MapGenerator.Map[(int)unit.hexTransform.RowColumn.x, (int)unit.hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)targetBuilding.hexTransform.RowColumn.x, (int)targetBuilding.hexTransform.RowColumn.y].ASI);
    }

}
