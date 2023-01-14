using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class LoginDatabaseProvider
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<bool> Call(string cmd) => DatabaseProvider.Call(cmd, LoginDatabaseConfig.SERVER_ADDRESS);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<string> SelectJson(string cmd) => DatabaseProvider.SelectJson(cmd, LoginDatabaseConfig.SERVER_ADDRESS);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<object> SelectObject(string cmd) => DatabaseProvider.SelectObject(cmd, LoginDatabaseConfig.SERVER_ADDRESS);
    }
}
