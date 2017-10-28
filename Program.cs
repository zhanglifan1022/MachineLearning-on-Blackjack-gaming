using System;
using System.IO;
using System.Text;

namespace BlackJack
{
    class Casino
    {
        int[] cardbase = new int[52];
        int money = 1000;

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
        
        int htrend = 25000;
        int dtrend = 25000;
        int strend = 25000;
        int rtrend = 25000;
        string knock;
        static int[] playerhand = new int[12];
                     
        public int HTrend
        {
            get
            {
                return htrend;
            }
            set
            {
                htrend = value;
            }
        }
        public int DTrend
        {
            get
            {
                return dtrend;
            }
            set
            {
                dtrend = value;
            }
        }
        public int STrend
        {
            get
            {
                return strend;
            }
            set
            {
                strend = value;
            }
        }
        public int RTrend
        {
            get
            {
                return rtrend;
            }
            set
            {
                rtrend = value;
            }
        }            
        public int GetPlayerHandLength()
        {
            return playerhand.Length;
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
        public string Knock
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
                knock = Console.ReadLine();
                if ((knock != "h") && (knock != "s") && (knock != "d") && (knock != "r"))
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
        public int GetResult(int sumplayer, int sumdealer, string knock, int money)
            {
            if (sumplayer < sumdealer)
            {
                if (knock == "d") money -= 200;
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
                    if (knock == "d") money += 200;
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
    {   //主方法
        static void Main(string[] args)
        {
            string MyPath = @"C: \Users\lifan\blackjack\Data.txt";
            Casino c = new Casino();
            do
            {
                Player p = new Player();
                Dealer d = new Dealer();
                FileStream fs = new FileStream(MyPath, FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                
                int i = 0;
                int mark = 0;
                int IfExist = 0;
                string card1;
                string card2;
                try
                {   //准备阶段
                    p.Clear();
                    d.Clear();
                    c.Shuffle();
                    Console.WriteLine("**********************************************");
                    Console.WriteLine("Welcome to BlackJack，Scoring 1500 to Claim a Prize");
                    Console.WriteLine("PlayerHand,Score=" + c.Money);
                    Console.WriteLine("**********************************************");
                    //Player Stage
                    c.Deliver(p.PlayerHand , i);//1st Card
                    i++;
                    c.Deliver(p.PlayerHand, i);//2nd Card
                    i++;
                    while ((card1 = sr.ReadLine()) != null)//EXP database loading
                    {
                        card2 = sr.ReadLine();
                        //if exist then download exp
                        if ((Convert.ToInt32(card1) == p.PlayerHand[0]) && (Convert.ToInt32(card2) == p.PlayerHand[1]))
                        {
                            IfExist++;
                            p.HTrend = Convert.ToInt32(sr.ReadLine());
                            p.DTrend = Convert.ToInt32(sr.ReadLine());
                            p.STrend = Convert.ToInt32(sr.ReadLine());
                            p.RTrend = Convert.ToInt32(sr.ReadLine());
                            mark++;
                            break;
                        }
                        mark += 2;
                    }
                    sr.Close();
                    fs.Close();

                    if (c.GetSum(p.PlayerHand) == 21)
                    {
                        throw (new YouGotABlackJack("You Got A Black Jack"));
                    }
                    else
                    {
                        Console.WriteLine("Win rate:H({0}%),D({1}%),S({2}%),R({3}%）", p.HTrend, p.DTrend, p.STrend, p.RTrend);
                        p.Ask1();
                        switch (p.Knock)
                        {
                            case "h":
                                while (p.Knock == "h")
                                {
                                    c.Deliver(p.PlayerHand, i);
                                    i++;
                                    if (c.GetSum(p.PlayerHand) > 21)
                                    {
                                        throw (new YouBursted("You Bursted"));
                                    }
                                    do
                                    {
                                        p.Ask1();
                                    } while (p.Knock == "d" || p.Knock == "r");
                                }
                                break;

                            //case 'p':
                            case "d":
                                c.Deliver(p.PlayerHand, i);
                                i++;
                                if (c.GetSum(p.PlayerHand) > 21)
                                {
                                    throw (new YouBursted("You Bursted"));
                                }
                                break;
                            case "r":
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
                    c.Money = p.GetResult(c.GetSum(p.PlayerHand), c.GetSum(d.DealerHand), p.Knock, c.Money);
                    EditFile(mark, Convert.ToString(p.PlayerHand[0]), Convert.ToString(p.PlayerHand[1]), Convert.ToString(p.HTrend), Convert.ToString(p.DTrend), Convert.ToString(p.STrend), Convert.ToString(p.RTrend), MyPath, IfExist);
                }
                catch (YouGotABlackJack)
                {
                    c.Money += 150;
                    Console.WriteLine("Good Fortune Sir!!Score=" + c.Money);
                    EditFile(mark, Convert.ToString(p.PlayerHand[0]), Convert.ToString(p.PlayerHand[1]), Convert.ToString(p.HTrend), Convert.ToString(p.DTrend), Convert.ToString(p.STrend), Convert.ToString(p.RTrend), MyPath, IfExist);
                }
                catch (YouBursted)
                {
                    if (p.Knock == "d")
                    {
                        c.Money -= 200;
                        p.DTrend -= 20;
                    }
                    else
                    {
                        c.Money -= 100;
                        p.HTrend -= 10;
                    }
                    Console.WriteLine("You Burst!Score=" + c.Money);
                    EditFile(mark, Convert.ToString(p.PlayerHand[0]), Convert.ToString(p.PlayerHand[1]), Convert.ToString(p.HTrend), Convert.ToString(p.DTrend), Convert.ToString(p.STrend), Convert.ToString(p.RTrend), MyPath, IfExist);
                } 
                catch (DealerBusted)
                {
                    if (p.Knock == "d")
                    {
                        c.Money += 200;
                        p.DTrend += 20;
                    }
                    else
                    {
                        c.Money += 100;
                        p.STrend += 10;
                    }
                    Console.WriteLine("Dealer Burst!You Win!Score=" + c.Money);
                    EditFile(mark, Convert.ToString(p.PlayerHand[0]), Convert.ToString(p.PlayerHand[1]), Convert.ToString(p.HTrend), Convert.ToString(p.DTrend), Convert.ToString(p.STrend), Convert.ToString(p.RTrend), MyPath, IfExist);
                }
                catch (YouSurrender)
                {
                    c.Money -= 50;
                    p.RTrend -= 5;
                    Console.WriteLine("You Surrender. Score=" + c.Money);
                    EditFile(mark, Convert.ToString(p.PlayerHand[0]), Convert.ToString(p.PlayerHand[1]), Convert.ToString(p.HTrend), Convert.ToString(p.DTrend), Convert.ToString(p.STrend), Convert.ToString(p.RTrend), MyPath, IfExist);
                }
                finally
                {
                    Console.WriteLine("**********************************************");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (true);
        }
        //数据读取
        public static void TendencyRead(string value)
        {
           
        }
        //文件改写
        public static void EditFile(int mark, string newLineValue1, string newLineValue2, string newLineValue3, string newLineValue4, string newLineValue5, string newLineValue6, string MyPath, int IfExist)
        {
            if (IfExist == 1)
            {
                FileStream fs = new FileStream(MyPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string line = sr.ReadLine();
                StringBuilder sb = new StringBuilder();
                for (int i = 1; line != null; i++)
                {
                    sb.Append(line + "\r\n");
                    if (i != mark - 1)
                        line = sr.ReadLine();
                    else
                    {
                        line = sr.ReadLine();
                        sb.Append(line + "\r\n");
                        line = sr.ReadLine();
                        sb.Append(line + "\r\n");
                        sb.Append(newLineValue3 + "\r\n");
                        sr.ReadLine();
                        sb.Append(newLineValue4 + "\r\n");
                        sr.ReadLine();
                        sb.Append(newLineValue5 + "\r\n");
                        sr.ReadLine();
                        sb.Append(newLineValue6 + "\r\n");
                        sr.ReadLine();
                        line = sr.ReadLine();
                    }
                }
                sr.Close();
                fs.Close();
                FileStream fs1 = new FileStream(MyPath, FileMode.Open, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(fs1, Encoding.Default);
                sw1.Write(sb.ToString());
                sb.Clear();
                sw1.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs = new FileStream(MyPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string line = sr.ReadLine();
                StringBuilder sb = new StringBuilder();
                for (int i = 1; line != null; i++)
                {
                    sb.Append(line + "\r\n");
                    line = sr.ReadLine();
                }
                sb.Append(newLineValue1 + "\r\n");
                sb.Append(newLineValue2 + "\r\n");
                sb.Append(newLineValue3 + "\r\n");
                sb.Append(newLineValue4 + "\r\n");
                sb.Append(newLineValue5 + "\r\n");
                sb.Append(newLineValue6 + "\r\n");
                sr.Close();
                fs.Close();
                FileStream fs1 = new FileStream(MyPath, FileMode.Open, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(fs1, Encoding.Default);
                sw1.Write(sb.ToString());
                sb.Clear();
                sw1.Close();
                fs1.Close();
            }
        }
        //随机趋势选择
        public static string RandomTrend(int i1, int i2, int i3, int i4)
        {
            double p1, p2, p3, p4, p;
            Random ran = new Random();
            p1 = i1 / (i1 + i2 + i3 + i4);
            p2 = i2 / (i1 + i2 + i3 + i4);
            p3 = i3 / (i1 + i2 + i3 + i4);
            p4 = i4 / (i1 + i2 + i3 + i4);
            p = ran.NextDouble();
            if ((p >= 0) && (p <p1))
            {
                return "h";
            }
            else if ((p >= p1) && (p < p1 + p2))
            {
                return "d";
            }
            else if ((p >= p1 + p2) && (p < p1 + p2 + p3))
            {
                return "s";
            }
            else
            {
                return "r";
            }
        }
    }
}
