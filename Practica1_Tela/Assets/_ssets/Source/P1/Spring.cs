using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cloth
{
    public class Spring //: MonoBehaviour
    {

        public MassSpringCloth manager;
        public Node nodeA, nodeB;

        public float Length0;
        public float Length;

        public float stiffness;

        public Tipo tipo;
        public Spring(Node node1, Node node2, Tipo tipo, MassSpringCloth man)
        {
            manager = man;
            nodeA = node1;
            nodeB = node2;
            this.tipo = tipo;
            if (this.tipo == 0){
                stiffness = man.flexion;
            }
            else 
            stiffness = man.traccion;
            Length = (node1.pos - node2.pos).magnitude;
            Length0 = Length;
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
            Debug.Log("UPDATE SPRING");
            if (tipo == 0)
            {
                stiffness = manager.flexion;
                Debug.Log(tipo + " " + stiffness);
            }
            else
            {
                stiffness = manager.traccion;
                Debug.Log(tipo + " " + stiffness);
            }
            }
        
        public void UpdateLength()
        {
            Length = (nodeA.pos - nodeB.pos).magnitude;
        }

        public void ComputeForces()
        {
            if (tipo == Tipo.Flexion)
            {
                stiffness = manager.flexion;
              //  Debug.Log(tipo + " " + stiffness);
            }
            else if (tipo == Tipo.Traccion)
            {
                stiffness = manager.traccion;
               // Debug.Log(tipo + " " + stiffness);
            }

            Vector3 u = nodeA.pos - nodeB.pos;
            u.Normalize();
            Vector3 force = -stiffness * (Length - Length0) * u;
            force += -manager.dampingRotation * (nodeA.vel - nodeB.vel);
            force += -manager.dampingDeformation * Vector3.Cross(u,(nodeA.vel - nodeB.vel));
            nodeA.force += force;
            nodeB.force -= force;

            
        }
        
    }
}