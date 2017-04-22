using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class PickupProduct : TargetedAction
{
    new BaseBuilding target;

    List<KalamataTicket> tickets;
    Courier unit;
    int itemsInInventory;
    int maxInventorySpace;


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
        Debug.Log("Retrieve has not been fully implemented in " + this);

        GetComponent<Courier>().AddToInventory(new List<Products>());
    }

    public override bool TestForFinished()
    {
        return (unit.inventory.Count > 0);
    }

    void GetUpdate(List<KalamataTicket> Ticks, int invCount)
    {
        itemsInInventory = invCount;
        tickets = Ticks;
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
