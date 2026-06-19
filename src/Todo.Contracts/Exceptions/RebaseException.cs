namespace Todo.Contracts.Exceptions;

public class RebaseException(string message) : TodoExceptionBase(message)
{
    
    
    public override string Advice()
    {
        throw new NotImplementedException();
    }
}