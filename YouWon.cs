using System;

namespace BlackJack
{
    public class YouWon : ApplicationException
    {
        public YouWon(string message) : base(message)
        {
        }
    }
}
