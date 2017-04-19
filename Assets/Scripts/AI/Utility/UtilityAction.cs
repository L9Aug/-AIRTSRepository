using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilityAction<T>
{
    public List<UtilityConsideration> Considerations;
    public float Weight;
    public T ObjectReference;
    public float Score;

    public UtilityAction()
    {
        Weight = 1;
    }

    public UtilityAction(float weight, T objectRef, params UtilityConsideration[] considerations)
    {
        Weight = weight;
        ObjectReference = objectRef;
        Considerations.AddRange(considerations);
    }
}
