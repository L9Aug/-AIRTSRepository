using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;
using Condition;
using GOAP;

public class MilitaryUnit : BaseUnit
{
    public float attackSpeed;
    public float attackDuration;
    public float damage;
    public int attackRange;

    public GameObject target;

    public GOAPAgent GOAP;

    public AttackTargetGoal attackGoal;

    public void SetTarget(GameObject Target)
    {
        target = Target;
    }

    public List<GOAPState> GetWorldState()
    {
        List<GOAPState> worldState = new List<GOAPState>();
        worldState.Add(new GOAPState("Has Target", (target != null)));
        if (target.GetType() == typeof(GameEntity))
        {
            worldState.Add(new GOAPState("Target in Range", (HexTransform.CalcHexManhattanDist(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)target.GetComponent<GameEntity>().hexTransform.RowColumn.x, (int)target.GetComponent<GameEntity>().hexTransform.RowColumn.y].ASI) < attackRange)));
            worldState.Add(new GOAPState("Target is Enemy", (target.GetComponent<GameEntity>().TeamID != TeamID)));
        }
        if(target.GetType() == typeof(HexTile))
        {

        }
        worldState.Add(new GOAPState("Target is Tile", target.GetType() == typeof(HexTile)));
        return worldState;
    }

    void Start()
    {
        SMActive = false;
        GOAP.AvailableActions.AddRange(GetComponents<GOAPAction>());
        GOAP.util.Actions.Add(attackGoal.UtilAction);
    }
    

}
