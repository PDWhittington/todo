using System;

namespace Todo.Git.Results;

public class GitVoidResult(bool success, Exception? exception) 
    : GitCommandResult(success, exception);
