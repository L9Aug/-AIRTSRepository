using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    public class GOAPAgent : MonoBehaviour
    {
        public List<GOAPAction> AvailableActions = new List<GOAPAction>();
        public List<GOAPAction> ActionPlan;
        public GOAPPlanner Planner;
        public List<GOAPState> WorldState;
        public GOAPGoal currentGoal;
        public UtilityEngine<GOAPGoal> util;
        
        /// <summary>
        /// Call this on the agent you've created in either Start or Awake
        /// </summary>
        public virtual void Initialise()
        {
            AvailableActions.AddRange(GetComponents<GOAPAction>());
        }

        public void SetWorldState(List<GOAPState> ws)
        {
            WorldState = ws;
        }

        public List<Action> UpdateAgent()
        {

            List<GOAPGoal> goalList = util.RunUtilityEngine();

            if(goalList[0] != currentGoal)
            {
                currentGoal = goalList[0];
                GOAPController.GC.EnqueueForPlanning(this);
                ActionPlan.Clear();
                //ActionPlan.AddRange(Planner.GetActionPlan(this, WorldState, AvailableActions, currentGoal));
            }

            if (ActionPlan[0].TestForFinished())
            {
                ActionPlan.RemoveAt(0);
            }

            return ActionPlan[0].effects;
        }
    }
}
