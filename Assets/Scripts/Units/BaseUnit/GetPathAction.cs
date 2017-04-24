using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetPathAction : GOAPAction
{
    BaseUnit unit;

    void SetupPrecons()
    {
        AddPrecondition(new GOAPState("Has Path", false));
    }

    void SetupEffects()
    {
        AddEffect(unit.GetPath);
    }

	// Use this for initialization
	void Start ()
    {
        unit = GetComponent<BaseUnit>();
        SetupPrecons();
        SetupEffects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
