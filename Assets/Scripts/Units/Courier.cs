using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Condition;
using SM;
using GOAP;

public class Courier : BaseUnit
{
    public BaseBuilding homeBuilding;
    public BaseBuilding destination;


    public GOAPAgent GOAP;

    HexTile position = new HexTile();

    int inventorySpace = 10;
    List<Products> inventory = new List<Products>();
    List<Products> shoppingList = new List<Products>();
    

    void Start()
    {
        //SetupStateMachine();
    }

    public List<GOAPState> GetWorldState()
    {
        List<GOAPState> worldState = new List<GOAPState>();
        worldState.Add(new GOAPState("Inventory Full", (inventory.Count >= inventorySpace)));
        worldState.Add(new GOAPState("Shopping List Empty", (shoppingList.Count == 0)));
        worldState.Add(new GOAPState("Has Destination", (destination != null)));
        worldState.Add(new GOAPState("At Home", (hexTransform.Position == homeBuilding.hexTransform.Position)));
        worldState.Add(new GOAPState("At Destination", (hexTransform.Position == destination.hexTransform.Position)));

        return worldState; 
    }

    /*void GetPathHome()
    {
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, 
            MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y].ASI, 
            HexTransform.CalcHexManhattanDist);
    }

    void ReturnHome()
    {
        // Add this unit to the building
        // Remove it from the map's list
        Destroy(gameObject);
    }

    void SetupStateMachine()
    {
        shoppingNotEmpty.Condition = shoppingListEmpty;
        inventoryNotFull.Condition = inventoryFull;
        canFindMore.A = shoppingNotEmpty;
        canFindMore.B = inventoryNotFull;

        positionCorrect.A = columnCorrect;
        positionCorrect.B = rowCorrect;

        allProductsFound = new Transition("All products found", shoppingListEmpty, GetPathHome);
        /*findNextProduct = new Transition("Find next products", canFindMore, new List<Action>());
        getHome = new Transition("Get home", positionCorrect, ReturnHome);

        pickUp = new State("Pick up",
            new List<Transition>() { allProductsFound, findNextProduct },
            null,
            new List<Action>() { Move },
            null);

        returnProduct = new State("Return Product",
            new List<Transition>() { getHome },
            null,
            new List<Action>() { Move },
            null);

        atHome = new State("At home",
            null,
            null,
            new List<Action>(),
            null);

        allProductsFound.SetTargetState(returnProduct);
        findNextProduct.SetTargetState(pickUp);
        getHome.SetTargetState(atHome);

        unitStateMachine = new StateMachine(null, pickUp, returnProduct, atHome);
        unitStateMachine.InitMachine();
        
    }
    

    /*void FindProduct()
    {
        List<StorageBuilding> allStorageBuildings = new List<StorageBuilding>();
        List<StorageBuilding> buildingsWithProduct = new List<StorageBuilding>();

        // Find all warehouses and town halls on your team
        for(int i = 0; i < GlobalAttributes.Global.Buildings.Count; ++i)
        {
            if((GlobalAttributes.Global.Buildings[i].BuildingType == Buildings.Warehouse || GlobalAttributes.Global.Buildings[i].BuildingType == Buildings.TownHall) && GlobalAttributes.Global.Buildings[i].TeamID == TeamID)
            {
                allStorageBuildings.Add((StorageBuilding)GlobalAttributes.Global.Buildings[i]);
            }
        }

        // Find all of those buildings that contain the products you need
        for(int i = 0; i < allStorageBuildings.Count; ++i)
        {
            for (int j = 0; j < shoppingList.Count; ++j)
            {
                if (allStorageBuildings[i].ItemsStored.Contains(shoppingList[j]))
                {
                    buildingsWithProduct.Add(allStorageBuildings[i]);
                }
            }
        }

        // Remove the duplicate buildings
        for(int i = 0; i < buildingsWithProduct.Count; ++i)
        {
            if(buildingsWithProduct[i] == buildingsWithProduct[i + 1])
            {
                buildingsWithProduct.RemoveAt(i + 1); 
                i--;
            }
        }

        // If there is a building with the product, find the closest building
        if (buildingsWithProduct.Count > 0)
        {
            path = DijkstraImplementation.DJI.DijkstraToBuilding(MapGenerator.Map[hexTransform.Q, hexTransform.R], new List<Buildings> { Buildings.TownHall, Buildings.Warehouse }, TeamID);
        }
     }*/
}
