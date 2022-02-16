using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class Fixer : MonoBehaviour
    {

        public MassSpringCloth manager;
        public Collider collider;
        Bounds esquinas;

        // Possibilities of the Fixer
        void Start()
        {
            esquinas = collider.bounds;
            Bounds bounds = GetComponent<Collider>().bounds;
           
        }




        public void Initialize()
        {
       //     Debug.Log("Initialized");
            foreach (Node nodes in manager.nodes)
            {
                Vector3 pos = nodes.pos;
                if (esquinas.Contains(pos))
                {
                    nodes.isFixed = true;
               
                }

            }
        }

    }
}