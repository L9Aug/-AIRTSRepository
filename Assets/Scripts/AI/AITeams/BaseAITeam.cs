// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseAITeam : MonoBehaviour
{

    #region Variables

    #region Public

    /// <summary>
    /// The ID for this team.
    /// </summary>
    [Tooltip("The ID for this team")]
    public int TeamID = 0;

    public Color TeamColour;

    /// <summary>
    /// The starting location for this team.
    /// </summary>
    [Tooltip("The starting location for this team.")]
    public Vector2 StartingLocation;

    /// <summary>
    /// The amount of gold this team has.
    /// </summary>
    public int Gold
    {
        get
        {
            int currentCount = 0;
            for(int i = 0; i < BuildingsList.Count; ++i)
            {
                currentCount += BuildingsList[i].ProductCount(Products.Gold);
            }
            return currentCount;
        }
    }

    /// <summary>
    /// The population available to this team. (Not population already in buildings)
    /// </summary>
    [Tooltip("The population available to this team.\n(Not population already in buildings.")]
    public PopulationClass Population = new PopulationClass();

    /// <summary>
    /// A list of the buildings that this team has.
    /// </summary>
    [Tooltip("A list of buildings that this team has.")]
    public List<BaseBuilding> BuildingsList = new List<BaseBuilding>();

    //Temp
    public int x;
    public int y;
    public Buildings TestSpawn;

    public bool isTeamActive = false;

    #endregion

    #region Private

    bool IsPlacingBuilding = false;

    #endregion

    #endregion

    #region Classes

    [System.Serializable]
    public class PopulationClass
    {
        public List<BaseUnit> Citizens = new List<BaseUnit>();
        public List<BaseUnit> Merchants = new List<BaseUnit>();
        public List<BaseUnit> Military = new List<BaseUnit>();

        public int CitizenCount = 0;
        public int MerchantCount = 0;
    }

    #endregion

    #region Functions

    #region Public

    /// <summary>
    /// 
    /// </summary>
    /// <param name="MyPos"></param>
    /// <param name="products"></param>
    /// <returns>A list of products and their locations, empty if no products were found.</returns>
    public List<KalamataTicket> ReserveProducts(HexTransform MyPos, params Products[] products)
    {
        List<KalamataTicket> productTickets = new List<KalamataTicket>();

        List<Products> remainingProducts = new List<Products>();
        remainingProducts.AddRange(products);

        // search buildings.
        foreach(BaseBuilding building in BuildingsList)
        {
            productTickets.AddRange(building.GetTicketForProducts(ref remainingProducts));
            if (remainingProducts.Count == 0) break;
        }

        return productTickets;
    }

    public bool FindProducts(params Products[] products)
    {
        foreach(BaseBuilding building in BuildingsList)
        {
            if(building.TestForProducts(products)) return true;
        }
        return false;
    }

    /// <summary>
    /// Attempts to construct the chosen building.
    /// </summary>
    /// <param name="building">The building to build.</param>
    /// <param name="Location">The location to build it.</param>
    /// <returns>Returns true if the building can and is being built.</returns>
    public bool ConstructBuilding(Buildings building, Vector2 Location)
    {
        // Check to see if the tile is on the map.
        if (ValidateLocation(Location))
        {
            int buildingTier = GlobalAttributes.Global.Buildings[(int)building].Tier;
            // Check to see if the team has the required Resources.
            if (CheckResources(buildingTier))
            {
                int BuildingSize = GlobalAttributes.Global.Buildings[(int)building].Size;
                // Check to see if there is space for the building.
                if (ValidateArea(Location, BuildingSize))
                {
                    // Create the building in the requested position and add it to this teams building list.
                    Vector3 BuildingPos = MapGenerator.Map[(int)Location.x, (int)Location.y].transform.position;
                    BuildingsList.Add((BaseBuilding)Instantiate(GlobalAttributes.Global.Buildings[(int)building], BuildingPos, Quaternion.identity, transform));

                    BuildingsList[BuildingsList.Count - 1].ConfigureBuilding(Location, TeamID);

                    // Set exlusion zone
                    SetExlusionZone(BuildingsList[BuildingsList.Count -1]);

                    // Clear Connections.
                    ClearArea(BuildingsList[BuildingsList.Count - 1]);

                    BuildingsList[BuildingsList.Count - 1].ConstructionCallback = ConstructionFinished;

                    // Deduct resources.
                    if (building != Buildings.TownHall)
                    {
                        DeductResources(buildingTier);
                    }

                    BegunConstruction();

                    return true;
                }
                else
                {
                    Debug.LogWarning("Insufficient space for building: " + building.ToString() + ". Team: " + TeamID + " Retrying...");                    
                }
            }
            else
            {                
                Debug.LogWarning("Insufficient resources for building: " + building.ToString() + ". Team: " + TeamID);
            }
        }
        else
        {
            Debug.LogWarning("Tile is not part of the map. Team: " + TeamID);
        }

        if (building != Buildings.TownHall)
        {
            BegunConstruction();
        }
        return false;
    }

    public virtual void BuildingDestroyed(BaseBuilding BuildingLost)
    {
        BuildingsList.Remove(BuildingLost);
    }

    #endregion

    #region Protected

    protected virtual void ActiveUpdate() { }

    protected virtual void ConstructionFinished() { }

    protected virtual void BegunConstruction() { }

    protected IEnumerator FindSpaceForBuilding(Buildings building)
    {
        if (!IsPlacingBuilding)
        {
            IsPlacingBuilding = true;

            int buildingSize = GlobalAttributes.Global.Buildings[(int)building].Size;

            BaseBuilding ourTownHall = BuildingsList.Find(x => x.BuildingType == Buildings.TownHall);

            int currentSearchRadius = buildingSize + ourTownHall.Size + 1;
            bool CannotFindTile = false;

            HexTile BuildingBaseTile = null;

            while ((BuildingBaseTile == null) && !CannotFindTile)
            {
                List<HexTile> possibleTiles = MapGenerator.Map[(int)ourTownHall.hexTransform.RowColumn.x, (int)ourTownHall.hexTransform.RowColumn.y].GetHexRing(currentSearchRadius);

                for (int i = 0; i < possibleTiles.Count; ++i)
                {
                    yield return null;
                    if (ValidateArea(possibleTiles[i].hexTransform.RowColumn, buildingSize))
                    {
                        BuildingBaseTile = possibleTiles[i];
                    }
                }

                ++currentSearchRadius;
                if (currentSearchRadius >= Vector2.Distance(Vector2.zero, new Vector2(MapGenerator.Map.GetLength(0), MapGenerator.Map.GetLength(1))))
                {
                    CannotFindTile = true;
                }
            }

            IsPlacingBuilding = false;

            if (BuildingBaseTile != null)
            {
                ConstructBuilding(building, BuildingBaseTile.hexTransform.RowColumn);
            }

        }
    }

    protected IEnumerator FindSpaceForBuilding(Buildings building, List<TerrainTypes> requiredTerrain)
    {
        if (!IsPlacingBuilding)
        {
            IsPlacingBuilding = true;

            int buildingSize = GlobalAttributes.Global.Buildings[(int)building].Size;

            BaseBuilding ourTownHall = BuildingsList.Find(x => x.BuildingType == Buildings.TownHall);

            int currentSearchRadius = buildingSize + ourTownHall.Size + 1;
            bool CannotFindTile = false;

            HexTile BuildingBaseTile = null;

            while ((BuildingBaseTile == null) && !CannotFindTile)
            {
                List<HexTile> possibleTiles = MapGenerator.Map[(int)ourTownHall.hexTransform.RowColumn.x, (int)ourTownHall.hexTransform.RowColumn.y].GetHexRing(currentSearchRadius).FindAll(x => requiredTerrain.Contains(x.TerrainType));

                for (int i = 0; i < possibleTiles.Count; ++i)
                {
                    yield return null;
                    List<HexTile> leveltwoPossibleTiles = MapGenerator.Map[(int)possibleTiles[i].hexTransform.RowColumn.x, (int)possibleTiles[i].hexTransform.RowColumn.y].GetHexArea(buildingSize);
                    for (int j = 0; j < leveltwoPossibleTiles.Count; ++j)
                    {
                        if (ValidateArea(leveltwoPossibleTiles[j].hexTransform.RowColumn, buildingSize))
                        {
                            BuildingBaseTile = leveltwoPossibleTiles[j];
                        }
                    }
                }

                ++currentSearchRadius;
                if (currentSearchRadius >= Vector2.Distance(Vector2.zero, new Vector2(MapGenerator.Map.GetLength(0), MapGenerator.Map.GetLength(1))))
                {
                    CannotFindTile = true;
                }
            }

            IsPlacingBuilding = false;

            if (BuildingBaseTile != null)
            {
                ConstructBuilding(building, BuildingBaseTile.hexTransform.RowColumn);
            }

        }
    }

    protected virtual void Start()
    {
        SetupStateMachine();
    }

    #endregion

    #region Private

    private void Update()
    {
        TeamStateMachine.SMUpdate();
    }

    /// <summary>
    /// Sets an exclusion zone around the building.
    /// </summary>
    /// <param name="Building">The building</param>
    private void SetExlusionZone(BaseBuilding Building)
    {
        Building.exclusionZone = MapGenerator.Map[(int)Building.hexTransform.RowColumn.x, (int)Building.hexTransform.RowColumn.y].GetHexRing(Building.Size + 1);
        Building.EntranceTiles.AddRange(Building.exclusionZone);
        //float count = 0;
        foreach (HexTile h in Building.exclusionZone)
        {
            h.IsExlusionZone = true;
            //h.SetColour(new Color(count, 0, 0));
            //count += 0.1f;
        }
    }

    /// <summary>
    /// Removes Connctions from tiles under the building.
    /// </summary>
    /// <param name="loc">The location of the building.</param>
    /// <param name="buildingSize">The size of the building.</param>
    private void ClearArea(BaseBuilding Building)
    {
        Building.BuildingArea = MapGenerator.Map[(int)Building.hexTransform.RowColumn.x, (int)Building.hexTransform.RowColumn.y].GetHexArea(Building.Size);
        foreach (HexTile h in Building.BuildingArea)
        {
            h.IsExlusionZone = true;
        }
    }

    /// <summary>
    /// Deducts the Required resources for the building.
    /// </summary>
    /// <param name="tier">The tier of the building being created.</param>
    private void DeductResources(int tier)
    {

        List<Products> GoldList = new List<Products>();

        // find gold
        for(int i = 0; i < tier; ++i)
        {
            for(int j = 0; j < BuildingsList.Count; ++j)
            {
                GoldList.AddRange(BuildingsList[j].GetProductsFromStorage(Products.Gold));                

                if(GoldList.Count >= tier)
                {
                    break;
                }
            }

            if (GoldList.Count >= tier)
            {
                break;
            }
        }

        Population.CitizenCount -= tier;
        //Dispatch Builders
        for (int i = 0; i < tier; ++i)
        {
            HexTile buildingEntranceTile = BuildingsList[0].EntranceTiles[Random.Range(0, BuildingsList[0].EntranceTiles.Count - 1)];
            //HexTile buildingEntranceTile = MapGenerator.Map[(int)BuildingsList[0].hexTransform.RowColumn.x, (int)BuildingsList[0].hexTransform.RowColumn.y];
            Population.Citizens.Add((BaseUnit)Instantiate(GlobalAttributes.Global.Units[(int)Units.Builder], buildingEntranceTile.transform.position, Quaternion.identity, transform));
            Population.Citizens[Population.Citizens.Count - 1].GetComponent<Builder>().Initialise(BuildingsList[0], BuildingsList[BuildingsList.Count - 1], TeamID, Products.Gold, buildingEntranceTile);
        }
        if (tier == 4)
        {
            HexTile buildingEntranceTile = BuildingsList[0].EntranceTiles[Random.Range(0, BuildingsList[0].EntranceTiles.Count - 1)];
            Population.MerchantCount -= 1;
            Population.Citizens.Add(Instantiate(GlobalAttributes.Global.Units[(int)Units.Merchant], buildingEntranceTile.transform.position, Quaternion.identity, transform));
            Population.Citizens[Population.Citizens.Count - 1].GetComponent<Merchant>().Initialise(BuildingsList[0], BuildingsList[BuildingsList.Count - 1], TeamID, buildingEntranceTile);
            //Dispatch Merchant.

        }
    }

    /// <summary>
    /// Tests to see if there is enough space for the building.
    /// </summary>
    /// <param name="loc">The location of the building.</param>
    /// <param name="buildingSize">The size of the building.</param>
    /// <returns>Returns true if there is enough space.</returns>
    private bool ValidateArea(Vector2 loc, int buildingSize)
    {
        List<HexTile> BuildingArea = MapGenerator.Map[(int)loc.x, (int)loc.y].GetHexArea(buildingSize);
        int AreaCount = BuildingArea.Count;

        //Hex Number equation 3n^2 + 3n + 1 from Wolfram Alpha : http://mathworld.wolfram.com/HexNumber.html
        int RequiredArea = (int)((3 * Mathf.Pow(buildingSize - 1, 2)) + (3 * (buildingSize - 1)) + 1);

        if (AreaCount >= RequiredArea)
        {
            if ((BuildingArea.Find(x => x.TerrainType == TerrainTypes.Sea) == null) && (BuildingArea.Find(x => x.IsExlusionZone == true) == null))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Test to see if the team has the required resources for the building tier.
    /// </summary>
    /// <param name="BuildingTier">The tier of the building.</param>
    /// <returns>Returns true if the team has the required resources.</returns>
    private bool CheckResources(int BuildingTier)
    {
        if (Gold >= BuildingTier && Population.CitizenCount >= BuildingTier)
        {
            if (BuildingTier == 4)
            {
                if (Population.MerchantCount >= 1)
                {
                    return true;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tests to see if the location exists.
    /// </summary>
    /// <param name="loc">The location to test.</param>
    /// <returns>Returns true if it exists.</returns>
    private bool ValidateLocation(Vector2 loc)
    {
        if (loc.x < MapGenerator.Map.GetLength(0) && loc.y < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

    private bool IsTeamActive()
    {
        return isTeamActive;
    }

    private void SetupStateMachine()
    {
        // Conditions
        Condition.BoolCondition IsTeamActiveCondition = new Condition.BoolCondition(IsTeamActive);
        Condition.NotCondition IsTeamInActiveCondition = new Condition.NotCondition(IsTeamActiveCondition);

        // Transitions
        Transition ActivateTeam = new Transition("Activate Team", IsTeamActiveCondition);
        Transition DeActivateTeam = new Transition("De-Activate Team", IsTeamInActiveCondition);

        // Sates
        State Active = new State("Active",
            new List<Transition>() { DeActivateTeam },
            null,
            new List<Action>() { ActiveUpdate },
            null);

        State InActive = new State("InActive",
            new List<Transition>() { ActivateTeam },
            null,
            null,
            null);

        // Transition Links
        ActivateTeam.SetTargetState(Active);
        DeActivateTeam.SetTargetState(InActive);

        // Create Machine
        TeamStateMachine = new StateMachine(null, InActive, Active);
        TeamStateMachine.InitMachine();

    }

    #endregion

    #endregion

    #region StateMachine

    protected StateMachine TeamStateMachine;

    #endregion

}


#if UNITY_EDITOR
[CustomEditor(typeof(BaseAITeam))]
public class AITeamBaseEditor : Editor
{

    public override void OnInspectorGUI()
    {
        BaseAITeam myTarget = (BaseAITeam)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Create Building"))
        {
            myTarget.ConstructBuilding(myTarget.TestSpawn, new Vector2(myTarget.x, myTarget.y));
        }
    }

}
#endif