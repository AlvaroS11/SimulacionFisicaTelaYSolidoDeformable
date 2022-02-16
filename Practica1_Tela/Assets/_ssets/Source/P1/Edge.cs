using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class Edge
    {
        public Node nodeA, nodeB, other;

        public float Length0;
        public float Length;

        public float stiffness;

        public Edge(Node node1, Node node2, Node node3)
        {
            if (node1.referencia < node2.referencia)
            {
                nodeA = node1;
                nodeB = node2;
            }

            if(node1.referencia > node2.referencia)
            {
                nodeA = node2;
                nodeB = node1;
            }
            other = node3;

            stiffness = 100;
            Length = (node1.pos - node2.pos).magnitude;
            Length0 = Length;
        }
    }
}