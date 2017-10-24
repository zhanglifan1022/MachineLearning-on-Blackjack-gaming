using System;
using System.Threading;
namespace BlackJack
{
    class Card
    {
        static int money = 1000;
        static int[] playerhand = new int[12];
        static int[] dealerhand = new int[12];
        static public int[] cardbase = new int[52];
        static int playercardnum = 0;

        static void GetResult(int sumplayer, int sumdealer, int playercardnum, char doub)
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
        }

        static void Shuffle()
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
                int tmp = cardbase [idx2];
                cardbase [idx1] = cardbase [idx2];
                cardbase [idx2] = tmp;
            }  
        }

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
            else if (a > 4 && a<=8)
            { Console.WriteLine(color + "  2 "); return 2; }
            else if (a > 8 && a<=12)
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

        static int Sumup(int[] arrInt)
        {
            int sum1 = 0;
            int sum2 = 0;
            int i;
            int flag = 0;
            for (i = 0; i < arrInt.GetLength(0); i++)
            {
                if (arrInt[i] == 1 && flag == 0)
                {
                    sum1 = sum1 + arrInt[i];
                    sum2 = sum2 + arrInt[i] + 10;
                    flag++;
                }
                else
                {
                    sum1 = sum1 + arrInt[i];
                    sum2 = sum2 + arrInt[i];
                }
            }
            if (sum2 > sum1 && sum2 <= 21)
            {
                return sum2;
            }
            else return sum1;
        }
        
        static void Main(string[] args)
        {
            char coin = 'y';
            char knock = 'n';
            int i = 2;
            int j = 2;
            do
            {
                Array.Clear(playerhand, 0, playerhand.Length);
                Array.Clear(dealerhand, 0, dealerhand.Length);

                Console.WriteLine("**********************************************");
                Shuffle();
                Console.WriteLine("Welcome to BlackJack，Scoring 1500 to Claim a Prize");
                Console.WriteLine("PlayerHand,Score=" + money);
                Thread.Sleep(2000);
                i++;
                playerhand[0] = Display(playerhand[0]);
                playercardnum++;
                playerhand[1] = cardbase[2];
                playerhand[1] = Display(playerhand[1]);
                playercardnum++;
                if (Sumup(playerhand) == 21)
                { 
                    GetResult(Sumup(playerhand), Sumup(dealerhand), playercardnum, knock);
                    goto end;
                }

        anoc:   if (knock == 'd')
                {
                    Console.WriteLine("Cant Double now!");
                    goto anoc;
                }else  Console.WriteLine("Hit(h)/Stand(n)/Double(d)/Surrender(r)?");
                try
                {
                     knock = Convert.ToChar(Console.ReadLine());
                }
                catch (System.FormatException)
                { 
                     Console.WriteLine("Warning:Enter h/n/d/r, then press Enter.");
                     goto anoc;
                }
                finally
                {
                     //goto aa;
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
        con:    Thread.Sleep(1000);
                Console.WriteLine(" ");
                Console.WriteLine("DealerHand");
                Console.WriteLine("----------");
                dealerhand[0] = cardbase[1];
                dealerhand[0] = Display(dealerhand[0]);
                dealerhand[1] = cardbase[3];
                dealerhand[1] = Display(dealerhand[1]);

        delc:   Thread.Sleep(1000);
                while(Sumup(dealerhand) < 17)
                {
                      dealerhand[j] = cardbase[i +2];
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

        end:    Console.WriteLine("**********************************************");
                Thread.Sleep(1000);
                if (money >= 1500)
                {
                    Console.WriteLine("passcode:winner winner chicken dinner!");
                }
                if (money <= 0)
                {
                    Console.WriteLine("Not Enough Gold！");
                    goto bye;
                }

                Console.WriteLine("Another Round? y/n?");
                try
                {
                    knock = 'n';
                    playercardnum = 0;
                    coin = Convert.ToChar(Console.ReadLine());
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Warning:Enter y/n, then press Enter.");
                    goto end;
                }
                finally
                { }
                Console.Clear();
            } while(coin == 'y');

        bye: Console.WriteLine("BYE！");
             Console.ReadKey();
        }
    }  
}