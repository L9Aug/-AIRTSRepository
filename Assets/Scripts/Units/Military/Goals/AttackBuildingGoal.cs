using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class AttackBuildingGoal : GOAPGoal
{
    BaseUnit unit;

    public void Initialise(BaseUnit Unit)
    {
        unit = Unit;
    }

    public override void SetupPrecons()
    {
        throw new NotImplementedException();
    }

    public override void SetupUtility()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
