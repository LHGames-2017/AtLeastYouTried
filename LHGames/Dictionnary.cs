using System;
using System.Collections.Generic;
using System.Text;
using StarterProject.Web.Api;
using LHGames;

namespace StarterProject.Web.Api.Dictionnary
{
    public class Dictionary
    {
        public Dictionary<byte, List<Point>> dico = new Dictionary<byte, List<Point>>();

        public void addValue(Tile tile)
        {
            /*Point point = new Point(tile.X, tile.Y);
            if(dico.ContainsKey(tile.C))

            dico.Add(tile.C, point);*/
        }

        /*public bool IsPresent(Tile tile)
        {
            return (dico.ContainsValue(new Point(tile.X, tile.Y)));
        }*/

        public Point IsClosest(Point posPlayer, byte typeTuile)
        {
            int distance = 1000;
            Point destinationPoint = new Point();
            /*foreach (KeyValuePair<byte,Point> element in dico)
            {
                if (element.Key == typeTuile)
                {
                    int pathCost = Node.GetPathCost(posPlayer, element.Value);
                    if (pathCost < distance)
                    {
                        distance = pathCost;
                        destinationPoint = new Point(element.Value.X, element.Value.Y);
                    }
                }
            }*/
            return destinationPoint;
        }
    }
}
