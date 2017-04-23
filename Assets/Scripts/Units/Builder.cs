using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Condition;
using SM;

public class Builder : BaseUnit
{
    public BaseBuilding assignedBuilding;
    public BaseBuilding homeBuilding;

    State moveToBuildingSite;
    State returnHome;

    List<Products> inventory;

    BoolCondition positionCorrect = new BoolCondition();

    AGreaterThanB buildTimeComplete = new AGreaterThanB();


    

    // Use this for initialization
    void Start ()
    {
        setupStateMachine();
	}

    void ReturnHome()
    {
        // Add this to the building and the global count
        // Remove from the map list
        Destroy(gameObject);
    }

    public void AssignInventory(List<Products> giveItems)
    {
        inventory = giveItems;
    }

    public void AssignDestination(BaseBuilding dest)
    {
        assignedBuilding = dest;
    }

    void FindHome()
    {
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y].ASI, HexTransform.CalcHexManhattanDist);
    }

    void FindDestination()
    {
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y].ASI, HexTransform.CalcHexManhattanDist);
    }

    bool TestTiles()
    {
        return hexTransform == assignedBuilding.hexTransform;
    }

    void Build()
    {
        Debug.Log("Build has not beein implemented fully");
        //assignedBuilding.Build;
    }

    private void setupStateMachine()
    {
        positionCorrect.Condition = TestTiles;

        Transition arriveAtSite = new Transition("Arrived At Site", positionCorrect, new List<Action>());
        Transition buildingComplete = new Transition("Building Complete", buildTimeComplete, new List<Action>());

        moveToBuildingSite = new State("Moving To building Site",
            new List<Transition>() { arriveAtSite },
            new List<Action> { FindDestination },
            new List<Action>() { Move },
            null);

        State build = new State("Constructing",
            new List<Transition>() { arriveAtSite },
            null,
            new List<Action>() { Build },
            null);

        returnHome = new State("Retuning Home",
            new List<Transition>(),
            new List<Action> { ReturnHome },
            null,
            null);

        arriveAtSite.SetTargetState(build);
        buildingComplete.SetTargetState(returnHome);

        unitStateMachine = new StateMachine(null, moveToBuildingSite, build, returnHome);

        unitStateMachine.InitMachine();
    }
}
