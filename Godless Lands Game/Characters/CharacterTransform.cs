using Godless_Lands_Game.DatabaseQuery;
using Godless_Lands_Game.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characters
{
   public class CharacterTransform: Transform
    {
        private Character character;
        public long timeStamp = 0;
        public long delay = 0;

        public CharacterTransform(Character character)
        {
            this.character = character;
            location = Location.CreateEmpty();
        }

        public void Load()
        {
            position = CharacterPosition.GetPostion(character.ID);//Считывание сохраненого положения игрока из БД
            Console.WriteLine("Load in " + position);
        }

        public void Save()
        {
            CharacterPosition.SavePosition(character.ID, position);
        }
    }
}
