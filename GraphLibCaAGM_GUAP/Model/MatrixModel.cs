using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibCaAGM_GUAP.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public class MatrixModel
    {
        private List<List<int>> _matrix = new List<List<int>>();
        private List<string> _namesVertex = new List<string>();

        public MatrixModel(List<string> namesVertex, List<List<int>> matrix)
        {
            Init(namesVertex, matrix);
        }

        public List<List<int>> Matrix => _matrix;

        public List<string> NamesVertex => _namesVertex;

        public void Init(List<string> namesVertex, List<List<int>> matrix)
        {
            _matrix = matrix;
            _namesVertex = namesVertex;
        }

        private int GetNumVertexByName(string name)
        {
            for (int i=0; i < _namesVertex.Count; i++)
            {
                if (_namesVertex[i] == name)
                    return i;
            }

            return -1;
        }

        public List<string> GetNeighborsVertexByName(string name)
        {
            List<string> res = new List<string>();

            int idVertex = GetNumVertexByName(name);

            for(int i=0; i < _matrix[idVertex].Count; i++)
            {
                if (_matrix[idVertex][i] != 0)
                    res.Add(_namesVertex[i]);
            }

            for (int i = 0; i < _matrix[idVertex].Count; i++)
            {
                if (_matrix[i][idVertex] != 0)
                    res.Add(_namesVertex[i]);
            }

            return res;
        }

        public bool PathExist(List<string> pathes)
        {
            for (int i = 0; i < pathes.Count - 1; i++)
            {
                string nameVertex = pathes[i];
                List<string> connPathes = new List<string>();

                for (int num=0; num < _namesVertex.Count; num++)
                {
                    if (_matrix[_namesVertex.IndexOf(nameVertex)][num] != 0)
                        connPathes.Add(_namesVertex[num]);
                }

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
            string currVertex = _namesVertex[0];
            List<int> sumInzEdge = new List<int>();
            

            for (int row=0; row < _matrix.Count; row++)
            {
                sumInzEdge.Add(0);

                for (int column = 0; column < _matrix.Count; column++)
                {
                    if (_matrix[row][column] != 0)
                    {
                        sumInzEdge[row] += _matrix[row][column];
                    }
                }

                for (int rowForInpEdge = 0; rowForInpEdge < _matrix.Count; rowForInpEdge++)
                {
                    if (_matrix[rowForInpEdge][row] != 0)
                    {
                        sumInzEdge[row] += _matrix[rowForInpEdge][row];
                    }
                }
            }

            List<int> res = new List<int>();
            for (int i = 0; i < sumInzEdge.Count; i++)
                if (sumInzEdge[i] > value)
                    res.Add(i);

            return res;
        }

        public int GetCountEdge()
        {
            int count = 0;
            for (int row = 0; row < _matrix.Count; row++)
            {
                for (int column = 0; column < _matrix.Count; column++)
                {
                    if (_matrix[row][column] != 0)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
