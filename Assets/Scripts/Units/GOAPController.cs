using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPController : MonoBehaviour
    {
        public static GOAPController GC;
        public int MaxAgentsPerFrame = 30;

        List<GOAPAgent> Agents = new List<GOAPAgent>();

        public void EnqueueForPlanning(GOAPAgent agent)
        {
            if(!Agents.Exists(x => x.Equals(agent)) && (Agents.Count < MaxAgentsPerFrame))
            {
                Agents.Add(agent);
            }
        }

        // Use this for initialization
        void Awake()
        {
            GC = this;
        }

        // Update is called once per frame
        void Update()
        {
            foreach(GOAPAgent agent in Agents)
            {
                agent.ActionPlan = agent.Planner.GetActionPlan(agent, agent.WorldState, agent.AvailableActions, agent.util.RunUtilityEngine()[0]);
            }
            Agents.Clear();
        }
    }

}