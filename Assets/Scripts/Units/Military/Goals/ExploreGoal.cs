using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class ExploreGoal : GOAPGoal
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
            new GOAPState("Target is Enemy", new List<object> {false })
        };
    }

    public override void SetupUtility()
    {
        UtilAction = new UtilityAction<GOAPGoal>(1, this, new UtilityConsideration());
        UtilAction.Considerations[0].GetInput = GetHealth;
    }

    float GetHealth()
    {
        Debug.Log("First Utility Consideration not created");
        return 0;
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
