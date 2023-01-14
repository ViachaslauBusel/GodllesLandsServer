using Godless_Lands_Game.Characters;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Teleport;
using RUCP;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

namespace Godless_Lands_Game.Map
{
    public partial class Tile
    {
        private ConcurrentDictionary<int, Character> players = new ConcurrentDictionary<int, Character>();

        public ICollection<Character> Players => players.Values;

        //Первый вход на карту
        public static void Enter(Character character)
        {

                Location location = Location.CreateLocation(character.Transform.position);

                if (!location.IsCorrectly)
                {//Если игрок находиться за картой
                    character.Transform.position = new Vector3(1180.0f, 183.0f, 1884.0f);
                    location = Location.CreateLocation(character.Transform.position);
                }
            character.Transform.location = location;


            foreach (Tile _map in new TileAround(location))
                {
                    SendAllCreate(character, _map.Players);//Отправка всем данных о вошедшем клиенте и отправка вошедшему клиенту информации о всех
                                                                  // sendMeMonsters(character, _map.monsters);//Отправка  данных о монстрах вошедшему клиенту
                                                                  //   sendMeResources(character, _map.resources);//Отправка  данных о ресурсах вошедшему клиенту
                                                                  //   sendMeMachines(character, _map.machines);
                                                                  //   sendMeCorpses(character, _map.corpses);
                                                                  //   sendMeNPC(character, _map.NPCs);
                }
                World.GetTile(location).AddLocalPlayer(character);
             

        }
        public static void Exit(Character character)
        {
            lock (character.Transform)
            {
                if (!character.Transform.location.IsCorrectly) return;
                foreach (Tile _map in new TileAround(character.Transform.location))
                {
                    SendAllDelete(character, _map.Players);
                }
                World.GetTile(character.Transform.location).DeleteLocalPlayer(character);
                character.Transform.location = Location.CreateEmpty();
            }
        }

        public static void ChangeLocation(Character character, Location newLocation)
        {
                if (!newLocation.IsCorrectly)
                {
                    // Message.systemMessage(character, "находиться в недопустимой локации: " + character.getTransform().position, MsgLayer.System);
                    World.TeleportToPoint(character, SpawnPoint.FindAraund(character.Transform.position));
                    return;
                }

                foreach (Tile _map in new TileLeft(newLocation, character.Transform.location))
                {
                
                    SendAllDelete(character, _map.Players);//Отправка команды для игрока player для удаление игроков из списка _map.players
                    SendMeDelete(character, _map.Players);//Отправка команды для игроков из списка _map.players на удаление игрока player
                                                                 //   sendMonstersDelete(character, _map.monsters);//Отправка команды для игрока player для удаление монстров из списка _map.monsters
                                                                 //   sendResourcesDelete(character, _map.resources);//Отправка команды для игрока player для удаление ресурсов из списка _map.resources
                                                                 //   sendMachinesDelete(character, _map.machines);
                                                                 //   sendCorpsesDelete(character, _map.corpses);
                                                                 //   sendNPCDelete(character, _map.NPCs);
                }

                foreach (Tile _map in new TileLeft(character.Transform.location, newLocation))
                {
                    SendAllCreate(character, _map.Players);
                    //  sendMeMonsters(character, _map.monsters);
                    //   sendMeResources(character, _map.resources);
                    //   sendMeMachines(character, _map.machines);
                    //   sendMeCorpses(character, _map.corpses);
                    //   sendMeNPC(character, _map.NPCs);
                }

                World.GetTile(newLocation).AddLocalPlayer(character);
                World.GetTile(character.Transform.location).DeleteLocalPlayer(character);
                character.Transform.location = newLocation;
        }




        protected void AddLocalPlayer(Character character)
        {

            players.TryAdd(character.ID, character);
        }

        protected void DeleteLocalPlayer(Character character)
        {
            players.TryRemove(character.ID, out Character character1);
        }

       

        /* public  static void deadPlayer(Character character, Corpse corpse) {

             for (Map _map : new MapEnum(character.getTransform().location)) {
                 sendAllDead(character, corpse, _map.players);
             }
             World.getMap(character.getTransform().location).deleteLocalPlayer(character);
             character.getTransform().location = Location.zero;

         }*/

        private static void SendAllCreate(Character character, ICollection<Character> players)
        {
            foreach (Character otherClient in players)
            {
                if (otherClient == null || character.Equals(otherClient)) continue;

                //Отправка информации о новом игроке подключеным игрокам-->>
                Packet packet = Packet.Create(Channel.Reliable);
                packet.OpCode = (Types.CharacterCreate);
                packet.WriteCharacter(character);
               

               otherClient.Socket.Send(packet); 
                //<<--

                //Отправка информации о подключеных игроках новому игроку -->>
                packet = Packet.Create(Channel.Reliable);
                packet.OpCode = (Types.CharacterCreate);
                packet.WriteCharacter(otherClient);

                character.Socket.Send(packet);
                //<<--

            }
        }
        //Отсылает другим клиентам команду на удалению клиента socket
        private static void SendAllDelete(Character character, ICollection<Character> players)
        {
            foreach (Character otherClient in players)
            {
                if (otherClient == null || character.Equals(otherClient)) continue;
                Packet packet = Packet.Create(Channel.Reliable);
                packet.OpCode = (Types.CharacterDelete);
                packet.WriteInt(character.ID);
                otherClient.Socket.Send(packet);
            }
        }

     /*   private static void SendAllDead(Character character, Corpse corpse, Iterable<Character> players)
        {
            for (Character otherClient: players)
            {
                if (character.equals(otherClient)) continue;
                Packet packet = new Packet(otherClient.socket, Channel.Reliable);
                packet.writeType(Types.CharacterDead);
                packet.writeInt(character.login_id);
                packet.writeInt(corpse.id);
                Sender.send(packet);
            }
        }*/

        //Отсылает клиенту socket команду на удалению клиентов из списка players
        private static void SendMeDelete(Character character, ICollection<Character> players)
        {
            foreach (Character otherClient in players)
            {
                if (otherClient == null || character.Equals(otherClient)) continue;
                Packet packet = Packet.Create(Channel.Reliable);
                packet.OpCode = (Types.CharacterDelete);
                packet.WriteInt(otherClient.ID);
                character.Socket.Send(packet);
            }
        }
    }
}
