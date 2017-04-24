using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class ReturnAction : GOAPAction
{

    public virtual void SetupEffects()
    {
        AddEffect(() => Destroy(gameObject));
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
