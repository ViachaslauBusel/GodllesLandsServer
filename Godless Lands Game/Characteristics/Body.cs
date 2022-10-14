using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Godless_Lands_Game.Characteristics
{
   public abstract class Body
    {
        private volatile int hp;
        private volatile int maxHP = 3400;
        private volatile int mp;
        private volatile int maxMP = 3000;
        private volatile int stamina;
        private volatile int maxStamina = 3000;
        private volatile bool alive = true;
        private Timer timer;
        private readonly object locker = new object();

        public int HP { get { lock (locker) { return hp; } } }
        public int MaxHP { get { lock (locker) { return maxHP; } } }
        public int MP { get { lock (locker) { return mp; } } }
        public int MaxMP { get { lock (locker) { return maxMP; } } }
        public int Stamina { get { lock (locker) { return stamina; } } }
        public int MaxStamina { get { lock (locker) { return maxStamina; } } }
        public bool Alive { get { lock (locker) { return alive; } } }

        public void Load()
        {
            lock (locker)
            {
                if (timer != null) return;
                Addition(maxHP, maxMP, maxStamina);
                timer = new Timer((a) => Tick(), null, 500, 500);
            }
        }
        private void Tick()
        {
            Addition(5, 6, 10);
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
        /// <summary>
        /// Вызывается при обновлении HP | MP | Stamina
        /// </summary>
        protected abstract void Synchronize();
        /// <summary>
        /// Вызывется при смерти персонажа
        /// </summary>
        protected abstract void Died();
        /// <summary>
        /// Вызывается при возрождении персонажа
        /// </summary>
        protected abstract void Awake();

       

        public (int hp, int mp, int stamina) Addition(int hp = 0, int mp = 0, int stamina = 0)
        {
            lock(locker) {
                if (!alive) return (0,0,0);
                hp = Math.Clamp(this.hp + hp, 0, maxHP);
                mp = Math.Clamp(this.mp + mp, 0, maxMP);
                stamina = Math.Clamp(this.stamina + stamina, 0, maxStamina);


                var result = (hp: hp - this.hp, mp: mp - this.mp, stamina: stamina - this.stamina);
                if (result.hp != 0 || result.mp !=0 || result.stamina != 0)
                {
                   
                    this.hp = hp;
                    this.mp = mp;
                    this.stamina = stamina;
                    if (this.hp == 0) Kill();
                    Synchronize();
                }
                return result;
            }
        }

        public void Resurrection()
        {
            lock(locker) {
                alive = true;
                Addition(maxHP, maxMP, maxStamina);
                Awake();
               // character.target().clearTargetOnMe();
            }
        }

        private void Kill()
        {
            if (!alive) return;
            alive = false;

           // corpse = Corpse.create(character);//Создать труп, он же удалит живого игрока
                                              //  Animator.playState(character, 4);//Проиграть анимацию смерти у всех игроков
                                              //   Map.deadPlayer(character, corpse);//Разослать команду всем в радиусу о смерти этого игрока
            
        }

    }
}
