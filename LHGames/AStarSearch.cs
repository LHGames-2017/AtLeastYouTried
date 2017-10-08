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

        private int deltaX, deltaY;
        private List<Node> frontier = new List<Node>();

        public AStarSearch(Node startNode, Node endNode, Node[,] map)
        {
            StartNode = startNode;
            EndNode = endNode;
            Map = map;
            deltaX = map[0, 0].Position.X;
            deltaY = map[0, 0].Position.Y;

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Node n = Map[i, j];
                    n.H = Node.GetPathCost(n.Position, EndNode.Position);
                }
            }
        }

        private void PrintMapConsole()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Node n = Map[j, i];
                    Console.Write(n.F + " - ");
                }
                Console.Write("\n");
            }
        }

        public List<Point> GetPath()
        {
            bool existingPath = Search(StartNode);

            List<Point> path = new List<Point>();
            List<Node> nodes = new List<Node>();
            if(existingPath)
            {
                Node currentNode = EndNode;
                path.Add(currentNode.Position);
                nodes.Add(currentNode);
                while (currentNode.PreviousNode != null)
                {
                    path.Add(currentNode.PreviousNode.Position);
                    nodes.Add(currentNode.PreviousNode);
                    currentNode = currentNode.PreviousNode;
                }
                path.Reverse();
                nodes.Reverse();
            }

            if(nodes.Count > 0 && !nodes.ElementAt(path.Count - 1).IsWalkable)
                path.RemoveAt(path.Count - 1);

            return path;
        }

        public bool Search(Node currentNode)
        {
            currentNode.State = NodeState.CLOSED;
            frontier.Remove(currentNode);
            frontier.AddRange(GetAdjacentWalkableNodes(currentNode));
            frontier.Sort((node1, node2) => node1.F.CompareTo(node2.F));

            var n = frontier.FirstOrDefault();
            if (n == null)
                return false;
            if (n.Position.X == EndNode.Position.X && n.Position.Y == EndNode.Position.Y)
                return true;
            else
            {
                return Search(n);
                
            }
        }

        private List<Node> GetAdjacentWalkableNodes(Node currentNode)
        {
            List<Node> adjacentNodes = new List<Node>();
            int currentX = currentNode.Position.X - deltaX, currentY = currentNode.Position.Y - deltaY;

            if (currentX < Map.GetLength(0) - 1)
                adjacentNodes.Add(Map[currentX + 1, currentY]);
            if (currentX > 0)
                adjacentNodes.Add(Map[currentX - 1, currentY]);
            if (currentY < Map.GetLength(1) - 1)
                adjacentNodes.Add(Map[currentX, currentY + 1]);
            if (currentY > 0)
                adjacentNodes.Add(Map[currentX, currentY - 1]);

            List<Node> walkableNodes = new List<Node>();

            foreach (Node n in adjacentNodes)
            {
                if (n.State == NodeState.CLOSED)
                    continue;

                if (!n.IsWalkable && n.Position != EndNode.Position)
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

            return walkableNodes;

        }
    }
}
