using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class TargetedAction : GOAPAction
{
    public GameObject target;
    public bool inRange;

    public override void ActionUpdate(GOAPAgent agent)
    {
        
    }

    public override bool CanRun(GOAPAgent agent)
    {
        Debug.Log("CanRun in " + this + " is not implemented.");
        return false;
    }

    public override bool TestForFinished()
    {
        Debug.Log("TestForFinished in " + this + " is not implemented.");
        return false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
