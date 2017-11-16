using System;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using static System.Math;

namespace BlackJack
{
    class Computer
    {
        private int j = 0;
        private int htrend = 2500000;
        private int dtrend = 2500000;
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

        public static double WinRate
        {
            get
            { return 100 * win / (win + lose); }
        }
        public int CardCombination
        { get { return Max(playerhand[0], playerhand[1]) * 10 + Min(playerhand[0], playerhand[1]); } }
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
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
                }
                else if (ifviceexpexist == 0 && knock != "d")
                {
                    win += 1;
                    htrend = Abs(htrend - 4);
                    dtrend = Abs(dtrend - 4);
                    strend = Abs(strend + 12);
                    rtrend = Abs(rtrend - 4);
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
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
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
                    if (htrend1 < 22) htrend1 += 21;
                    if (strend1 < 22) strend1 += 21;
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
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
                }
                else if (ifviceexpexist == 0 && knock != "d")
                {
                    lose += 1;
                    htrend = Abs(htrend + 12);
                    dtrend = Abs(dtrend + 6);
                    strend = Abs(strend - 24);
                    rtrend = Abs(rtrend + 6);
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
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
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
                    if (htrend1 < 22) htrend1 += 21;
                    if (strend1 < 22) strend1 += 21;
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
                    sw31.Write((sb3.ToString()));//写入文件
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
                    sw41.Write((sb4.ToString()));//写入文件
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
                    sw51.Write((sb5.ToString()));//写入文件
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
                            sr6.ReadLine();      //读取下一行但不动作
                            sb6.Append(htrend1 + "\r\n");                          //写入倾向一
                            sr6.ReadLine();      //读取下一行但不动作
                            sb6.Append(strend1 + "\r\n");                          //写入倾向二
                            sr6.ReadLine();      //读取下一行但不动作
                            line6 = sr6.ReadLine();      //读取下一行也就是紧接着的别的数据开端
                        }
                    }
                    sr6.Close();
                    fs6.Close();
                    FileStream fs61 = new FileStream(VicePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw61 = new StreamWriter(fs61, Encoding.Default);
                    sw61.Write((sb6.ToString()));//写入文件
                    sw61.Flush();
                    sb6.Clear();
                    sw61.Close();
                    fs61.Close();
                    break;
            }
        }

        public void ReadTendencyFromSql(int mode, int value)
        {
            switch (mode)
            {
                case 0://读取SQL主经验库
                    string rmcon = "Server =.; DataBase = BlackJack; Trusted_Connection = SSPI";
                    using (SqlConnection rmsqlcon = new SqlConnection(rmcon))
                    {
                        string rmcmd = "SELECT HTrend,STrend,DTrend,Rtrend FROM MainExp WHERE CardCombination = @CardCombination";
                        rmsqlcon.Open();
                        SqlCommand rmsqlcmd = new SqlCommand(rmcmd, rmsqlcon);
                        rmsqlcmd.Parameters.AddWithValue("@CardCombination", value);
                        SqlDataReader mreader = rmsqlcmd.ExecuteReader();
                        if (mreader.Read())
                        {
                            HTrend = Convert.ToInt32(mreader["Htrend"]);
                            DTrend = Convert.ToInt32(mreader["Dtrend"]);
                            STrend = Convert.ToInt32(mreader["Strend"]);
                            RTrend = Convert.ToInt32(mreader["Rtrend"]);
                            mreader.Close();
                            ifmainexpexist = 1;
                        }
                    }
                    break;
                case 1://读取副经验库
                    string rvcon = "Server =.; DataBase = BlackJack; Trusted_Connection = SSPI";
                    using (SqlConnection rvsqlcon = new SqlConnection(rvcon))
                    {
                        string rvcmd = "SELECT HTrend,STrend FROM ViceExp WHERE Sum = @Sum";
                        rvsqlcon.Open();
                        SqlCommand rvsqlcmd = new SqlCommand(rvcmd, rvsqlcon);
                        rvsqlcmd.Parameters.AddWithValue("@Sum", value);
                        SqlDataReader vreader = rvsqlcmd.ExecuteReader();
                        if (vreader.Read())
                        {
                            HTrend1 = Convert.ToInt32(vreader["Htrend"]);
                            STrend1 = Convert.ToInt32(vreader["Strend"]);
                            vreader.Close();
                            ifviceexpexist = 3;
                        }
                    }
                    break;
            }
        }
        public void EditTendencyToSql(int mode,int value)
        {
            switch (mode)
            {
                case 3:
                    string emcon = "Server =.; DataBase = BlackJack; Trusted_Connection = SSPI";
                    using (SqlConnection emsqlcon = new SqlConnection(emcon))
                    {
                        emsqlcon.Open();
                        string emcmd = "IF EXISTS(SELECT * FROM MainExp WHERE CardCombination = @CardCombination) BEGIN UPDATE MainExp SET HTrend = @HTrend,STrend = @STrend,DTrend = @DTrend,RTrend = @RTrend WHERE CardCombination = @CardCombination END ELSE BEGIN INSERT INTO MainExp(CardCombination,HTrend,STrend,DTrend,RTrend) VALUES(@CardCombination,@HTrend,@STrend,@DTrend,@RTrend) END";
                        SqlCommand emsqlcmd = new SqlCommand(emcmd, emsqlcon);
                        emsqlcmd.Parameters.AddWithValue("@CardCombination", value);
                        emsqlcmd.Parameters.AddWithValue("@HTrend", HTrend);
                        emsqlcmd.Parameters.AddWithValue("@STrend", STrend);
                        emsqlcmd.Parameters.AddWithValue("@DTrend", DTrend);
                        emsqlcmd.Parameters.AddWithValue("@RTrend", RTrend);
                        emsqlcmd.ExecuteNonQuery();
                    }
                    break;

                case 4:
                    string evcon = "Server =.; DataBase = BlackJack; Trusted_Connection = SSPI";
                    using (SqlConnection evsqlcon = new SqlConnection(evcon))
                    {
                        evsqlcon.Open();
                        string evcmd = "IF EXISTS(SELECT * FROM ViceExp WHERE Sum = @Sum) BEGIN UPDATE ViceExp SET HTrend = @HTrend,STrend = @STrend WHERE Sum = @Sum END ELSE BEGIN INSERT INTO ViceExp(Sum,HTrend,STrend) VALUES(@Sum,@HTrend,@STrend) END";
                        SqlCommand evsqlcmd = new SqlCommand(evcmd, evsqlcon);
                        evsqlcmd.Parameters.AddWithValue("@Sum", value);
                        evsqlcmd.Parameters.AddWithValue("@HTrend", HTrend1);
                        evsqlcmd.Parameters.AddWithValue("@STrend", STrend1);
                        evsqlcmd.ExecuteNonQuery();
                    }
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
                int tmp = 0;
                Console.WriteLine("Computer Round:");
                Casino.Deliver(playerhand, j, I);//1st Card 发第一张
                I++;
                j++;
                Casino.Deliver(playerhand, j, I);//2nd Card 发第二张
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
                    ReadTendencyFromSql(0,CardCombination );//模式0==读取主经验库
                    knock = RandomMainTrend(htrend, dtrend, strend, rtrend);//根据倾向，计算机进行随机判断
                    Console.WriteLine("The computer selects {0}", knock);
                    //QandA();//预留的人工应答界面
                    //Console.ReadKey();
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
                                    EditTendencyToSql(4,sum);
                                    playerhand[j - 1] = tmp;
                                    sum = GetSum(playerhand);
                                }
                                ifviceexpexist = 2;
                                ReadTendencyFromSql(1,sum);
                                knock = RandomViceTrend(htrend1, strend1);
                                //p.QandA();
                                Console.WriteLine("The Computer selects {0}", knock);
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
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
                }
                else if (ifviceexpexist == 0)
                {
                    lose += 1;
                    flag++;
                    htrend = Abs(htrend - 12);
                    strend = Abs(strend + 8);
                    rtrend = Abs(rtrend + 4);
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
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
                    if (htrend < 22) htrend += 21;
                    if (dtrend < 22) dtrend += 21;
                    if (strend < 22) strend += 21;
                    if (rtrend < 22) rtrend += 21;
                    if (htrend1 < 22) htrend1 += 21;
                    if (strend1 < 22) strend1 += 21;
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
                if (htrend < 22) htrend += 21;
                if (dtrend < 22) dtrend += 21;
                if (strend < 22) strend += 21;
                if (rtrend < 22) rtrend += 21;
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
            Console.WriteLine("Win rate:H({0}%),D({1}%),S({2}%),R({3}%）", Math.Round(100 * p1), Math.Round(100 * p2), Math.Round(100 * p3), Math.Round(100 * p4));//实时显示当前倾向
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
            Console.WriteLine("Current Win rate:H({0}%),S({1}%)", Math.Round(100 * p1), Math.Round(100 * p2));
            if ((p >= 0) && (p < p1))
            {
                return "h";
            }
            else
            {
                return "s";
            }
        }
        public string TestTendency(int i)
        {
            if (i < 17) return "h";
            else return "s";
        }
    }
}
