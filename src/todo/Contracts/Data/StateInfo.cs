using System;

namespace Todo.Contracts.Data
{
    public class StateInfo
    {
        public DateTime Date { get; }

        private StateInfo(DateTime date)
        {
            Date = date;
        }

        public static StateInfo Of(DateTime date) => new(date);
    }
}