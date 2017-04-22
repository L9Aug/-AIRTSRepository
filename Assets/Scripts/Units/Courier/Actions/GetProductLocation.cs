using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetProductLocation : GOAPAction
{
    Courier unit;

    void SetupPrecons()
    {
        AddPrecondition(new GOAPState("Has Tickets", false));
        AddPrecondition(new GOAPState("Products Available", true));
    }

    void SetupEffects()
    {
        effects.Add(SetDestination);
    }

    void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Has Tickets", true),
            new GOAPState("Has Inventory Space", true)
        };
    }

    public override bool TestForFinished()
    {
        return (unit.DestinationBuilding != null);
    }

    void SetDestination()
    {

        unit.ticketList = TeamManager.TM.Teams[GetComponent<Courier>().TeamID].ReserveProducts(GetComponent<Courier>().hexTransform, GetComponent<Courier>().shoppingList.ToArray());

        unit.DestinationBuilding = unit.ticketList[0].ProductOwner;
    }

	// Use this for initialization
	void Start ()
    {
        unit = GetComponent<Courier>();

    }
}
