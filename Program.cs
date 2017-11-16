using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace BlackJack
{
    //主程序入口
    public static class Program
    {   //主方法
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static void BJ()
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
                Console.WriteLine("Welcome to BlackJack，Round:{0}", round);
                Console.WriteLine("Computer Win rate={0}%", Computer.WinRate);
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
                if (com.Flag == 1)//电脑玩家和人类玩家同时有特殊状况？有则不进入庄家回合直接下一轮
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
                //if ((round >= 100) && ((100*p.Win/(p.Win+p.Lose)) >= com.WinRate))
                //{
                //   Console.WriteLine("红包密码666！");
                //}
                //Console.ReadKey();

                com.EditTendencyToSql(3, com.CardCombination);

                if (com.IfViceExp !=  0)
                {
                    com.EditTendencyToSql(4, com.Sum);
                }

                using (SqlConnection sqlcon = new SqlConnection("Server=.;DataBase=BlackJack;Trusted_Connection=SSPI"))
                {
                    sqlcon.Open();
                    string cmdstring = "UPDATE BJWinRate SET WinRate = @WinRate, Time = @Time WHERE ID = 1";
                    SqlCommand cmd = new SqlCommand(cmdstring, sqlcon);
                    cmd.Parameters.AddWithValue("@WinRate", Convert.ToString(Computer.WinRate));
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("**********************************************");
                round++;
                Console.Clear();
            } while (true);
        }
    }
}