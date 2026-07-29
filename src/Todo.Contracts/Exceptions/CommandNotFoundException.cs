namespace Todo.Contracts.Exceptions;

public class CommandNotFoundException(string commandText) : Exception($"Command not recognised: {commandText}")
{ }
