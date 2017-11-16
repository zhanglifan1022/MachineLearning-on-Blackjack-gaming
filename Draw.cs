using System;

namespace BlackJack
{
    public class Draw : ApplicationException
    {
        public Draw(string message) : base(message)
        {
        }
    }
}
