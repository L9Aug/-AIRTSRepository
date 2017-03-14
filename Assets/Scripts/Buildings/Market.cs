// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
            TeamManager.TM.Teams[TeamID].Gold += (int)CumulativeTierAdd[i];
            CumulativeTierAdd[i] -= (int)CumulativeTierAdd[i];
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

    #endregion

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(Market))]
[CanEditMultipleObjects]
public class MarketEditor : BaseBuildingEditor
{
    private Market myMTarget;

    static bool DisplayAmountPerTier;
    static bool DisplayAmountOfTier;
    static bool DisplayCumulativeTierAdd;

    protected override void OnEnable()
    {
        base.OnEnable();
        myMTarget = (Market)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            EditorGUILayout.FloatField("Period", myMTarget.Period);

            DisplayIntArray(myMTarget.AmountPerTier, ref DisplayAmountPerTier);

            DisplayIntArray(myMTarget.AmountOfTier, ref DisplayAmountOfTier);

            DisplayFloatArray(myMTarget.CumulativeTierAdd, ref DisplayCumulativeTierAdd);

        }        
    }

    void DisplayIntArray(int[] intArray, ref bool ShouldShow)
    {
        ShouldShow = EditorGUILayout.Foldout(ShouldShow, intArray.ToString());
        if (ShouldShow)
        {
            foreach (int item in intArray)
            {
                EditorGUILayout.IntField(item);
            }
        }
    }

    void DisplayFloatArray(float[] floatArray, ref bool ShouldShow)
    {
        ShouldShow = EditorGUILayout.Foldout(ShouldShow, floatArray.ToString());
        if (ShouldShow)
        {
            foreach (float item in floatArray)
            {
                EditorGUILayout.FloatField(item);
            }
        }
    }

}
#endif