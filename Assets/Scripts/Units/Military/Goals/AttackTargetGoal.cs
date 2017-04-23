using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class AttackTargetGoal : GOAPGoal
{
    GameEntity target;

    new MilitaryUnit unit;

    public AttackTargetGoal()
    {

    }

    public void SetupPreconditions()
    {

    }

    public void SetTarget(GameEntity Target)
    {
        target = Target;
    }


	// Use this for initialization
	void Start ()
    {
        Name = "Attack Target";
        SetupPreconditions();
        SetupUtility();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void SetupPrecons()
    {

        Preconditions.Add(new GOAPState("Has Target", true));
        Preconditions.Add(new GOAPState("Target in Range", true));
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration(), new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = Distance;
        UtilAction.Considerations[1].GetInput = UnitTypes;
    }

    float Distance()
    {
        return unit.hexTransform.CalcHexManhattanDist(unit.Destination);
    }

    float UnitTypes()
    {
        if (unit.gameObject.name == "Infantry" || unit.gameObject.name == "Archer")
        {
            UtilAction.Considerations[1].CurveType = UtilityConsideration.CurveTypes.Polynomial;
        }
        if(unit.gameObject.name == "Catapult")
        {
            UtilAction.Considerations[1].CurveType = UtilityConsideration.CurveTypes.Trigonometric;
        }

        return 0.7f;
    }
}
