using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class ExploreGoal : GOAPGoal
{

    public override void SetupPrecons()
    {
        Preconditions = new List<GOAPState>
        {
            new GOAPState("Target is Enemy", false)
        };
    }

    public override void SetupUtility()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
