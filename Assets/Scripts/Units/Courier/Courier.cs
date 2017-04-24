using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Condition;
//using SM;
using GOAP;

public class Courier : BaseUnit
{
    public BaseBuilding HomeBuilding;
    public BaseBuilding DestinationBuilding;

    public GOAPAgent GOAP = new GOAPAgent();

    public int inventorySpace = 10;
    public List<Products> inventory = new List<Products>();
    public List<Products> shoppingList = new List<Products>();
    public List<KalamataTicket> ticketList = new List<KalamataTicket>();

    DepositGoal deposit = new DepositGoal();
    PickupGoal pickup = new PickupGoal();


    private void Awake()
    {
        GOAP = new GOAPAgent();
        SMActive = false;
        //GOAP.AvailableActions.AddRange(GetComponents<GOAPAction>());
        foreach (GOAPAction action in GOAP.AvailableActions)
        {
            action.Agent = GOAP;
        }
        deposit.Initialise(this);
        pickup.Initialise(this);
        GOAP.util.Actions = new List<UtilityAction<GOAPGoal>>
        {
            deposit.UtilAction,
            pickup.UtilAction
        };
    }

    protected override void Start()
    {
        base.Start();
        GOAP.Planner.GetActionPlan(GOAP, GetWorldState(), GOAP.AvailableActions, GOAP.util.RunUtilityEngine()[0]);
    }

    public List<GOAPState> GetWorldState()
    {
        List<GOAPState> worldState = new List<GOAPState>
        {
            new GOAPState("Has Inventory Space", new List<object> {(inventory.Count < inventorySpace) }),
            new GOAPState("Must Deposit Products", new List<object> {(inventory.Count >= inventorySpace) || (ticketList.Count == 0) }),
            new GOAPState("Has Destination", new List<object> {(DestinationBuilding != null) }),
            new GOAPState("Tasks Complete", new List<object> {(inventory.Count == 0) && (ticketList.Count == 0)}),
            new GOAPState("At Home", new List<object> {(hexTransform.Position == HomeBuilding.hexTransform.Position) }),
            new GOAPState("At Destination", new List<object> {(hexTransform.Position == DestinationBuilding.hexTransform.Position) }),
            new GOAPState("Products Available", new List<object> {TeamManager.TM.Teams[TeamID].FindProducts(shoppingList.ToArray()) }),
            new GOAPState("Has Tickets", new List<object> {(ticketList.Count > 0) }),
            new GOAPState("Has Path", new List<object> {(path.Count > 0) })
        };
        return worldState; 
    }

    protected  override void Update()
    {
        base.Update();
        GOAP.SetWorldState(GetWorldState());
        foreach(Action a in GOAP.UpdateAgent())
        {
            a();
        }
    }

    public void SetOutboundCourier(BaseBuilding home, BaseBuilding destination)
    {
        HomeBuilding = home;
        DestinationBuilding = destination;
    }

    public void AddToInventory(List<Products> prods)
    {
        foreach(Products product in prods)
        {
            inventory.Add(product);
        }
    }

    public override void GetNewDestination()
    {
        BaseBuilding temp = new BaseBuilding();
        temp = HomeBuilding;
        HomeBuilding = DestinationBuilding;
        DestinationBuilding = temp;
    }

    public override void GetPath()
    {
        path = aStar.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y].ASI, DestinationBuilding.EntranceTiles[Random.Range(0, DestinationBuilding.EntranceTiles.Count)].ASI, HeuristicFunc);
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
