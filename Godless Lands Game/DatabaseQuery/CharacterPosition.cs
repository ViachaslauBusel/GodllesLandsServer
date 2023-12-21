using Godless_Lands_Game.Database;
using System.Numerics;

namespace Godless_Lands_Game.DatabaseQuery
{
    class CharacterPosition
    {
        internal static Vector3 GetPostion(int characterID)
        {
            Vector3 position = new Vector3();

            GDB.ExecuteQuery("SELECT pos_x, pos_y, pos_z FROM ship_position WHERE character_id=" + characterID + ";", (dataReader) =>
            {
                if (dataReader.Read())
                {
                    position.X = dataReader.GetFloat(0);
                    position.Y = dataReader.GetFloat(1);
                    position.Z = dataReader.GetFloat(2);
                }
            });

            return position;
        }

        internal static void SavePosition(int characterID, Vector3 position)
        {
            GDB.ExecuteUpdate("INSERT INTO ship_position (character_id,pos_x,pos_y,pos_z) VALUES('" + characterID + "','" + position.X + "','" + position.Y + "','" + position.Z + "') " +
                    "ON DUPLICATE KEY UPDATE `pos_x` = " + position.X + ", `pos_y` = " + position.Y + ", `pos_z` = " + position.Z + ";");
        }
    }
}
