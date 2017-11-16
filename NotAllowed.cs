using System;

namespace BlackJack
{
    public class NotAllowed : ApplicationException
    {
        public NotAllowed(string message) : base(message)
        {
        }
    }
}
