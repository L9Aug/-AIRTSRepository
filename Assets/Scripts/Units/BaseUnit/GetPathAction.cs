using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetPathAction : GOAPAction
{
    BaseUnit unit;

    public override bool CanRun(GOAPAgent agent)
    {
        return true;
    }

    void SetupPrecons()
    {
        AddPrecondition(new GOAPState("Has Path", new List<object> { false }));
    }

    void SatisfiesStates()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("Has Path", new List<object> {true })
        };
    }

    void SetupEffects()
    {
        AddEffect(unit.GetPath);
    }

	// Use this for initialization
	void Awake ()
    {

        unit = GetComponent<BaseUnit>();

        if (GetComponent<MilitaryUnit>() != null)
        {
            Agent = GetComponent<MilitaryUnit>().GOAP;
        }
        if(GetComponent<Courier>() != null)
        {
            Agent = GetComponent<Courier>().GOAP;
        }

        Agent.AvailableActions.Add(this);

        SetupPrecons();
        SatisfiesStates();
        SetupEffects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
