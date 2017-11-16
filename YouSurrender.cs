using System;

namespace BlackJack
{
    public class YouSurrender : ApplicationException
    {
        public YouSurrender(string message) : base(message)
        {
        }
    }
}
