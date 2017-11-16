using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; 
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
namespace BlackJack
{
    public partial class Form1 : Form
    {
        static ThreadStart bj = new ThreadStart(Program.BJ);
        Thread bjThread = new Thread(bj);
        public Form1()
        {
            InitializeComponent();
            InitChart();
        }

        System.Windows.Forms.Timer chartTimer = new System.Windows.Forms.Timer();

        private void InitChart()
        {
            DateTime time = DateTime.Now;
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;//Tick事件与方法关联
            chart1.DoubleClick += Chart1_DoubleClick;//将事件与方法关联

            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = false;

            timer1.Start();

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            bjThread.Start();
            MessageBox.Show(String.Format("启动成功！"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            bjThread.Abort();
            MessageBox.Show(String.Format("关闭成功！"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Chart1_DoubleClick(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            //throw new NotImplementedException();  
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Series series = chart1.Series[0];

            string constring = "Server=.;DataBase=BlackJack;Trusted_Connection=SSPI";
            using (SqlConnection sqlcon1 = new SqlConnection(constring))
            {
                sqlcon1.Open();
                string cmdstring1 = "SELECT Time,WinRate FROM BJWinRate WHERE ID = 1";
                SqlCommand cmd1 = new SqlCommand(cmdstring1, sqlcon1);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    string time = reader1["Time"].ToString();
                    string winrate = reader1["Winrate"].ToString();
                    reader1.Close();
                    series.Points.AddXY(time, winrate);//读取新的数据，绘制新的点
                }
                chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;//图表平移5个数据点
            }
        }
    }
}
