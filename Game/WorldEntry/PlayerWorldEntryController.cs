using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;

namespace Godless_Lands_Game.WorldEntry
{
    public class PlayerSlot
    {
        private readonly PlayerProfile _playerProfile;
        private GameObject _characterObject;

        public PlayerProfile PlayerProfile => _playerProfile;
        public GameObject CharacterObject => _characterObject;

        public PlayerSlot(PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;
        }

        internal void SetCharacterObject(GameObject characterObject)
        {
            _characterObject = characterObject;
        }
    }
    internal class PlayerWorldEntryController
    {
      
        private static PlayerWorldEntryController _instance;
        private Dictionary<int, PlayerSlot> _playerSlots = new Dictionary<int, PlayerSlot>();
        private Dictionary<int, int> _removingCharObjTasks = new Dictionary<int, int>();
        private object _lock = new object();
        private bool _entrance = false;


        public static PlayerWorldEntryController Instance => _instance;

        public static bool Activated => _instance != null && _instance._entrance;

        public PlayerWorldEntryController()
        {
            _instance = this;
        }

        /// <summary>
        /// Вызывается при разрыве соединения с клиентом игрока
        /// </summary>
        /// <param name="playerProfile"></param>
        internal void Disconnect(int loginId)
        {
            lock (_lock)
            {
                if (_playerSlots.TryGetValue(loginId, out PlayerSlot playerSlot) == false)
                {
                    Debug.Log.Error($"[Disconnet] unable to find player with id {loginId}");
                    return;
                }

                if(playerSlot.CharacterObject != null)
                {
                    if (_removingCharObjTasks.ContainsKey(playerSlot.CharacterObject.ID) == false)
                    {
                        _removingCharObjTasks.Add(playerSlot.CharacterObject.ID, loginId);
                        playerSlot.CharacterObject.SendCommand(new DisconnectCommand());
                    }
                }
                else
                {
                    _playerSlots.Remove(loginId);
                }
            }
        }

        public void DisconnectedCharacter(int chracterObjId)
        {
            lock (_lock)
            {
                if (_removingCharObjTasks.TryGetValue(chracterObjId, out int loginId))
                {
                    Debug.Log.Info($"[DisconnectedCharacter] player with id {loginId} disconnected");
                    _playerSlots.Remove(loginId);
                    _removingCharObjTasks.Remove(chracterObjId);
                }
                else Debug.Log.Error($"[DisconnectedCharacter] unable to find player with id {chracterObjId}");
            }
        }

        // Вызывается после успешной авторизации игрока
        internal bool Connect(int lognId, PlayerProfile playerProfile)
        {
            lock (_lock)
            {
                if(_entrance == false)
                {
                    Debug.Log.Error($"[Connect] entrance is not activated");
                    return false;
                }
                if (_playerSlots.ContainsKey(lognId))
                {
                    Debug.Log.Error($"[Connect] player with id {lognId} already in game");
                    return false;
                }
                Debug.Log.Info($"[Connect] player with id {lognId} connected");
                _playerSlots.Add(lognId, new PlayerSlot(playerProfile));
                return true;
            }
        }

        // Инитициализация процесса отключения игрока от сервера
        internal void InitializeDisconnect(int loginID)
        {
            lock (_lock)
            {
                if (_playerSlots.TryGetValue(loginID, out PlayerSlot playerSlot) == false)
                {
                    Debug.Log.Error($"[TryDisconnect] unable to find player with id {loginID}");
                    return;
                }

                playerSlot.PlayerProfile.Client.Close();
            }
        }

        //Присоеинение обьекта персонажа к слоту игрока
        internal bool ConnectCharacterObject(int loginId, GameObject characterObject)
        {
            lock (_lock)
            {
                if (_playerSlots.TryGetValue(loginId, out PlayerSlot playerSlot))
                {
                    if(playerSlot.CharacterObject != null)
                    {
                        Debug.Log.Fatal($"player with id {loginId} already has a character object");
                        return false;
                    }
                    playerSlot.SetCharacterObject(characterObject);
                    return true;

                }
                else
                {
                    Debug.Log.Fatal($"unable to find player with id {loginId}");
                    return false;
                }
            }
        }

        internal void ActivateEntarnce()
        {
            _entrance = true;
        }
    }
}
