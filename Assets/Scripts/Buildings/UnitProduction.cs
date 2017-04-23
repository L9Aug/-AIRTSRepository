// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DT;
using Decisions;

public class UnitProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The Products required for this building to function.")]
    public List<Products> ProductionRequirements = new List<Products>();

    /// <summary>
    /// The unit/s that this building creates.
    /// </summary>
    [Tooltip("The unit/s that this building creates.")]
    public List<Units> UnitOutput = new List<Units>();

    #endregion

    #endregion

    #region Functions

    #region Protected

    protected override void Start()
    {
        base.Start();

        SetupDecisionTree();
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
                for (int i = 0; i < UnitOutput.Count; ++i)
                {
                    print("New " + UnitOutput[i].ToString() + " for Team " + TeamID);
                    ProductionFinished();
                }
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
        for(int i = 0; i < ProductionRequirements.Count; ++i)
        {
            if (ProductionRequirements[i] != Products.Food)
            {
                ProductionStorage.Remove(ProductionStorage.Find(x => x == ProductionRequirements[i]));
            }
            else
            {
                RemoveFoodFromStorage();
            }
        }
    }

    #endregion

    #region Private

    private void RemoveFoodFromStorage()
    {
        if(!ProductionStorage.Remove(Products.Fish))
        {
            if (!ProductionStorage.Remove(Products.Fruit))
            {
                if (!ProductionStorage.Remove(Products.Water))
                {
                    if (!ProductionStorage.Remove(Products.Meat))
                    {
                        if (!ProductionStorage.Remove(Products.Wine))
                        {
                            ProductionStorage.Remove(Products.Bread);                        
                        }
                    }
                }
            }
        }
    }

    private object ProductCheck()
    {
        bool haveProducts = true;

        for(int i = 0; i < ProductionRequirements.Count; ++i)
        {
            if(ProductionStorage[i] == 0)
            {
                haveProducts = false;
                break;
            }
        }

        return haveProducts;
    }

    private object FutureProductCheck()
    {
        bool haveProducts = true;

        for (int i = 0; i < ProductionRequirements.Count; ++i)
        {
            /*
            if(ProductionStorage[i] < 5)
            {
                haveProducts = false;
                break;
            }
            */
        }

        return haveProducts;
    }

    private object AreProductsOnMap()
    {
        return TeamManager.TM.Teams[TeamID].FindProducts(ProductionRequirements.ToArray());
    }

    private object HaveISentACourier()
    {
        return false;
    }

    private void SetupDecisionTree()
    {
        // create decisions
        ObjectDecision isInProduction = new ObjectDecision(InProduction);

        ObjectDecision doIHaveProductsForProduction = new ObjectDecision(ProductCheck);

        ObjectDecision doIHaveProductsForFuture = new ObjectDecision(FutureProductCheck);

        ObjectDecision canIGetProductsForProduction = new ObjectDecision(AreProductsOnMap);

        ObjectDecision haveISentACouierForProducts = new ObjectDecision(HaveISentACourier);

        // create leaves
        Leaf WaitForNextCycle = new Leaf();

        Leaf SendCourierForProducts = new Leaf();

        Leaf BeginProductionLeaf = new Leaf(BeginProduction);

        // create verticies
        Vertex HaveISentCourier = new Vertex(haveISentACouierForProducts, WaitForNextCycle, SendCourierForProducts);

        Vertex CanIGetProductsForProduction = new Vertex(canIGetProductsForProduction, HaveISentCourier, WaitForNextCycle);

        Vertex DoIHaveProductsForThisProduction = new Vertex(doIHaveProductsForProduction, BeginProductionLeaf, CanIGetProductsForProduction);

        Vertex DoIHaveProductsForFutureProductions = new Vertex(doIHaveProductsForFuture, WaitForNextCycle, CanIGetProductsForProduction);

        Vertex IsInProduction = new Vertex(isInProduction, DoIHaveProductsForFutureProductions, DoIHaveProductsForThisProduction);

        // create decition tree
        ProductionTree = new DecisionTree(IsInProduction);
    }

    #endregion

    #endregion
}
