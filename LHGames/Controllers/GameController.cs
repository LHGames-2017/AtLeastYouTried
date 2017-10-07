namespace StarterProject.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using LHGames;

    [Route("/")]
    public class GameController : Controller
    {
        AIHelper player = new AIHelper();

        [HttpPost]
        public string Index([FromForm]string map)
        {
            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);

            Node[,] nodes = new Node[carte.GetLength(0), carte.GetLength(1)];
            for (int i = 0; i < carte.GetLength(0); i++)
            {
                for (int j = 0; j < carte.GetLength(1); j++)
                {
                    Tile t = carte[i, j];
                    nodes[i, j] = new Node(t.C, t.X, t.Y);
                }
            }

            PrintMapConsole(carte);

            // INSERT AI CODE HERE.

            Point pos = new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y);

            string action = AIHelper.CreateMoveAction(pos);
            return action;
        }

        private void PrintMapConsole(Tile[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    
                    Tile t = map[i, j];
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
    }
}
