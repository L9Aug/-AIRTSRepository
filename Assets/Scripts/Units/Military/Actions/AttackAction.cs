using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class AttackAction : TargetedAction
{
    MilitaryUnit unit;

    void SetupPreconditions()
    {
        AddPrecondition(new GOAPState("Target is Enemy", true));
        AddPrecondition(new GOAPState("Target in Range", true));
    }

    void SetupSatisfactions()
    {
        satisfiesStates.Add(new GOAPState("Has Target", false));
    }

    public void SetUpEffects()
    {
        effects.Add(Attack);
    }

    void Attack()
    {
        unit.target.GetComponent<BaseUnit>().DealDamage(unit.damage);
    }

	// Use this for initialization
	void Start ()
    {
        unit = GetComponent <MilitaryUnit>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
