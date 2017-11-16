using System;

namespace BlackJack
{
    class Human
    {
        private int j = 0;
        private int flag = 0;
        private static double win = 0;
        private static double lose = 0;
        private int sum = 0;
        private int ifmainexpexist = 0;                      //主经验库相关数据是否存在的标志位  常态为0  无数据为0，有数据为1
        private int ifviceexpexist = 0;                      //副经验库相关数据是否存在的标志位  常态为0   没有用到副经验库为0，无数据为2，有数据为3
        private static int[] playerhand = new int[20];
        private string knock;

        public int J
        { get; set; }
        public void GetResult(int sumplayer, int sumdealer)
        {
            try
            {
                if (sumplayer < sumdealer)
                {
                    throw (new YouLost("YouLost"));
                }
                else if (sumplayer == sumdealer)
                {
                    throw (new Draw("Draw"));
                }
                else
                {
                    throw (new YouWon("YouWon"));
                }
            }
            catch (Draw)
            {
                Console.WriteLine("Draw!");
            }
            catch (YouWon)
            {
                if (knock == "d")
                {
                    win += 2;
                }
                else
                {
                    win += 1;
                }
                Console.WriteLine("You Win!");
            }
            catch (YouLost)
            {
                if (knock == "d")
                {
                    lose += 2;
                }
                else
                {
                    lose += 1;
                }
                Console.WriteLine("You Lose.");
            }
            finally
            {
                Console.WriteLine("**********************************************");
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
        public double Win
        {
            get
            {
                return win;
            }
            set
            {
                win = value;
            }
        }
        public double Lose
        {
            get
            {
                return lose;
            }
            set
            {
                lose = value;
            }
        }
        public int IfMainExp
        {
            get
            {
                return ifmainexpexist;
            }
            set
            {
                ifmainexpexist = value;
            }
        }
        public int IfViceExp
        {
            get
            {
                return ifviceexpexist;
            }
            set
            {
                ifviceexpexist = value;
            }
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
        public void Clear()
        {
            Array.Clear(playerhand, 0, playerhand.Length);
        }
        public void QandA()
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
                QandA();
            }
            catch (NotAllowed)
            {
                Console.WriteLine("Warning:Please Re-Enter.");
                QandA();
            }
            finally
            {

            }
        }
        //人工操作玩家回合
        public int PersonRound(int I)
        {
            try
            {
                Clear();//清玩家手牌
                //int tmp = 0;
                Console.WriteLine("Human Round:");
                Casino.Deliver(playerhand, j, I);//1st Card 发第一张
                j++;
                I++;
                Casino.Deliver(playerhand, j, I);//2nd Card 发第二张
                j++;
                I++;
                sum = GetSum(playerhand);//累积求和
                if (sum == 21)//21点直接结束，不改变任何倾向，因为与操作无关
                {
                    ifmainexpexist = 1;
                    throw (new YouGotABlackJack("You Got A Black Jack"));
                }
                else
                {
                    //ReadTendency(0);//模式0==读取主经验库
                    //knock = RandomMainTrend(htrend, dtrend, strend, rtrend);//根据倾向，计算机进行随机判断
                    //Console.WriteLine("The computer selects {0}", knock);
                    QandA();//预留的人工应答界面
                    switch (knock)
                    {
                        case "h":
                            while (knock == "h")
                            {
                                Casino.Deliver(playerhand, j, I);
                                I++;
                                j++;
                                if ((sum = GetSum(playerhand)) > 21 && ifviceexpexist == 0)
                                {
                                    throw (new YouBursted("You Bursted"));
                                }
                                //ifviceexpexist = 2;
                                //vicemark = 0;
                                //ReadTendency(1);
                                //knock = RandomViceTrend(htrend1, strend1);
                                QandA();
                                //Console.WriteLine("The computer selects {0}", knock);
                                //Console.ReadKey();
                            }
                            break;

                        //case 'p':
                        case "d":
                            Casino.Deliver(playerhand, j, I);
                            I++;
                            j++;
                            sum = GetSum(playerhand);
                            if (sum > 21)
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
            }
            catch (YouGotABlackJack)
            {
                win += 1.5;
                flag++;
                Console.WriteLine("Good Fortune Sir!!");
            }
            catch (YouBursted)
            {
                if (knock == "d")
                {
                    lose += 2;
                    flag++;
                }
                else if (ifviceexpexist == 0)
                {
                    lose += 1;
                    flag++;
                }
                else
                {
                    lose += 1;
                    flag++;
                }
                Console.WriteLine("You Burst!");
            }
            catch (YouSurrender)
            {
                lose += 0.5;
                flag++;
                Console.WriteLine("You Surrender.");
            }
            finally
            {
            }
            return I;
        }
        //随机趋势选择
        public string RandomMainTrend(int i1, int i2, int i3, int i4)//根据主经验库进行随机判断,归一化
        {
            double p1, p2, p3, p4, p;
            Random ran = new Random();
            p1 = (double)(i1) / (i1 + i2 + i3 + i4);//各自倾向占比
            p2 = (double)(i2) / (i1 + i2 + i3 + i4);//有四种选择
            p3 = (double)(i3) / (i1 + i2 + i3 + i4);
            p4 = (double)(i4) / (i1 + i2 + i3 + i4);
            p = ran.NextDouble();//随机0~1
            Console.WriteLine("Win rate:H({0}%),D({1}%),S({2}%),R({3}%）", 100 * p1, 100 * p2, 100 * p3, 100 * p4);//实时显示当前倾向
            if ((p >= 0) && (p < p1))    //根据倾向大小划分0~1的范围
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
        public string RandomViceTrend(int i1, int i2)//根据副经验库进行随机判断
        {
            double p1, p2, p;
            Random ran = new Random();
            p1 = (double)(i1) / (i1 + i2);//各自倾向占比
            p2 = (double)(i2) / (i1 + i2);//有四种选择
            p = ran.NextDouble();
            Console.WriteLine("Current Win rate:H({0}%),S({1}%)", 100 * p1, 100 * p2);
            if ((p >= 0) && (p < p1))
            {
                return "h";
            }
            else
            {
                return "s";
            }
        }
    }
}
