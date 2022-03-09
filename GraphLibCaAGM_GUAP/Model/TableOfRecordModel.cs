using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibCaAGM_GUAP.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public class RowTable
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int ChildCount { get; set; }
        public int ParentCount { get; set; }
        public int NeighborsCount { get; set; }
        public List<int> NumbersChildVertex { get; set; } = new List<int>();
        public List<int> NumbersParentVertex { get; set; } = new List<int>();
        public List<int> NumbersNeighborsVertex { get; set; } = new List<int>();
        public List<int> WeightsOutputEdge { get; set; } = new List<int>();
        public List<int> WeightsInputEdge { get; set; } = new List<int>();

    }


    public class TableOfRecordModel
    {
        private List<RowTable> _table;
        public TableOfRecordModel(MatrixModel matrix)
        {
            _table = new List<RowTable>();

            Init(matrix);
        }

        public List<RowTable> Table => _table;

        private void Init(MatrixModel matrixModel)
        {
            List<string> namesVertex = matrixModel.NamesVertex;

            for (int i = 0; i < namesVertex.Count; i++)
                _table.Add(new RowTable() { Number = i, Name = namesVertex[i] });

            for (int numRow = 0; numRow < matrixModel.Matrix.Count; numRow++)
            {
                List<int> line = matrixModel.Matrix[numRow];
                RowTable currRow = _table[numRow];


                for (int numColumn = 0; numColumn < line.Count; numColumn++)
                {
                    int value = line[numColumn];

                    if (value == 0)
                        continue;

                    currRow.ChildCount++;
                    currRow.NeighborsCount++;
                    currRow.NumbersChildVertex.Add(numColumn);
                    currRow.NumbersNeighborsVertex.Add(numColumn);
                    currRow.WeightsOutputEdge.Add(value);
                    _table[numColumn].ParentCount++;
                    _table[numColumn].NumbersParentVertex.Add(numRow);
                    _table[numColumn].WeightsInputEdge.Add(value);
                    _table[numColumn].NeighborsCount++;
                    _table[numColumn].NumbersNeighborsVertex.Add(value);
                }

                
            }
        }

        private RowTable GetRowByVertexName(string name)
        {
            foreach (RowTable row in _table)
                if (row.Name == name)
                    return row;

            return null;
        }

        public List<string> GetNeighborsVertexByName(string name)
        {
            RowTable row = GetRowByVertexName(name);

            List<string> res = new List<string>();

            foreach (int num in row.NumbersNeighborsVertex)
                res.Add(_table[num].Name);

            return res;
        }

        public bool PathExist(List<string> pathes)
        {
            for (int num=0; num < pathes.Count - 1; num++)
            {
                RowTable curRow = GetRowByVertexName(pathes[num]);
                bool flag = false;

                for (int numNeighbor = 0; numNeighbor < curRow.NumbersNeighborsVertex.Count; numNeighbor++)
                {
                    if (pathes[num + 1] == _table[curRow.NumbersNeighborsVertex[numNeighbor]].Name)
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
            for (int i=0; i < _table.Count; i++)
            {
                RowTable currVertex = _table[i];
                sumInzEdge.Add(0);

                foreach (int weight in currVertex.WeightsInputEdge)
                    sumInzEdge[i] += weight;

                foreach (int weight in currVertex.WeightsOutputEdge)
                    sumInzEdge[i] += weight;
            }

            List<int> res = new List<int>();
            for (int i = 0; i < sumInzEdge.Count; i++)
                if (sumInzEdge[i] > value)
                    res.Add(i);

            return res;
        }

        public int GetCountEdge()
        {
            int res = 0;

            foreach (RowTable row in _table)
                res += row.NumbersChildVertex.Count;

            return res;
        }
    }
}
