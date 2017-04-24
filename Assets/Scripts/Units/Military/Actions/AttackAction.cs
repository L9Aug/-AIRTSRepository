using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class AttackAction : TargetedAction
{
    MilitaryUnit unit;

    void SetupPreconditions()
    {
        AddPrecondition(new GOAPState("Target is Enemy", new List<object> { true }));
        AddPrecondition(new GOAPState("Target in Range", new List<object> { true }));
    }

    void SetupSatisfactions()
    {
        satisfiesStates.Add(new GOAPState("Has Target", new List<object> { false }));
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
	void Awake()
    {
        unit = GetComponent <MilitaryUnit>();
        Agent = unit.GOAP;
        Agent.AvailableActions.Add(this);
        SetupPreconditions();
        SetUpEffects();
        SetupSatisfactions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
