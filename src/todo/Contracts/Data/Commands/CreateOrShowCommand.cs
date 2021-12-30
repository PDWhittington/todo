using System;

namespace Todo.Contracts.Data.Commands
{
    public class CreateOrShowCommand : CommandBase
    {
        public DateOnly Date { get; }

        private CreateOrShowCommand(DateOnly date)
        {
            Date = date;
        }

        public static CreateOrShowCommand Of(DateOnly date) => new(date);
    }
}