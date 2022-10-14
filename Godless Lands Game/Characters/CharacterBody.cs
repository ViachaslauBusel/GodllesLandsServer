﻿using Godless_Lands_Game.Characteristics;
using Godless_Lands_Game.Handler;
using RUCP;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

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
            Packet packet = new Packet(character.Socket, Channel.Reliable);//Послать команду себе
            packet.WriteType(Types.PlayerDead);
            packet.Send();
        }

        protected override void Synchronize()
        {
            BodySynchronizer.PlayerUpdate(this, character.Socket);
        }
    }
}
