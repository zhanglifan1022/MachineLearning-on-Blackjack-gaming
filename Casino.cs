using System;

namespace BlackJack
{
    class Casino
    {
        private static int[] cardbase = new int[52];
        private int i = 0;//第几张

        public int[] CardBase
        {
            get
            {
                return cardbase;
            }
            set
            {
                cardbase = value;
            }
        }
        public int I
        {
            get
            {
                return i;
            }
            set
            {
                i = value;
            }
        }
        public static int Display(int a)
        {
            char color = ' ';
            switch (a % 4)
            {
                case 1:
                    color = (char)4;
                    break;
                case 2:
                    color = (char)5;
                    break;
                case 3:
                    color = (char)3;
                    break;
                case 0:
                    color = (char)6;
                    break;
            }

            if (a <= 4)
            { Console.WriteLine("  " + color + " A "); return 1; }
            else if (a > 4 && a <= 8)
            { Console.WriteLine("  " + color + " 2 "); return 2; }
            else if (a > 8 && a <= 12)
            { Console.WriteLine("  " + color + " 3 "); return 3; }
            else if (a > 12 && a <= 16)
            { Console.WriteLine("  " + color + " 4 "); return 4; }
            else if (a > 16 && a <= 20)
            { Console.WriteLine("  " + color + " 5 "); return 5; }
            else if (a > 20 && a <= 24)
            { Console.WriteLine("  " + color + " 6 "); return 6; }
            else if (a > 24 && a <= 28)
            { Console.WriteLine("  " + color + " 7 "); return 7; }
            else if (a > 28 && a <= 32)
            { Console.WriteLine("  " + color + " 8 "); return 8; }
            else if (a > 32 && a <= 36)
            { Console.WriteLine("  " + color + " 9 "); return 9; }
            else if (a > 36 && a <= 40)
            { Console.WriteLine("  " + color + " 10 "); return 10; }
            else if (a > 40 && a <= 44)
            { Console.WriteLine("  " + color + " J "); return 10; }
            else if (a > 44 && a <= 48)
            { Console.WriteLine("  " + color + " Q "); return 10; }
            else
            { Console.WriteLine("  " + color + " K "); return 10; }
        }
        public static void Deliver(int[] arrint, int j, int i)
        {
            arrint[j] = Casino.cardbase[i];
            arrint[j] = Casino.Display(arrint[j]);
        }
        public void Shuffle()
        {
            Random r = new Random();
            for (int i = 0; i < 52; i++)
            {
                cardbase[i] = i + 1;
            }

            for (int i = 0; i < 52; i++)
            {
                int idx1 = i;
                int idx2 = r.Next(1, 52);
                int tmp = cardbase[idx1];
                cardbase[idx1] = cardbase[idx2];
                cardbase[idx2] = tmp;
            }
        }
    }
}
