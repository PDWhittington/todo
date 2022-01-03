using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Execution;

namespace Todo
{
    public class TodoService : ITodoService
    {
        private readonly ICommandProvider _commandProvider;
        private readonly ISyncExecutor _syncExecutor;
        private readonly ICreateOrShowExecutor _createOrShowExecutor;

        public TodoService(ICommandProvider commandProvider, ISyncExecutor syncExecutor, ICreateOrShowExecutor createOrShowExecutor)
        {
            _commandProvider = commandProvider;
            _syncExecutor = syncExecutor;
            _createOrShowExecutor = createOrShowExecutor;
        }
        
        public void PerformTask()
        {
            var command = _commandProvider.GetCommand();

            switch (command)
            {
                case CreateOrShowCommand createOrShowCommand:
                    _createOrShowExecutor.Execute(createOrShowCommand);
                    break;
                
                case SyncCommand syncCommand:
                    _syncExecutor.Execute(syncCommand);
                    break;
                
                default:
                    throw new Exception();
            }
        }
    }
}