using System;

namespace BlackJack
{
    public class YouLost : ApplicationException
    {
        public YouLost(string message) : base(message)
        {
        }
    }
}
