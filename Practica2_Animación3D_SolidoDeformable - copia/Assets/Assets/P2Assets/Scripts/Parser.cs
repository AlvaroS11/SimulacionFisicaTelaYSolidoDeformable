using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

namespace Cloth {
    public class Parser : MonoBehaviour
    {

        public TextAsset fileNameNodes;
        public TextAsset fileNameSprings;
        public ElasticSolid elasticSolid;
        int numNodes;
        int numSprings;
        string[] textSprings;
        string[] textNodes;
        public float massDensity;

        public List<Tetraedro> tetraedros;

        CultureInfo locale = new CultureInfo("en-US");
        void Start()
        {
            // Ayuda TextAsset: https://docs.unity3d.com/ScriptReference/TextAsset.html
            // Ayuda Unity de String https://docs.unity3d.com/ScriptReference/String.html
            // Ayuda MSDN de String https://docs.microsoft.com/en-us/dotnet/api/system.string?redirectedfrom=MSDN&view=netframework-4.8
            // Ayuda MSDN de String.Split https://docs.microsoft.com/en-us/dotnet/api/system.string.split?view=netframework-4.8

            textNodes = fileNameNodes.text.Split(new string[] { " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            numNodes = int.Parse(textNodes[0]);
            //Debug.Log(textNodes);
            Console.WriteLine(textNodes);
            textSprings = fileNameSprings.text.Split(new string[] { " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            numSprings = int.Parse(textSprings[0]);
            tetraedros = new List<Tetraedro>();
        }

        public void ParsearNodos()
        {
            int number = 0;
            Debug.Log("Recibida llamaeda");
            Debug.Log(numNodes);
            for(int i = 5; i < textNodes.Length; i = i+4)
            {
                Vector3 pos = new Vector3(float.Parse(textNodes[i], locale), float.Parse(textNodes[i+1], locale), float.Parse(textNodes[i+2], locale));
                Node node = new Node(pos, number, elasticSolid);
                elasticSolid.nodes.Add(node);
                Debug.Log("Node" + number + " en pos: " + pos);
                number++;
            }
        }
        
        public void ParsearMuelles()
        {
            Debug.Log("Recibida llamaeda springs ");
            int numero = 0;
            int numeroCuad = 0;
            for(int i = 4; i< textSprings.Length;i = i + 5)
            {
                int numberI;
                int number2;
                int number3;
                int number4;
                int.TryParse(textSprings[i], out numberI);
                int.TryParse(textSprings[i+1], out number2);
                int.TryParse(textSprings[i+2], out number3);
                int.TryParse(textSprings[i+3], out number4);
                Tetraedro tetraedro = new Tetraedro(elasticSolid.nodes[numberI - 1], elasticSolid.nodes[number2 - 1], elasticSolid.nodes[number3 - 1], elasticSolid.nodes[number4 - 1], massDensity);
                tetraedros.Add(tetraedro);

                elasticSolid.springs.Add(new Spring(  elasticSolid.nodes[numberI - 1],    elasticSolid.nodes[number2 - 1], Spring.Tipo.Traccion,  elasticSolid, tetraedro));
                elasticSolid.springs.Add(new Spring(  elasticSolid.nodes[number2 - 1],   elasticSolid.nodes[number3 - 1], Spring.Tipo.Traccion,   elasticSolid, tetraedro));
                elasticSolid.springs.Add(new Spring(  elasticSolid.nodes[number3 - 1],   elasticSolid.nodes[numberI - 1], Spring.Tipo.Traccion,   elasticSolid, tetraedro));
                elasticSolid.springs.Add(new Spring(  elasticSolid.nodes[number3 - 1],   elasticSolid.nodes[number4 - 1], Spring.Tipo.Traccion,   elasticSolid, tetraedro));
                elasticSolid.springs.Add(new Spring(  elasticSolid.nodes[number4 - 1],   elasticSolid.nodes[numberI - 1], Spring.Tipo.Traccion,   elasticSolid, tetraedro));
                elasticSolid.springs.Add(new Spring(  elasticSolid.nodes[number2 - 1],   elasticSolid.nodes[number4 - 1], Spring.Tipo.Traccion,   elasticSolid, tetraedro));

                
                Debug.Log("Cuad nº: " + numeroCuad);
                Debug.Log("Spring " + numero + " con nodos " + elasticSolid.nodes[numberI - 1].referencia + " "+ elasticSolid.nodes[number2 - 1].referencia);
                numero++;
                Debug.Log("Spring " + numero + " con nodos " + elasticSolid.nodes[number2 - 1].referencia+ " " + elasticSolid.nodes[number3 - 1].referencia);
                numero++;
                Debug.Log("Spring " + numero + " con nodos " + elasticSolid.nodes[number3 - 1].referencia+ " " + elasticSolid.nodes[numberI - 1].referencia);
                numero++;
                Debug.Log("Spring " + numero + " con nodos " + elasticSolid.nodes[number3 - 1].referencia+ " " + elasticSolid.nodes[number4 - 1].referencia);
                numero++;
                Debug.Log("Spring " + numero + " con nodos " + elasticSolid.nodes[numberI - 1].referencia+ " " + elasticSolid.nodes[number4 - 1].referencia);
                numero++;
                Debug.Log("Spring " + numero + " con nodos " + elasticSolid.nodes[number2 - 1].referencia + " " + elasticSolid.nodes[number4 - 1].referencia);
                numero++;
                Debug.Log(" otros 6");
                numeroCuad++;

                Debug.Log("Tetraedro " + numeroCuad + tetraedro.nodeA.pos + " " + tetraedro.nodeB.pos + " " + tetraedro.nodeC.pos + " " + tetraedro.nodeD.pos + " ");
            }
        }
        
    }
}

