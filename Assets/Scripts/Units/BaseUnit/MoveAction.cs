using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class MoveAction : GOAPAction
{
    BaseUnit unit;

    public void SetupPrecons()
    {
        AddPrecondition(new GOAPState("Has Path", new List<object> { true }));
    }

    public override bool CanRun(GOAPAgent agent)
    {
        return unit.path.Count < 0;
    }

    public void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("At Destination", new List<object> {true }),
            new GOAPState("At Home", new List<object> {true }),
            new GOAPState("Target in Range", new List<object> {true }),

        };
    }

    public override bool TestForFinished()
    {
        if((unit.hexTransform.CalcHexManhattanDist(unit.Destination) == 0))
        {
            unit.GetNewDestination();
            return true;
        }
        return false;
    }

    public void SetUpEffects()
    {
        AddEffect(() => unit.Move());
    }

	// Use this for initialization
	public virtual void Awake ()
    {
        unit = GetComponent<BaseUnit>();
        if (GetComponent<MilitaryUnit>() != null)
        {
            Agent = GetComponent<MilitaryUnit>().GOAP;
        }
        if (GetComponent<Courier>() != null)
        {
            Agent = GetComponent<Courier>().GOAP;
        }

        Agent.AvailableActions.Add(this);
        SetUpEffects();
        SetupSatisfactions();
        SetupPrecons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
