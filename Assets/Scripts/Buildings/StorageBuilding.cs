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

    public int RemainingSpace
    {
        get
        {
            return Capacity - ItemsStored.Count;
        }
    }

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
