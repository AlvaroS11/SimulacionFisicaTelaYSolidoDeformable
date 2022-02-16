using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class Comparer : IComparer<Edge>
    {

        public int Compare(Edge x, Edge y)
        {
            if (x.nodeA.referencia > y.nodeA.referencia)
                return 1;

            else if (x.nodeA.referencia < y.nodeA.referencia)
                return -1;

            else if (x.nodeB.referencia > y.nodeB.referencia)
                return 1;

            else if (x.nodeB.referencia < y.nodeB.referencia)
                return -1;

            else
                return 0;
        }
    }
}