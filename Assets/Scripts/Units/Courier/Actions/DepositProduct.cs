using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class DepositProduct : TargetedAction
{
    BaseBuilding target;
    Courier unit;

    public override bool CanRun(GOAPAgent agent)
    {
        return unit.inventory.Count > 0;
    }

    void SetupRequirements()
    {
        AddPrecondition(new GOAPState("At Destination", new List<object> { true }));
        AddPrecondition(new GOAPState("Has Destination", new List<object> { true }));
        AddPrecondition(new GOAPState("Must Deposit Products", new List<object> { true }));
    }

    void SetupEffects()
    {
        AddEffect(DepositProducts);
    }

    void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Has Inventory", new List<object> {false }),
            new GOAPState("Must Deposit Products", new List<object> {false }),
        };
    }

    public override bool TestForFinished()
    {
        if (unit.inventory.Count == 0)
        {
            unit.GetNewDestination();
            return true;
        }
        return false;
    }

    void DepositProducts()
    {
        unit.inventory = target.DeliverProducts(unit.inventory.ToArray());
    }

    public void SetTarget(BaseBuilding Target)
    {
        target = unit.DestinationBuilding;
    } 


	// Use this for initialization
	void Awake ()
    {
        unit = GetComponent<Courier>();
        Agent = unit.GOAP;
        Agent.AvailableActions.Add(this);
        SetupRequirements();
        SetupSatisfactions();
        SetupEffects();
	}


}
