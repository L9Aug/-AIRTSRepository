// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StorageBuilding : BaseBuilding
{

    #region Public Variables

    /// <summary>
    /// The number of products this building can store.
    /// </summary>
    [Tooltip("The number of products this building can store.")]
    public int Capacity;

    /// <summary>
    /// The items stored in this buildings.
    /// </summary>
    [HideInInspector]
    public List<StorageItem> ItemsStored = new List<StorageItem>();

    #endregion

    #region Functions

    #region Public

    public override List<Products> DeliverProducts(params Products[] products)
    {
        return AddProduct(products);
    }

    public List<Products> AddProduct(params Products[] products)
    {
        List<Products> returnList = new List<Products>();
        foreach (Products product in products)
        {
            if (ItemsStored.Count < Capacity)
            {
                ItemsStored.Add(new StorageItem(product));
            }
            else
            {
                returnList.Add(product);
            }
        }
        return returnList;
    }

    public override List<KalamataTicket> GetTicketForProducts(ref List<Products> products)
    {
        return GetTicketsForProducts(ItemsStored, ref products);
    }

    public override bool TestForProducts(params Products[] products)
    {
        return TestForProducts(ItemsStored, products);
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

    #endregion

    #endregion

}
