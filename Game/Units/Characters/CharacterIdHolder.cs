namespace NetworkGameEngine.Units.Characters
{
    public class CharacterIdHolder : Component
    {
        public int CharacterID { get; init; }

        public CharacterIdHolder(int characterID)
        {
            CharacterID = characterID;
        }


        public override async Task Init()
        {
        }

        public override void Awake()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        public override void OnDestroy()
        {
        }

        public override void CallReact(ICommand cmd)
        {
        }
    }
}
