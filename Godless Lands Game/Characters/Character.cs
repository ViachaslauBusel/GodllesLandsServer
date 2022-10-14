using Godless_Lands_Game.Characteristics;
using Godless_Lands_Game.DatabaseQuery;
using Godless_Lands_Game.Equipment;
using RUCP.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characters
{
   public class Character
    {
        public ClientSocket Socket { get; private set; }
        public int ID { get; private set; }
        public CharacterStats Stats { get; private set; }
        public Armor Armor { get; private set; }
        public CharacterBody Body { get; private set; }
        public CharacterTransform Transform { get; private set; }
        public bool IsLoaded => load;

        public bool CombatState => false;

        private volatile bool load = false;

        public Character(ClientSocket socket, int characterID)
        {
            Socket = socket;
            ID = characterID;
        }

        private readonly object loadLock = new object();
        internal void Load()
        {
            lock (loadLock)
            {

                if (load) return;

                Stats = new CharacterStats(this);
                Armor = new Armor(this);
                Body = new CharacterBody(this);
                Transform = new CharacterTransform(this);

                Stats.Load();
                Armor.Load();
                Body.Load();
                Transform.Load();

                Stats.Calculete();

                StatsSynchronizer.Load(Socket, Stats);

                load = true;
            }
        }


        public override int GetHashCode()
        {
            return ID;
        }
    }
}
