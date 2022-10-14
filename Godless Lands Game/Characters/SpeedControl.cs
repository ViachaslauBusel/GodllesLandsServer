
using BulletXNA.LinearMath;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Physics;
using RUCP;
using RUCP.Client;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characters
{
    class SpeedControl
    {
        public static Vector3 Check(Character character, Vector3 newPosition)
        {
            CharacterTransform transform = character.Transform;
            //Направление возврата, если игрок превысил скорость
            Vector3? returnDirection = null;
            //Расстояние которое прошел персонаж
            float distance = (transform.position.ClearY() - newPosition.ClearY()).Length();
   
            //За один пакет нельзя преодолеть расстояния больше чем за 1сек
            if (distance > character.Stats.MoveSpeed)
            {
                returnDirection = ReturnDirection(transform.position, newPosition, distance - character.Stats.MoveSpeed);
             
                distance = character.Stats.MoveSpeed;
                newPosition = CorrectPosition(transform.position, newPosition, distance);
            }
            //Время необходимое для преодоления этого растояния 
            double time = distance / character.Stats.MoveSpeed;
            //Время прошедшее с момента получения последнего пакета
            long timeServer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - transform.timeStamp;
         

            long delay = timeServer - (long)(time * 1_000); 
            //Если время для преодоления расстояния меньше чем время между пакетами
            if (delay > 0)
            {
                //Отнимаем его от общей задержки
                transform.delay -= delay;
                if (transform.delay < 0) transform.delay = 0;
            }
            else //Если пакет пришел раньше чем необходима времени для преодоления расстояния, добавляем это время к задержке 
                transform.delay += Math.Abs(delay);
           // Console.WriteLine($"Delay: {delay} sum: {transform.delay}");

            if (transform.delay > 1_000)
            {
                //Время между задержкой 1_000 и предыдущей задержкой
                /* long leftTime = 1_000 - (transform.delay - delay);
                 if(leftTime > 0)
                 {
                     double newDistance = (leftTime / 1_000.0) * character.Stats.MoveSpeed;
                     if(returnDirection != null)
                        returnDirection += ReturnDirection(transform.position, newPosition, distance - newDistance);
                     else returnDirection = ReturnDirection(transform.position, newPosition, distance - newDistance);

                     distance = newDistance;
                     newPosition = CorrectPosition(transform.position, newPosition, distance);
                 }*/
                MoveCorrection(character.Socket, ReturnDirection(transform.position, newPosition, distance));
                return transform.position;
            }
            transform.timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (returnDirection != null)
            {
                MoveCorrection(character.Socket, returnDirection.Value);
            }

            return newPosition;
        }

        public static void MoveCorrection(ClientSocket socket, Vector3 direction)
        {
            Packet packet = new Packet(socket, Channel.Reliable);
            packet.WriteType(Types.MoveCorrection);
            packet.WriteVector3(direction);
            packet.Send();
        }
        
        public static Vector3 CorrectPosition(Vector3 oldPosition, Vector3 newPosition, float distance)
        {
            Vector3 direction = newPosition - oldPosition;
            direction.Normalize();
            return oldPosition + (direction * distance);
        }
        /// <summary>
        /// Рассчитать направления в котором нужно возвратить игрока в скоректированную позицию
        /// </summary>
        public static Vector3 ReturnDirection(Vector3 oldPosition, Vector3 newPosition, float distance)
        {
            Vector3 returnPosition = oldPosition - newPosition;
            returnPosition.Normalize();
            returnPosition *= distance;
            return returnPosition;
        }
    }
}
