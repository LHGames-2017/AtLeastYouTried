using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames
{
    public class AStarSearch
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
        public Node[,] Map { get; set; }

        public AStarSearch(Node startNode, Node endNode, Node[,] map)
        {
            StartNode = startNode;
            EndNode = endNode;
            Map = map;

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Node n = Map[i, j];
                    n.G = Node.GetPathCost(StartNode.Position, n.Position);
                    n.H = Node.GetPathCost(n.Position, EndNode.Position);
                }
            }
        }

        public List<Point> GetPath()
        {
            bool existingPath = Search(StartNode);

            List<Point> path = new List<Point>();
            if(existingPath)
            {
                Node currentNode = EndNode;
                while(currentNode.PreviousNode != null)
                    path.Add(currentNode.PreviousNode.Position);
                path.Reverse();
            }        

            return path;
        }

        public bool Search(Node currentNode)
        {
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

            foreach(Node n in nextNodes)
            {
                if (n.Position == EndNode.Position)
                    return true;
                else
                {
                    if (Search(n))
                        return true;
                }
            }
            return false;
        }

        private List<Node> GetAdjacentWalkableNodes(Node currentNode)
        {
            List<Node> adjacentNodes = new List<Node>();
            int currentX = currentNode.Position.X, currentY = currentNode.Position.Y;

            adjacentNodes.Add(Map[currentX + 1,currentY]);
            adjacentNodes.Add(Map[currentX - 1,currentY]);
            adjacentNodes.Add(Map[currentX,currentY + 1]);
            adjacentNodes.Add(Map[currentX,currentY - 1]);

            List<Node> walkableNodes = new List<Node>();

            foreach(Node n in adjacentNodes)
            {
                if (n.State == NodeState.CLOSED)
                    continue;

                if (!n.IsWalkable)
                    continue;

                if(n.State == NodeState.OPENED)
                {
                    int pathCost = Node.GetPathCost(n.Position, n.PreviousNode.Position);
                    int gTemp = currentNode.G + pathCost;
                    if(gTemp < n.G)
                    {
                        n.PreviousNode = currentNode;
                        walkableNodes.Add(n);
                    }
                }
                else
                {
                    n.PreviousNode = currentNode;
                    n.State = NodeState.OPENED;
                    walkableNodes.Add(n);
                }
            }

            return adjacentNodes;

        }
    }
}
