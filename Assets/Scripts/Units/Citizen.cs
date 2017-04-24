using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM;
using Condition;

public class Citizen : BaseUnit
{
    public BaseBuilding assignedBuilding;
    public BaseBuilding homeBuilding;

    State moveToBuildingSite;
    State returnHome;

    BoolCondition positionCorrect = new BoolCondition();
    ALessThanB buildTimeComplete = new ALessThanB();

    public void Initialise(BaseBuilding home, BaseBuilding assign, int teamID, HexTile startTile)
    {
        TeamManager.TM.Teams[TeamID].Population.CitizenCount--;
        homeBuilding = home;
        assignedBuilding = assign;
        TeamID = teamID;
        hexTransform = startTile.hexTransform;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        SetupStateMachine();
    }

    protected override void Update()
    {
        base.Update();
    }

    void ReturnHome()
    {
        Destroy(gameObject);
    }

    public void AssignDestination(BaseBuilding dest)
    {
        assignedBuilding = dest;
    }

    void FindHome()
    {
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y].ASI, HeuristicFunc);
    }

    void SendPath(List<AStarInfo<HexTile>> Path)
    {
        path = Path;
    }

    void FindDestination()
    {
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)assignedBuilding.hexTransform.RowColumn.x, (int)assignedBuilding.hexTransform.RowColumn.y].ASI, HeuristicFunc);
    }

    bool TestTiles()
    {
        return hexTransform.RowColumn == assignedBuilding.hexTransform.RowColumn;
    }

    void Build()
    {
        assignedBuilding.BuilderArrived();
    }

    private void SetupStateMachine()
    {
        positionCorrect.Condition = TestTiles;

        Transition arriveAtSite = new Transition("Arrived At Site", positionCorrect, new List<Action> { ReturnHome });

        moveToBuildingSite = new State("Moving To building Site",
            new List<Transition>() { arriveAtSite },
            new List<Action> { FindDestination },
            new List<Action>() { Move },
            null);

        returnHome = new State("Retuning Home",
            new List<Transition>(),
            new List<Action> { },
            null,
            null);

        arriveAtSite.SetTargetState(returnHome);

        unitStateMachine = new StateMachine(null, moveToBuildingSite, returnHome);

        unitStateMachine.InitMachine();
    }
}
