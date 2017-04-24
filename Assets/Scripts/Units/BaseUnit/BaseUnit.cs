using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;

public class BaseUnit : GameEntity
{
    public enum unitType { civilian, military, sacrifice };
    
    public unitType type;
    public string unitName;
    public float moveSpeed;
    public int visionRadius;
    public StateMachine unitStateMachine;
    public bool SMActive = true;

    public HexTransform Destination;

    public State move;
    public List<AStarInfo<HexTile>> path;

    public ASImplementation<HexTile> aStar = new ASImplementation<HexTile>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (SMActive)
        {
            unitStateMachine.SMUpdate();
        }
    }

    public virtual void GetNewDestination()
    {

    }

    public virtual void GetPath()
    {

    }

    public void Move()
    {

        if (path.Count > 0)
        {
            Vector3 temp = SteeringBehaviours.Arrive(this, path[0].current, 0.1f, 0.1f);
            transform.Translate(temp * Time.deltaTime);

            if (Vector3.Distance(transform.position, path[0].current.transform.position) < 0.2f)
            {
                hexTransform = path[0].current.hexTransform;
                path.RemoveAt(0);
            }
        }
    }

    public void NullAction()
    {

    }
}
