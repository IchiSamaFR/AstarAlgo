using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstarLibrary
{
    public class PathFinder
    {
        private bool _isDiagonal;
        private bool _isFinished;
        private Node[,] _nodes;

        public bool PathFind
        {
            get
            {
                return EndingNode.PathFound;
            }
        }
        public bool PathFinished
        {
            get
            {
                return _isFinished || PathFind;
            }
            set
            {
                _isFinished = value;
            }
        }
        public bool IsDiagonal
        {
            get
            {
                return _isDiagonal;
            }
            set
            {
                if (_isDiagonal != value)
                {
                    _isDiagonal = value;
                    SetNodesAround();
                }
            }
        }

        public int Width;
        public int Height;

        public Node[,] Nodes
        {
            get
            {
                return _nodes;
            }
            private set
            {
                _nodes = value;
            }
        }
        private Node StartingNode;
        private Node EndingNode;

        private Position StartingPos;
        private Position EndingPos;

        public PathFinder(int width, int height)
        {
            GenNodes(width, height);
            SetStartPos(new Position(0, 0));
            SetEndPos(new Position(width - 1, height - 1));
        }
        public PathFinder(int width, int height, Position start, Position end)
        {
            GenNodes(width, height);
            SetStartPos(start);
            SetEndPos(end);
        }

        public Node SelectNextNode()
        {
            if (PathFinished) return null;

            Node nodeToSelect = null;
            foreach (var node in Nodes)
            {
                if (!node.IsWall && !node.IsChecked && (nodeToSelect == null || node.Fcost < nodeToSelect.Fcost) && node.Fcost > 0)
                {
                    nodeToSelect = node;
                }
            }
            if (nodeToSelect != null)
            {
                nodeToSelect.Select();
                return nodeToSelect;
            }
            else
            {
                PathFinished = true;
                return null;
            }
        }
        public List<Node> SelectPath()
        {
            while(!PathFinished)
            {
                Node nodeToSelect = null;
                foreach (var node in Nodes)
                {
                    if (!node.IsWall && !node.IsChecked && (nodeToSelect == null || node.Fcost < nodeToSelect.Fcost) && node.Fcost > 0)
                    {
                        nodeToSelect = node;
                    }
                }
                if (nodeToSelect != null)
                {
                    nodeToSelect.Select();
                }
                else
                {
                    PathFinished = true;
                }
            }
            return EndingNode.GetEndPath();
        }

        public void Reset()
        {
            PathFinished = false;
            foreach (var item in Nodes)
            {
                item.ResetValues();
            }
            SetStartPos(StartingPos);
            SetEndPos(EndingPos);
        }
        private void GenNodes(int width, int height)
        {
            Width = width;
            Height = height;
            Nodes = new Node[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Nodes[x, y] = new Node(new Position(x, y));
                    Nodes[x, y].EndNodePos = EndingPos;
                }
            }
            SetNodesAround();
        }
        private void SetNodesAround()
        {
            foreach (var node in Nodes)
            {
                node.NodesAround = GetNodesAround(node.Pos);
            }
        }
        private void GenCosts(float[,] costs)
        {
            for (int x = 0; x < costs.GetLength(0); x++)
            {
                for (int y = 0; y < costs.GetLength(1); y++)
                {
                    var tmp = GetNode(x, y);
                    if (tmp != null)
                    {
                        tmp.SetCostMultiplier(costs[x, y]);
                    }
                }
            }
        }
        private void SetStartPos(Position startPos)
        {
            var node = GetNode(startPos.X, startPos.Y);
            if(node == null)
            {
                return;
            }
            StartingPos = startPos;
            StartingNode = node;
            if (EndingPos != null)
            {
                node.SetCost(0, EndingPos);
            }
            node.IsStartNode = true;
        }
        private void SetEndPos(Position endPos)
        {
            var node = GetNode(endPos.X, endPos.Y);
            if (node == null)
            {
                return;
            }
            EndingPos = endPos;
            EndingNode = node;
            if (StartingNode != null)
            {
                StartingNode.SetCost(0, EndingPos);
            }
            node.IsEndNode = true;
        }

        private List<Node> GetNodesAround(Position pos)
        {
            List<Node> nodes = new List<Node>();

            if (IsDiagonal)
            {
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
            }
            else
            {
                if (GetNode(pos.X + 1, pos.Y) != null)
                    nodes.Add(GetNode(pos.X + 1, pos.Y));
                if (GetNode(pos.X - 1, pos.Y) != null)
                    nodes.Add(GetNode(pos.X - 1, pos.Y));

                if (GetNode(pos.X, pos.Y + 1) != null)
                    nodes.Add(GetNode(pos.X, pos.Y + 1));
                if (GetNode(pos.X, pos.Y - 1) != null)
                    nodes.Add(GetNode(pos.X, pos.Y - 1));
            }


            return nodes;
        }
        private Node GetNode(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return null;

            return Nodes[x, y];
        }
    }
}
