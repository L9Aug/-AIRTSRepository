using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SM;
using Condition;

public class Merchant : BaseUnit
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
        setupStateMachine();
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
        //StartCoroutine(aStar.ASTAR(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y].ASI, HeuristicFunc, SendPath));
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y].ASI, HeuristicFunc);
    }

    void SendPath(List<AStarInfo<HexTile>> Path)
    {
        path = Path;
    }

    float HomeHeuristicCalc()
    {
        return hexTransform.CalcHexManhattanDist(homeBuilding.hexTransform);
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
        assignedBuilding.MerchantArrived();
    }

    float GetConstructionTime()
    {
        return assignedBuilding.ConstructionTimer;
    }

    float ReturnZero()
    {
        return 0;
    }

    private void setupStateMachine()
    {
        positionCorrect.Condition = TestTiles;
        buildTimeComplete.A = GetConstructionTime;
        buildTimeComplete.B = ReturnZero;

        Transition arriveAtSite = new Transition("Arrived At Site", positionCorrect, new List<Action> { Build });
        Transition buildingComplete = new Transition("Building Complete", buildTimeComplete, new List<Action>() { ReturnHome });

        moveToBuildingSite = new State("Moving To building Site",
            new List<Transition>() { arriveAtSite },
            new List<Action> { FindDestination },
            new List<Action>() { Move },
            null);

        State build = new State("Constructing",
            new List<Transition>() { buildingComplete },
            new List<Action> { },
            new List<Action>() { },
            null);

        returnHome = new State("Retuning Home",
            new List<Transition>(),
            new List<Action> { },
            null,
            null);

        arriveAtSite.SetTargetState(build);
        buildingComplete.SetTargetState(returnHome);

        unitStateMachine = new StateMachine(null, moveToBuildingSite, build, returnHome);

        unitStateMachine.InitMachine();
    }
}
