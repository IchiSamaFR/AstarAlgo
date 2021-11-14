using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstarAlgo.Class
{
    public class PathFinder
    {
        public Node[,] Nodes;

        public bool PathFind;

        public int Width;
        public int Height;
        
        public Position StartingNode;
        public Position EndingNode;

        public PathFinder(int width, int height)
        {
            Width = width;
            Height = height;
            StartingNode = new Position(0, 0);
            EndingNode = new Position(width - 1, height - 1);
            GenNodes();
            Start();
        }

        public PathFinder(int width, int height, Position start, Position end)
        {
            Width = width;
            Height = height;
            StartingNode = start;
            EndingNode = end;
            Start();
        }

        private void Start()
        {
            GenNodes();
            GenWall();
            Nodes[StartingNode.X, StartingNode.Y].SetCost(0, EndingNode);
        }

        public void SelectNextNode()
        {
            if (PathFind) return;
            
            Node nodeToSelect = null;
            int count = 0;
            foreach (var node in Nodes)
            {
                if (!node.isWall && !node.isChecked && (nodeToSelect == null || node.Fcost < nodeToSelect.Fcost) && node.Fcost > 0)
                {
                    nodeToSelect = node;
                }
                count++;
            }
            if(nodeToSelect != null)
            {
                Console.WriteLine(nodeToSelect.Pos.X + " " + nodeToSelect.Pos.Y);
                Console.WriteLine(nodeToSelect.Fcost);
                nodeToSelect.Select();
            }
            else
            {
                Console.WriteLine("Impossible");
            }
        }

        private void GenNodes()
        {
            Nodes = new Node[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Nodes[x, y] = new Node(new Position(x, y), this);
                    Nodes[x, y].End = EndingNode;
                }
            }
            Nodes[StartingNode.X, StartingNode.Y].IsStart = true;
            Nodes[EndingNode.X, EndingNode.Y].IsEnd = true;
        }
        private void GenWall()
        {
            Nodes[0, 6].isWall = true;
            Nodes[1, 6].isWall = true;
            Nodes[2, 6].isWall = true;

            Nodes[4, 5].isWall = true;
            Nodes[4, 6].isWall = true;
            Nodes[4, 7].isWall = true;
            Nodes[4, 8].isWall = true;

            Nodes[1, 4].isWall = true;
            Nodes[2, 4].isWall = true;
            Nodes[4, 4].isWall = true;
            Nodes[5, 4].isWall = true;
            Nodes[6, 4].isWall = true;
            Nodes[7, 4].isWall = true;
        }

        public List<Node> GetNodesAround(Position pos)
        {
            List<Node> nodes = new List<Node>();

            for (int x = pos.X - 1; x <= pos.X + 1; x++)
            {
                for (int y = pos.Y - 1; y <= pos.Y + 1; y++)
                {
                    if (x != pos.X || y != pos.Y)
                    {
                        Node node = GetNode(x, y);
                        if (node != null)
                            nodes.Add(node);
                    }
                }
            }

            return nodes;
        }
        public Node GetNode(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return null;

            return Nodes[x, y];
        }
    }
}
