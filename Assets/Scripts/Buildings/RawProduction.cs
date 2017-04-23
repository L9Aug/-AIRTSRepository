// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Decisions;
using DT;

public class RawProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The terrain/s required for this building to perform.
    /// </summary>
    [Tooltip("The terrain/s required for this building to perform.")]
    public List<TerrainTypes> TerrainRequirement = new List<TerrainTypes>();

    /// <summary>
    /// The products that this building can create.
    /// </summary>
    [Tooltip("the products that this building can create.")]
    public Products OutputProduct;

    #endregion

    #region Private

    bool hasCorrectTerrain = false;

    #endregion

    #endregion

    #region Classes

    #region Public

    [System.Serializable]
    public class TerrainMode
    {
        public List<TerrainTypes> TerrainRequirment = new List<TerrainTypes>();
    }

    #endregion

    #endregion

    #region Functions

    #region Public

    public override void TreeTick()
    {
        base.TreeTick();
    }

    #endregion

    #region Protected

    protected override void Start()
    {
        base.Start();
        TestTerrain();
        SetupDecisionTree();
    }

    protected override void Update()
    {
        base.Update();
        //here for testing purposes.
    }

    protected override IEnumerator DecisionTreeRunIntervals()
    {
        float WaitInterval = 0;

        while (ProductionTree != null && hasCorrectTerrain)
        {
            TreeTick();
            WaitInterval = (inProduction) ? (ProductionTime - ProductionTimer) + Time.deltaTime : 1;

            yield return new WaitForSeconds(WaitInterval);
        }
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

    #endregion

    #region Private

    /// <summary>
    /// Test to see if the building has the required terrain for production.
    /// </summary>
    private void TestTerrain()
    {
        // If we want production speed based on percentage of valid tiles that would be done here.

        // Cycle through all the tiles that make up this building and check to see
        // if any of them are the required terrain type for this mode of production.
        foreach(HexTile hex in BuildingArea)
        {
            foreach(TerrainTypes terra in TerrainRequirement)
            {
                if(hex.TerrainType == terra)
                {
                    hasCorrectTerrain = true;
                }
            }
        }

        if (!hasCorrectTerrain)
        {
            // if the building doesn't have the correct terrain send a warning, with identification for the building.
            Debug.LogWarning("Missing Terrain Requirement: " + BuildingType.ToString() + ", Team: " + TeamID);
        }
    }

    /// <summary>
    /// returns true if output storage is full
    /// </summary>
    /// <returns></returns>
    object TestOutputStorage()
    {
        return (ItemsStored.Count >= 5) ? true : false;
    }

    private object GetHaveValidTerrain()
    {
        return hasCorrectTerrain;
    }

    void SetupDecisionTree()
    {
        // Create Conditions
        ObjectDecision InProductionCond = new ObjectDecision(InProduction);

        ObjectDecision IsOutputStorageFullCond = new ObjectDecision(TestOutputStorage);

        ObjectDecision IsThereStorageBuildingCond = new ObjectDecision(IsThereAStorageBuilding);

        ObjectDecision DoIHaveCourierCond = new ObjectDecision(IsThereAnAvailableCourier);

        ObjectDecision haveCorrectTerrainDec = new ObjectDecision(GetHaveValidTerrain);

        // Create leaves
        Leaf WaitLeaf = new Leaf();

        Leaf BeginProductionLeaf = new Leaf(BeginProduction);

        Leaf SendCourierLeaf = new Leaf();

        Leaf HaltProduction = new Leaf();

        // Create Verticies
        Vertex HaveCourier = new Vertex(DoIHaveCourierCond, SendCourierLeaf, WaitLeaf);

        Vertex IsThereStorage = new Vertex(IsThereStorageBuildingCond, HaveCourier, WaitLeaf);

        Vertex DoWeHaveCorrectTerrain = new Vertex(haveCorrectTerrainDec, BeginProductionLeaf, HaltProduction);

        Vertex IsOutputStorageFull = new Vertex(IsOutputStorageFullCond, IsThereStorage, DoWeHaveCorrectTerrain);

        Vertex InProductionVert = new Vertex(InProductionCond, WaitLeaf, IsOutputStorageFull);

        // Create Tree
        ProductionTree = new DecisionTree(InProductionVert);
    }

    #endregion

    #endregion

}
