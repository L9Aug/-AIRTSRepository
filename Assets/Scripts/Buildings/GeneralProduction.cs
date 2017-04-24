// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Decisions;
using DT;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneralProduction : BaseProduction
{

    #region Variables

    #region Public    

    /// <summary>
    /// The product this building creates.
    /// </summary>
    [Tooltip("The product this building creates.")]
    public Products OutputProduct;

    #endregion

    #endregion

    #region Functions

    #region Public

    #endregion

    #region Protected

    protected override void Start()
    {
        base.Start();

        SetupDecisionTree();

        for(int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < ProductionRequirements.Count; ++j) {
                //ProductionStorage.Add(ProductionRequirements[j] == Products.Food ? Products.Water : ProductionRequirements[j]);
            }
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator ProductionCycle()
    {
        while (inProduction)
        {
            yield return null;
            ProductionTimer += Time.deltaTime;

            if (ProductionTimer >= ProductionTime)
            {
                ItemsStored.Add(new StorageItem(OutputProduct));
                ProductionFinished();
                ProductionTimer = 0;
                inProduction = false;
            }
        }
    }

    protected override IEnumerator DecisionTreeRunIntervals()
    {
        while (ProductionTree != null)
        {
            TreeTick();
            yield return null;
        }
    }

    protected override void BeginProduction()
    {
        base.BeginProduction();
        for (int i = 0; i < ProductionRequirements.Count; ++i)
        {
            ProductionStorage.Remove(ProductionStorage.Find(x => x == ProductionRequirements[i]));
        }
    }

    #endregion

    #region Private

    private object TestOutputStorage()
    {
        return (ItemsStored.Count >= 5) ? true : false;
    }

    private object FutureProductsTest()
    {
        if (GetMissingProducts().Count > 0)
        {
            return false;
        }
        return true;
    }

    private object AreProductsOnMap()
    {
        return TeamManager.TM.Teams[TeamID].FindProducts(ProductionRequirements.ToArray());
    }

    private void SetupDecisionTree()
    {
        ObjectDecision isInProduction = new ObjectDecision(InProduction);

        ObjectDecision doIHaveProductsForFuture = new ObjectDecision(FutureProductsTest);

        ObjectDecision doIHaveProductsForProduction = new ObjectDecision(ProductCheck);

        ObjectDecision isStorageOutputFull = new ObjectDecision(TestOutputStorage);

        ObjectDecision canIGetProductsForProduction = new ObjectDecision(AreProductsOnMap);

        ObjectDecision isThereAnAvailableStorageFacility = new ObjectDecision(IsThereAStorageBuilding);

        ObjectDecision DoIHaveACourier = new ObjectDecision(IsThereAnAvailableCourier);

        Leaf WaitForNextCycle = new Leaf();

        Leaf beginProduction = new Leaf(BeginProduction);

        Leaf SendCourierWithProducts = new Leaf(SendCourierWithProductsFunc);

        Leaf SendCourierForProducts = new Leaf(SendCourierForProductsFunc);

        Vertex HasCourierBeenSentForProducts = new Vertex(DoIHaveACourier, WaitForNextCycle, SendCourierForProducts);

        Vertex HasCourierBeenSentWithProducts = new Vertex(DoIHaveACourier, WaitForNextCycle, SendCourierWithProducts);

        Vertex IsThereStorageFacility = new Vertex(isThereAnAvailableStorageFacility, HasCourierBeenSentWithProducts, WaitForNextCycle);

        Vertex IsOutputStorageFull = new Vertex(isStorageOutputFull, IsThereStorageFacility, beginProduction);

        Vertex CanIGetProductsForProduction = new Vertex(canIGetProductsForProduction, HasCourierBeenSentForProducts, WaitForNextCycle);

        Vertex DoIHaveProductsForProduction = new Vertex(doIHaveProductsForProduction, IsOutputStorageFull, CanIGetProductsForProduction);

        Vertex DoIHaveProductsForFutureProductions = new Vertex(doIHaveProductsForFuture, WaitForNextCycle, CanIGetProductsForProduction);

        Vertex IsInProduction = new Vertex(isInProduction, DoIHaveProductsForFutureProductions, DoIHaveProductsForProduction);

        ProductionTree = new DecisionTree(IsInProduction);
    }

    #endregion

    #endregion

}
