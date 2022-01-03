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
        
        private readonly IArchiveExecutor _archiveExecutor;
        private readonly ICreateOrShowExecutor _createOrShowExecutor;
        private readonly ICommitExecutor _commitExecutor;
        private readonly IPushExecutor _pushExecutor;
        private readonly ISyncExecutor _syncExecutor;
        private readonly IShowHtmlExecutor _showHtmlExecutor;

        public TodoService(ICommandProvider commandProvider, 
            IArchiveExecutor archiveExecutor, ICreateOrShowExecutor createOrShowExecutor,
            ICommitExecutor commitExecutor, IPushExecutor pushExecutor, ISyncExecutor syncExecutor,
            IShowHtmlExecutor showHtmlExecutor)
        {
            _commandProvider = commandProvider;
            _archiveExecutor = archiveExecutor;
            _createOrShowExecutor = createOrShowExecutor;
            _commitExecutor = commitExecutor;
            _pushExecutor = pushExecutor;
            _syncExecutor = syncExecutor;
            _showHtmlExecutor = showHtmlExecutor;
        }
        
        public void PerformTask()
        {
            var command = _commandProvider.GetCommand();

            switch (command)
            {
                case CreateOrShowCommand createOrShowCommand:
                    _createOrShowExecutor.Execute(createOrShowCommand);
                    break;
                
                case ArchiveCommand archiveCommand:
                    _archiveExecutor.Execute(archiveCommand);
                    break;
                
                case CommitCommand commitCommand:
                    _commitExecutor.Execute(commitCommand);
                    break;
                
                case PushCommand pushCommand:
                    _pushExecutor.Execute(pushCommand);
                    break;

                case ShowHtmlCommand showHtmlCommand:
                    _showHtmlExecutor.Execute(showHtmlCommand);
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