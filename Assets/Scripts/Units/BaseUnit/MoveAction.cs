using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class MoveAction : GOAPAction
{
    BaseUnit unit;

    public void SetupPrecons()
    {
        AddPrecondition(new GOAPState("Has Path", true));
    }

    public void SetupSatisfactions()
    {
        satisfiesStates = new List<GOAPState>
        {
            new GOAPState("At Destination", true),
            new GOAPState("At Home", true),
            new GOAPState("Target in Range", true),

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
	public virtual void Start ()
    {
        unit = GetComponent<BaseUnit>();
        SetUpEffects();
        SetupPrecons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
