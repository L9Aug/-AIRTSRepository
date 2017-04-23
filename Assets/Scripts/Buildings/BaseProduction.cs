// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public int CourierCount = 0;

    public List<GameEntity> OnMapCouriers = new List<GameEntity>();

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
        // create calamata tickets for the products used to create for this building

        // create unit with tickets
    }

    protected virtual void SendCourierWithProductsFunc()
    {
        // find closest storage building with space

        // send courier with products
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

    /// <summary>
    /// Needs to be implemented still
    /// </summary>
    /// <returns></returns>
    protected StorageBuilding FindStorageBuilding()
    {
        return null;
    }

    protected object IsThereAStorageBuilding()
    {
        return (FindStorageBuilding() != null) ? true : false;
    }

    /// <summary>
    /// Requires implementation
    /// </summary>
    /// <returns></returns>
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

    #endregion

    #region Private

    #endregion

    #endregion

    #region Decicion Trees

    protected DT.DecisionTree ProductionTree;

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseProduction))]
[CanEditMultipleObjects]
public class BaseProductionEditor : BaseBuildingEditor
{

    private BaseProduction myBPTarget;

    protected override void OnEnable()
    {
        base.OnEnable();
        myBPTarget = (BaseProduction)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            EditorGUILayout.LabelField("Production Time:", myBPTarget.ProductionTimer.ToString("F2") + " / " + myBPTarget.ProductionTime.ToString());
        }
    }

}
#endif