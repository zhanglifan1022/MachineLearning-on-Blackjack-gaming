using System;

namespace BlackJack
{
    public class YouGotABlackJack : ApplicationException
    {
        public YouGotABlackJack(string message) : base(message)
        {
        }
    }
}
