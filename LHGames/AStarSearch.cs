using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarterProject.Web.Api;

namespace LHGames
{
    public class AStarSearch
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
        public Node[,] Map { get; set; }
        public Tile[,] Carte { get; set; }

        public AStarSearch(Node startNode, Node endNode, Node[,] map)
        {
            StartNode = startNode;
            EndNode = endNode;
            Map = map;
        }

        public AStarSearch(Node startNode, Node endNode, Tile [,] carte)
        {
            StartNode = startNode;
            EndNode = endNode;
            Carte = carte;
        }

        public bool Search(Node currentNode)
        {
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));



            return false;
        }

        private List<Node> GetAdjacentWalkableNodes(Node currentNode)
        {
            List<Node> adjacentNodes = new List<Node>();
            int currentX = currentNode.Position.X, currentY = currentNode.Position.Y;

            if (Map[currentX + 1,currentY].IsWalkable)
                adjacentNodes.Add(Map[currentX + 1,currentY]);
            if (Map[currentX - 1,currentY].IsWalkable)
                adjacentNodes.Add(Map[currentX - 1,currentY]);
            if (Map[currentX,currentY + 1].IsWalkable)
                adjacentNodes.Add(Map[currentX,currentY + 1]);
            if (Map[currentX,currentY - 1].IsWalkable)
                adjacentNodes.Add(Map[currentX,currentY - 1]);

            return adjacentNodes;

        }
    }
}
