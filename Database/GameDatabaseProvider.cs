using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class GameDatabaseProvider
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<bool> Call(string cmd) => DatabaseProvider.Call(cmd, GameDatabaseConfig.SERVER_ADDRESS);

        public static async Task<bool> Call(object value)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<T> Select<T>(string cmd) => DatabaseProvider.Select<T>(cmd, GameDatabaseConfig.SERVER_ADDRESS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<string> SelectJson(string cmd) => DatabaseProvider.SelectJson(cmd, GameDatabaseConfig.SERVER_ADDRESS);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<object> SelectObject(string cmd) => DatabaseProvider.SelectObject(cmd, GameDatabaseConfig.SERVER_ADDRESS);
    }
}
