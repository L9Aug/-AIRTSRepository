// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseBuilding : GameEntity
{

    #region Variables

    #region Public 

    /// <summary>
    /// The tiles that make up the exclusion zone for this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> exclusionZone = new List<HexTile>();

    /// <summary>
    /// The tiles that act as the entrance to this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> EntranceTiles = new List<HexTile>();

    /// <summary>
    /// The tiles that make up the area of this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> BuildingArea = new List<HexTile>();

    /// <summary>
    /// The type of building that this is.
    /// </summary>
    [Tooltip("The type of building this is.")]
    public Buildings BuildingType;

    /// <summary>
    /// The tier of this building.
    /// </summary>
    [Tooltip("The tier of this building.")]
    [Range(0, 4)]
    public int Tier;

    /// <summary>
    /// The size of this building.
    /// </summary>
    [Tooltip("The Size of this building.")]
    [Range(1, 5)]
    public int Size;

    /// <summary>
    /// The time it takes to build this building.
    /// </summary>
    [Tooltip("The time it takes to build this building.\n T0 : 0s\n T1 : 5s\n T2 : 10s\n T3 : 20s\n T4 : 40s")]
    [Range(0, 40)]
    public float ConstructionTime;

    /// <summary>
    /// The mode of production.
    /// ie. Will a barracks make Soldiers(0) or Arhers(1).
    /// 0 by default.
    /// </summary>
    [Tooltip("The mode of production.\nie. Will a barracks make Soldiers(0) or Arhers(1).\n0 by default.")]
    [Range(0, 1)]
    public int ProductionMode = 0;
    
    /// <summary>
    /// The time left for the construction of the building.
    /// </summary>
    [HideInInspector]
    public float ConstructionTimer = 0;

    /// <summary>
    /// The time left until this building dissabpears from the map.
    /// </summary>
    [HideInInspector]
    public float DestructionTimer = 0;

    public delegate void ConstructionCallbackFunction();

    public ConstructionCallbackFunction ConstructionCallback
    {
        set
        {
            ConstructionCallbacks.Add(value);
        }
    }

    /// <summary>
    /// The items stored in this buildings.
    /// </summary>
    [HideInInspector]
    public List<StorageItem> ItemsStored = new List<StorageItem>();

    #endregion

    #region Protected

    /// <summary>
    /// The object that hold the 3D model data for construction. 
    /// </summary>
    protected GameObject ConstructionObject;

    /// <summary>
    /// The gameobject that has the 3D model under it.
    /// </summary>
    protected GameObject OperationalModelData;

    protected int BuildingUnits = 0;

    protected List<GameEntity> OnMapCouriers = new List<GameEntity>();

    protected int CourierCount = 0;

    #endregion

    #region Private

    private List<ConstructionCallbackFunction> ConstructionCallbacks = new List<ConstructionCallbackFunction>();

    private int MerchantCount = 0;

    #endregion

    #endregion

    #region Classes

    #region Public    

    #endregion

    #endregion

    #region Functions

    #region Public

    public void BuilderArrived()
    {
        ++BuildingUnits;
    }

    public void MerchantArrived()
    {
        ++MerchantCount;
    }

    public void ConfigureBuilding(int Q, int R, int teamID)
    {
        hexTransform = new HexTransform(Q, R);
        Health = MaxHealth;
        TeamID = teamID;
    }

    public void ConfigureBuilding(float Q, float R, int teamID)
    {
        ConfigureBuilding((int)Q, (int)R, teamID);
    }

    public void ConfigureBuilding(Vector2 Loc, int teamID)
    {
        ConfigureBuilding((int)Loc.x, (int)Loc.y, teamID);
    }

    public virtual void BuildingUpdate() { }

    public List<KalamataTicket> GetTicketForProducts(ref List<Products> products)
    {
        List<KalamataTicket> tickets = new List<KalamataTicket>();

        // only loop the usually larger list once.
        foreach (StorageItem item in ItemsStored)
        {
            // if the item isn't reserved then continue tests on it.
            if (!item.Reserved)
            {
                // this usuially being the smaller list gets looped more often.
                foreach (Products product in products)
                {
                    // if the product is the one we want, create a ticket for it and remove it from the critea.
                    if (product == item.Product)
                    {
                        KalamataTicket nTicket = new KalamataTicket(product, this, item.ReserveProduct());
                        tickets.Add(nTicket);
                        products.Remove(product);
                        break;
                    }
                }
            }
            if (products.Count == 0) break;
        }

        return tickets;
    }

    public bool TestForProducts(params Products[] products)
    {
        foreach (StorageItem item in ItemsStored)
        {
            if (!item.Reserved)
            {
                foreach (Products product in products)
                {
                    if (item.Product == product)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public virtual List<Products> DeliverProducts(params Products[] products)
    {
        return new List<Products>();
    }

    public List<Products> GetProductsFromStorage(params Products[] products)
    {
        List<Products> RetList = new List<Products>();

        foreach (Products product in products)
        {
            if (TestForProducts(product))
            {
                StorageItem MyProduct = (ItemsStored.Find(x => x.Reserved == false && x.Product == product));
                ItemsStored.Remove(MyProduct);
                RetList.Add(MyProduct.Product);
            }
        }

        return RetList;
    }

    public List<Products> EmptyStorage()
    {
        List<StorageItem> AvailableProducts = ItemsStored.FindAll(x => x.Reserved == false);

        List<Products> RetList = new List<Products>();

        for(int i = 0; i < AvailableProducts.Count; ++i)
        {
            RetList.Add(AvailableProducts[i].Product);
        }

        return RetList;
    }

    public int ProductCount(Products product)
    {
        return ItemsStored.FindAll(x => x.Reserved == false && x.Product == product).Count;
    }

    public Products RedeemKalamataTicket(KalamataTicket Ticket)
    {
        StorageItem myItem = ItemsStored.Find(x => x.ReserveID == Ticket.ReservationID);
        ItemsStored.Remove(myItem);
        return myItem.Product;
    }

    public virtual void CourierReturned()
    {
        ++CourierCount;
    }

    #endregion

    #region Protected

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        OperationalModelData = transform.FindChild("ModelData").gameObject;
        if (OperationalModelData.GetComponent<Renderer>() != null)
        {
            OperationalModelData.GetComponent<Renderer>().material.color = TeamManager.TM.Teams[TeamID].TeamColour;
        }
        else
        {
            Renderer[] ModelData = OperationalModelData.GetComponentsInChildren<Renderer>();
            for(int i = 0; i < ModelData.Length; ++i)
            {
                ModelData[i].material.color = TeamManager.TM.Teams[TeamID].TeamColour;
            }
        }
        SetUpStateMachine();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Start();
        BuildingStateMachine.SMUpdate();
    }

    protected virtual void ConstructionFinished()
    {
        //Destroy the Construction model
        Destroy(ConstructionObject);

        // Here change the update the model to the actual building model rather than the construction model.
        OperationalModelData.SetActive(true);

        //Building.BuildingArea = MapGenerator.Map[(int)Building.hexTransform.RowColumn.x, (int)Building.hexTransform.RowColumn.y].GetHexArea(Building.Size);

        foreach(HexTile hex in BuildingArea)
        {
            hex.ClearConnections();
        }

        if (ConstructionCallbacks.Count > 0)
        {
            for (int i = 0; i < ConstructionCallbacks.Count; ++i)
            {
                if (ConstructionCallbacks[i] != null)
                {
                    ConstructionCallbacks[i]();
                }
            }
        }
    }

    protected virtual void BuildingDestroyed()
    {
        TeamManager.TM.Teams[TeamID].BuildingDestroyed(this);        

        // Spawn Destroyed model.
        // maybe some smoke?
    }

    protected virtual void BeginOperational() { }

    protected StorageBuilding FindStorageBuilding()
    {
        // find closest storage building with space
        List<BaseBuilding> StorageBuildings = TeamManager.TM.Teams[TeamID].BuildingsList.FindAll(x => x is StorageBuilding);

        StorageBuilding storageBuildingToUse = null;

        float dist = float.MaxValue;

        for (int i = 0; i < StorageBuildings.Count; ++i)
        {
            float tempDist = Vector3.Distance(transform.position, StorageBuildings[i].transform.position);

            if (tempDist < dist)
            {
                if (((StorageBuilding)StorageBuildings[i]).RemainingSpace > 5)
                {
                    storageBuildingToUse = (StorageBuilding)StorageBuildings[i];
                }
            }
        }

        return storageBuildingToUse;
    }

    protected virtual void SendCourierWithProductsFunc()
    {
        StorageBuilding storageBuildingToUse = FindStorageBuilding();

        if (storageBuildingToUse != null)
        {

            // send courier with products
            if (storageBuildingToUse != null)
            {
                HexTile tileToUse = EntranceTiles[Random.Range(0, EntranceTiles.Count - 1)];
                OnMapCouriers.Add(Instantiate(GlobalAttributes.Global.Units[(int)Units.Courier], tileToUse.transform.position, Quaternion.identity, transform));
                OnMapCouriers[OnMapCouriers.Count - 1].GetComponent<Courier>().AddToInventory(EmptyStorage());
                OnMapCouriers[OnMapCouriers.Count - 1].GetComponent<Courier>().SetOutboundCourier(this, storageBuildingToUse);
                CourierCount--;
            }
            //use on map couriers to track who has been sent.
            
        }
    }

    #endregion

    #region Private

    private void BeginConstruction()
    {
        // Create appropriate 3D model for being under construction.
        ConstructionObject = (GameObject)Instantiate(Resources.Load("ConstructionBuildings/ConstructionBuilding0" + Size));
        ConstructionObject.transform.SetParent(transform, false);
        if (ConstructionObject.GetComponent<Renderer>() != null)
        {
            ConstructionObject.GetComponent<Renderer>().material.color = TeamManager.TM.Teams[TeamID].TeamColour;
        }
        // Hide the operational model data.
        OperationalModelData.SetActive(false);

        ConstructionTimer = ConstructionTime;
    }

    private void ConstructionUpdate()
    {
        if (BuildingUnits == Tier && (Tier == 4 ? MerchantCount == 1 : true))
        {
            ConstructionTimer -= Time.deltaTime;
        }
    }

    private void DestructionUpdate()
    {
        DestructionTimer -= Time.deltaTime;
        if(DestructionTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool HasConstructionFinished()
    {
        if(ConstructionTimer <= 0)
        {
            return true;
        }
        return false;
    }

    private bool IsBuildingDestroyed()
    {
        return Health <= 0;
    }

    private void SetUpStateMachine()
    {
        // Configure Transistion requiremnts.
        Condition.BoolCondition ConstructionCondition = new Condition.BoolCondition(HasConstructionFinished);
        Condition.BoolCondition DestructionCondition = new Condition.BoolCondition(IsBuildingDestroyed);

        // Create Transitions.
        SM.Transition ConstructionFinishedTrans = new SM.Transition("Construction Finished", ConstructionCondition, ConstructionFinished);
        SM.Transition BuildingDestroyedTrans = new SM.Transition("Building Destroyed", DestructionCondition, BuildingDestroyed);

        // Create States.
        SM.State UnderConstruction = new SM.State("Under Construction", 
            new List<SM.Transition>() { ConstructionFinishedTrans }, 
            new List<SM.Action>() { BeginConstruction },
            new List<SM.Action>() { ConstructionUpdate },
            new List<SM.Action>() {  });

        SM.State Operational = new SM.State("Operational",
            new List<SM.Transition>() { BuildingDestroyedTrans },
            new List<SM.Action>() { BeginOperational },
            new List<SM.Action>() { BuildingUpdate },
            null);

        SM.State Destroyed = new SM.State("Destroyed",
            null,
            null,
            new List<SM.Action>() { DestructionUpdate },
            null);

        // Add target state to transitions
        ConstructionFinishedTrans.SetTargetState(Operational);
        BuildingDestroyedTrans.SetTargetState(Destroyed);

        BuildingStateMachine = new SM.StateMachine(null, UnderConstruction, Operational, Destroyed);    

        // Initialise the machine.
        BuildingStateMachine.InitMachine();
    }

    #endregion

    #endregion

    #region StateMachines

    protected SM.StateMachine BuildingStateMachine;

    #endregion

}
