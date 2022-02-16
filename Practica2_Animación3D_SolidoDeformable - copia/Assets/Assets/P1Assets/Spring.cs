using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cloth
{
    public class Spring //: MonoBehaviour
    {


        public Node nodeA, nodeB;

        public float Length0;
        public float Length;

        public float stiffness;

        public Tipo tipo;

        public ElasticSolid manager;
        public Tetraedro tetaedro;
        
        
        public Spring(Node node1, Node node2, Tipo tipo, ElasticSolid man, Tetraedro tetra)
        {
            manager = man;
            nodeA = node1;
            nodeB = node2;
            this.tipo = tipo;
            stiffness = man.traccion;
            Length = (node1.pos - node2.pos).magnitude;
            Length0 = Length;
            tetaedro = tetra;
        }

        public enum Tipo
        {
            Flexion,
            Traccion,
        }

        // Use this for initialization
        /*
        void Start()
        {
            UpdateLength();
            Length0 = Length;
        }

        // Update is called once per frame
         /*/
        void Update()
        {
         
            }
        
        public void UpdateLength()
        {
            Length = (nodeA.pos - nodeB.pos).magnitude;
        }

        public void ComputeForces(float density)
        {
           
             stiffness = manager.traccion;
           
            Vector3 u = nodeA.pos - nodeB.pos;
            u.Normalize();

          // Vector3 force = -stiffness * (Length - Length0) * u;
            Vector3 force = -(tetaedro.volume / (Mathf.Sqrt(Length0))) * density * (Length - Length0) * ((nodeA.pos - nodeB.pos) / Length) * stiffness;
            force += -manager.dampingRotation * (nodeA.vel - nodeB.vel);
            force += -manager.dampingDeformation * Vector3.Cross(u,(nodeA.vel - nodeB.vel));
            nodeA.force += force;
            nodeB.force -= force;

            
        }
        
    }
}