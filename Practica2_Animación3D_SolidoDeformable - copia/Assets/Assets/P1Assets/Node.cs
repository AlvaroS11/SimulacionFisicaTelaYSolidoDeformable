using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth {
    public class Node //: MonoBehaviour
    {
        public Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);
        public Vector3 pos;
        public Vector3 vel;
        public Vector3 force;

        public float mass;
        public bool isFixed;
        public Vector3 relative;
        public int referencia;
        public Vector3[] barCords = new Vector3[3];
        public float[] peso = new float[4];
        public float[] volume = new float[4];

        public ElasticSolid manager;

        public Node(Vector3 position, int refe, ElasticSolid man)
        {
            manager = man;
            pos = position;
            mass = man.nodeMass;
            referencia = refe;
            vel = Vector3.zero;
            isFixed = false;
           // barCords = new Vector3[];
        }
        
       
        public void ComputeForces()
        {
            force = Vector3.zero;
            force += mass * manager.Gravity;
            force += -manager.dampingMovement * vel;
        }
    }
}