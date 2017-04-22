using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class MoveToGoal : GOAPGoal
{
    HexTile targetTile;

    public override void SetupPrecons()
    {
        Preconditions.Add(new GOAPState("Has Target", true));

    }

    public void SetTarget(HexTile target)
    {
        targetTile = target;
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = ScoringFunction;
    }

    float ScoringFunction()
    {
        Debug.Log(this + " has not been implemented fully.");
        return 1;
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
