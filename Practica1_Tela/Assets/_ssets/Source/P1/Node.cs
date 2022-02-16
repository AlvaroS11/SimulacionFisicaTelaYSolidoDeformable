using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth {
    public class Node //: MonoBehaviour
    {
        public MassSpringCloth manager;
        public Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);
        public Vector3 pos;
        public Vector3 vel;
        public Vector3 force;

        public float mass;
        public bool isFixed;
        public Vector3 relative;
        public int referencia;

        public Node(Vector3 position, int refe, MassSpringCloth man)
        {
            manager = man;
            pos = position;
            mass = man.nodeMass;
            referencia = refe;
            vel = Vector3.zero;
            isFixed = false;
        }


        /*
        // Use this for initialization
        private void Awake()
        {
            pos = transform.position;
            vel = Vector3.zero;
        }

        void Start()
        {
        }

        // Update is called once per frame
         /*/
        void Update()
        {
            mass = manager.nodeMass;
        }
       
        public void ComputeForces()
        {
            force = Vector3.zero;
            force += mass * manager.Gravity;
            force += -manager.dampingMovement * vel;
        }
    }
}