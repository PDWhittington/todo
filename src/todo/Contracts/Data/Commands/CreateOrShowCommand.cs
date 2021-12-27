using System;

namespace Todo.Contracts.Data.Commands
{
    public class CreateOrShowCommand
    {
        public DateTime Date { get; }

        private CreateOrShowCommand(DateTime date)
        {
            Date = date;
        }

        public static CreateOrShowCommand Of(DateTime date) => new(date);
    }
}