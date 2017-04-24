// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseProduction : BaseBuilding
{

    #region Enums

    #region Public

    public enum ProductionTypes { Unit, Resource, Culture }

    #endregion

    #endregion

    #region Variables

    #region Public

    public float ProductionTime;    

    // used for testing victrory conditions
    public ProductionTypes ProductionType;

    public float ProductionTimer = 0;

    public bool inProduction = false;

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The products required for this building to function.")]
    public List<Products> ProductionRequirements = new List<Products>();

    /// <summary>
    /// Storage for production resources.
    /// </summary>
    [HideInInspector]
    public List<Products> ProductionStorage = new List<Products>();

    #endregion

    #region Protected

    #endregion

    #endregion

    #region Functions

    #region Public  

    public virtual void TreeTick()
    {
        if(ProductionTree != null)
        {
            ProductionTree.RunTree();
        }
    }

    public override void BuildingUpdate()
    {
        base.BuildingUpdate();
    }

    public override List<Products> DeliverProducts(params Products[] products)
    {
        ProductionStorage.AddRange(products);
        return new List<Products>();
    }

    #endregion

    #region Protected

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void SendCourierForProductsFunc()
    {
        // create kalamata tickets for the products used to create for this building
        List<Products> requiredProducts = GetMissingProducts();

        if (requiredProducts.Count > 0)
        {
            List<KalamataTicket> Tickets = TeamManager.TM.Teams[TeamID].ReserveProducts(hexTransform, requiredProducts.ToArray());

            // create unit with tickets


        }
    }

    protected override void BeginOperational()
    {
        base.BeginOperational();
        StartCoroutine(DecisionTreeRunIntervals());
    }

    protected virtual IEnumerator DecisionTreeRunIntervals()
    {
        while(ProductionTree != null)
        {
            TreeTick();

            yield return null;
        }
    }

    protected object InProduction()
    {
        return inProduction;
    }

    protected object IsThereAStorageBuilding()
    {
        return (FindStorageBuilding() != null) ? true : false;
    }

    protected object IsThereAnAvailableCourier()
    {
        return (CourierCount > 0) ? true : false;
    }

    protected virtual void BeginProduction()
    {
        inProduction = true;
        StartCoroutine(ProductionCycle());
    }

    protected virtual IEnumerator ProductionCycle()
    {
        yield return null;
    }

    protected void ProductionFinished()
    {
        switch (ProductionType)
        {
            case ProductionTypes.Unit:
            default:
                break;
            case ProductionTypes.Resource:
                VictoryController.VC.AddEconomicScore(Tier, TeamID);
                break;
            case ProductionTypes.Culture:
                // if we want generation of culture to add to economic do so here.
                VictoryController.VC.AddCulturalScore(1, TeamID);
                break;
        }
    }

    protected override void ConstructionFinished()
    {
        base.ConstructionFinished();
        CourierCount = BuildingUnits;
    }

    protected List<Products> GetMissingProducts()
    {
        List<Products> RetList = new List<Products>();

        for(int i = 0; i < ProductionRequirements.Count; ++i)
        {
            int timesNeeded = ProductionRequirements.FindAll(x => x == ProductionRequirements[i]).Count;

            int missing = (ProductionRequirements[i] != Products.Food ? Mathf.Clamp((timesNeeded * 5) - ProductionStorage.FindAll(x => x == ProductionRequirements[i]).Count, 0, timesNeeded * 5) : Mathf.Clamp((timesNeeded * 5) - FoodCount(), 0, timesNeeded * 5));

            for(int j = 0; j < missing; ++j)
            {
                RetList.Add(ProductionRequirements[i]);
            }

            i += timesNeeded - 1;
        }

        return RetList;
    }

    protected object ProductCheck()
    {
        bool haveProducts = true;

        for (int i = 0; i < ProductionRequirements.Count; ++i)
        {
            int timesNeeded = ProductionRequirements.FindAll(x => x == ProductionRequirements[i]).Count;

            int have = (ProductionRequirements[i] != Products.Food ? ProductionStorage.FindAll(x => x == ProductionRequirements[i]).Count : FoodCount());

            if (have < timesNeeded)
            {
                haveProducts = false;
            }

            i += timesNeeded - 1;
        }

        return haveProducts;
    }

    #endregion

    #region Private

    private int FoodCount()
    {
        return ProductionStorage.FindAll(x => x == Products.Fish || x == Products.Fruit || x == Products.Water || x == Products.Meat || x == Products.Wine || x == Products.Bread).Count;
    }

    #endregion

    #endregion

    #region Decicion Trees

    protected DT.DecisionTree ProductionTree;

    #endregion
}
