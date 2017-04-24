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

    public override bool CanRun(GOAPAgent agent)
    {
        return Vector3.Distance(unit.target.transform.position, unit.transform.position) < unit.attackRange;
    }

    void Attack()
    {
        if (unit.target.GetComponent<GameEntity>() != null)
        {
            unit.target.GetComponent<GameEntity>().DealDamage(unit.damage);
        }
        else
        {
            Debug.Log("Attacking an invalid target");
        }
    }

	// Use this for initialization
	void Start ()
    {
        unit = GetComponent <MilitaryUnit>();
        SetupPreconditions();
        SetUpEffects();
        SetupSatisfactions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
