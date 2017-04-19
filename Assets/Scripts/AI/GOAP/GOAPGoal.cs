using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GOAP
{
    public class GOAPGoal : MonoBehaviour
    {
        public string Name;
        public List<GOAPState> Precondition;
        public UtilityAction<GOAPGoal> UtilAction;

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