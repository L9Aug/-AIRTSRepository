// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;

public class Market : BaseBuilding
{

    #region Variables

    #region Public

    public float Period;

    public int[] AmountPerTier;

    public int[] AmountOfTier;

    public float[] CumulativeTierAdd = new float[4];

    #endregion

    #region Private

    #endregion

    #endregion

    #region Functions

    #region Public

    public override void BuildingUpdate()
    {
        // for each building count the amount of each tier that there is.

        int[] TierCounts = new int[4];

        foreach (BaseBuilding building in TeamManager.TM.Teams[TeamID].BuildingsList)
        {
            if (building.Tier != 0)
            {
                ++TierCounts[building.Tier - 1];
            }
        }

        for(int i = 0; i < 4; ++i)
        {
            CumulativeTierAdd[i] += ((float)TierCounts[i] / (float)AmountOfTier[i]) * (Time.deltaTime / Period) * (float)AmountPerTier[i];
            //TeamManager.TM.Teams[TeamID].Gold += (int)CumulativeTierAdd[i];
            if(CumulativeTierAdd[i] > 1)
            {
                for(int j = 0; j < (int)CumulativeTierAdd[i]; ++j)
                {
                    ItemsStored.Add(new StorageItem(Products.Gold));
                }
                CumulativeTierAdd[i] -= (int)CumulativeTierAdd[i];
            }
        }

        if (ItemsStored.Count > 5)
        {
            // attempt to send courier
            if (CourierCount > 0)
            {
                SendCourierWithProductsFunc();
            }
        }

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

    protected override void ConstructionFinished()
    {
        base.ConstructionFinished();
        CourierCount = Tier;
    }

    public override void CourierReturned()
    {
        base.CourierReturned();
        ++CourierCount;
    }

    #endregion

    #endregion
}
