using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services
{
    public interface ICommandProvider
    {
        public CreateOrShowCommand GetCommand();
    }
}