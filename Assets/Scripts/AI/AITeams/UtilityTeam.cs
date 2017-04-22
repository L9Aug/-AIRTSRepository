using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UtilityTeam : BaseAITeam
{

    UtilityEngine<BaseBuilding> myEngine;// = new UtilityEngine<BaseBuilding>();


    // Use this for initialization
    void Start()
    {
        SetupUtitlityEngine();
    }

    // Update is called once per frame
    void Update()
    {
        print(myEngine.RunUtilityEngine()[0].BuildingType);
    }

#region Utility Engine Functions

    float GetNumBuildings(params Buildings[] BuildingTypes)
    {
        float count = 0;

        foreach(Buildings building in BuildingTypes)
        {
            count += BuildingsList.FindAll(x => x.BuildingType == building).Count;
        }

        return count;
    }

    float GetNumBuildingsReqs(params Products[] products)
    {
        float CurrentNum = 0;

        foreach (BaseBuilding building in BuildingsList)
        {
            foreach (Products product in products)
            {
                if (building is GeneralProduction)
                {
                    if (((GeneralProduction)building).ProductionRequirements.Contains(product))
                    {
                        ++CurrentNum;
                    }
                }
                else if (building is UnitProduction)
                {
                    for(int i = 0; i < ((UnitProduction)building).ProductionRequirements.Count; ++i)
                    {
                        if (((UnitProduction)building).ProductionRequirements[i].RequiredProducts.Contains(product))
                        {
                            ++CurrentNum;
                            break;
                        }
                    }
                }
            }
        }
        return CurrentNum;
    }

    float FoodProductionBuildingsCount()
    {
        return GetNumBuildings(Buildings.Fishery, Buildings.Orchard, Buildings.WaterWell, Buildings.Butcher, Buildings.Winery, Buildings.Bakery);
    }

    float HaveBuildingRequirements(int Tier)
    {
        if (Gold >= Tier && Population.CitizenCount >= Tier && (Tier == 4 ? Population.MerchantCount >= 1 : true))
        {
            return 1;
        }
        return 0;
    }

    #region Tier 1 Functions

    float HaveTeirOneRequirments()
    {
        return HaveBuildingRequirements(1);
    }

    #region Farm

    float GetNumFarms()
    {
        return GetNumBuildings(Buildings.Farm);
    }

    float GetFarmRequirements()
    {
        return GetNumBuildingsReqs(Products.Wheat);
    }

    #endregion

    #region Fishery

    float GetNumFisherys()
    {
        return GetNumBuildings(Buildings.Fishery);
    }

    float GetNumFisheryRequirements()
    {
        return GetNumBuildingsReqs(Products.Fish);
    }

    #endregion

    #region Mine

    float GetNumMines()
    {
        return GetNumBuildings(Buildings.Mine);
    }

    float GetNumMineRequirements()
    {
        return GetNumBuildingsReqs(Products.Iron, Products.Gold);
    }

    #endregion

    #region Orchard

    float GetNumOrchards()
    {
        return GetNumBuildings(Buildings.Orchard);
    }

    float GetNumOrchardRequirements()
    {
        return GetNumBuildingsReqs(Products.Fruit);
    }

    #endregion

    #region Pasture

    float GetNumPastures()
    {
        return GetNumBuildings(Buildings.Pasture);
    }

    float GetNumPastureRequirements()
    {
        return GetNumBuildingsReqs(Products.Cattle);
    }

    #endregion

    #region Plantation

    float GetNumPlantations()
    {
        return GetNumBuildings(Buildings.Plantation);
    }

    float GetNumPlantationRequirements()
    {
        return GetNumBuildingsReqs(Products.Cotton);
    }

    #endregion

    #region Quarry

    float GetNumQuarrys()
    {
        return GetNumBuildings(Buildings.Quarry);
    }

    float GetNumQuarryRequirements()
    {
        return GetNumBuildingsReqs(Products.Stone);
    }

    #endregion

    #region Sawmill

    float GetNumSawmills()
    {
        return GetNumBuildings(Buildings.Mine);
    }

    float GetNumSawmillRequirements()
    {
        return GetNumBuildingsReqs(Products.Wood);
    }

    #endregion

    #region WaterWell

    float GetNumWaterWells()
    {
        return GetNumBuildings(Buildings.WaterWell);
    }

    float GetNumWaterWellRequirements()
    {
        return GetNumBuildingsReqs(Products.Water);
    }

    #endregion

    #endregion

    #region Tier 2 Functions

    float HaveTeirTwoRequirments()
    {
        return HaveBuildingRequirements(2);
    }

    #region Butcher

    float GetNumButchers()
    {
        return GetNumBuildings(Buildings.Butcher);
    }

    #endregion

    #region Carpenter

    float GetNumCarpenters()
    {
        return GetNumBuildings(Buildings.Carpenter);
    }

    float GetCarpenterRequirements()
    {
        return GetNumBuildingsReqs(Products.Planks);
    }

    #endregion

    #region House

    float NeedHouse()
    {
        return Population.CitizenCount;
    }

    #endregion

    #region Mill

    float GetNumMills()
    {
        return GetNumBuildings(Buildings.Mill);
    }

    float GetMillRequirements()
    {
        return GetNumBuildingsReqs(Products.Flour);
    }

    #endregion

    #region Tailor

    float GetNumTailors()
    {
        return GetNumBuildings(Buildings.Tailor);
    }

    float GetTailorRequirements()
    {
        return GetNumBuildingsReqs(Products.Clothing);
    }

    #endregion

    #region Tanner

    float GetNumTanners()
    {
        return GetNumBuildings(Buildings.Tanner);
    }

    float GetTannerRequirements()
    {
        return GetNumBuildingsReqs(Products.Leather);
    }

    #endregion

    #region Winery

    float GetNumWinerys()
    {
        return GetNumBuildings(Buildings.Winery);
    }

    #endregion

    #endregion

    #region Tier 3 Functions

    float HaveTeirThreeRequirments()
    {
        return HaveBuildingRequirements(3);
    }

    #region Armoursmith

    float GetNumArmoursmiths()
    {
        return GetNumBuildings(Buildings.Armoursmith);
    }

    float GetArmoursmithRequirements()
    {
        return GetNumBuildingsReqs(Products.Armour);
    }

    #endregion

    #region Bakery

    float GetNumBakerys()
    {
        return GetNumBuildings(Buildings.Bakery);
    }

    #endregion

    #region Fletcher

    float GetNumFletchers()
    {
        return GetNumBuildings(Buildings.Fletcher);
    }

    float GetFletcherRequirements()
    {
        return GetNumBuildingsReqs(Products.Bows);
    }

    #endregion

    #region Typography

    float GetNumTypographys()
    {
        return GetNumBuildings(Buildings.Typography);
    }

    float GetTypographyRequirements()
    {
        return GetNumBuildingsReqs(Products.Paper);
    }

    #endregion

    #region Villa

    float NeedVilla()
    {
        return Population.MerchantCount;
    }

    #endregion

    #region Warehouse

    float CurrentStorageSpace()
    {
        float StorageSpace = 0;

        List<BaseBuilding> StorageBuildings = BuildingsList.FindAll(x => x is StorageBuilding);

        for(int i = 0; i < StorageBuildings.Count; ++i)
        {
            StorageSpace += ((StorageBuilding)StorageBuildings[i]).Capacity - ((StorageBuilding)StorageBuildings[i]).ItemsStored.Count;
        }

        return StorageSpace;
    }

    #endregion

    #region Weaponsmith

    float GetNumWeaponsmiths()
    {
        return GetNumBuildings(Buildings.Weaponsmith);
    }

    float GetWeaponsmithRequirements()
    {
        return GetNumBuildingsReqs(Products.Swords);
    }

    #endregion

    #endregion

    #region Tier 4 Functions

    float HaveTeirFourRequirments()
    {
        return HaveBuildingRequirements(4);
    }

    #region Barracks

    float GetNumBarracks()
    {
        return GetNumBuildings(Buildings.Barracks);
    }

    float NumEnemyUnits()
    {
        float UnitCount = 0;

        for(int i = 0; i < TeamManager.TM.Teams.Count; ++i)
        {
            if (i != TeamID)
            {
                UnitCount += TeamManager.TM.Teams[i].Population.Military.Count;
            }
        }
        return UnitCount;
    }

    float OurMilitaryCount()
    {
        return Population.Military.Count;
    }

    #endregion

    #region Library

    float GetNumLibrarys()
    {
        return GetNumBuildings(Buildings.Library);
    }

    #endregion

    #region Market

    float GetNumMarkets()
    {
        return GetNumBuildings(Buildings.Market);
    }

    float GetGoldCount()
    {
        return Gold;
    }

    #endregion

    #region Workshop

    float GetNumWorkshops()
    {
        return GetNumBuildings(Buildings.Workshop);
    }

    float GetEnemyBuildingsCount()
    {
        float BuildingCount = 0;

        for (int i = 0; i < TeamManager.TM.Teams.Count; ++i)
        {
            if (i != TeamID)
            {
                BuildingCount += TeamManager.TM.Teams[i].BuildingsList.Count;
            }
        }

        return BuildingCount;
    }

    #endregion

    #endregion

    void SetupUtitlityEngine()
    {
        myEngine = new UtilityEngine<BaseBuilding>();

        // Multiple Use
        UtilityConsideration NumberOfFoodBuildings = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 60), FoodProductionBuildingsCount, -1, 1, 0, 1, 1);
        UtilityConsideration HaveTierOneResources = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 1), HaveTeirOneRequirments, 1, 1, 0, 1, 0);
        UtilityConsideration HaveTierTwoResources = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 1), HaveTeirTwoRequirments, 1, 1, 0, 1, 0);
        UtilityConsideration HaveTierThreeResources = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 1), HaveTeirThreeRequirments, 1, 1, 0, 1, 0);
        UtilityConsideration HaveTierFourResources = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 1), HaveTeirFourRequirments, 1, 1, 0, 1, 0);
        
        #region Teir 1

        // Farm
        UtilityConsideration NumberOfFarms = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumFarms, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfFarmRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetFarmRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> FarmAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Farm], NumberOfFarms, NumberOfFarmRequirements, HaveTierOneResources);
        myEngine.Actions.Add(FarmAction);

        // Fishery
        UtilityConsideration NumberOfFisherys = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumFisherys, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfFisheryRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumFisheryRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> FisheryAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Fishery],
            NumberOfFisherys, NumberOfFisheryRequirments, NumberOfFoodBuildings, HaveTierOneResources);
        myEngine.Actions.Add(FisheryAction);

        // Mine
        UtilityConsideration NumberOfMines = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumMines, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfMineRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumMineRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> MineAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Mine], NumberOfMines, NumberOfMineRequirments, HaveTierOneResources);
        myEngine.Actions.Add(MineAction);

        // Orchard
        UtilityConsideration NumberOfOrchards = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumOrchards, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfOrchardRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumOrchardRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> OrchardAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Orchard],
            NumberOfMines, NumberOfMineRequirments, NumberOfFoodBuildings, HaveTierOneResources);
        myEngine.Actions.Add(OrchardAction);

        // Pasture
        UtilityConsideration NumberOfPastures = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumPastures, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfPastureRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumPastureRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> PastureAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Pasture], NumberOfOrchards, NumberOfPastureRequirments, HaveTierOneResources);
        myEngine.Actions.Add(PastureAction);

        // Plantation
        UtilityConsideration NumberOfPlantations = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumPlantations, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfPlantationRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumPlantationRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> PlantationAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Plantation], NumberOfPlantations, NumberOfPlantationRequirments, HaveTierOneResources);
        myEngine.Actions.Add(PlantationAction);

        // Quarry
        UtilityConsideration NumberOfQuarrys = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumQuarrys, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfQuarryRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumQuarryRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> QuarryAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Quarry], NumberOfQuarrys, NumberOfQuarryRequirments, HaveTierOneResources);
        myEngine.Actions.Add(QuarryAction);

        // Sawmill
        UtilityConsideration NumberOfSawmills = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumSawmills, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfSawmillRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumSawmillRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> SawmillAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Sawmill], NumberOfSawmills, NumberOfSawmillRequirments, HaveTierOneResources);
        myEngine.Actions.Add(SawmillAction);

        // WaterWell
        UtilityConsideration NumberOfWaterWells = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumWaterWells, -1, 1, 0, 1, 0);
        UtilityConsideration NumberOfWaterWellRequirments = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumWaterWellRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> WaterWellAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.WaterWell], 
            NumberOfWaterWells, NumberOfWaterWellRequirments, NumberOfFoodBuildings, HaveTierOneResources);
        myEngine.Actions.Add(WaterWellAction);


        #endregion
        
        #region Tier 2

        // Butcher
        UtilityConsideration NumberOfButchers = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumButchers, -1, 1, 0, 1, 1);
        UtilityAction<BaseBuilding> ButcherAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Butcher], NumberOfButchers, NumberOfFoodBuildings, HaveTierTwoResources);
        myEngine.Actions.Add(ButcherAction);

        // Carpenter
        UtilityConsideration NumberOfCarpenters = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumCarpenters, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfCarpenterRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetCarpenterRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> CarpenterAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Carpenter],
            NumberOfCarpenters, NumberOfCarpenterRequirements, HaveTierTwoResources);

        myEngine.Actions.Add(CarpenterAction);

        // House
        UtilityConsideration NeedHouseConsideration = new UtilityConsideration(UtilityConsideration.CurveTypes.Polynomial, new Vector2(0, 20), NeedHouse, -1, 1, 0, 4, 1);
        UtilityAction<BaseBuilding> HouseAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.House], NeedHouseConsideration, HaveTierTwoResources);
        myEngine.Actions.Add(HouseAction);

        // Mill
        UtilityConsideration NumberOfMills = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumMills, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfMillRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetMillRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> MillAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Mill], NumberOfMills, NumberOfMillRequirements, HaveTierTwoResources);
        myEngine.Actions.Add(MillAction);

        // Tailor
        UtilityConsideration NumberOfTailors = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumTailors, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfTailorRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetTailorRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> TailorAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Tailor], NumberOfTailors, NumberOfTailorRequirements, HaveTierTwoResources);
        myEngine.Actions.Add(TailorAction);

        // Tanner
        UtilityConsideration NumberOfTanners = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumTanners, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfTannerRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetTannerRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> TannerAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Tanner], NumberOfTanners, NumberOfTannerRequirements, HaveTierTwoResources);
        myEngine.Actions.Add(TannerAction);

        // Winery
        UtilityConsideration NumberOfWinerys = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumWinerys, -1, 1, 0, 1, 1);
        UtilityAction<BaseBuilding> WineryAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Winery], NumberOfWinerys, NumberOfFoodBuildings, HaveTierTwoResources);
        myEngine.Actions.Add(WineryAction);

        #endregion
        
        #region Tier 3

        // Armoursmith
        UtilityConsideration NumberOfArmoursmiths = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumArmoursmiths, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfArmoursmithRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetArmoursmithRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> ArmoursmithAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Armoursmith],
            NumberOfArmoursmiths, NumberOfArmoursmithRequirements, HaveTierThreeResources);

        myEngine.Actions.Add(ArmoursmithAction);

        // Bakery
        UtilityConsideration NumberOfBakerys = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumBakerys, -1, 1, 0, 1, 1);
        UtilityAction<BaseBuilding> BakeryAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Bakery], NumberOfBakerys, HaveTierThreeResources);
        myEngine.Actions.Add(BakeryAction);

        // Fletcher
        UtilityConsideration NumberOfFletchers = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumFletchers, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfFletcherRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetFletcherRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> FletcherAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Fletcher],
            NumberOfFletchers, NumberOfFletcherRequirements, HaveTierThreeResources);

        myEngine.Actions.Add(FletcherAction);

        // Typography
        UtilityConsideration NumberOfTypographys = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumTypographys, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfTypographyRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetTypographyRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> TypographyAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Typography],
            NumberOfTypographys, NumberOfTypographyRequirements, HaveTierThreeResources);

        myEngine.Actions.Add(TypographyAction);

        // Villa
        UtilityConsideration NeedVillaConsideration = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 5), NeedVilla, -1, 1, 0, 1, 1);
        UtilityAction<BaseBuilding> VillaAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Villa], NeedVillaConsideration, HaveTierThreeResources);
        myEngine.Actions.Add(VillaAction);

        // Warehouse
        UtilityConsideration NeedWarehouse = new UtilityConsideration(UtilityConsideration.CurveTypes.Polynomial, new Vector2(0, 2000), CurrentStorageSpace, -1, 1, 0.25f, 4, 1);
        UtilityAction<BaseBuilding> WarehouseAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Warehouse], NeedWarehouse, HaveTierThreeResources);
        myEngine.Actions.Add(WarehouseAction);

        // Weaponsmith
        UtilityConsideration NumberOfWeaponsmiths = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumWeaponsmiths, -1, 1, 0, 1, 1);
        UtilityConsideration NumberOfWeaponsmithRequirements = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetWeaponsmithRequirements, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> WeaponsmithAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Weaponsmith],
            NumberOfWeaponsmiths, NumberOfWeaponsmithRequirements, HaveTierThreeResources);

        myEngine.Actions.Add(WeaponsmithAction);

        #endregion

        #region Tier 4

        // Barracks
        UtilityConsideration NumOfBarracks = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumBarracks, -1, 1, 0, 1, 1);
        UtilityConsideration NumOfEnemyUnits = new UtilityConsideration(UtilityConsideration.CurveTypes.Log, new Vector2(0, 100), NumEnemyUnits, 1, 1, 0, 10, 1);
        UtilityConsideration OurMilitary = new UtilityConsideration(UtilityConsideration.CurveTypes.Trigonometric, new Vector2(0, 100), OurMilitaryCount, -1.3f, 1, 0.25f, 4, 1);
        UtilityAction<BaseBuilding> BarracksAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Barracks],
            NumOfBarracks, NumOfEnemyUnits, OurMilitary, HaveTierFourResources);
        myEngine.Actions.Add(BarracksAction);

        // Library
        UtilityConsideration NumLibrarys = new UtilityConsideration(UtilityConsideration.CurveTypes.Log, new Vector2(0, 10), GetNumLibrarys, 1, 2, 0.5f, 10, 1);
        UtilityConsideration NumOfEnemyUnitsInverse = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 100), NumEnemyUnits, -1, 1, 0, 1, 1);
        UtilityAction<BaseBuilding> LibraryAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Library], NumLibrarys, NumOfEnemyUnitsInverse, HaveTierFourResources);
        myEngine.Actions.Add(LibraryAction);

        // Market
        UtilityConsideration NumMarkets = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumMarkets, -1, 1, 0, 1, 1);
        UtilityConsideration GoldAmount = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 50), GetGoldCount, -1, 1, 0, 1, 1);
        UtilityAction<BaseBuilding> MarketAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Market], NumMarkets, GoldAmount, HaveTierFourResources);
        myEngine.Actions.Add(MarketAction);

        // Workshop
        UtilityConsideration NumOfWorkshops = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 10), GetNumWorkshops, -1, 1, 0, 1, 1);
        UtilityConsideration EnemyBuildingCount = new UtilityConsideration(UtilityConsideration.CurveTypes.Linear, new Vector2(0, 50), GetEnemyBuildingsCount, 1, 1, 0, 1, 0);
        UtilityAction<BaseBuilding> WorkshopAction = new UtilityAction<BaseBuilding>(1, GlobalAttributes.Global.Buildings[(int)Buildings.Workshop], NumOfWorkshops, EnemyBuildingCount, HaveTierFourResources);
        myEngine.Actions.Add(WorkshopAction);

        #endregion

    }

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(UtilityTeam))]
public class UtilityTeamEditor : AITeamBaseEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

}
#endif