using NetworkGameEngine.JobsSystem;

namespace Game.DB
{
    public interface IDatabaseReadable
    {
          Job ReadFromDatabase();
    }
}
