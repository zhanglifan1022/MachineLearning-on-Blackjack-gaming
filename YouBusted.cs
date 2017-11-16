using System;

namespace BlackJack
{
    public class YouBursted : ApplicationException
    {
        public YouBursted(string message) : base(message)
        {
        }
    }
}
