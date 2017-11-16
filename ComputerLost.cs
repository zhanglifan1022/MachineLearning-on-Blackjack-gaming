using System;

namespace BlackJack
{
    public class ComputerLost : ApplicationException
    {
        public ComputerLost(string message) : base(message)
        {
        }
    }
}
