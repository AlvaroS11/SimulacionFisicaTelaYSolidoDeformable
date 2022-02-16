using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class Triangulo// : MonoBehaviour
    {
        public Node nodeA;
        public Node nodeB;
        public Node nodeC;
        public Spring springA;
        public Spring springB;
        public Spring springC;

        // Start is called before the first frame update

        public Triangulo(Node nA, Node nB, Node nC, Spring sA, Spring sB, Spring sC)
        {
            nodeA = nA;
            nodeB = nB;
            nodeC = nC;
            springA = sA;
            springB = sB;
            springC = sC;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}