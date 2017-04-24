using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class PickupGoal : GOAPGoal
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
        Preconditions = new List<GOAPState>
        {
            new GOAPState("At Destination", new List<object> {true }),
            new GOAPState("Has Tickets", new List<object> {true }),
            new GOAPState("Has Inventory Space", new List<object> {true })
        };
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration(), new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = DistanceToDestination;
        UtilAction.Considerations[1].GetInput = HasInventorySpace;
    }

    float DistanceToDestination()
    {
        return (float)unit.hexTransform.CalcHexManhattanDist(unit.DestinationBuilding.hexTransform);
    }

    float HasInventorySpace()
    {
        return 1 - (unit.inventory.Count / unit.inventorySpace);
    }

    public void SetUnit(Courier Unit)
    {
        unit = Unit;
    }
}
