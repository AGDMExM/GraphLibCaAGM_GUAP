using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibCaAGM_GUAP
{
    public class Graphvizio
    {
        AdjacencyGraph<string, Edge<string>> graph = new AdjacencyGraph<string, Edge<string>>();
        List<List<string>> _pathes = new List<List<string>>();

        public void Test()
        {
            graph.AddVertex("A");
            graph.AddVertex("B");

            graph.AddEdge(new Edge<string>("A", "B"));

            var graphViz = new GraphvizAlgorithm<string, Edge<string>>(graph, @".\", GraphvizImageType.Gif);
            graphViz.FormatVertex += FormatVertex;
            graphViz.FormatEdge += FormatEdge;
            graphViz.CommonEdgeFormat.Label.Value = "label";
            graphViz.Generate(new FileDotEngine(), "graph.dot");
        }

        public void Execute(List<string> namesVertex, List<List<string>> pathes)
        {
            foreach (string name in namesVertex)
                graph.AddVertex(name);

            foreach (List<string> path in pathes)
            {
                graph.AddEdge(new Edge<string>(path[0], path[1]));
            }

            _pathes = pathes;

            var graphViz = new GraphvizAlgorithm<string, Edge<string>>(graph, @".\", GraphvizImageType.Gif);
            graphViz.FormatVertex += FormatVertex;
            graphViz.FormatEdge += FormatEdge;
            graphViz.Generate(new FileDotEngine(), "graph.dot");
        }

        private string GetDstFromName(string source, string target)
        {
            foreach (List<string> path in _pathes)
            {
                if (path[0] == source && path[1] == target)
                    return path[2];
            }

            return "";
        }

        private void FormatVertex(object sender, FormatVertexEventArgs<string> e)
        {
            e.VertexFormatter.Label = e.Vertex;
        }

        private void FormatEdge(object sender, FormatEdgeEventArgs<string, Edge<string>> e)
        {
            e.EdgeFormatter.Label = new GraphvizEdgeLabel();
            e.EdgeFormatter.Label.Value = GetDstFromName(e.Edge.Source, e.Edge.Target);
        }
    }

    public sealed class FileDotEngine : IDotEngine
    {
        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            string output = outputFileName;
            System.IO.File.WriteAllText(output, dot);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("./Quizgraph/bin/dot.exe");
            process.StartInfo.Arguments = @"dot -Tgif graph.dot -o graph.png";
            process.Start();
            process.WaitForExit();

            return output;
        }
    }
}
