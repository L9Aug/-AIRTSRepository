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

    void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Must Deposit Items", true),
            new GOAPState("Tasks Complete", true)
        };
    }

    void SetupPrereqs()
    {
        AddPrecondition(new GOAPState("Has Tickets", true));
        AddPrecondition(new GOAPState("At Destination", true));
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
	void Start ()
    {
        unit = GetComponent<Courier>();
        SetupSatisfactions();
        SetupEffects();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
