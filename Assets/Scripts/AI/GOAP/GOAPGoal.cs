using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GOAP
{
    public abstract class GOAPGoal
    {
        public string Name;
        public List<GOAPState> Preconditions;
        public UtilityAction<GOAPGoal> UtilAction;


        public GOAPGoal()
        {

        }

        public GOAPGoal(string name, List<GOAPState> precons, UtilityAction<GOAPGoal> util)
        {
            Name = name;
            Preconditions = precons;
            UtilAction = util;
        }

        public abstract void SetupPrecons();

        public abstract void SetupUtility();

        public void AddPrecon(GOAPState state)
        {
            Preconditions.Add(state);
        }

        public void AddPrecons(List<GOAPState> preconditions)
        {
            foreach(GOAPState state in preconditions)
            {
                Preconditions.Add(state);
            }
        }

        public void AddPrecons(params GOAPState[] states)
        {
            Preconditions.AddRange(states);
        }

        public void RemovePrecon(GOAPState state)
        {
            if(Preconditions.Exists(x => x.Equals(state)))
            {
                Preconditions.Remove(state);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}