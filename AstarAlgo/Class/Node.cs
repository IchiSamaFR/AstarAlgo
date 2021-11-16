using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstarAlgo.Class
{
    public class Node
    {
        public bool IsEnd;
        public bool IsStart;

        public bool isWall;
        public bool isEndPath;
        public bool isChecked;
        public Node Previous;
        public PathFinder Path;
        public Position Pos;
        public Position End;
        public int Gcost = -1; //Distance from starting node
        public int Hcost = -1; //Distance from ending node
        public int Fcost { get => Gcost + Hcost; } //Distance from ending node

        public Node(Position pos, PathFinder path)
        {
            Pos = pos;
            Path = path;
        }

        public void Select()
        {
            isChecked = true;
            CheckAround();
        }

        public void SetEndPath()
        {
            isEndPath = true;
            if (Previous != null)
                Previous.SetEndPath();
        }

        public void SetCost(int gcost, Position end)
        {
            if (isWall)
            {
                Gcost = -1;
                Hcost = -1;
                return;
            }

            Gcost = gcost;
            End = end;

            if (IsEnd)
            {
                Path.PathFind = true;
                Previous.SetEndPath();
                return;
            }


            Hcost = CalcHcost(end);
        }

        public int CalcHcost(Position end)
        {
            int x = end.X - Pos.X >= 0 ? end.X - Pos.X : Pos.X - end.X;
            int y = end.Y - Pos.Y >= 0 ? end.Y - Pos.Y : Pos.Y - end.Y;

            int val = 0;
            if (x - y >= 0)
            {
                val += x * 10;
                val += (x - (x - y)) * 4;
            }
            else
            {
                val += y * 10;
                val += (y - (y - x)) * 4;
            }
            return val;
        }

        public void SetCost(int gcost, int hcost)
        {
            Gcost = gcost;
            Hcost = hcost;
        }
        public void CheckAround()
        {
            List<Node> nodes = Path.GetNodesAround(Pos);

            for (int i = 0; i < nodes.Count; i++)
            {
                int newGCost = 0;
                if (nodes[i].Pos.X != Pos.X && nodes[i].Pos.Y != Pos.Y)
                    newGCost = Gcost + 14;
                else
                    newGCost = Gcost + 10;

                if (nodes[i].Gcost == -1 || nodes[i].Previous?.Gcost > newGCost)
                {
                    nodes[i].Previous = this;
                    nodes[i].SetCost(newGCost, End);
                    if (nodes[i].isEndPath) break;
                }
            }
        }
    }
}
