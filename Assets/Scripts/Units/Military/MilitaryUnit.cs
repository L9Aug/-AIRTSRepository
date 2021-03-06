﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;
using Condition;
using GOAP;

public class MilitaryUnit : BaseUnit
{
    public float damage;
    public int attackRange;
    public float attackTime;
    float attackTimer = 0;

    public GameObject target;

    public GOAPAgent GOAP = new GOAPAgent();

    public AttackTargetGoal attackGoal;
    public AttackBuildingGoal buildingAttack;
    public ExploreGoal explore;

    public void SetTarget(GameObject Target)
    {
        target = Target;
    }

    public List<GOAPState> GetWorldState()
    {
        List<GOAPState> worldState = new List<GOAPState>()
        {
            new GOAPState("Has Target", new List<object> {(target != null) }),
            new GOAPState("Target in Range", new List<object> {(target.GetType() == typeof(GameEntity)) ? (HexTransform.CalcHexManhattanDist(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)target.GetComponent<GameEntity>().hexTransform.RowColumn.x, (int)target.GetComponent<GameEntity>().hexTransform.RowColumn.y].ASI) <= attackRange) : false }),
            new GOAPState("Target is Enemy", new List<object> {(target.GetType() == typeof(GameEntity)) ? (target.GetComponent<GameEntity>().TeamID != TeamID) : false }),
            new GOAPState("Target is Tile", new List<object> {target.GetType() == typeof(HexTile) }),
            new GOAPState("Has Path", new List<object> {(path.Count > 0) })
        };
        return worldState;
    }

    void AttackTimer()
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void Initialise(int teamId, HexTile startingTile)
    {
        TeamID = teamId;
        hexTransform = startingTile.hexTransform;
    }

    public override void GetPath()
    {
        //path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, , HeuristicFunc);
    }

    protected override void Start()
    {
        base.Start();
        SMActive = false;
        GOAP = new GOAPAgent();
        GOAP.AvailableActions.AddRange(GetComponents<GOAPAction>());
        GOAP.util.Actions.Add(attackGoal.UtilAction);
        GOAP.util.Actions.Add(buildingAttack.UtilAction);
        GOAP.util.Actions.Add(explore.UtilAction);

        foreach(UtilityAction<GOAPGoal> goal in GOAP.util.Actions)
        {
            goal.ObjectReference.Initialise(this);
        }
    }
    

}
