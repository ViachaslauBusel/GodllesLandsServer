namespace NetworkGameEngine
{
    public interface IReactCommand<T> where T: ICommand
    {
        void React(T command);
    }
}