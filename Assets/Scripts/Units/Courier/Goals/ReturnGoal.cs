using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class ReturnGoal : GOAPGoal
{
    public override void SetupPrecons()
    {
        Preconditions = new List<GOAPState>
        {
            new GOAPState("Tasks Complete", true),
            new GOAPState("At Home", true)
        };
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = ReadyToFinish;
    }

    float ReadyToFinish()
    {
        if (Preconditions[0].Items[0])
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
