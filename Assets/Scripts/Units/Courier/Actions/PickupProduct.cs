using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class PickupProduct : TargetedAction
{
    Courier unit;

    public override void ActionUpdate(GOAPAgent agent)
    {
        base.ActionUpdate(agent);

    }

    public override bool CanRun(GOAPAgent agent)
    {
        return unit.inventory.Count < unit.inventorySpace;
    }

    void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Must Deposit Items", new List<object> {true }),
            new GOAPState("Tasks Complete", new List<object> {true })
        };
    }

    void SetupPrereqs()
    {
        AddPrecondition(new GOAPState("Has Tickets", new List<object> { true }));
        AddPrecondition(new GOAPState("At Destination", new List<object> { true }));
    }

    void SetupEffects()
    {
        effects.Add(Retrieve);
    }

    void Retrieve()
    {
        // Trade the tickets for items 
        if (unit.inventory.Count < unit.inventorySpace && unit.ticketList.Count > 0)
        {
            if (unit.ticketList[0].ProductOwner == unit.DestinationBuilding)
            {
                unit.inventory.Add(unit.DestinationBuilding.RedeemKalamataTicket(unit.ticketList[0]));
                unit.ticketList.RemoveAt(0);
            }
        }
    }

    public override bool TestForFinished()
    {
        if (unit.ticketList[0].ProductOwner != unit.DestinationBuilding)
        {
            unit.GetNewDestination();
            return true;
        }
        return false;
    }

	// Use this for initialization
	void Awake()
    {
        unit = GetComponent<Courier>();
        Agent = unit.GOAP;

        Agent.AvailableActions.Add(this);
        SetupSatisfactions();
        SetupEffects();
        SetupPrereqs();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
