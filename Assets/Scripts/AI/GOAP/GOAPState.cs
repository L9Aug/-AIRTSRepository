using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    public class GOAPState
    {
        //public bool isSatisfied = false;
        //public List<GOAPState> PreConditions;
        public string Name;
        public List<object> Items;

        public void Initialise(string name, List<object> items)
        {
            Name = name;
            Items = items;
        }

        public GOAPState()
        {

        }

        public GOAPState(string name, params object[] items)
        {
            Name = name;

            foreach(bool obj in items)
            {
                Items.Add(obj);
            }
        }

        public GOAPState(string name, List<object> items)
        {
            Name = name;
        }

        public bool Equals(GOAPState obj)
        {
            bool listEq = true;
            for (int i = 0; i < Items.Count; ++i)
            {
                if (obj.Items.Count != Items.Count)
                {
                    listEq = false;
                    break;
                }
                if(!ReferenceEquals(Items[i].GetType(), obj.Items[i].GetType()))
                {
                    listEq = false;
                    break;
                }
                if(!Items[i].Equals(obj.Items[i]))
                {
                    listEq = false;
                    break;
                }
            }

            return (Name == obj.Name) && listEq;
        }
    }
}