using AstarLibrary;
using AstarView.Controls;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AstarView
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
        public bool IsDiagonal { get => isDiagonal.IsChecked ?? false; }

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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            ShowMap();
            if (Started)
            {
                pathFinder.SelectNextNode();
                if (pathFinder.PathFinished)
                {
                    Started = false;
                }

                isDiagonal.IsEnabled = false;
            }
            else
            {
                isDiagonal.IsEnabled = true;
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
                    var node = Nodes[x, y];
                    var pathnode = pathFinder.Nodes[x, pathFinder.Height - y - 1];
                    if (ShowValues)
                    {
                        node.Gvalue.Text = pathnode.Gcost.ToString();
                        node.Hvalue.Text = pathnode.Hcost.ToString();
                        node.Fvalue.Text = pathnode.Fcost.ToString();
                        node.Mvalue.Text = "x" + pathnode.Multiplier.ToString();
                    }
                    else
                    {
                        node.Gvalue.Text = "";
                        node.Hvalue.Text = "";
                        node.Fvalue.Text = "";
                        node.Mvalue.Text = "";
                    }

                    if (pathnode.IsEndNode)
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#00F"));
                    else if (pathnode.IsStartNode)
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#F00"));
                    else if (pathnode.PathFound)
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#0F0"));
                    else if (pathnode.IsChecked)
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#DDD"));
                    else if (pathnode.Gcost > 0)
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#AAA"));
                    else if (pathnode.IsWall)
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#000"));
                    else
                        node.background.Background = (Brush)(new BrushConverter().ConvertFrom("#EEE"));
                }
            }
        }

        public void AddMultiplier(Position pos)
        {
            if (Started || !float.TryParse(multiplier.Text.Replace(".", ","), out float mult)) return;

            pathFinder.Reset();
            var node = pathFinder.GetNode(pos.X, pathFinder.Height - pos.Y - 1);
            node.SetCostMultiplier(mult);
        }
        private void StartPath_Click(object sender, RoutedEventArgs e)
        {
            pathFinder.Reset();
            pathFinder.IsDiagonal = IsDiagonal;
            Started = true;
        }
        private void WallMultiplier_Click(object sender, RoutedEventArgs e)
        {
            multiplier.Text = "0";
        }
        private void PathMultiplier_Click(object sender, RoutedEventArgs e)
        {
            multiplier.Text = "1";
        }
    }
}
