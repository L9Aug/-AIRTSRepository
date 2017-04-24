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

        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < ProductionRequirements.Count; ++j)
            {
                //ProductionStorage.Add(ProductionRequirements[j] == Products.Food ? Products.Bread : ProductionRequirements[j]);
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
                for (int i = 0; i < UnitOutput.Count; ++i)
                {
                    print("New " + UnitOutput[i].ToString() + " for Team " + TeamID);
                    HexTile tileToUse = EntranceTiles[Random.Range(0, EntranceTiles.Count - 1)];
                    switch (UnitOutput[i])
                    {
                        case Units.Soldier:
                            CreateMilitaryUnit(Units.Soldier, tileToUse);
                            break;
                        case Units.Archer:
                            CreateMilitaryUnit(Units.Archer, tileToUse);
                            break;
                        case Units.Catapult:
                            CreateMilitaryUnit(Units.Catapult, tileToUse);
                            break;
                        case Units.Builder:

                            break;
                        case Units.Courier:

                            break;
                        case Units.Merchant:
                            TeamManager.TM.Teams[TeamID].Population.MerchantCount++;
                            TeamManager.TM.Teams[TeamID].Population.Merchants.Add(Instantiate(GlobalAttributes.Global.Units[(int)Units.Merchant], tileToUse.transform.position, Quaternion.identity, transform.parent));
                            TeamManager.TM.Teams[TeamID].Population.Merchants[TeamManager.TM.Teams[TeamID].Population.Merchants.Count - 1].GetComponent<Merchant>().Initialise(this, TeamManager.TM.Teams[TeamID].BuildingsList[0], TeamID, tileToUse);
                            break;
                        case Units.Citizen:
                            TeamManager.TM.Teams[TeamID].Population.CitizenCount++;
                            TeamManager.TM.Teams[TeamID].Population.Citizens.Add(Instantiate(GlobalAttributes.Global.Units[(int)Units.Citizen], tileToUse.transform.position, Quaternion.identity, transform.parent));
                            TeamManager.TM.Teams[TeamID].Population.Citizens[TeamManager.TM.Teams[TeamID].Population.Citizens.Count - 1].GetComponent<Citizen>().Initialise(this, TeamManager.TM.Teams[TeamID].BuildingsList[0], TeamID, tileToUse);
                            break;
                    }
                    ProductionFinished();
                }
                ProductionTimer = 0;
                inProduction = false;
            }
        }
    }

    void CreateMilitaryUnit(Units unit, HexTile tileToUse)
    {
        TeamManager.TM.Teams[TeamID].Population.Military.Add(Instantiate(GlobalAttributes.Global.Units[(int)unit], tileToUse.transform.position, Quaternion.identity, transform.parent));
        TeamManager.TM.Teams[TeamID].Population.Military[TeamManager.TM.Teams[TeamID].Population.Military.Count - 1].GetComponent<MilitaryUnit>().Initialise(TeamID, tileToUse);
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

    private object FutureProductCheck()
    {
        if(GetMissingProducts().Count > 0)
        {
            return false;
        }
        return true;
    }

    private object AreProductsOnMap()
    {
        return TeamManager.TM.Teams[TeamID].FindProducts(ProductionRequirements.ToArray());
    }

    private object HaveISentACourier()
    {
        return CourierCount == 0;
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

        Leaf SendCourierForProducts = new Leaf(SendCourierForProductsFunc);

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
