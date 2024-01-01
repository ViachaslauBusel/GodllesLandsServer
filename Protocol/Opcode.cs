﻿namespace Protocol
{
    public static class Opcode
    {
        public const short MSG_AUTHORIZATION_Request = 1;
        public const short MSG_AUTHORIZATION_Response = 2;
        public const short MSG_REGISTRATION_Request = 3;
        public const short MSG_REGISTRATION_Response = 4;
        public const short MSG_SERVER_LIST = 5;
        public const short MSG_HP_VIEW_UPDATE = 6;  
        public const short MSG_CHAT_MESSAGE = 7;
        public const short MSG_GAMEOBJECT_DESTROY = 8;
        public const short MSG_LOBBY_ENTRANCE = 9;
        public const short MSG_SELECT_CHARACTER = 10;
        public const short MSG_GAME_AUTHORIZATION = 11;
        public const short MSG_CHARACTERS_LIST = 12;
        public const short MSG_CREATE_CHARACTER = 13;
        public const short MSG_WORLD_ENTRANCE = 14;
        public const short MSG_GAMEOBJECT_UPDATE = 15;
        public const short MSG_UNIT_DELET = 16;
        public const short MSG_PLAYER_INPUT_CS = 17;
        public const short MSG_UNIT_TARGET_REQUEST_CS = 18;
        public const short MSG_UNIT_TARGET_STATE_SC = 19;
        public const short MSG_UPDATE_STATS = 20;
        public const short MSG_SCENE_STATUS = 21;
        public const short MSG_PREPARE_SCENE = 22;
    }
}
