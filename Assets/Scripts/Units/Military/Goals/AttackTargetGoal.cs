using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class AttackTargetGoal : GOAPGoal
{
    GameEntity target;

    new MilitaryUnit unit;

    public override void Initialise(BaseUnit Unit)
    {
        unit = (MilitaryUnit)Unit;
    }

    public AttackTargetGoal()
    {

    }

    public override void SetupPrecons()
    {
        Preconditions = new List<GOAPState>
        {
            new GOAPState("Target is Enemy", true),
            new GOAPState("Target in Range", true)
        };
    }

    public void SetTarget(GameEntity Target)
    {
        target = Target;
    }

	// Use this for initialization
	void Start ()
    {
        Name = "Attack Target";
        SetupPrecons();
        SetupUtility();
	}

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration(), new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = Distance;
        if (unit.gameObject.name == "Infantry" || unit.gameObject.name == "Archer")
        {
            UtilAction.Considerations[1].CurveType = UtilityConsideration.CurveTypes.Polynomial;
        }
        if (unit.gameObject.name == "Catapult")
        {
            UtilAction.Considerations[1].CurveType = UtilityConsideration.CurveTypes.Trigonometric;
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
}
