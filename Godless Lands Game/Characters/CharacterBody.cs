using Godless_Lands_Game.Characteristics;
using Godless_Lands_Game.Handler;
using RUCP;
using System;

namespace Godless_Lands_Game.Characters
{
    public class CharacterBody: Body
    {
        private Character character;

        public CharacterBody(Character character)
        {
            this.character = character;
        }
        public new void Load()
        {
            base.Load();
        }

        protected override void Awake()
        {
            throw new NotImplementedException();
        }

        protected override void Died()
        {
            Packet packet = Packet.Create(Channel.Reliable);//Послать команду себе
            packet.OpCode = (Types.PlayerDead);
            character.Socket.Send(packet);
        }

        protected override void Synchronize()
        {
            BodySynchronizer.PlayerUpdate(this, character.Socket);
        }
    }
}
