using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    [System.Serializable]
    public class GOAPPlanner
    {
        public List<GOAPAction> GetActionPlan(GOAPAgent Agent, List<GOAPState> WorldState, List<GOAPAction> AvailableActions, GOAPGoal goal)
        {
            List<GOAPAction> usable = new List<GOAPAction>();
            for (int i = 0; i < AvailableActions.Count; ++i)
            {
                if (AvailableActions[i].CanRun(Agent))
                {
                    usable.Add(AvailableActions[i]);
                }
            }

            List<Node> leaves = new List<Node>();
            Node startingNode = new Node(null, WorldState, 0, null);

            bool ableToGetPlan = BuildGraph(startingNode, leaves, AvailableActions, goal);

            if (!ableToGetPlan)
            {
                Debug.Log("Failed to get plan");
                return null;
            }

            Node cheapestAction = null;

            foreach(Node leaf in leaves)
            {
                if(cheapestAction == null)
                {
                    cheapestAction = leaf;
                }
                else
                {
                    if (leaf.Cost < cheapestAction.Cost)
                    {
                        cheapestAction = leaf;
                    }
                }
            }

            List<GOAPAction> results = new List<GOAPAction>();
            Node temp = cheapestAction;
            while(temp != null)
            {
                if(temp.Action != null)
                {
                    results.Insert(0, temp.Action);
                }

                temp = temp.Parent;
            }

            return results;
        }

        bool BuildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, GOAPGoal goal)
        {
            bool foundLeaf = false;

            for(int i = 0; i < usableActions.Count; ++i)
            {
                if(StatesInGoal(usableActions[i].requiredStates, parent.State))
                {
                    List<GOAPState> currentState = CreateNewState(parent.State, usableActions[i].satisfiesStates);

                    Node node = new Node(parent, currentState, parent.Cost + usableActions[i].Cost, usableActions[i]);

                    if(StatesInGoal(goal.Preconditions, currentState))
                    {
                        leaves.Add(node);
                        foundLeaf = true;
                    }
                    else
                    {
                        List<GOAPAction> subset = ActionSubset(usableActions, usableActions[i]);
                        bool found = BuildGraph(node, leaves, subset, goal);
                        if (found)
                        {
                            foundLeaf = true;
                        }
                    }
                }
            }
            return foundLeaf;
        }

        List<GOAPAction> ActionSubset(List<GOAPAction> actions, GOAPAction removeThis)
        {
            List<GOAPAction> subset = new List<GOAPAction>();
            foreach(GOAPAction action in actions)
            {
                if (!action.Equals(removeThis))
                {
                    subset.Add(action);
                }
            }
            return subset;
        }

        bool StatesInGoal(List<GOAPState> testing, List<GOAPState> against)
        {
            bool allMatches = true;
            for(int i = 0; i < testing.Count; ++i)
            {
                bool singleMatch = false;
                for(int j = 0; j < against.Count; j++)
                {
                    if (testing[i].Equals(against[j]))
                    {
                        singleMatch = true;
                        break;
                    }
                }
                if (!singleMatch)
                {
                    allMatches = false;
                }
            }
            return allMatches;
        }

        List<GOAPState> CreateNewState(List<GOAPState> current, List<GOAPState> changes)
        {
            List<GOAPState> state = new List<GOAPState>();

            foreach(GOAPState contains in current)
            {
                state.Add(contains);
            }

            for(int i = 0; i < changes.Count; i++)
            {
                bool exists = false;
                foreach(GOAPState check in state)
                {
                    if (check.Equals(changes[i]))
                    {
                        exists = true;
                        break;
                    }
                }


                if (exists)
                {
                    state.RemoveAt(i);
                    GOAPState updated = changes[i];
                    state.Add(updated);
                }
            }

            return state;
        }

        private class Node
        {
            public Node Parent;
            public float Cost;
            public List<GOAPState> State;
            public GOAPAction Action;

            public Node(Node parent, List<GOAPState> state, float cost, GOAPAction action)
            {
                Parent = parent;
                Cost = cost;
                State = state;
                Action = action;
            }
        }
        /*public AStarInfo<GOAPState> Goal;
        public Queue<AStarInfo<GOAPState>> ActionPlan;
        public List<AStarInfo<GOAPState>> PossibleStates;

        AStarInfo<GOAPState> lastGoal;
        ASImplementation<GOAPState> Astar;

        public List<SM.Action> UpdateGOAP()
        {
            if (ActionPlan.Peek().current.isSatisfied)
            {
                ActionPlan.Dequeue();
            }

            if(Goal != lastGoal)
            {
                RecalculateActionPlan();
            }

            lastGoal = Goal;

            return ActionPlan.Peek().current.Actions;
        }

        void RecalculateActionPlan()
        {
            ActionPlan.Clear();
            List<AStarInfo<State>> states = Astar.AStar((x, y) => { return Goal.cost * 0.75f; });
            for(int i = 0; i < states.Count; ++i)
            {
                ActionPlan.Enqueue(states[i]);
            }
        }*/
    }
}
