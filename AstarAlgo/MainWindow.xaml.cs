using AstarAlgo.Class;
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

namespace AstarAlgo
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PathFinder pathFinder = new PathFinder(10, 10);
        private static Timer timer = null;

        public MainWindow()
        {
            InitializeComponent();

            Start();
            timer = new Timer(Update, null, 0, 100);
            Console.ReadLine();
        }
        private static void Start()
        {
            ShowMap();
            SelectNextNode();
        }

        private static void Update(Object o)
        {
            ShowMap();
            SelectNextNode();
        }

        private static void ShowMap()
        {

            for (int y = 0; y < pathFinder.Height; y++)
            {
                string line = string.Empty;
                for (int x = 0; x < pathFinder.Width; x++)
                {
                    if (pathFinder.Nodes[x, pathFinder.Height - y - 1].IsEnd)
                    {
                        line += "E";
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].IsStart)
                    {
                        line += "S";
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isEndPath)
                    {
                        line += "¤";
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].Gcost > -1)
                    {
                        line += "_";
                    }
                    else if (pathFinder.Nodes[x, pathFinder.Height - y - 1].isWall)
                    {
                        line += "█";
                    }
                    else
                    {
                        line += ".";
                    }
                }
                Console.WriteLine(line);
            }
        }

        private static void SelectNextNode()
        {
            pathFinder.SelectNextNode();
        }
    }
}
