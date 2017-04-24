using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class DepositProduct : TargetedAction
{
    BaseBuilding target;
    Courier unit;
    
    void SetupRequirements()
    {
        AddPrecondition(new GOAPState("At Destination", true));
        AddPrecondition(new GOAPState("Has Destination", true));
        AddPrecondition(new GOAPState("Must Deposit Products", true));
    }

    void SetupEffects()
    {
        AddEffect(DepositProducts);
    }

    void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Has Inventory", false),
            new GOAPState("Must Deposit Products", false),
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
	void Start ()
    {
        unit = GetComponent<Courier>();
        SetupRequirements();
        SetupEffects();
	}


}
