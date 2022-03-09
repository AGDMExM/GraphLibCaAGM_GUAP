using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibCaAGM_GUAP.Model
{
    public class Edge
    {
        private string _from;
        private string _to;
        private int _weigth;

        public Edge(string from, string to, int weight)
        {
            _from = from;
            _to = to;
            _weigth = weight;
        }

        public string From => _from;

        public string To => _to;

        public int Weight => _weigth;

        public object Clone() => MemberwiseClone();

    }
}
