using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class MoveAction : GOAPAction
{
    BaseUnit unit;


    public override bool TestForFinished()
    {
        return (unit.hexTransform.CalcHexManhattanDist(unit.Destination) == 0);
    }

    public void SetUpEffects()
    {
        AddEffect(() => unit.Move());
    }

	// Use this for initialization
	public virtual void  Start ()
    {
        unit = GetComponent<BaseUnit>();
        SetUpEffects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
