using Database;
using NetworkGameEngine.Units.Characters;
using System.Numerics;

namespace NetworkGameEngine.Physics
{
    public class TransformComponent : Component
    {
        private Vector3 m_position;
        public override async Task Init()
        {
            int characterID = GetComponent<CharacterIdHolder>().CharacterID;
            m_position = await GameDatabaseProvider.Select<Vector3>($"SELECT get_chatacer_position('{characterID}')");
        }



        public override void Awake()
        {
        }

        public override void OnDestroy()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        public override void CallReact(ICommand cmd)
        {
        }
    }
}
