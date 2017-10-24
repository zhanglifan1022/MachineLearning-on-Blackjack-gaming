using System;
using System.Threading;
namespace BlackJack
{
    class Card  
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
        }                       //洗牌
        static int Display(int a)
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
        }                   //显示牌
        public int GetSum(int decide)
        {
            int sum1 = 0;
            int sum2 = 0;
            int i;
            int flag = 0;
            switch (decide)
            {
                case 0:
                    
                    for (i = 0; i < GetPlayerHandLength(); i++)
                    {
                        if (playerHand[i] == 1 && flag == 0)
                        {
                            sum1 = sum1 + playerHand[i];
                            sum2 = sum2 + playerHand[i] + 10;
                            flag++;
                        }
                        else
                        {
                            sum1 = sum1 + playerHand[i];
                            sum2 = sum2 + playerHand[i];
                        }
                    }
                    if (sum2 > sum1 && sum2 <= 21)
                    {
                        return sum2;
                    }
                    else return sum1;

                default:
                    for (i = 0; i < GetDealerHandLength(); i++)
                    {
                       if (dealerHand[i] == 1 && flag == 0)
                       {
                          sum1 = sum1 + dealerHand[i];
                          sum2 = sum2 + dealerHand[i] + 10;
                          flag++;
                       }
                       else
                       {
                          sum1 = sum1 + dealerHand[i];
                          sum2 = sum2 + dealerHand[i];
                        }
                    }
                    if (sum2 > sum1 && sum2 <= 21)
                    {
                        return sum2;
                    }
                    else return sum1;
            }
        }               //计算当前牌值
        public void Deliver(int i, string who)
        {
            switch (who)
            {
                case "player":
                    playerHand[i] = cardbase[i];
                    playerHand[i] = Display(playerHand[i]);
                    i++;
                    break;
                case "dealer":
                    dealerHand[i] = cardbase[i];
                    dealerHand[i] = Display(dealerHand[i]);
                    i++;
                    break;
            }
        }      //发牌
        public void Clear()
        {
            Array.Clear(playerHand, 0, playerHand.Length);
            Array.Clear(dealerHand, 0, dealerHand.Length);
        }                         //手牌数据清零
        public void GetResult(int sumplayer, int sumdealer, int playercardnum, char doub)
        {
            if (sumplayer == 21 && playercardnum == 2)
            {
                money += 150;
                Console.WriteLine("Good Fortune Sir!!Score=" + money);
            }
            else if (sumplayer > 21)
            {
                if (doub == 'd') money -= 200;
                else money -= 100;
                Console.WriteLine("You Burst!Score=" + money);
            }
            else if (sumdealer > 21)
            {
                if (doub == 'd') money += 200;
                else money += 100;
                Console.WriteLine("Dealer Burst!You Win!Score=" + money);
            }
            else if (sumplayer < sumdealer)
            {
                if (doub == 'd') money -= 200;
                else money -= 100;
                Console.WriteLine("You Lose.Score=" + money);
            }
            else if (sumplayer == sumdealer)
            {
                money += 0;
                Console.WriteLine("Draw！Score=" + money);
            }
            else if (sumplayer > sumdealer)
            {
                if (doub == 'd') money += 200;
                else money += 100;
                Console.WriteLine("You Win!Score=" + money);
            }
        }       //结算
    }

    class Player
    {
        int money = 1000;
        int playercardnum = 0;
        int playerhandlength = playerHand.Length;
        static int[] playerHand = new int[12];

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
        }                            //封装读取和写入当前奖金
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
        }                    //封装读取和写入已发的玩家手牌数
        public int GetPlayerHandLength()
        {
            return playerhandlength;
        }            //当前玩家手牌长度
        public int[] PlayerHand
        {
            get
            {
                return playerHand;
            } 
            set
            {
                playerHand = value;
            }
        }                     //封装获取玩家手牌
    }

    class Dealer
    {
        int dealerhandlength = dealerHand.Length;
        static int[] dealerHand = new int[12];

        public int GetDealerHandLength()
        {
            return dealerhandlength;
        }            //当前庄家手牌长度
        public int[] GetDealerHand()
        {
            return dealerHand;
        }                //封装获取庄家手牌
    }

    class Main
    {
        static void Cardmain(string[] args)
        {
                Card c = new Card();
                Player p = new Player();
                char coin = 'y';
                char knock = 'n';
                int i = 0;
                int j = 2;
                do
                {
                    c.Clear();
                    c.Shuffle();
                    Console.WriteLine("**********************************************");
                    Console.WriteLine("Welcome to BlackJack，Scoring 1500 to Claim a Prize");
                    Console.WriteLine("PlayerHand,Score=" + p.Money);
                    Console.WriteLine("----------");
                    Thread.Sleep(2000);
                    c.Deliver(i, "player");
                    c.Deliver(i, "player");
                    Console.WriteLine(" ");
                    Console.WriteLine("DealerHand");
                    Console.WriteLine("----------");
                    c.Deliver(i, "dealer");
                    c.Deliver(i, "dealer");
                    c.PlayerCardNum++;
                    c.PlayerCardNum++;
                    if (c.GetSum(0) == 21 && c.GetSum(1) != 21)
                       {  
                        c.GetResult(c.GetSum(0), c.GetSum(1), c.PlayerCardNum, knock);
                        goto end;
                        }

                    anoc:
                    if (knock == 'd')
                       {
                        Console.WriteLine("Cant Double now!");
                        goto anoc;
                       }
                    else Console.WriteLine("Hit(h)/Stand(n)/Double(d)/Surrender(r)?");//没有投降功能
                    try
                       {
                        knock = Convert.ToChar(Console.ReadLine());//没考虑输入其他单个字符时出现的异常
                       }
                    catch (System.FormatException)
                       {
                        Console.WriteLine("Warning:Enter h/n/d/r, then press Enter.");
                        goto anoc;
                       }
                    finally
                       {
                        //goto anoc;
                       }
                    if (knock == 'd')
                       {
                        Thread.Sleep(1000);
                        playerhand[i] = cardbase[i + 2];
                        playerhand[i] = Display(playerhand[i]);
                        i++;
                        playercardnum++;
                        if (Sumup(playerhand) > 21)
                           {
                            GetResult(Sumup(playerhand), Sumup(dealerhand), playercardnum, knock);
                            goto end;
                           }
                        goto con;
                        }
                    while (knock == 'h')
                    {
                        Thread.Sleep(1000);
                        playerhand[i] = cardbase[i + 2];
                        playerhand[i] = Display(playerhand[i]);
                        i++;
                        playercardnum++;
                        if (Sumup(playerhand) > 21)
                        {
                            GetResult(Sumup(playerhand), Sumup(dealerhand), playercardnum, knock);
                            goto end;
                        }
                        goto anoc;
                    }
                    con: Thread.Sleep(1000);



                    delc: Thread.Sleep(1000);
                    while (Sumup(dealerhand) < 17)
                    {
                        dealerhand[j] = cardbase[i + 2];
                        dealerhand[j] = Display(dealerhand[j]);
                        j++; i++;
                        if (Sumup(dealerhand) > 21)
                        {
                            GetResult(Sumup(playerhand), Sumup(dealerhand), playercardnum, knock);
                            goto end;
                        }
                        goto delc;
                    }
                    GetResult(Sumup(playerhand), Sumup(dealerhand), playercardnum, knock);

                    end: Console.WriteLine("**********************************************");
                    Thread.Sleep(1000);
                    if (p.Money >= 1500)
                    {
                        Console.WriteLine("passcode:winner winner chicken dinner!");
                    }
                    if (p.Money <= 0)
                    {
                        Console.WriteLine("Not Enough Gold！");
                        goto bye;
                    }

                    Console.WriteLine("Another Round? y/n?");
                    try
                    {
                        knock = 'n';
                        playercardnum = 0;
                        coin = Convert.ToChar(Console.ReadLine());//同样没考虑输入其他单个字符时的情况
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("Warning:Enter y/n, then press Enter.");
                        goto end;
                    }
                    finally
                    { }
                    Console.Clear();
                } while (coin == 'y');

                bye: Console.WriteLine("BYE！");
                Console.ReadKey();
            }
        }
}