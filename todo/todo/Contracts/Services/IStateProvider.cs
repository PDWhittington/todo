using Todo.Contracts.Data;

namespace Todo.Contracts.Services
{
    public interface IStateProvider
    {
        public StateInfo GetState();
    }
}