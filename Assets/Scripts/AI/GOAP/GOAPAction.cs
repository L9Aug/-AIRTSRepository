﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    public delegate void Action();

    public class GOAPAction : MonoBehaviour
    {
        public GOAPAgent Agent;
        public List<GOAPState> satisfiesStates = new List<GOAPState>();
        public List<GOAPState> requiredStates = new List<GOAPState>();
        public float Cost;
        bool isActive;
        public List<Action> effects = new List<Action>(); 
        public bool isFinished;

        protected virtual void Start()
        {

        }



        public virtual bool TestForFinished()
        {
            Debug.Log("TestForFinished in " + this + " has not been implemented");
            return false;
        }

        /// <summary>
        /// Run all of your actions, then call TestForFinished
        /// </summary>
        public virtual void ActionUpdate(GOAPAgent agent)
        {
            foreach(Action effect in effects)
            {
                effect();
            }
            isActive = !TestForFinished();
        }

        public virtual bool CanRun(GOAPAgent agent)
        {
            Debug.Log("CanRun in " + this + " has not been implemented");
            bool statesMatch = true;
            foreach(GOAPState state in requiredStates)
            {
                foreach(bool match in state.Items)
                {
                    if (!match)
                    {
                        statesMatch = false;
                        break;
                    }
                }
            }
            return statesMatch;
        }

        public void AddPrecondition(GOAPState cond)
        {
            requiredStates.Add(cond);
        }

        public void RemovePrecondition(GOAPState cond)
        {
            if (requiredStates.Exists(x => x.Equals(cond)))
            {
                requiredStates.Remove(cond);
            }
        }

        public void AddEffect(Action action)
        {
            effects.Add(action);
        }

        public void RemoveEffect(Action action)
        {
            if(effects.Exists(x => x.Equals(action)))
            {
                effects.Remove(action);
            }
        }

    }

}