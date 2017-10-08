namespace StarterProject.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using LHGames;
    using StarterProject.Web.Api.Dictionnary;

    [Route("/")]
    public class GameController : Controller
    {
        AIHelper player = new AIHelper();
        ActionTypes gameState = ActionTypes.DefaultAction;
        List<Point> currentPath = new List<Point>();
        List<Node> resources = new List<Node>();
        Node house;
        bool canUpdate = true;
        Node[,] nodes;

        [HttpPost]
        public string Index([FromForm]string map)
        {
            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);
            if (canUpdate)
            {             

                nodes = new Node[carte.GetLength(0), carte.GetLength(1)];
                for (int i = 0; i < carte.GetLength(0); i++)
                {
                    for (int j = 0; j < carte.GetLength(1); j++)
                    {
                        Tile t = carte[i, j];
                        nodes[i, j] = new Node(t.C, t.X, t.Y);
                        if (t.C == 4)
                            resources.Add(nodes[i, j]);
                        if (t.C == 2)
                            house = nodes[i, j];
                        //dico.addValue(t);
                    }
                }
                canUpdate = false;
            }

            PrintMapConsole(carte);

            string action = StateAction(nodes, gameInfo);
            return action;
        }

        public void updateDico(Tile[,] map, Dictionary dico)
        {
            for (int i =0; i < map.GetLength(0);i++)
            {
                for (int j=0; j < map.GetLength(1); j++)
                {
                    Tile t = map[i, j];
                    /*if (!dico.IsPresent(t))
                    {
                        dico.addValue(t);
                    }*/
                }
            }
        }

        private void PrintMapConsole(Tile[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    
                    Tile t = map[j, i];
                    switch((TileContent)t.C)
                    {
                        case TileContent.House:
                            Console.Write("H ");
                            break;
                        case TileContent.Lava:
                            Console.Write("L ");
                            break;
                        case TileContent.Resource:
                            Console.Write("R ");
                            break;
                        case TileContent.Shop:
                            Console.Write("S ");
                            break;
                        case TileContent.Player:
                            Console.Write("P ");
                            break;
                        case TileContent.Empty:
                            Console.Write("- ");
                            break;
                        case TileContent.Wall:
                            Console.Write("W ");
                            break;
                    }
                }
                Console.Write("\n");
            }
        }

        public string StateAction(Node[,] nodes, GameInfo gameInfo)
        {
            string gameAction = "";

            switch (gameState)
            {
                case ActionTypes.DefaultAction:
                    if (resources.Count > 0)
                    {
                        foreach(Node r in resources)
                        {
                            AStarSearch search = new AStarSearch(new Node(6, gameInfo.Player.Position.X, gameInfo.Player.Position.Y), r, nodes);
                            currentPath = search.GetPath();
                            if (currentPath.Count > 0) break;
                        }
                        
                        if(currentPath.Count > 1)
                        {
                            gameState = ActionTypes.MoveAction;
                            gameAction = AIHelper.CreateMoveAction(currentPath[1]);
                        }
                    }
                    break;
                case ActionTypes.MoveAction:
                    if (currentPath.Count > 0)
                    {
                        gameAction = AIHelper.CreateMoveAction(currentPath[0]);
                        currentPath.RemoveAt(0);
                    }
                    else
                        gameState = ActionTypes.CollectAction;
                    Console.Write("Move");
                    break;
                //case "AttackAction":
                //    Console.Write("Attack");
                //    break;
                case ActionTypes.CollectAction:

                    if (gameInfo.Player.CarriedResources < gameInfo.Player.CarryingCapacity)
                        gameAction = AIHelper.CreateCollectAction(resources[0].Position);
                    else
                    {
                        AStarSearch searchHouse = new AStarSearch(new Node(6, gameInfo.Player.Position.X, gameInfo.Player.Position.Y), house, nodes);
                        currentPath = searchHouse.GetPath();
                        gameState = ActionTypes.MoveAction;
                    }

                    Console.Write("Collect");
                    break;
                //case "UpgradeAction":
                //    Console.Write("Upgrade");
                //    break;
                //case "StealAction":
                //    Console.Write("Steal");
                //    break;
                //case "PurchaseAction":
                //    Console.Write("Purchase");
                //    break;
                //case "HealAction":
                //    Console.Write("Heal");
                //    break;
            }

            return gameAction;
        }
    }
}
