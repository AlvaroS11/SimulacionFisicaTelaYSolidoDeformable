using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class fixerMovement : MonoBehaviour
    {
        // Start is called before the first frame update
        Vector3 lastPost;
        public ElasticSolid controller;

        
        void Start()
        {
            lastPost = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (lastPost != transform.position)
            {
                foreach (Node node in controller.nodes)
                {
                    if (node.isFixed)
                    {

                        node.pos += transform.position - lastPost;
                    }
                }
                lastPost = transform.position;
            }
        }
    }
}