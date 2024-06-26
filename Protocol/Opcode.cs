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
        public const short MSG_UNIT_TARGET_FULL_SC = 19;
        public const short MSG_UPDATE_STATS = 20;
        public const short MSG_SCENE_STATUS = 21;
        public const short MSG_PREPARE_SCENE = 22;
        public const short MSG_LOAD_STATES = 23;
        public const short MSG_SKILLS_UPDATE = 24;
        public const short MSG_HOTBAR_SET_CELL_VALUE = 25;
        public const short MSG_HOTBAR_UPDATE = 26;
        public const short MSG_SKILL_USE = 27;
        public const short MSG_MESSAGE_CS = 28;
        public const short MSG_MESSAGE_SC = 29;
        public const short MSG_UNIT_TARGET_HP_SC = 30;
        public const short MSG_RAYCAST_TEST = 31;
        public const short MSG_PLAYER_DEATH_STATE = 32;
        public const short MSG_OBJECT_INTERACTION_REQUEST = 33;
        public const short MSG_DROP_LIST_SYNC = 34;
        public const short MSG_TAKE_DROP = 35;
        public const short MSG_OBJECT_INTERACTION_RESPONSE = 36;
        public const short MSG_INVENTORY_SYNC = 37;
        public const short MSG_USE_ITEM = 38;
        public const short MSG_SWAMP_ITEMS = 39;
        public const short MSG_TRANSFER_ITEM_TO_ANOTHER_BAG = 40;
        public const short MSG_EQUIPMENT_SYNC = 42;
        public const short MSG_UNEQUIP_ITEM_TO_INVENTORY = 43;
        public const short MSG_SWITCH_COMBAT_MODE = 44;
        public const short MSG_DESTROY_ITEM_INVENTORY = 45;
        public const short MSG_DESTROY_ITEM_EQUIPMENT = 46;
        public const short MSG_WORKBENCH_TOGGLE_WINDOW = 47;
        public const short MSG_WORKBENCH_CLOSE_WINDOW = 48;
        public const short MSG_WORKBENCH_CREATE_ITEM = 49;
        public const short MSG_PROFESSIONS_SYNC = 50;
        public const short MSG_QUESTS_SYNC = 51;
        public const short MSG_QUEST_STAGE_UP_REQUEST = 52;
        public const short MSG_QUEST_STAGE_UP_RESPONSE = 53;
        public const short MSG_HOTBAT_SWAMP_CELLS = 54;
        public const short MSG_DRAW_POINTS = 55;
        public const short MSG_DROP_ITEM = 56;
        public const short MSG_TRADE_REQUEST = 57;
        public const short MSG_TRADE_REQUEST_RESPONSE = 58;
        public const short MSG_OPEN_TRADE_WINDOW = 59;
        public const short MSG_MOVE_ITEM_TO_TRADE = 60;
        public const short MSG_SYNC_TRADE_WINDOW = 61;
        public const short MSG_TRADE_CONTROL_COMMAND = 62;
        public const short MSG_CONFIRM_TRADE = 63;
    }
}
