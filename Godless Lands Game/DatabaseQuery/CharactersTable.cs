using Godless_Lands_Game.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.DatabaseQuery
{
    class CharactersTable
    {
        internal static List<(int chatacterID, string characterName)> GetCharacters(int loginID)
        {
            List<(int chatacterID, string characterName)> characters = new List<(int chatacterID, string characterName)>();

            GDB.ExecuteQuery("SELECT character_id, character_name FROM characters_table WHERE login_id=" + loginID + ";", (dataReader) =>
            {
                while (dataReader.Read())
                {
                    characters.Add((dataReader.GetInt32(0), dataReader.GetString(1)));
                }
            });

            return characters;
        }

        internal static string GetCharacterName(int characterID)
        {
            string name = "";

            GDB.ExecuteQuery("SELECT character_name FROM characters_table WHERE character_id=" + characterID + ";", (dataReader) =>
            {
                if (dataReader.Read())
                {
                    name = dataReader.GetString(0);
                }
            });

            return name;
        }

        internal static int GetCharacterID(string name)
        {
            int characterID = 0;

            GDB.ExecuteQuery("SELECT character_id FROM characters_table WHERE character_name='" + name + "';", (dataReader) =>
            {
                if (dataReader.Read())
                {
                    characterID = dataReader.GetInt32(0);
                }
            });

            return characterID;
        }

        internal static bool IsBelong(int loginID, int characterID)
        {
            bool answer = false;
            GDB.ExecuteQuery("SELECT login_id FROM characters_table WHERE character_id=" + characterID + ";", (dataReader) =>
            {
                if(dataReader.Read())
                {
                    answer = loginID == dataReader.GetInt32(0);
                }
            });
            return answer;
        }

        internal static void CreateCharacter(string name, int loginID)
        {
            GDB.ExecuteUpdate("INSERT INTO characters_table (character_name,login_id) VALUES('" + name + "','" + loginID + "');");
        }

        internal static bool NameExist(string name)
        {
            bool answer = false;
            GDB.ExecuteQuery("SELECT character_name FROM characters_table WHERE character_name='" + name + "';", (dataReader) =>
            {
                answer = dataReader.Read();
            });
            return answer;
        }
    }
}
