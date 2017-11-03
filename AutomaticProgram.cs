using System;
using System.IO;
using System.Text;
using static System.Math;

namespace BlackJack
{
    //主程序入口
    static class Program
    {   //主方法
        static void Main(string[] args)
        {
            int round = 0;
            do//一局的开始
            {
                Casino c = new Casino();//系统
                Computer com = new Computer();//新的电脑玩家实例
                Dealer d = new Dealer();//新的庄家实例
                Human p = new Human();//新的个人玩家实例
                c.I = 0;//准备阶段
                c.Shuffle();//洗牌
                Console.WriteLine("**********************************************");
                Console.WriteLine("Welcome to BlackJack，Round:{0}",round);
                Console.WriteLine("Computer Win rate={0}%", 100 * com.Win / (com.Win + com.Lose));
                Console.WriteLine("Versus");
                Console.WriteLine("Human Win rate={0}%", 100 * p.Win / (p.Win + p.Lose));
                Console.WriteLine("**********************************************");

                //玩家回合
                c.I = com.PlayerRound(c.I);
                //c.I = p.PersonRound(c.I);

                Console.WriteLine(" ");
                Console.WriteLine("DealerHand");
                Console.WriteLine("----------");

                //庄家回合
                if (com.Flag == 1 && p.Flag == 1)//电脑玩家和人类玩家同时有特殊状况？有则不进入庄家回合直接下一轮
                { }
                else
                {
                    d.Flag = d.DealerRound(c.I, com, p);
                    //结算
                    if (d.Flag == 0)//庄家没爆
                    {
                        if (com.Flag == 0)//电脑玩家没特殊情况就允许结算
                        {
                            com.GetResult(com.Sum, d.Sum);
                        }
                        if (p.Flag == 0)//人类玩家没特殊情况就允许结算
                        {
                            //p.GetResult(p.Sum, d.Sum);
                        }
                    }
                }
                //if (p.Money >= 1500)
                //{
                //   Console.WriteLine("红包密码666！");
                //}
                //Console.ReadKey();
                switch (com.IfMainExp)
                {
                    case 0:
                        com.EditTendency(3);
                        break;
                    case 1:
                        com.EditTendency(4);
                        break;
                    default:
                        break;
                }
                switch (com.IfViceExp)
                {
                    case 2:
                        com.EditTendency(5);
                        break;
                    case 3:
                        com.EditTendency(6);
                        break;
                    default:
                        break;
                }
                Console.WriteLine("**********************************************");
                round++;
                Console.Clear();
            } while (true);
        }
    }
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
            { Console.WriteLine("  "+color + " A "); return 1; }
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
        public static void Deliver(int[] arrint, int j,int i)
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
    class Computer
    {
        private int j = 0;
        private int htrend = 2500000;
        private int dtrend = 2000000;
        private int strend = 2500000;
        private int rtrend = 2500000;
        private int htrend1 = 2500000;
        private int strend1 = 2500000;
        private int flag = 0;
        private static double win = 0;
        private static double lose = 0;
        private int sum = 0;
        private int ifmainexpexist = 0;                      //主经验库相关数据是否存在的标志位  常态为0  无数据为0，有数据为1
        private int ifviceexpexist = 0;                      //副经验库相关数据是否存在的标志位  常态为0   没有用到副经验库为0，无数据为2，有数据为3
        private int mainmark = 0;                            //计算主经验库相关数据在第几行的计数器
        private int vicemark = 0;                            //计算副经验库相关数据在第几行的计数器
        private static int[] playerhand = new int[20];
        private string knock;                 
        private string MainPath = @"C:\Users\lifan\blackjack\Data.txt"; //主经验库
        private string VicePath = @"C:\Users\lifan\blackjack\Data1.txt";//副经验库
        
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
        public int J
        { get; set; }
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
        public void GetResult(int sumplayer, int sumdealer)
        {
            try
            {
                if (sumplayer < sumdealer)
                {
                    throw (new ComputerLost("ComputerLost"));
                }
                else if (sumplayer == sumdealer)
                {
                    throw (new Draw("Draw"));
                }
                else
                {
                    throw (new ComputerWon("ComputerWon"));
                }
            }
            catch (Draw)
            {
                Console.WriteLine("Draw!");
            }
            catch (ComputerWon)
            {
                if (knock == "d")
                {
                    win += 2;
                    htrend = Abs(htrend - 8);
                    dtrend = Abs(dtrend + 24);
                    strend = Abs(strend - 8);
                    rtrend = Abs(rtrend - 8);
                }
                else if (ifviceexpexist  == 0 && knock != "d")
                {
                    win += 1;
                    htrend = Abs(htrend - 4);
                    dtrend = Abs(dtrend - 4);
                    strend = Abs(strend + 12);
                    rtrend = Abs(rtrend - 4);
                }
                else
                {
                    win += 1;
                    htrend = Abs(htrend + 12);
                    dtrend = Abs(dtrend - 4);
                    strend = Abs(strend - 4);
                    rtrend = Abs(rtrend - 4);
                    htrend1 = Abs(htrend1 - 12);
                    strend1 = Abs(strend1 + 12);
                }
                Console.WriteLine("Computer Win!");
            }
            catch (ComputerLost)
            {
                if (knock == "d")
                {
                    lose += 2;
                    htrend = Abs(htrend + 12);
                    dtrend = Abs(dtrend - 24);
                    strend = Abs(strend + 6);
                    rtrend = Abs(rtrend + 6);
                }
                else if (ifviceexpexist  == 0 && knock != "d")
                {
                    lose += 1;
                    htrend = Abs(htrend + 12);
                    dtrend = Abs(dtrend + 6);
                    strend = Abs(strend - 24);
                    rtrend = Abs(rtrend + 6);
                }
                else
                {
                    lose += 1;
                    htrend = Abs(htrend + 12);
                    dtrend = Abs(dtrend - 4);
                    strend = Abs(strend - 4);
                    rtrend = Abs(rtrend - 4);
                    htrend1 = Abs(htrend1 + 12);
                    strend1 = Abs(strend1 - 12);
                }
                Console.WriteLine("Computer Lose.");
            }
            finally
            {
                Console.WriteLine("**********************************************");
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
        public int HTrend1
        {
            get
            {
                return htrend1;
            }
            set
            {
                htrend1 = value;
            }
        }
        public int STrend1
        {
            get
            {
                return strend1;
            }
            set
            {
                strend1 = value;
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

        public void ReadTendency(int mode)
        {
            switch (mode)
            {
                case 0://读取主经验库
                    FileStream fs1 = new FileStream(MainPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);//创建主经验库文件流
                    StreamReader sr1 = new StreamReader(fs1, Encoding.Default);//主经验库读取器
                    string card1;
                    string card2;
                    while ((card1 = sr1.ReadLine()) != null)//循环读取文件流的下一行赋值到卡一，循环条件为文件下一行不为空
                    {
                        card2 = sr1.ReadLine();//再读一行为卡二
                                               //每两位进行文件读取，看这两位是否与当前手牌完全一致
                        if ((Convert.ToInt32(card1) == playerhand[0]) && (Convert.ToInt32(card2) == playerhand[1]))
                        {
                            ifmainexpexist++;//成功找到经验 置位1
                            htrend = Convert.ToInt32(sr1.ReadLine());//分别读取接下来的四个倾向
                            dtrend = Convert.ToInt32(sr1.ReadLine());
                            strend = Convert.ToInt32(sr1.ReadLine());
                            rtrend = Convert.ToInt32(sr1.ReadLine());
                            break;
                        }
                        mainmark += 2;//相关数据所在行数+2，每读两个数累计加二
                    }
                    sr1.Close();//关闭读取器
                    fs1.Close();//关闭文件流
                    break;
                case 1://读取副经验库
                    FileStream fs2 = new FileStream(VicePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr2 = new StreamReader(fs2, Encoding.Default);
                    string reading;
                    while ((reading = sr2.ReadLine()) != null)
                    {
                        //if exist then download exp
                        if ((Convert.ToInt32(reading) == sum))
                        {
                            ifviceexpexist++;
                            htrend1 = Convert.ToInt32(sr2.ReadLine());
                            strend1 = Convert.ToInt32(sr2.ReadLine());
                            break;
                        }
                        vicemark++;
                    }
                    sr2.Close();
                    fs2.Close();
                    break;
            }
        }
        public void EditTendency(int mode)
        {
            switch (mode)
            {
                case 3://主经验库无相关数据需要创建
                    FileStream fs3 = new FileStream(MainPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);  //根据Ifexist的情况分别有 0,1,2,3 分别对应 创建主经验库，更新主经验库，创建副经验库，更新副经验库的数据
                    StreamReader sr3 = new StreamReader(fs3, Encoding.Default);//新的文件流阅读器
                    string line3 = sr3.ReadLine();//读取第一行
                    StringBuilder sb3 = new StringBuilder();//字符串创建器
                    for (int i = 1; line3 != null; i++)    //整个文件全部复制一遍
                    {
                        sb3.Append(line3 + "\r\n");
                        line3 = sr3.ReadLine();
                    }
                    sb3.Append(playerhand[0] + "\r\n");     //文件末尾加上新创建的值
                    sb3.Append(playerhand[1] + "\r\n");    //文件末尾加上新创建的值
                    sb3.Append(htrend + "\r\n");          //文件末尾加上新创建的值
                    sb3.Append(dtrend + "\r\n");         //文件末尾加上新创建的值
                    sb3.Append(strend + "\r\n");        //文件末尾加上新创建的值
                    sb3.Append(rtrend);                //文件末尾加上新创建的值
                    sr3.Close();
                    fs3.Close();
                    FileStream fs31 = new FileStream(MainPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw31 = new StreamWriter(fs31, Encoding.Default);
                    sw31.Write(sb3.ToString());//写入文件
                    sw31.Flush();
                    sb3.Clear();
                    sw31.Close();
                    fs31.Close();
                    break;

                case 4://主经验库有相关数据需要更新
                    FileStream fs4 = new FileStream(MainPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);  //根据Ifexist的情况分别有 0,1,2,3 分别对应 创建主经验库，更新主经验库，创建副经验库，更新副经验库的数据
                    StreamReader sr4 = new StreamReader(fs4, Encoding.Default);//新的文件流阅读器
                    string line4 = sr4.ReadLine();//读取第一行
                    StringBuilder sb4 = new StringBuilder();//字符串创建器
                    for (int i = 1; line4 != null; i++)    //原文件从头读到尾
                    {
                        sb4.Append(line4 + "\r\n");        //暂时写进字符串创建器中
                        if (i != mainmark)               //只要不是需要修改的行，就进行简单的读出写入。在到达所在行之后，i的赋值再无意义，循环的判断只在于是否到达文件的底部
                            line4 = sr4.ReadLine();
                        else                                   //到达所在行
                        {
                            sb4.Append(playerhand[0] + "\r\n");                           //写入卡一
                            sr4.ReadLine();                     //读取下一行但不动作
                            sb4.Append(playerhand[1] + "\r\n");                           //写入卡二
                            sr4.ReadLine();                     //读取下一行但不动作
                            sb4.Append(htrend + "\r\n");                                  //写入倾向一
                            sr4.ReadLine();                     //读取下一行但不动作
                            sb4.Append(dtrend + "\r\n");                                  //写入倾向二
                            sr4.ReadLine();                     //读取下一行但不动作
                            sb4.Append(strend + "\r\n");                                  //写入倾向三
                            sr4.ReadLine();                     //读取下一行但不动作
                            sb4.Append(rtrend + "\r\n");                                  //写入倾向四
                            sr4.ReadLine();                     //读取下一行但不动作
                            line4 = sr4.ReadLine();              //读取下一行也就是紧接着的别的数据开端
                        }
                    }
                    sr4.Close();
                    fs4.Close();
                    FileStream fs41 = new FileStream(MainPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw41 = new StreamWriter(fs41, Encoding.Default);
                    sw41.Write(sb4.ToString());//写入文件
                    sw41.Flush();
                    sb4.Clear();
                    sw41.Close();
                    fs41.Close();
                    break;

                case 5:                                        //副经验库无相关数据需要创建
                    FileStream fs5 = new FileStream(VicePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);  //根据Ifexist的情况分别有 0,1,2,3 分别对应 创建主经验库，更新主经验库，创建副经验库，更新副经验库的数据
                    StreamReader sr5 = new StreamReader(fs5, Encoding.Default);//新的文件流阅读器
                    string line5 = sr5.ReadLine();//读取第一行
                    StringBuilder sb5 = new StringBuilder();//字符串创建器
                    for (int i = 1; line5 != null; i++)
                    {
                        sb5.Append(line5 + "\r\n");
                        line5 = sr5.ReadLine();
                    }
                    sr5.Close();
                    fs5.Close();
                    sb5.Append(sum + "\r\n");
                    sb5.Append(htrend1 + "\r\n");
                    sb5.Append(strend1);
                    FileStream fs51 = new FileStream(VicePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw51 = new StreamWriter(fs51, Encoding.Default);
                    sw51.Write(sb5.ToString());//写入文件
                    sw51.Flush();
                    sb5.Clear();
                    sw51.Close();
                    fs51.Close();
                    break;

                case 6:                                       //副经验库有相关数据需要更新
                    FileStream fs6 = new FileStream(VicePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);  //根据Ifexist的情况分别有 0,1,2,3 分别对应 创建主经验库，更新主经验库，创建副经验库，更新副经验库的数据
                    StreamReader sr6 = new StreamReader(fs6, Encoding.Default);//新的文件流阅读器
                    string line6 = sr6.ReadLine();//读取第一行
                    StringBuilder sb6 = new StringBuilder();//字符串创建器
                    for (int i = 1; line6 != null; i++)
                    {
                        sb6.Append(line6 + "\r\n");
                        if (i != vicemark)
                            line6 = sr6.ReadLine();
                        else
                        {
                            sb6.Append(sum + "\r\n");                            //写入和
                            line6 = sr6.ReadLine();      //读取下一行但不动作
                            sb6.Append(htrend1 + "\r\n");                          //写入倾向一
                            line6 = sr6.ReadLine();      //读取下一行但不动作
                            sb6.Append(strend1 + "\r\n");                          //写入倾向二
                            line6 = sr6.ReadLine();      //读取下一行但不动作
                            line6 = sr6.ReadLine();      //读取下一行也就是紧接着的别的数据开端
                        }
                    }
                    sr6.Close();
                    fs6.Close();
                    FileStream fs61 = new FileStream(VicePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw61 = new StreamWriter(fs61, Encoding.Default);
                    sw61.Write(sb6.ToString());//写入文件
                    sw61.Flush();
                    sb6.Clear();
                    sw61.Close();
                    fs61.Close();
                    break;
            }
        }
        //电脑控制的玩家回合
        public int PlayerRound(int I)
        {
            try
            {
                Clear();//清玩家手牌
                IfMainExp = 0;
                IfViceExp = 0;
                mainmark = 0;
                vicemark = 0;
                int tmp = 0;
                Console.WriteLine("Computer Round:");
                Casino.Deliver(playerhand, j,I);//1st Card 发第一张
                I++;
                j++;
                Casino.Deliver(playerhand, j,I);//2nd Card 发第二张
                I++;
                j++;
                sum = GetSum(playerhand);//累积求和
                if (sum == 21)//21点直接结束，不改变任何倾向，因为与操作无关
                {
                    ifmainexpexist = 1;
                    throw (new YouGotABlackJack("Computer Got A Black Jack"));
                }
                else
                {
                    ReadTendency(0);//模式0==读取主经验库
                    knock = RandomMainTrend(htrend, dtrend, strend, rtrend);//根据倾向，计算机进行随机判断
                    Console.WriteLine("The computer selects {0}", knock);
                    //QandA();//预留的人工应答界面
                    //Console.ReadKey();
                    switch (knock)
                    {
                        case "h":
                            while (knock == "h")
                            {
                                Casino.Deliver(playerhand,j, I);
                                I++;
                                j++;
                                if ((sum = GetSum(playerhand)) > 21 && ifviceexpexist == 0)
                                {
                                    throw (new YouBursted("Computer Bursted"));
                                }
                                if (sum > 21)
                                {
                                    tmp = playerhand[j - 1];
                                    playerhand[j - 1] = 0;
                                    sum = GetSum(playerhand);
                                    throw (new YouBursted("Computer Bursted"));
                                }
                                if (ifviceexpexist == 3)
                                {
                                    tmp = playerhand[j - 1];
                                    playerhand[j - 1] = 0;
                                    sum = GetSum(playerhand);
                                    htrend1 = Abs(htrend1 + 6);
                                    strend1 = Abs(strend1 - 6);
                                    EditTendency(6);
                                    playerhand[j - 1] = tmp;
                                    sum = GetSum(playerhand);
                                }
                                ifviceexpexist = 2;
                                vicemark = 0;
                                ReadTendency(1);
                                knock = RandomViceTrend(htrend1, strend1);
                                //p.QandA();
                                Console.WriteLine("The Computer selects {0}", knock);
                                //Console.ReadKey();
                            }
                            break;

                        //case 'p':
                        case "d":
                            Casino.Deliver(playerhand, j,I);
                            I++;
                            j++;
                            sum = GetSum(playerhand);
                            if (sum > 21)
                            {
                                throw (new YouBursted("Computer Bursted"));
                            }
                            break;
                        case "r":
                            throw (new YouSurrender("Computer Surrender"));
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
                    htrend = Abs(htrend + 8);
                    dtrend = Abs(dtrend - 24);
                    strend = Abs(strend + 8);
                    rtrend = Abs(rtrend + 8);
                }
                else if (ifviceexpexist == 0)
                {
                    lose += 1;
                    flag++;
                    htrend = Abs(htrend - 12);
                    strend = Abs(strend + 8);
                    rtrend = Abs(rtrend + 4);
                }
                else
                {
                    lose += 1;
                    flag++;
                    htrend = Abs(htrend + 12);
                    dtrend = Abs(dtrend - 4);
                    strend = Abs(strend - 4);
                    rtrend = Abs(rtrend - 4);
                    htrend1 = Abs(htrend1 - 12);
                    strend1 = Abs(strend1 + 12);
                }
                Console.WriteLine("Computer Burst!");
            }
            catch (YouSurrender)
            {
                lose += 0.5;
                flag++;
                htrend = Abs(htrend + 2);
                dtrend = Abs(dtrend + 2);
                strend = Abs(strend + 2);
                rtrend = Abs(rtrend - 6);
                Console.WriteLine("Computer Surrender.");
            }
            finally
            {
                Console.WriteLine("**********************************************");
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
                Casino.Deliver(playerhand,j,I);//1st Card 发第一张
                j++;
                I++;
                Casino.Deliver(playerhand,j,I);//2nd Card 发第二张
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
                                Casino.Deliver(playerhand, j,I);
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
                            Casino.Deliver(playerhand,j, I);
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
    class Dealer
    {
        private int j=0;
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
        public int DealerRound(int I,Computer com,Human p)
        {
            try
            {
                Clear();//清庄家手牌
                Casino.Deliver(dealerhand,j, I);
                I++;
                j++;
                Casino.Deliver(dealerhand,j, I);
                I++;
                j++;
                while ((sum = GetSum(dealerhand)) < 17)
                {
                    Casino.Deliver(dealerhand, j,I);
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
                        }
                        else if (com.IfViceExp == 0)
                        {
                            com.Win += 1;
                            com.HTrend = Abs(com.HTrend - 4);
                            com.DTrend = Abs(com.DTrend - 4);
                            com.STrend = Abs(com.STrend + 12);
                            com.RTrend = Abs(com.RTrend - 4);
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

    //特殊情况异常处理
    public class NotAllowed : ApplicationException
    {
        public NotAllowed(string message) : base(message)
        {
        }
    }
    public class ComputerWon : ApplicationException
    {
        public ComputerWon(string message) : base(message)
        {
        }
    }
    public class ComputerLost : ApplicationException
    {
        public ComputerLost(string message) : base(message)
        {
        }
    }
    public class YouGotABlackJack : ApplicationException
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
    public class YouWon : ApplicationException
    {
        public YouWon(string message) : base(message)
        {
        }
    }
    public class YouLost : ApplicationException
    {
        public YouLost(string message) : base(message)
        {
        }
    }
    public class Draw : ApplicationException
    {
        public Draw(string message) : base(message)
        {
        }
    }
}