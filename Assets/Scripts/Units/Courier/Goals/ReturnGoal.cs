using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class ReturnGoal : GOAPGoal
{
    BaseUnit unit;

    public void Initialise(BaseUnit Unit)
    {
        unit = Unit;
    }

    public override void SetupPrecons()
    {
        Preconditions = new List<GOAPState>
        {
            new GOAPState("Tasks Complete", new List<object> {true }),
            new GOAPState("At Home", new List<object> {true })
        };
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = ReadyToFinish;
    }

    float ReadyToFinish()
    {
        if (Preconditions[0].Items[0].Equals(true))
            return 1;
        else
            return 0;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
