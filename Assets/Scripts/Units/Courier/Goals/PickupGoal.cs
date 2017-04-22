using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class PickupGoal : GOAPGoal
{
    BaseBuilding dest;
    Courier unit;

    public override void SetupPrecons()
    {
        Preconditions = new List<GOAPState>
        {
            new GOAPState("At Destination", true),
            new GOAPState("Has Tickets", true),
            new GOAPState("Has Inventory Space", true)
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
        return (float)unit.hexTransform.CalcHexManhattanDist(dest.hexTransform);
    }

    float HasInventorySpace()
    {
        return 1 - (unit.inventory.Count / unit.inventorySpace);
    }

    public void SetUnit(Courier Unit)
    {
        unit = Unit;
    }

    public void UpdateGoal(BaseBuilding destination)
    {
        dest = destination;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
