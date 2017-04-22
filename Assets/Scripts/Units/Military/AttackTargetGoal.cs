using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class AttackTargetGoal : GOAPGoal
{
    GameEntity target;

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
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = GoalInput;
    }

    float GoalInput()
    {
        Debug.Log(this + " has not been implemented fully.");
        return 1;
    }
}
