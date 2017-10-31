using System;
using System.IO;
using System.Text;
using System.Linq;

namespace BlackJack
{
    class Casino
    {
        private int[] cardbase = new int[52];
        private int i = 0;//第几张

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
    }
    class Player
    {
        private int htrend = 25000000;
        private int dtrend = 25000000;
        private int strend = 25000000;
        private int rtrend = 25000000;
        private int htrend1 = 25000000;
        private int strend1 = 25000000;
        private static int money = 1000;
        private int f2sum = 0;
        private int ifmainexpexist = 0;                       //主经验库相关数据是否存在的标志位  常态为0  无数据为0，有数据为1
        private int ifviceexpexist = 0;                      //副经验库相关数据是否存在的标志位  常态为0   没有用到副经验库为0，无数据为2，有数据为3
        private int mainmark = 0;                            //计算主经验库相关数据在第几行的计数器
        private int vicemark = 0;                            //计算副经验库相关数据在第几行的计数器
        private string knock;                 
        string MainPath = @"C:\Users\lifan\blackjack\Data.txt"; //主经验库
        string VicePath = @"C:\Users\lifan\blackjack\Data1.txt";//副经验库
        private static int[] playerhand = new int[20];
        
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
                    string sum;
                    while ((sum = sr2.ReadLine()) != null)
                    {
                        //if exist then download exp
                        if ((Convert.ToInt32(sum) == f2sum))
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
                    sb5.Append(f2sum + "\r\n");
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
                            sb6.Append(f2sum + "\r\n");                            //写入和
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
        public int F2Sum
        {
            get
            {
                return f2sum;
            }
            set
            {
                f2sum = value;
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
        public int MainMark
        {
            get
            {
                return mainmark;
            }
            set
            {
                mainmark = value;
            }
        }
        public int ViceMark
        {
            get
            {
                return vicemark;
            }
            set
            {
                vicemark = value;
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
        public void GetResult(int sumplayer, int sumdealer)
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
    }
    class Dealer
    {
        static private int[] dealerhand = new int[20];
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
    }
    class Computer : Player
    {
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
            p1 = (double)(i1) / (i1+i2);//各自倾向占比
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
    //特殊情况异常处理
    public class NotAllowed : ApplicationException
    {
        public NotAllowed(string message) : base(message)
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
    //主程序入口
    static class Program
    {   //主方法
        static void Main(string[] args)
        {
            do//一局的开始
            {
                Casino c = new Casino();//系统
                Player p = new Player();//新的玩家实例
                Dealer d = new Dealer();//新的庄家实例
                Computer com = new Computer();
                try
                {   //准备阶段
                    p.Clear();//清玩家手牌
                    d.Clear();//清庄家手牌
                    c.Shuffle();//洗牌
                    Console.WriteLine("**********************************************");
                    Console.WriteLine("Welcome to BlackJack，Scoring 1500 to Claim a Prize");
                    Console.WriteLine("PlayerHand,Score=" + p.Money);
                    Console.WriteLine("**********************************************");
                    //玩家回合
                    c.Deliver(p.PlayerHand, c.I);//1st Card 发第一张
                    c.I++;
                    c.Deliver(p.PlayerHand, c.I);//2nd Card 发第二张
                    c.I++;
                    p.F2Sum = p.GetSum(p.PlayerHand);//累积求和
                    if (p.F2Sum  == 21)//21点直接结束，不改变任何倾向，因为与操作无关
                    {
                        p.IfMainExp = 1;
                        throw (new YouGotABlackJack("You Got A Black Jack"));
                    }
                    else
                    {
                        p.ReadTendency(0);//模式0==读取主经验库
                        p.Knock = com.RandomMainTrend(p.HTrend, p.DTrend, p.STrend, p.RTrend);//根据倾向，计算机进行随机判断
                        Console.WriteLine("The computer selects {0}", p.Knock);
                        //p.QandA();//预留的人工应答界面
                        //Console.ReadKey();
                        switch (p.Knock)
                        {
                            case "h":
                                while (p.Knock == "h")
                                {
                                    c.Deliver(p.PlayerHand, c.I);
                                    c.I++;
                                    if(p.GetSum (p.PlayerHand ) > 21 && p.IfViceExp  ==0)
                                    {
                                        throw (new YouBursted("You Bursted"));
                                    }
                                    p.F2Sum =p.GetSum(p.PlayerHand);
                                    if (p.F2Sum > 21)
                                    {
                                        p.F2Sum -= p.PlayerHand[c.I - 1];
                                        throw (new YouBursted("You Bursted"));
                                    }
                                    p.IfViceExp = 2;
                                    p.ViceMark = 0;
                                    p.ReadTendency(1);
                                    p.Knock = com.RandomViceTrend(p.HTrend1, p.STrend1);
                                    //p.QandA();
                                    Console.WriteLine("The computer selects {0}", p.Knock);
                                    //Console.ReadKey();
                                }
                                break;

                            //case 'p':
                            case "d":
                                c.Deliver(p.PlayerHand, c.I);
                                c.I++;
                                p.F2Sum = p.GetSum(p.PlayerHand);
                                if (p.F2Sum > 21)
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
                    c.Deliver(d.DealerHand, c.I);
                    c.I++;
                    c.Deliver(d.DealerHand, c.I);
                    c.I++;
                    while (d.GetSum(d.DealerHand) < 17)
                    {
                        c.Deliver(d.DealerHand, c.I);
                        c.I++;
                        if (d.GetSum(d.DealerHand) > 21)
                        {
                            throw (new DealerBusted("DealerBusted"));
                        }
                    }
                    //结算
                    p.GetResult(p.F2Sum, d.GetSum(d.DealerHand));
                }
                catch (Draw)
                {
                    p.Money += 0;
                    Console.WriteLine("Draw！Score=" + p.Money);
                }
                catch (YouWon)
                {
                    if (p.Knock == "d")
                    {
                        p.Money += 200;
                        p.HTrend -= 8;
                        p.DTrend += 24;
                        p.STrend -= 8;
                        p.RTrend -= 8;
                    }
                    else if (p.ViceMark  == 0 && p.Knock != "d")
                    {
                        p.Money += 100;
                        p.HTrend -= 4;
                        p.DTrend -= 4;
                        p.STrend += 12;
                        p.RTrend -= 4;
                    }
                    else
                    {
                        p.Money -= 100;
                        p.HTrend += 12;
                        p.DTrend -= 4;
                        p.STrend -= 4;
                        p.RTrend -= 4;
                        p.HTrend1 -= 12;
                        p.STrend1 += 12;
                    }
                    Console.WriteLine("You Win!Score=" + p.Money);
                }
                catch (YouLost)
                {
                    if (p.Knock == "d")
                    {
                        p.Money -= 200;
                        p.HTrend += 8;
                        p.DTrend -= 24;
                        p.STrend += 8;
                        p.RTrend += 8;
                    }
                    else if (p.IfViceExp  == 0 && p.Knock != "d")
                    {
                        p.Money -= 100;
                        p.HTrend += 4;
                        p.DTrend += 4;
                        p.STrend -= 12;
                        p.RTrend += 4;
                    }
                    else
                    {
                        p.Money -= 100;
                        p.HTrend += 9;
                        p.DTrend -= 3;
                        p.STrend -= 3;
                        p.RTrend -= 3;
                        p.HTrend1 += 12;
                        p.STrend1 -= 12;
                    }
                    Console.WriteLine("You Lose.Score=" + p.Money);
                }
                catch (YouGotABlackJack)
                {
                    p.Money += 150;
                    Console.WriteLine("Good Fortune Sir!!Score=" + p.Money);
                }
                catch (YouBursted)
                {
                    if (p.Knock == "d")
                    {
                        p.Money -= 200;
                        p.HTrend += 8;
                        p.DTrend -= 24;
                        p.STrend += 8;
                        p.RTrend += 8;
                    }
                    else if (p.IfViceExp == 0)
                    {
                        p.Money -= 100;
                        p.HTrend -= 21;
                        p.DTrend += 7;
                        p.STrend += 7;
                        p.RTrend += 7;
                    }
                    else
                    {
                        p.Money -= 100;
                        p.HTrend1 -= 12;
                        p.STrend1 += 12;
                    }
                    Console.WriteLine("You Burst!Score=" + p.Money);
                }
                catch (DealerBusted)
                {
                    if (p.Knock == "d")
                    {
                        p.Money += 200;
                        p.HTrend -= 4;
                        p.DTrend += 12;
                        p.STrend -= 4;
                        p.RTrend -= 4;

                    }
                    else if (p.IfViceExp == 0)
                    {
                        p.Money += 100;
                        p.HTrend += 12;
                        p.DTrend -= 4;
                        p.STrend -= 4;
                        p.RTrend -= 4;
                    }
                    else
                    {
                        p.Money += 100;
                        p.HTrend1 -= 12;
                        p.STrend1 += 12;
                    }
                    Console.WriteLine("Dealer Burst!You Win!Score=" + p.Money);
                }
                catch (YouSurrender)
                {
                    p.Money -= 50;
                    p.HTrend += 2;
                    p.DTrend += 2;
                    p.STrend += 2;
                    p.RTrend -= 6;
                    Console.WriteLine("You Surrender. Score=" + p.Money);
                }
                finally
                {
                    switch(p.IfMainExp)
                    {
                        case 0:
                            p.EditTendency(3);
                            break;
                        case 1:
                            p.EditTendency(4);
                            break;
                        default:
                            break;
                    }
                    switch (p.IfViceExp)
                    {
                        case 2:
                            p.EditTendency(5);
                            break;
                        case 3:
                            p.EditTendency(6);
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine("**********************************************");
                    //if (p.Money >= 1500)
                    //{
                    //   Console.WriteLine("红包密码666！");
                    //}
                    //Console.ReadKey();
                    Console.Clear();
                }
            } while (true);
        }
    }
}