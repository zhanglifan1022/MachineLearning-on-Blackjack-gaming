using System;

namespace BlackJack
{
    public class ComputerWon : ApplicationException
    {
        public ComputerWon(string message) : base(message)
        {
        }
    }
}
