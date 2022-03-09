using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibCaAGM_GUAP.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public class EdgeModel
    {
        private List<Edge> _listOfEdges = new List<Edge>();
        private List<string> _namesVertex = new List<string>();

        public EdgeModel(MatrixModel matrixModel)
        {
            Init(matrixModel);
        }

        public List<Edge> ListOfEdges => _listOfEdges;
        
        private void Init(MatrixModel matrixModel)
        {
            _namesVertex = matrixModel.NamesVertex;
            for (int numRow = 0; numRow < matrixModel.Matrix.Count; numRow++)
            {
                List<int> line = matrixModel.Matrix[numRow];

                for (int numColumn=0; numColumn < line.Count; numColumn++)
                {
                    int value = line[numColumn];

                    if (value == 0)
                        continue;
                    
                    _listOfEdges.Add(new Edge(_namesVertex[numRow], _namesVertex[numColumn], value));
                    
                }
            }
        }

        private int GetNumVertexByName(string name)
        {
            for (int i = 0; i < _listOfEdges.Count; i++)
                if (_listOfEdges[i].From == name)
                    return i;

            return -1;
        }

        public List<string> GetNeighborsVertexByName(string name)
        {
            List<string> res = new List<string>();

            foreach (Edge edge in _listOfEdges)
            {
                if (name == edge.From)
                    res.Add(edge.To);
                if (name == edge.To)
                    res.Add(edge.From);
            }

            return res;
        }

        public bool PathExist(List<string> pathes)
        {
            for (int i=0; i < pathes.Count - 1; i++)
            {
                string nameVertex = pathes[i];
                List<string> connPathes = new List<string>();
                foreach (Edge edge in _listOfEdges)
                    if (edge.From == nameVertex)
                        connPathes.Add(edge.To);

                bool flag = false;
                foreach (string connName in connPathes)
                {

                    if (connName == pathes[i + 1])
                        flag = true;
                }

                if (!flag)
                    return false;
            }

            return true;
        }

        public List<int> GetNumVertexWhoHaveSumWeightInzidentEdgeMoreValue(int value)
        {
            List<int> sumInzEdge = new List<int>();

            for (int i = 0; i < _namesVertex.Count; i++)
                sumInzEdge.Add(0);

            for (int numEdge = 0; numEdge < _listOfEdges.Count; numEdge++)
            {
                sumInzEdge[_namesVertex.IndexOf(_listOfEdges[numEdge].From)] += _listOfEdges[numEdge].Weight;
                sumInzEdge[_namesVertex.IndexOf(_listOfEdges[numEdge].To)] += _listOfEdges[numEdge].Weight;
            }

            List<int> res = new List<int>();
            for (int i = 0; i < sumInzEdge.Count; i++)
                if (sumInzEdge[i] > value)
                    res.Add(i);

            return res;
        }

        public int GetCountEdge()
        {
            return _listOfEdges.Count;
        }
    }
}
