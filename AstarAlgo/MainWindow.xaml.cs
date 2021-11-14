using AstarAlgo.Class;
using AstarAlgo.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AstarAlgo
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NodeView[,] Nodes = new NodeView[10, 10];
        public PathFinder pathFinder = new PathFinder(10, 10);
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            Start();
        }
        private void Start()
        {
            /*
            ShowMap();
            pathFinder.SelectNextNode();
            */
            GenMap();
            timer.Tick += new EventHandler(Update);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            ShowMap();
            pathFinder.SelectNextNode();
        }

        private void GenMap()
        {
            for (int y = 0; y < pathFinder.Height; y++)
            {
                for (int x = 0; x < pathFinder.Width; x++)
                {
                    Nodes[x, y] = new NodeView();
                    Nodes[x, y].Margin = new Thickness(x * 82, y * 82, 0, 0);
                    gridView.Children.Add(Nodes[x, y]);
                }
            }
        }
        private void ShowMap()
        {
            for (int y = 0; y < pathFinder.Height; y++)
            {
                string line = string.Empty;
                for (int x = 0; x < pathFinder.Width; x++)
                {
                    Nodes[x, y].Gvalue.Text = pathFinder.Nodes[x, pathFinder.Height - y - 1].Gcost.ToString();
                    Nodes[x, y].Hvalue.Text = pathFinder.Nodes[x, pathFinder.Height - y - 1].Hcost.ToString();
                    Nodes[x, y].Fvalue.Text = pathFinder.Nodes[x, pathFinder.Height - y - 1].Fcost.ToString();
                    if (pathFinder.Nodes[x, pathFinder.Height - y - 1].IsEnd)
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#00F"));
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].IsStart)
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#F00"));
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isEndPath)
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#0F0"));
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isChecked)
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#DDD"));
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].Gcost > 0)
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#AAA"));
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isWall)
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#000"));
                    }
                    else
                    {
                        Nodes[x, y].background.Fill = (Brush)(new BrushConverter().ConvertFrom("#EEE"));
                    }
                }
            }
        }
    }
}
