using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames
{
    public enum NodeState { UNTESTED, OPENED, CLOSED }

    public class Node
    {
        /* The X and Y coordinates of the node */
        public Point Position { get; set; }
        public bool IsWalkable { get; set; }

        /* Movement cost to get to the node */
        public float G { get; set; }
        /* Estimated movement cost to destination point */
        public float H { get; set; }
        /* The score of the node */
        public float F { get { return G + H; } }

        /* The state of the node */
        public NodeState State { get; set; }
        /* The type of the node */
        public TileType Type { get; set; }
        /* The previous node of the path */
        public Node PreviousNode { get; set; }

        public Node(Byte C, int X, int Y)
        {
            Position = new Point(X, Y);
            Type = (TileType)C;
            IsWalkable = Type == TileType.T ? true : false;
            State = NodeState.UNTESTED;
            PreviousNode = null;
        }

        public static int GetPathCost(Point fromPos, Point toPos)
        {
            return Math.Abs(fromPos.X - toPos.X) + Math.Abs(fromPos.Y - toPos.Y);
        }
    }
}
