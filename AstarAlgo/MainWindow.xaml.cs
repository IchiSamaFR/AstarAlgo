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
        public static MainWindow instance;

        public NodeView[,] Nodes = new NodeView[10, 10];
        public PathFinder pathFinder = new PathFinder(10, 10);
        private DispatcherTimer timer = new DispatcherTimer();

        public bool ShowValues { get => showValues.IsChecked ?? false; }
        public bool Started;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;

            Start();
        }
        private void Start()
        {
            GenMap();
            timer.Tick += new EventHandler(Update);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            ShowMap();
            if (Started)
            {
                pathFinder.SelectNextNode();
                if (pathFinder.PathFind)
                    Started = false;
            }
        }

        private void GenMap()
        {
            for (int y = 0; y < pathFinder.Height; y++)
            {
                for (int x = 0; x < pathFinder.Width; x++)
                {
                    Nodes[x, y] = new NodeView();
                    Nodes[x, y].Margin = new Thickness(2 + x * (Nodes[x, y].Width + 2), y * (Nodes[x, y].Height + 2), 0, 0);
                    Nodes[x, y].Pos = new Position(x, y);
                    Nodes[x, y].Pos = new Position(x, y);
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
                    if (ShowValues)
                    {
                        Nodes[x, y].Gvalue.Text = pathFinder.Nodes[x, pathFinder.Height - y - 1].Gcost.ToString();
                        Nodes[x, y].Hvalue.Text = pathFinder.Nodes[x, pathFinder.Height - y - 1].Hcost.ToString();
                        Nodes[x, y].Fvalue.Text = pathFinder.Nodes[x, pathFinder.Height - y - 1].Fcost.ToString();
                    }
                    else
                    {
                        Nodes[x, y].Gvalue.Text = "";
                        Nodes[x, y].Hvalue.Text = "";
                        Nodes[x, y].Fvalue.Text = "";
                    }

                    if (pathFinder.Nodes[x, pathFinder.Height - y - 1].IsEndNode)
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#00F"));
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].IsStartNode)
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#F00"));
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].PathFound)
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#0F0"));
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isChecked)
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#DDD"));
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].Gcost > 0)
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#AAA"));
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isWall)
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#000"));
                    else
                        Nodes[x, y].background.Background = (Brush)(new BrushConverter().ConvertFrom("#EEE"));
                }
            }
        }

        public void AddWall(Position pos)
        {
            if (Started) return;

            pathFinder.Reset();
            pathFinder.Nodes[pos.X, pathFinder.Height - pos.Y - 1].isWall = !pathFinder.Nodes[pos.X, pathFinder.Height - pos.Y - 1].isWall;
        }
        private void StartPath_Click(object sender, RoutedEventArgs e)
        {
            pathFinder.Reset();
            Started = true;
        }
    }
}
