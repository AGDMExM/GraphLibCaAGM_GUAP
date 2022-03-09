using GraphLibCaAGM_GUAP.Model;
using System;
using System.Collections.Generic;

namespace GraphLibCaAGM_GUAP
{
    public class CaAGM
    {
        private MatrixModel _matrixModel;
        private EdgeModel _edgeModel;
        private TableOfRecordModel _tableOfRecordModel;

        public MatrixModel MatrixModel => _matrixModel;
        public EdgeModel EdgeModel => _edgeModel;
        public TableOfRecordModel TableOfRecordModel => _tableOfRecordModel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namesVertex">Названия вершин, в порядке их следования в матрице смежности</param>
        /// <param name="matrix">Матрица смежности</param>
        public void Init(List<string> namesVertex, List<List<int>> matrix)
        {
            _matrixModel = new MatrixModel(namesVertex, matrix);
            _edgeModel = new EdgeModel(_matrixModel);
            _tableOfRecordModel = new TableOfRecordModel(_matrixModel);

            //_GenerateVertexList();
            //_GenerateListEdge();
        }

        /// <summary>
        /// Создаёт png изображение графа graph.png
        /// </summary>
        public void GenerateImage()
        {
            Graphvizio gph = new Graphvizio();

            List<List<string>> pathesList = new List<List<string>>();

            foreach (Edge edge in _edgeModel.ListOfEdges)
            {
                pathesList.Add(new List<string>() { edge.From, edge.To, Convert.ToString(edge.Weight) });
            }

            gph.Execute(_matrixModel.NamesVertex, pathesList);
        }
    }
}
