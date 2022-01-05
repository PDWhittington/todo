namespace Todo.Contracts.Data.HelpMessages;

public class CommandHelpMessage
{
    public string [] HelpWords { get; }

    public string [] CommandDescription { get; }

    public CommandHelpMessage(string[] helpWords, string [] commandDescription)
    {
        HelpWords = helpWords;
        CommandDescription = commandDescription;
    }
}
