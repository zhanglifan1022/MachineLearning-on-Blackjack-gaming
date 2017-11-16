using System;
using static System.Math;
namespace BlackJack
{
    class Dealer
    {
        private int j = 0;
        static private int[] dealerhand = new int[20];
        private int sum = 0;
        private int flag = 0;

        public int J
        { get; set; }
        public int[] DealerHand
        {
            get
            {
                return dealerhand;
            }
            set
            {
                dealerhand = value;
            }
        }
        public int Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
            }
        }
        public int Sum
        {
            get
            {
                return sum;
            }
            set
            {
                sum = value;
            }
        }
        public int GetSum(int[] arrint)
        {
            int sum1 = 0;
            int sum2 = 0;
            int i;

            int flag = 0;
            for (i = 0; arrint[i] != 0; i++)
            {
                if (arrint[i] == 1 && flag == 0)
                {
                    sum1 = sum1 + arrint[i];
                    sum2 = sum2 + arrint[i] + 10;
                    flag++;
                }
                else
                {
                    sum1 = sum1 + arrint[i];
                    sum2 = sum2 + arrint[i];
                }
            }
            if (sum2 > sum1 && sum2 <= 21)
            {
                return sum2;
            }
            else return sum1;
        }
        public void Clear()
        {
            Array.Clear(dealerhand, 0, dealerhand.Length);
        }
        public int DealerRound(int I, Computer com, Human p)
        {
            try
            {
                Clear();//清庄家手牌
                Casino.Deliver(dealerhand, j, I);
                I++;
                j++;
                Casino.Deliver(dealerhand, j, I);
                I++;
                j++;
                while ((sum = GetSum(dealerhand)) < 17)
                {
                    Casino.Deliver(dealerhand, j, I);
                    I++;
                    j++;
                    if ((sum = GetSum(dealerhand)) > 21)
                    {
                        throw (new DealerBusted("DealerBusted"));
                    }
                }
            }
            catch (DealerBusted)
            {
                flag++;
                switch (com.Flag)
                {
                    case 0:
                        if (com.Knock == "d")
                        {
                            com.Win += 2;
                            com.HTrend = Abs(com.HTrend - 8);
                            com.DTrend = Abs(com.DTrend + 24);
                            com.STrend = Abs(com.STrend - 8);
                            com.RTrend = Abs(com.RTrend - 8);
                            if (com.HTrend < 22) com.HTrend += 21;
                            if (com.DTrend < 22) com.DTrend += 21;
                            if (com.STrend < 22) com.STrend += 21;
                            if (com.RTrend < 22) com.RTrend += 21;
                        }
                        else if (com.IfViceExp == 0)
                        {
                            com.Win += 1;
                            com.HTrend = Abs(com.HTrend - 4);
                            com.DTrend = Abs(com.DTrend - 4);
                            com.STrend = Abs(com.STrend + 12);
                            com.RTrend = Abs(com.RTrend - 4);
                            if (com.HTrend < 22) com.HTrend += 21;
                            if (com.DTrend < 22) com.DTrend += 21;
                            if (com.STrend < 22) com.STrend += 21;
                            if (com.RTrend < 22) com.RTrend += 21;
                        }
                        else
                        {
                            com.Win += 1;
                            com.HTrend = Abs(com.HTrend + 12);
                            com.DTrend = Abs(com.DTrend - 4);
                            com.STrend = Abs(com.STrend - 4);
                            com.RTrend = Abs(com.RTrend - 4);
                            com.HTrend1 = Abs(com.HTrend1 - 12);
                            com.STrend1 = Abs(com.STrend1 + 12);
                            if (com.HTrend < 22) com.HTrend += 21;
                            if (com.DTrend < 22) com.DTrend += 21;
                            if (com.STrend < 22) com.STrend += 21;
                            if (com.RTrend < 22) com.RTrend += 21;
                            if (com.HTrend1 < 22) com.HTrend1 += 21;
                            if (com.STrend1 < 22) com.STrend1 += 21;
                        }
                        Console.WriteLine("Dealer Burst!Computer Win!");
                        break;
                    case 1:
                        break;
                }
                switch (p.Flag)
                {
                    case 0:
                        if (p.Knock == "d")
                        {
                            p.Win += 2;
                        }
                        else
                        {
                            p.Win += 1;
                        }
                        Console.WriteLine("Dealer Burst!You Win!");
                        break;
                    case 1:
                        break;
                }
            }
            finally
            {
            }
            return flag;
        }
    }
}
