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
            GenNodes(Width, Height);
            GenPoints(StartingNode, EndingNode);
            GenWall();
        }

        public void SelectNextNode()
        {
            if (PathFind) return;
            
            Node nodeToSelect = null;
            foreach (var node in Nodes)
            {
                if (!node.isWall && !node.isChecked && (nodeToSelect == null || node.Fcost < nodeToSelect.Fcost) && node.Fcost > 0)
                {
                    nodeToSelect = node;
                }
            }
            if (nodeToSelect != null)
                nodeToSelect.Select();
            else
                PathFind = true;
        }

        public void Reset()
        {
            foreach (var item in Nodes)
            {
                item.ResetValues();
            }
            GenPoints(StartingNode, EndingNode);
            PathFind = false;
        }

        private void GenNodes(int width, int height)
        {
            Nodes = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Nodes[x, y] = new Node(new Position(x, y), this);
                    Nodes[x, y].EndNodePos = EndingNode;
                }
            }
        }
        private void GenWall()
        {

            Nodes[5, 6].isWall = true;
            Nodes[6, 6].isWall = true;

            Nodes[7, 6].isWall = true;

            Nodes[7, 5].isWall = true;
            Nodes[7, 4].isWall = true;
        }
        private void GenPoints(Position startingNode, Position endingNode)
        {
            Nodes[startingNode.X, startingNode.Y].SetCost(0, endingNode);
            Nodes[startingNode.X, startingNode.Y].IsStartNode = true;
            Nodes[endingNode.X, endingNode.Y].IsEndNode = true;
        }

        public List<Node> GetNodesAround(Position pos)
        {
            List<Node> nodes = new List<Node>();


            /*
            if(GetNode(pos.X + 1, pos.Y) != null)
                nodes.Add(GetNode(pos.X + 1, pos.Y));
            if (GetNode(pos.X - 1, pos.Y) != null)
                nodes.Add(GetNode(pos.X - 1, pos.Y));

            if (GetNode(pos.X, pos.Y + 1) != null)
                nodes.Add(GetNode(pos.X, pos.Y + 1));
            if (GetNode(pos.X, pos.Y - 1) != null)
                nodes.Add(GetNode(pos.X, pos.Y - 1));
            */

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
