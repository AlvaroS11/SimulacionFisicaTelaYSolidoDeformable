using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class Tetraedro //: MonoBehaviour
    {
        public Node nodeA, nodeB, nodeC, nodeD;
       // public Node[] nodes = new Node[4];
        public Vector3 normalA, normalB, normalC, normalD;
        public float volume;
        public List<Node> nodes = new List<Node>();
        public Color color = Random.ColorHSV();
        public float massDensity;
        public float mass;
        // Start is called before the first frame update
        public Tetraedro(Node a, Node b, Node c, Node d, float density)
        {
            nodeA = a;
            nodeB = b;
            nodeC = c;
            nodeD = d;
           
            volume = Vector3.Dot((Vector3.Cross((nodeB.pos - nodeA.pos), (nodeC.pos - nodeA.pos))) , (nodeD.pos - nodeA.pos)) / 6; 
            normalA = Vector3.Cross((nodeB.pos - nodeA.pos), (nodeC.pos -nodeA.pos));
            normalB = Vector3.Cross((nodeC.pos - nodeB.pos), (nodeD.pos - nodeB.pos));
            normalC = Vector3.Cross((nodeD.pos - nodeB.pos), (nodeA.pos - nodeC.pos));
            normalD = Vector3.Cross((nodeA.pos - nodeD.pos), (nodeB.pos - nodeD.pos));
            massDensity = density;
        }

        public bool isInside(Node node)
        {
            Vector3 pos = node.pos;
            bool a = false , b = false, c = false, d = false;
            if (Vector3.Dot(normalA, pos) < 0)
                a = true;
            if (Vector3.Dot(normalB, pos) < 0)
                b = true;
            if (Vector3.Dot(normalC, pos) < 0)
                c = true;
            if (Vector3.Dot(normalD, pos) < 0)
                d = true;

            if (a && b && c && d)
            {
                nodes.Add(node);
                calculateBar(node);
                return true;
            }
            else
                return false;

        }

        public void calculateBar(Node node)
        {
            float [] volumeBar = new float [4];
            Vector3 helper;

            node.volume[0] = Vector3.Dot((Vector3.Cross((nodeB.pos - node.pos), (nodeC.pos - node.pos))), (nodeD.pos - node.pos)) / 6;
            node.volume[1] = Vector3.Dot((Vector3.Cross((node.pos - nodeA.pos), (nodeC.pos - nodeA.pos))), (nodeD.pos - nodeA.pos)) / 6;
            node.volume[2] = Vector3.Dot((Vector3.Cross((nodeB.pos - nodeA.pos), (node.pos - nodeA.pos))), (nodeD.pos - nodeA.pos)) / 6;
            node.volume[3] = Vector3.Dot((Vector3.Cross((nodeB.pos - nodeA.pos), (nodeC.pos - nodeA.pos))), (node.pos - nodeA.pos)) / 6;
            for(int i = 0; i < 4; i++)
            {
                node.peso[i] = node.volume[i]/volume;
            }
            helper = node.peso[0] * nodeA.pos + node.peso[1]*nodeB.pos + node.peso[2]*nodeC.pos + node.peso[3]*nodeD.pos ;
            node.pos = helper;
            calculateTetMass();

        }

        public void calculatePos(Node node)
        {
            node.pos = node.peso[0] * nodeA.pos + node.peso[1] * nodeB.pos + node.peso[2] * nodeC.pos + node.peso[3] * nodeD.pos;
        }

        public void calculateTetMass()
        {
            mass = massDensity / volume;
            nodeA.mass = mass/4;
            nodeB.mass = mass/4;
            nodeC.mass = mass/4;
            nodeD.mass = mass/4;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("ª");
        }
    }
}
