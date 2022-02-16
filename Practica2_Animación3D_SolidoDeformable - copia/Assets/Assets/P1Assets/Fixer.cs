using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class Fixer : MonoBehaviour
    {

       public ElasticSolid  manager;
        public Collider colliderr;
        Bounds esquinas;

        // Possibilities of the Fixer
        void Start()
        {
            esquinas = colliderr.bounds;
            Bounds bounds = GetComponent<Collider>().bounds;
           
        }




        public void Initialize()
        {
       //     Debug.Log("Initialized");
            foreach (Node nodes in manager.nodes)
            {
                Vector3 pos = nodes.pos;
                Debug.Log(nodes.referencia);
                Debug.Log(pos);
                if (esquinas.Contains(pos))
                {
                    Debug.Log("dentro");
                    nodes.isFixed = true;
               
                }

            }
        }

    }
}