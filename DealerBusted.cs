using System;

namespace BlackJack
{
    public class DealerBusted : ApplicationException
    {
        public DealerBusted(string message) : base(message)
        {
        }
    }
}
