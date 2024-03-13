using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Dan
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        List<LevelGraphs> tree;
        LevelGraphs prevLevelGraphs;
        List<LevelGraphs> aloneGraphs;
        List<Alfas> alfas;
        int numLevel = 1;
        int count = 2;
        int countAloneGraphs = 0;
        int m = 0;
        int N = 0;
        bool isEnded = false;
        bool isTreeComplete = false;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("", "Все вершины");
            dataGridView1.Columns.Add("", "Висячие вершины");
            dataGridView2.Columns.Add("", "№");
            dataGridView2.Columns.Add("", "Число вершин");
            dataGridView2.Columns.Add("", "Число висячих вершин");
            dataGridView2.Columns.Add("", "Альфа");
            dataGridView2.Columns.Add("", "Глубина");

            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.Series[0].IsVisibleInLegend = false;

            Axis ax = new Axis();
            ax.Title = "m";
            chart1.ChartAreas[0].AxisX = ax;

            Axis ay = new Axis();
            ay.Title = "P";
            chart1.ChartAreas[0].AxisY = ay;
        }

        int countRandomEdge = 0;
        bool createOneTree = false;

        private void button1_Click(object sender, EventArgs e)
        {
            isEnded = false;
            isTreeComplete = false;
            createOneTree = true;
            countRandomEdge = 0;
            dataGridView1.Rows.Clear();
            chart1.Series[0].Points.Clear();

            m = Convert.ToInt32(textBox1.Text);
            N = Convert.ToInt32(textBox2.Text);
            numLevel = 1;
            count = 2;
            countAloneGraphs = 0;
            tree = new List<LevelGraphs>();
            aloneGraphs = new List<LevelGraphs>();

            Graph graph = new Graph();
            LevelGraphs levelGraphs = new LevelGraphs();
            levelGraphs.numLevel = numLevel++;
            graph.Num = 1;
            graph.Parent = 0;
            levelGraphs.graphs.Add(graph);
            prevLevelGraphs = levelGraphs;
            tree.Add(levelGraphs);

            while (count <= N && !isEnded)
            {
                CreateTree();
            }

            LastLevelAloneTree();

            int rowIndex = 0;

            textBoxResult.Text = "";

            for (int i = 0; i < tree.Count; i++)
            {
                LevelGraphs level = tree[i];
                textBoxResult.AppendText(level.numLevel + ": ");
                for (int j = 0; j < level.graphs.Count; j++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex++].Cells[0].Value = level.graphs[j].Num + "-" + level.graphs[j].Parent;
                    textBoxResult.AppendText(level.graphs[j].Num + "-" + level.graphs[j].Parent + ", ");
                }
                textBoxResult.AppendText("\n");
            }
            textBoxResult.AppendText("\n\n");

            rowIndex = 0;
            textBoxAloneGraphs.Text = "";
            for (int i = 0; i < aloneGraphs.Count; i++)
            {
                LevelGraphs level = aloneGraphs[i];
                textBoxAloneGraphs.AppendText(level.numLevel + ": ");
                for (int j = 0; j < level.graphs.Count; j++)
                {
                    dataGridView1.Rows[rowIndex++].Cells[1].Value = level.graphs[j].Num + "-" + level.graphs[j].Parent;
                    textBoxAloneGraphs.AppendText(level.graphs[j].Num + "-" + level.graphs[j].Parent + ", ");
                }
                textBoxAloneGraphs.AppendText("\n");
            }
            textBoxAloneGraphs.AppendText("\n\n");

            label4.Text = "Висячие вершины- " + countAloneGraphs;
            double alfa = (count - 1.0) / countAloneGraphs;
            labelAlfa.Text = "Alfa = " + Math.Round(alfa, 3);

            createOneTree = false;
            button1.BackColor = Color.Bisque;
            button6.BackColor = Color.Transparent;
        }

        public void CreateTree()
        {
            LevelGraphs levelGraphs = new LevelGraphs();
            LevelGraphs alonelevelGraphs = new LevelGraphs();
            alonelevelGraphs.numLevel = numLevel - 1;
            levelGraphs.numLevel = numLevel++;
            int k = prevLevelGraphs.graphs.Count;
            isEnded = true;

            for (int i = 0; i < k; i++)
            {
                int numPar = prevLevelGraphs.graphs[i].Num;
                int ran;
                if (isDetermTree)
                {
                    ran = m;
                }
                else
                {
                    if (numPar == 1)
                    {
                        ran = random.Next(2, m);
                    }
                    else
                    {
                        ran = random.Next(m);
                    }
                }

                if (createOneTree)
                {
                    chart1.Series[0].Points.AddXY(ran, ++countRandomEdge);
                }

                if (isTreeComplete) ran = 0;

                if (ran != 0) isEnded = false;

                if (ran == 0)
                {
                    Graph aloneGraph = new Graph();
                    aloneGraph.Num = numPar;
                    aloneGraph.Parent = prevLevelGraphs.graphs[i].Parent;
                    alonelevelGraphs.graphs.Add(aloneGraph);
                    countAloneGraphs++;
                }

                if (!isTreeComplete)
                {
                    for (int j = 0; j < ran; j++)
                    {
                        Graph graph = new Graph();
                        graph.Num = count++;
                        graph.Parent = numPar;
                        levelGraphs.graphs.Add(graph);
                        if (count > N)
                        {
                            isTreeComplete = true;
                            break;
                        }
                    }
                }
            }
            prevLevelGraphs = levelGraphs;
            tree.Add(levelGraphs);
            aloneGraphs.Add(alonelevelGraphs);
        }

        public void LastLevelAloneTree()
        {
            LevelGraphs levelGraphs = new LevelGraphs();
            LevelGraphs alonelevelGraphs = new LevelGraphs();
            alonelevelGraphs.numLevel = numLevel - 1;
            levelGraphs.numLevel = numLevel++;
            int k = prevLevelGraphs.graphs.Count;
            isEnded = true;

            for (int i = 0; i < k; i++)
            {
                int numPar = prevLevelGraphs.graphs[i].Num;
                Graph aloneGraph = new Graph();
                aloneGraph.Num = numPar;
                aloneGraph.Parent = prevLevelGraphs.graphs[i].Parent;
                alonelevelGraphs.graphs.Add(aloneGraph);
                countAloneGraphs++;
            }
            aloneGraphs.Add(alonelevelGraphs);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            isDetermTree = true;
            createOneTree = true;
            countRandomEdge = 0;
            dataGridView1.Rows.Clear();
            chart1.Series[0].Points.Clear();
            isEnded = false;
            isTreeComplete = false;

            m = Convert.ToInt32(textBox1.Text);
            N = Convert.ToInt32(textBox2.Text);
            numLevel = 1;
            count = 2;
            countAloneGraphs = 0;
            tree = new List<LevelGraphs>();
            aloneGraphs = new List<LevelGraphs>();

            Graph graph = new Graph();
            LevelGraphs levelGraphs = new LevelGraphs();
            levelGraphs.numLevel = numLevel++;
            graph.Num = 1;
            graph.Parent = 0;
            levelGraphs.graphs.Add(graph);
            prevLevelGraphs = levelGraphs;
            tree.Add(levelGraphs);

            while (count <= N && !isEnded)
            {
                CreateTree();
            }

            LastLevelAloneTree();

            int rowIndex = 0;
            textBoxResult.Text = "";

            for (int i = 0; i < tree.Count; i++)
            {
                LevelGraphs level = tree[i];
                textBoxResult.AppendText(level.numLevel + ": ");
                for (int j = 0; j < level.graphs.Count; j++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex++].Cells[0].Value = level.graphs[j].Num + "-" + level.graphs[j].Parent;
                    textBoxResult.AppendText(level.graphs[j].Num + "-" + level.graphs[j].Parent + ", ");
                }
                textBoxResult.AppendText("\n");
            }
            textBoxResult.AppendText("\n\n");

            rowIndex = 0;
            textBoxAloneGraphs.Text = "";
            for (int i = 0; i < aloneGraphs.Count; i++)
            {
                LevelGraphs level = aloneGraphs[i];
                textBoxAloneGraphs.AppendText(level.numLevel + ": ");
                for (int j = 0; j < level.graphs.Count; j++)
                {
                    dataGridView1.Rows[rowIndex++].Cells[1].Value = level.graphs[j].Num + "-" + level.graphs[j].Parent;
                    textBoxAloneGraphs.AppendText(level.graphs[j].Num + "-" + level.graphs[j].Parent + ", ");
                }
                textBoxAloneGraphs.AppendText("\n");
            }
            textBoxAloneGraphs.AppendText("\n\n");

            label4.Text = "Висячие вершины- " + countAloneGraphs;
            double alfa = (count - 1.0) / countAloneGraphs;
            labelAlfa.Text = "Alfa = " + Math.Round(alfa, 3);

            createOneTree = false;

            button6.BackColor = Color.Bisque;
            button1.BackColor = Color.Transparent;
        }

        bool isDetermTree = false;

        public class LevelGraphs
        {
            public int numLevel;
            public List<Graph> graphs = new List<Graph>();
        }

        public class Graph
        {
            public int Num { get; set; }
            public int Parent { get; set; }
            public Vertex V { get; set; }
        }

        public class Alfas
        {
            public double Alfa { get; set; }
            public int CountVertex { get; set; }
        }

        public class Vertex
        {
            // vertex properties
        }
    }

}