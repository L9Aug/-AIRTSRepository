using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class AttackBuildingGoal : GOAPGoal
{
    new MilitaryUnit unit;

    public new void Initialise(BaseUnit Unit)
    {
        unit = (MilitaryUnit)Unit;
    }

    public override void SetupPrecons()
    {
        Preconditions = new List<GOAPState>
        {
            new GOAPState("Has Target", new List<object> {true }),
            new GOAPState("Target in Range", new List<object> {true })
        };
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration(), new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = Distance;
        if (unit.gameObject.name == "Infantry" || unit.gameObject.name == "Archer")
        {
            UtilAction.Considerations[1].CurveType = UtilityConsideration.CurveTypes.Trigonometric;
        }
        if (unit.gameObject.name == "Catapult")
        {
            UtilAction.Considerations[1].CurveType = UtilityConsideration.CurveTypes.Polynomial;
        }
        UtilAction.Considerations[1].GetInput = UnitTypes;
        UtilAction.Considerations[1].m = 0.3f;
        UtilAction.Considerations[1].d = -0.7f;
        UtilAction.Considerations[1].k = 1.4f;
        UtilAction.Considerations[1].p = 0.4f;
        UtilAction.Considerations[1].c = -0.1f;
    }

    float Distance()
    {
        return unit.hexTransform.CalcHexManhattanDist(unit.Destination);
    }

    float UnitTypes()
    {
        return 0.9f;
    }

    // Use this for initialization
    void Start ()
    {
        SetupPrecons();
        SetupUtility();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
