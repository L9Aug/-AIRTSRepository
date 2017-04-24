using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetProductLocation : GOAPAction
{
    Courier unit;

    void SetupPrecons()
    {
        AddPrecondition(new GOAPState("Has Tickets", new List<object> { false }));
        AddPrecondition(new GOAPState("Products Available", new List<object> { true }));
    }

    void SetupEffects()
    {
        effects.Add(SetDestination);
    }

    public override bool CanRun(GOAPAgent agent)
    {
        return unit.ticketList.Count == 0;
    }

    void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Has Tickets", new List<object> {true }),
            new GOAPState("Has Inventory Space", new List<object> {true })
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
	void Awake ()
    {
        unit = GetComponent<Courier>();
        Agent = unit.GOAP;
        Agent.AvailableActions.Add(this);
        SetupPrecons();
        SetupEffects();
        SetupSatisfactions();

    }
}
