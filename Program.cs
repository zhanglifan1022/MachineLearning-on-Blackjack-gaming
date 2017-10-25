using System;
using System.IO;

namespace BlackJack
{
    class Casino
    {
        int[] cardbase = new int[52];

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
        public int Display(int a)
        {
            string color = " ";
            switch (a % 4)
            {
                case 1:
                    color = "Diamond";
                    break;
                case 2:
                    color = "Club";
                    break;
                case 3:
                    color = "Heart";
                    break;
                case 0:
                    color = "Spade";
                    break;
            }

            if (a <= 4)
            { Console.WriteLine(color + "  A "); return 1; }
            else if (a > 4 && a <= 8)
            { Console.WriteLine(color + "  2 "); return 2; }
            else if (a > 8 && a <= 12)
            { Console.WriteLine(color + "  3 "); return 3; }
            else if (a > 12 && a <= 16)
            { Console.WriteLine(color + "  4 "); return 4; }
            else if (a > 16 && a <= 20)
            { Console.WriteLine(color + "  5 "); return 5; }
            else if (a > 20 && a <= 24)
            { Console.WriteLine(color + "  6 "); return 6; }
            else if (a > 24 && a <= 28)
            { Console.WriteLine(color + "  7 "); return 7; }
            else if (a > 28 && a <= 32)
            { Console.WriteLine(color + "  8 "); return 8; }
            else if (a > 32 && a <= 36)
            { Console.WriteLine(color + "  9 "); return 9; }
            else if (a > 36 && a <= 40)
            { Console.WriteLine(color + "  10 "); return 10; }
            else if (a > 40 && a <= 44)
            { Console.WriteLine(color + "  J "); return 10; }
            else if (a > 44 && a <= 48)
            { Console.WriteLine(color + "  Q "); return 10; }
            else
            { Console.WriteLine(color + "  K "); return 10; }
        }
        public int GetSum(int[] arrint)
        {
            int sum1 = 0;
            int sum2 = 0;
            int i;
            int flag = 0;
            for (i = 0; i < 12; i++)
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
        public void Deliver(int[] arrint, int i)
        {
            arrint[i] = cardbase[i];
            arrint[i] = Display(arrint[i]);
        }
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
    }
    class Player
    {
        int money = 1000;
        int playercardnum = 0;
        int playerhandlength = playerhand.Length;
        double trend = 0;
        char knock;
        static int[] playerhand = new int[12];

        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
            }
        }                           
        public double Trend
        {
            get
            {
                return trend;
            }
            set
            {
                trend = value;
            }
        }
        public int PlayerCardNum
        {
            get
            {
                return playercardnum;
            }
            set
            {
                playercardnum = value;
            }
        }                   
        public int GetPlayerHandLength()
        {
            return playerhandlength;
        }           
        public int[] PlayerHand
        {
            get
            {
                return playerhand;
            }
            set
            {
                playerhand = value;
            }
        }
        public void Clear()
        {
            Array.Clear(playerhand, 0, playerhand.Length);
        }
        public char Knock
        {
            get
            {
                return knock;
            }
            set
            {
                knock = value;
            }
        }
        public void Ask1()
        {
            try
            {
                Console.WriteLine("Hit(h)/Stand(s)/Double(d)/Surrender(r)?");
                knock = Convert.ToChar(Console.ReadLine());
                if ((knock != 'h') && (knock != 's') && (knock != 'd') && (knock != 'r'))
                {
                    throw (new NotAllowed("NotAllowed"));
                }
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Warning:Please Re-Enter.");
                Ask1();
            }
            catch (NotAllowed)
            {
                Console.WriteLine("Warning:Please Re-Enter.");
                Ask1();
            }
            finally
            {
            }
        }
        public int GetResult(int sumplayer, int sumdealer, char knock, int money)
            {
            if (sumplayer < sumdealer)
            {
                if (knock == 'd') money -= 200;
                else money -= 100;
                Console.WriteLine("You Lose.Score=" + money);
                return money;
            }
            else if (sumplayer == sumdealer)
            {
                money += 0;
                Console.WriteLine("Draw！Score=" + money);
                return money;
            }
            else
                {
                    if (knock == 'd') money += 200;
                    else money += 100;
                    Console.WriteLine("You Win!Score=" + money);
                    return money;
                }
            }
    }
    class Dealer
    {
        int dealerhandlength = dealerhand.Length;
        static int[] dealerhand = new int[12];

        public int GetDealerHandLength()
        {
            return dealerhandlength;
        }           
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
        public void Clear()
        {
            Array.Clear(dealerhand, 0, dealerhand.Length);
        }
    }
    //特殊情况异常处理
    public class NotAllowed : ApplicationException
    {
        public NotAllowed(string message) : base(message)
        {
        }
    }
    public class YouGotABlackJack: ApplicationException
    {
        public YouGotABlackJack(string message) : base(message)
        {
        }
    }
    public class YouBursted : ApplicationException
    {
        public YouBursted(string message) : base(message)
        {
        }
    }
    public class DealerBusted : ApplicationException
    {
        public DealerBusted(string message) : base(message)
        {
        }
    }
    public class YouSurrender : ApplicationException
    {
        public YouSurrender(string message) : base(message)
        {
        }
    }
    //主程序入口
    static class Program
    {
        static void Main(string[] args)
        {
            string MyPath = @"C: \Users\lifan\blackjack\Data.txt";
            Casino c = new Casino();
            Player p = new Player();
            Dealer d = new Dealer();
            FileStream fs = new FileStream(MyPath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            do
            {
                int i = 0;
                p.PlayerCardNum = 0;
                try
                {   //准备阶段
                    p.Clear();
                    d.Clear();
                    c.Shuffle();
                    Console.WriteLine("**********************************************");
                    Console.WriteLine("Welcome to BlackJack，Scoring 1500 to Claim a Prize");
                    Console.WriteLine("PlayerHand,Score=" + p.Money);
                    Console.WriteLine("**********************************************");
                    //玩家回合
                    c.Deliver(p.PlayerHand , i);
                    sw.Write(p.PlayerHand[i] + " ");
                    i++;
                    c.Deliver(p.PlayerHand, i);
                    sw.Write(p.PlayerHand[i] + " ");
                    i++;
                    p.PlayerCardNum++;
                    p.PlayerCardNum++;

                    if (c.GetSum(p.PlayerHand) == 21)
                    {
                        throw (new YouGotABlackJack("You Got A Black Jack"));
                    }
                    else
                    {
                        p.Ask1();
                        sw.Write(p.Knock + " ");
                        switch (p.Knock)
                        {
                            case 'h':
                                while (p.Knock == 'h')
                                {
                                    c.Deliver(p.PlayerHand, i);
                                    sw.Write(p.PlayerHand[i] + " ");
                                    i++;
                                    p.PlayerCardNum++;
                                    if (c.GetSum(p.PlayerHand) > 21)
                                    {
                                        throw (new YouBursted("You Bursted"));
                                    }
                                    do
                                    {
                                        p.Ask1();
                                        sw.Write(p.Knock + " ");
                                    } while (p.Knock == 'd');
                                }
                                break;

                            //case 'p':
                            case 'd':
                                c.Deliver(p.PlayerHand, i);
                                sw.Write(p.PlayerHand[i] + " ");
                                i++;
                                p.PlayerCardNum++;
                                if (c.GetSum(p.PlayerHand) > 21)
                                {
                                    throw (new YouBursted("You Bursted"));
                                }
                                break;
                            case 'r':
                                throw (new YouSurrender("You Surrender"));
                            default:
                                break;
                        }
                    }
                    //庄家回合
                    Console.WriteLine(" ");
                    Console.WriteLine("DealerHand");
                    Console.WriteLine("----------");
                    c.Deliver(d.DealerHand, i);
                    i++;
                    c.Deliver(d.DealerHand, i);
                    i++;
                    while (c.GetSum(d.DealerHand) < 17)
                        {
                            c.Deliver(d.DealerHand, i);
                            i++;
                            if (c.GetSum(d.DealerHand) > 21)
                            {
                                throw (new DealerBusted("DealerBusted"));
                            }
                        }
                    //结算
                    p.Money = p.GetResult(c.GetSum(p.PlayerHand), c.GetSum(d.DealerHand), p.Knock, p.Money);
                    sw.Write(p.Trend + " ");

                }
                catch (YouGotABlackJack)
                {
                    p.Money += 150;
                    p.Trend = 0.15;
                    Console.WriteLine("Good Fortune Sir!!Score=" + p.Money);
                    sw.Write(p.Trend + " ");
                }
                catch (YouBursted)
                {
                    if (p.Knock == 'd')
                    {
                        p.Money -= 200;
                        p.Trend = -0.2;
                    }
                    else
                    {
                        p.Money -= 100;
                        p.Trend = -0.1;
                    }
                    Console.WriteLine("You Burst!Score=" + p.Money);
                    sw.Write(p.Trend + " ");
                }
                catch (DealerBusted)
                {
                    if (p.Knock == 'd')
                    {
                        p.Money += 200;
                        p.Trend = 0.2;
                    }
                    else
                    {
                        p.Money += 100;
                        p.Trend = 0.1;
                    }
                    Console.WriteLine("Dealer Burst!You Win!Score=" + p.Money);
                    sw.Write(p.Trend + " ");
                }
                catch (YouSurrender)
                {
                    p.Money -= 50;
                    p.Trend = -0.05;
                    Console.WriteLine("You Surrender. Score=" + p.Money);
                    sw.Write(p.Trend + " ");
                }
                finally
                {
                    Console.WriteLine("**********************************************");
                    Console.ReadKey();
                    Console.Clear();
                }
                sw.Write("\r\n");
                sw.Flush();
            } while (true);
            sw.Close();
            fs.Close();
        }
    }
}
//note10/25
//****finished data output to txt. file.
//adding 4 trendency display after every result.
//txt. file data need to sort out.
//same patterns will sum up accordingly.