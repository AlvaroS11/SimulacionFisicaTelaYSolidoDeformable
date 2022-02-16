using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Cloth
{
    public class ElasticSolid : MonoBehaviour
    {
        // Start is called before the first frame update

        public ElasticSolid()
        {
            this.Paused = true;
            this.TimeStep = 0.01f;
            this.Gravity = new Vector3(0.0f, -9.81f, 0.0f);
            this.IntegrationMethod = Integration.Symplectic;
        }

        public enum Integration
        {
            Explicit = 0,
            Symplectic = 1,
        };


        public List<Node> nodes = new List<Node>();
        public List<Spring> springs = new List<Spring>();
        public float nodeMass = 1;
        public float traccion;
        public Parser parser;


        public bool Paused;
        public float TimeStep;
        public Vector3 Gravity;
        public Integration IntegrationMethod;
        public List<Spring> ayudaSpr;
        public Fixer fixer;
      //  public float flexion;
        private Vector3 relativeFixerPos;
        private Vector3 lastPos;
        public float dampingMovement;
        public float dampingRotation;
        public float dampingDeformation;
        public int substeps;
        public Vector3 wind;
        public float windForce;
        Mesh mesh;
        public List<Triangulo> triangulos = new List<Triangulo>();
        List<Node> nodesT = new List<Node>();
        int[] triangles;
        Vector3[] vertices;
        public float rigidDensity;

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (Node node in nodes)
            {
                Gizmos.DrawSphere(node.pos, 0.2f);
            }
            
              Gizmos.color = Color.red;

            foreach (Spring spring in springs)
            {
                Gizmos.DrawLine(spring.nodeA.pos, spring.nodeB.pos);
            }

            try
            {
                foreach (Tetraedro tetraedro in parser.tetraedros)
                {
                    Gizmos.color = tetraedro.color;
                    foreach (Node node in tetraedro.nodes)
                    {
                        Vector3 position = transform.TransformPoint(node.pos);
                        Gizmos.DrawSphere(position, 0.1f);
                    }

                }
            }
            catch (NullReferenceException e)
            {
            }
        }



        void Start()
        {
          
           // Debug.Log(" parseando");
            parser.ParsearNodos();
            parser.ParsearMuelles();
            mesh = this.GetComponentInChildren<MeshFilter>().mesh;
            triangles = mesh.triangles;
            vertices = mesh.vertices;


            for (int m = 0; m < vertices.Length; m++)
            {
                Vector3 pos = transform.TransformPoint(vertices[m]);
                Node node = new Node(pos, m, this);
                nodesT.Add(node);
                node.relative = relativeFixerPos - pos;
            }
           
            for (int j = 0; j < nodesT.Count; j = j + 3)
            {
                
                Spring a = new Spring(nodesT[j], nodesT[j + 1], Spring.Tipo.Traccion, this, null);
                Spring b = new Spring(nodesT[j+1], nodesT[j + 2], Spring.Tipo.Traccion, this, null);
                Spring c = new Spring(nodesT[j+2], nodesT[j], Spring.Tipo.Traccion, this, null);
                triangulos.Add(new Triangulo(nodesT[triangles[j]], nodesT[triangles[j + 1]], nodesT[triangles[j + 2]], a, b, c)); 
                Debug.Log(j);
            }
            foreach (Tetraedro tetraedro in parser.tetraedros)
            {
                foreach(Node node in nodesT)
                {
                    if (tetraedro.isInside(node))
                    {
                        tetraedro.nodes.Add(node);
                    }
                }
            }
           

            fixer.Initialize();
        }

        // Update is called once per frame
   
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


            //calculo de el viento
            for (int j = 0; j < triangulos.Count; j++)
            {
                Vector3 speedTriangle = (triangulos[j].nodeA.vel + triangulos[j].nodeB.vel + triangulos[j].nodeC.vel)/3;

                Vector3 normal = Vector3.Cross((triangulos[j].nodeB.pos - triangulos[j].nodeB.pos), (triangulos[j].nodeB.pos - triangulos[j].nodeB.pos));
                float area = (normal.magnitude / 2);
                Vector3 totalWindForce = area * windForce * Vector3.Dot(normal, (wind - speedTriangle)) * normal;
                triangulos[j].nodeA.force += totalWindForce;
                triangulos[j].nodeB.force += totalWindForce;
                triangulos[j].nodeC.force += totalWindForce;

            }
            
            foreach (Spring tubito in springs)
            {

                tubito.UpdateLength();
                tubito.ComputeForces(rigidDensity);
            }

            foreach (Node node in nodes)
            {

                if (!node.isFixed)
                {
                    node.pos += (TimeStep / substeps) * node.vel;
                    node.vel += (TimeStep / substeps) / nodeMass * node.force;
                }
            }
            //calculo de la distancia de los muelles
            foreach (Spring tubito in springs)
            {
                tubito.Length = (tubito.nodeA.pos - tubito.nodeB.pos).magnitude;
            }


            foreach (Tetraedro tetraedro in parser.tetraedros)
            {
                foreach (Node node in tetraedro.nodes)
                {
                    tetraedro.calculatePos(node);
                }
            }

                Vector3[] aux = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                aux[i] = transform.InverseTransformPoint(nodesT[i].pos);

            }
            mesh.vertices = aux;

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

            for (int j = 0; j < triangulos.Count; j++)
            {
                Vector3 speedTriangle = (triangulos[j].nodeA.vel + triangulos[j].nodeB.vel + triangulos[j].nodeC.vel) / 3;

                Vector3 normal = Vector3.Cross((triangulos[j].nodeB.pos - triangulos[j].nodeB.pos), (triangulos[j].nodeB.pos - triangulos[j].nodeB.pos));
                float area = (normal.magnitude / 2);
                Vector3 totalWindForce = area * windForce * Vector3.Dot(normal, (wind - speedTriangle)) * normal;
                triangulos[j].nodeA.force += totalWindForce;
                triangulos[j].nodeB.force += totalWindForce;
                triangulos[j].nodeC.force += totalWindForce;

            }

            foreach (Spring tubito in springs)
            {
                tubito.UpdateLength();
                tubito.ComputeForces(rigidDensity);
            }


            foreach (Node node in nodes)
            {

                if (!node.isFixed)
                {
                    node.vel += (TimeStep / substeps) / nodeMass * node.force;
                    node.pos += (TimeStep / substeps) * node.vel;
                    
                }
            }
            foreach (Spring tubito in springs)
            {
                tubito.Length = (tubito.nodeA.pos - tubito.nodeB.pos).magnitude;
            }

       

            foreach(Tetraedro tetraedro in parser.tetraedros)
            {
                foreach(Node node in tetraedro.nodes)
                {
                    tetraedro.calculatePos(node);
                }

            }

            Vector3[] aux = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                aux[i] = transform.InverseTransformPoint(nodesT[i].pos);

            }
            mesh.vertices = aux;
        }

    }
}