using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    [System.Serializable]
    public class GOAPAgent
    {
        public List<GOAPAction> AvailableActions = new List<GOAPAction>();
        public List<GOAPAction> ActionPlan = new List<GOAPAction>();
        public GOAPPlanner Planner = new GOAPPlanner();
        public List<GOAPState> WorldState;
        public GOAPGoal currentGoal;
        public UtilityEngine<GOAPGoal> util = new UtilityEngine<GOAPGoal>();

        public void SetWorldState(List<GOAPState> ws)
        {
            WorldState = ws;
        }

        public List<Action> UpdateAgent()
        {
            List<GOAPGoal> goalList = util.RunUtilityEngine();

            if (goalList[0] != currentGoal)
            {
                currentGoal = goalList[0];
                GOAPController.GC.EnqueueForPlanning(this);
                ActionPlan.Clear();
                //ActionPlan.AddRange(Planner.GetActionPlan(this, WorldState, AvailableActions, currentGoal));
            }

            if (ActionPlan != null)
            {
                if (ActionPlan.Count > 0)
                {
                    if (ActionPlan[0].TestForFinished())
                    {
                        ActionPlan.RemoveAt(0);
                    }
                    return ActionPlan[0].effects;
                }
            }
            return new List<Action>();
        }
    }
}
