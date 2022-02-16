using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cloth
{
    /// <summary>
    /// Basic physics manager capable of simulating a given ISimulable
    /// implementation using diverse integration methods: explicit,
    /// implicit, Verlet and semi-implicit.
    /// </summary>
    public class MassSpringCloth : MonoBehaviour
    {
        /// <summary>
        /// Default constructor. Zero all. 
        /// </summary>
        public MassSpringCloth()
        {
            this.Paused = true;
            this.TimeStep = 0.01f;
            this.Gravity = new Vector3(0.0f, -9.81f, 0.0f);
            this.IntegrationMethod = Integration.Symplectic;
        }

        /// <summary>
        /// Integration method.
        /// </summary>
        public enum Integration
        {
            Explicit = 0,
            Symplectic = 1,
        };

        #region InEditorVariables

        public bool Paused;
        public float TimeStep;
        public Vector3 Gravity;
        public Integration IntegrationMethod;
        public List<Node> nodes;
        public List<Spring> springs;
        public List<Edge> edges;
        public List<Spring> ayudaSpr;
        public Fixer fixer;
        public float nodeMass;
        public float traccion;
        public float flexion;
        private Vector3 relativeFixerPos;
        private Vector3 lastPos;
        public float dampingMovement;
        public float dampingRotation;
        public float dampingDeformation;
        public int substeps;
        public Vector3 wind;
        public float windForce;
        #endregion

        #region OtherVariables
        #endregion

        #region MonoBehaviour

        public void Start()
        {
            Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;


            nodes = new List<Node>();
            springs = new List<Spring>();
            ayudaSpr = new List<Spring>();
            edges = new List<Edge>();

           // relativeFixerPos = fixer.transform.position;
            for (int m = 0; m < vertices.Length; m++)
            {
                Vector3 pos = transform.TransformPoint(vertices[m]);
                Node node = new Node(pos, m, this);
                nodes.Add(node);
                //Debug.Log(m + "nodos");

                node.relative = relativeFixerPos - pos;
            }

            //relativeFixerPos = 0;
            lastPos = fixer.transform.position;

           // Debug.Log(triangles.Length);
            int r = 0;
            int s = 0;

            Spring spring;

            for (int j = 0; j < triangles.Length; j = j + 3)
            {
                edges.Add(new Edge(nodes[triangles[j]], nodes[triangles[j + 1]], nodes[triangles[j + 2]]));
                edges.Add(new Edge(nodes[triangles[j + 1]], nodes[triangles[j + 2]], nodes[triangles[j]]));
                edges.Add(new Edge(nodes[triangles[j+2]], nodes[triangles[j]], nodes[triangles[j + 1]]));
            }
            Comparer comparer = new Comparer();
            edges.Sort(comparer);


            springs.Add(new Spring(edges[0].nodeA, edges[0].nodeB, Spring.Tipo.Traccion, this));
            s++;
            //Debug.Log("LLega al comparer");
            //Debug.Log(edges.Count);


            for (int i = 1; i < edges.Count; i++)
            {
                //Debug.Log(i);
                if (comparer.Compare(edges[i], edges[i - 1]) != 0)
                {
                    springs.Add(new Spring(edges[i].nodeA, edges[i].nodeB, Spring.Tipo.Traccion, this));
                    s++;
                    //Debug.Log(s);
                }
                else
                {
                    springs.Add(new Spring(edges[i].other, edges[i - 1].other, Spring.Tipo.Flexion, this));
                    r++;
                }
            }

         //   Debug.Log("Comparer termina");
            Debug.Log("Tracción: " + s + " Flexion: " + r);
            fixer.Initialize();


            
            
            #endregion
        }

        /*
        private bool IsTaken(Spring tubito)
        {
            foreach (Spring spring in springs)
            {
                if (tubito.GetHashCode() == spring.GetHashCode())
                {
                    return true;
                }
                else
                    return false;
            }
            return true;
        }
        */
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
                this.Paused = !this.Paused;

            for (int i = 0; i < springs.Count; i++)
            {
                if (springs[i].tipo == 0)
                {
                    Debug.DrawLine(springs[i].nodeA.pos, springs[i].nodeB.pos, Color.blue);
                }
                else
                    Debug.DrawLine(springs[i].nodeA.pos, springs[i].nodeB.pos, Color.red);
            }
        }

        public void FixedUpdate()
        {
            if (this.Paused)
                return; // Not simulating

            for (int i = 0; i < substeps; i++)
            {
                // Select integration method
                switch (this.IntegrationMethod)
                {
                    case Integration.Explicit: this.stepExplicit(); break;
                    case Integration.Symplectic: this.stepSymplectic(); break;
                    default:
                        throw new System.Exception("[ERROR] Should never happen!");
                }
            }

        }



        /// <summary>
        /// Performs a simulation step in 1D using Explicit integration.
        /// </summary>
        private void stepExplicit()
        {
            foreach (Node node in nodes)
            {
                node.force = Vector3.zero;
                node.force += nodeMass * node.gravity;
            }

            Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            int[] triangles = mesh.triangles;
            for (int j = 0; j < triangles.Length; j = j + 3)
            {
                Vector3 speedTriangle = (nodes[triangles[j]].vel + nodes[triangles[j+1]].vel + nodes[triangles[j+2]].vel)/3;
                Vector3 normal = Vector3.Cross((nodes[triangles[j + 1]].pos - nodes[triangles[j]].pos), (nodes[triangles[j + 2]].pos - nodes[triangles[j]].pos));
                float area = (normal.magnitude/2);
                Vector3 totalWindForce = area * windForce * Vector3.Dot(normal, (wind - speedTriangle)) * normal;
                nodes[triangles[j]].force += totalWindForce / 3;
                nodes[triangles[j+1]].force += totalWindForce / 3;
                nodes[triangles[j+2]].force += totalWindForce / 3;
            }
            


                foreach (Spring tubito in springs)
            {

                tubito.UpdateLength();
                tubito.ComputeForces();
            }

            foreach (Node node in nodes)
            {

                if (!node.isFixed)
                {
                    node.pos += (TimeStep/substeps) * node.vel;
                    node.vel += (TimeStep/substeps) / nodeMass * node.force;
                }
            }
            foreach (Spring tubito in springs)
            {
                tubito.Length = (tubito.nodeA.pos - tubito.nodeB.pos).magnitude;
            }

          //  Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = new Vector3[mesh.vertexCount];   

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transform.InverseTransformPoint(nodes[i].pos);

            }
            mesh.vertices = vertices;

        }

        /// <summary>
        /// Performs a simulation step in 1D using Symplectic integration.
        /// </summary>
        private void stepSymplectic()
        {

            foreach (Node node in nodes)
            {
                node.ComputeForces();   
            }

            Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            int[] triangles = mesh.triangles;
            for (int j = 0; j < triangles.Length; j = j + 3)
            {
                Vector3 speedTriangle = (nodes[triangles[j]].vel + nodes[triangles[j + 1]].vel + nodes[triangles[j + 2]].vel) / 3;
                Vector3 normal = Vector3.Cross((nodes[triangles[j + 1]].pos - nodes[triangles[j]].pos), (nodes[triangles[j + 2]].pos - nodes[triangles[j]].pos));
                float area = (normal.magnitude / 2);
                Vector3 totalWindForce = area * windForce * Vector3.Dot(normal, (wind - speedTriangle)) * normal;
                nodes[triangles[j]].force += totalWindForce / 3;
                nodes[triangles[j + 1]].force += totalWindForce / 3;
                nodes[triangles[j + 2]].force += totalWindForce / 3;
            }

            foreach (Spring tubito in springs)
            {
                tubito.UpdateLength();
                tubito.ComputeForces();
            }


            foreach (Node node in nodes)
            {

                if (!node.isFixed)
                {
                    node.vel += (TimeStep/substeps) / nodeMass * node.force;
                    node.pos += (TimeStep/substeps) * node.vel;
                }
            }
            foreach (Spring tubito in springs)
            {
                tubito.Length = (tubito.nodeA.pos - tubito.nodeB.pos).magnitude;
            }

          //  Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = new Vector3[mesh.vertexCount];


            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transform.InverseTransformPoint(nodes[i].pos);

            }
            mesh.vertices = vertices;


        }

    }
}