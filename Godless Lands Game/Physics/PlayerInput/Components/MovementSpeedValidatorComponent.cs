using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.Stats;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics.PlayerInput.Components
{
    public class MovementSpeedValidatorComponent : Component
    {
        private TransformComponent _transform;
        private StatsComponent _stats;
        private long _delay = 0;
        private long _timeStamp;

        public override void Init()
        {
            _transform = GetComponent<TransformComponent>();
            _stats = GetComponent<StatsComponent>();
        }

        public Vector3 Check(Vector3 newPosition, out bool isNeedCorrect)
        {
            //Расстояние которое прошел персонаж
            float distance = (_transform.Position.ClearY() - newPosition.ClearY()).Length();
            float moveSpeed = _stats.GetStat(StatCode.MoveSpeed);

            //За один пакет нельзя преодолеть расстояния больше чем за 1сек
            if (distance > moveSpeed)
            {
                isNeedCorrect = true;
                return _transform.Position;
            }
            //Время необходимое для преодоления этого растояния 
            double time = distance / moveSpeed;
            //Время прошедшее с момента получения последнего пакета
            long elapsedTimeSinceLastPacket = Time.Milliseconds - _timeStamp;


            long delay = elapsedTimeSinceLastPacket - (long)(time * 1_000);
            //Если время для преодоления расстояния меньше чем время между пакетами
            if (delay > 0)
            {
                //Отнимаем его от общей задержки
                _delay = Math.Clamp(_delay - delay, 0, 10_000);
            }
            else //Если пакет пришел раньше чем необходима времени для преодоления расстояния, добавляем это время к задержке 
            {
                _delay = Math.Clamp(_delay + Math.Abs(delay), 0, 10_000);
            }

            if (_delay > 1_000)
            {
                isNeedCorrect = distance > 1f;
                return _transform.Position;
            }
            _timeStamp = Time.Milliseconds;

            isNeedCorrect = false;
            return newPosition;
        }
    }
}
