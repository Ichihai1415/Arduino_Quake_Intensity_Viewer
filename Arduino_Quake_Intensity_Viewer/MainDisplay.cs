using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Quake_Intensity_Viewer
{
    public partial class MainDisplay : Form
    {
        public MainDisplay()
        {
            InitializeComponent();
        }
        public const string saveDir = "D:\\Logs\\acc-1";

        private void Form1_Load(object sender, EventArgs e)
        {
            /*//フォルダ作成用
            for (int M = 1; M <= 12; M++)
            {
                Directory.CreateDirectory($"{saveDir}\\{M}");
                for (int d = 1; d <= 31; d++)
                {
                    Directory.CreateDirectory($"{saveDir}\\{M}\\{d}");
                    for (int h = 0; h <= 23; h++)
                    {
                        Directory.CreateDirectory($"{saveDir}\\{M}\\{d}\\{h}");
                        for (int m = 0; m <= 59; m++)
                            Directory.CreateDirectory($"{saveDir}\\{M}\\{d}\\{h}\\{m}");
                    }
                }

            }*/

            SerialPort.Open();
            //GalInt.Show();
            GalInt2.Show();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string SerialData = SerialPort.ReadLine();

                if (!SerialData.Contains("----------"))
                {
                    Task.Run(() => { Main2(SerialData.Replace("\n", "")); });
                    //Task.Run(() => { Main1(SerialData); });
                }
                else
                    Console.WriteLine("受信:" + SerialData.Replace("\n", ""));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public string Data = "";
        /// <summary>
        /// x,y,z,aで送られてくるやつ
        /// </summary>
        /// <param name="SerialData"></param>
        public void Main1(string SerialData)
        {
            try
            {
                string[] GalsSt = SerialData.Split(',');
                if (GalsSt.Length != 4)
                    return;
                List<double> Gals = GalsSt.Select(double.Parse).ToList();

                GalInt.GalNow = Gals;
                if (Data == "")
                    Data = SerialData;
                else
                    Data += "\n" + SerialData;
                if (Max < Gals[3])
                    Max = Gals[3];
                if (DateTime.Now.Second % 2 == 1)//奇数秒
                {
                    Gals1.Add(Gals);
                    if (ConvertedTime != DateTime.Now.Second && Gals2.Count != 0)//奇数秒になって最初
                    {
                        Console.WriteLine("#2保存開始");
                        DateTime dt = DateTime.Now - TimeSpan.FromSeconds(1);
                        if (!Directory.Exists("Logs"))
                            Directory.CreateDirectory("Logs");
                        if (!Directory.Exists($"Logs\\{dt.Year}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}");
                        File.WriteAllText($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}\\{dt:yyyyMMddHHmmss}.txt", Data);
                        Data = "";
                        Console.WriteLine("#2保存終了計算開始 データ個数:" + Gals2.Count);
                        ConvertedTime = DateTime.Now.Second;
                        List<List<double>> SendGals = Gals2;
                        //Task<double> ToInt = Task.Run(() =>  { return GalToJMAInt(SendGals); });
                        GalInt.GalMax = Max;
                        Max = 0;
                        Gals2.Clear();
                        //GalInt.IntNow = ToInt.Result;
                    }
                }
                else
                {
                    Gals2.Add(Gals);
                    if (ConvertedTime != DateTime.Now.Second && Gals1.Count != 0)
                    {
                        if (DateTime.Now.Second == 0)
                            OutlierCheck(Gals1);
                        Console.WriteLine("#1保存開始");
                        DateTime dt = DateTime.Now - TimeSpan.FromSeconds(1);
                        if (!Directory.Exists("Logs"))
                            Directory.CreateDirectory("Logs");
                        if (!Directory.Exists($"Logs\\{dt.Year}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}");
                        if (!Directory.Exists($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}"))
                            Directory.CreateDirectory($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}");
                        File.WriteAllText($"Logs\\{dt.Year}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}\\{dt:yyyyMMddHHmmss}.txt", Data);
                        Data = "";
                        Console.WriteLine("#1保存終了計算開始 データ個数:" + Gals1.Count);
                        ConvertedTime = DateTime.Now.Second;
                        //Task<double> ToInt = Task.Run(() => { return GalToJMAInt(Gals1); });
                        GalInt.GalMax = Max;
                        Max = 0;
                        Gals1.Clear();
                        // GalInt.IntNow = ToInt.Result;
                    }
                }
            }
            catch (FormatException)
            {
                GalInt.GalNow = new List<double> { 0.001, 0, 0, 0 };
            }
            catch (IOException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// MaxGal*x,y,z,a/x,y,z,a/で送られてくるやつ
        /// </summary>
        /// <param name="SerialData"></param>
        public void Main2(string SerialData)
        {
            try
            {
                string[] datas = SerialData.Split('*');
                double PGA = double.Parse(datas[1]);
                /*
                double PGV = double.Parse(datas[2]);
                double Int;
                if (PGV < 7)
                    Int = 2.165 + 2.262 * Math.Log10(PGV);
                else
                    Int = 2.002 + 2.603 * Math.Log10(PGV) - 0.213 * Math.Pow(Math.Log10(PGV), 2);
                if (PGA >= 1000)
                    PGA = Math.Round(PGA, MidpointRounding.AwayFromZero);
                else if (PGA >= 100)
                    PGA = Math.Round(PGA, 1, MidpointRounding.AwayFromZero);
                if (PGV >= 1000)
                    PGV = Math.Round(PGV, MidpointRounding.AwayFromZero);
                else if (PGV >= 100)
                    PGV = Math.Round(PGV, 1, MidpointRounding.AwayFromZero);
                Int = Math.Round(Int, 2, MidpointRounding.AwayFromZero);*/
                GalInt2.PGA = PGA;
                DateTime dt = DateTime.Now - TimeSpan.FromSeconds(1);
                File.WriteAllText($"{saveDir}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}\\{dt:HHmmss}.txt", datas[0].Replace("/", "\n").Replace("\n\n", "\n"));
                File.WriteAllText($"{saveDir}\\{dt.Month}\\{dt.Day}\\{dt.Hour}\\{dt.Minute}\\_year.txt", dt.Year.ToString());
                GalInt2.Change();
            }/*
            catch (IOException)
            {

            }*/
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// 設置ずれ等の異常値を検知します。
        /// </summary>
        /// <remarks>差が15gal未満で最大0gal以下か最小0gal以上なら検知</remarks>
        /// <param name="Gals">1秒での加速度各方向全て。</param>
        public void OutlierCheck(List<List<double>> Gals)
        {
            List<double> GalsX = new List<double>();
            List<double> GalsY = new List<double>();
            List<double> GalsZ = new List<double>();
            foreach (List<double> Gals_ in Gals)
            {
                GalsX.Add(Gals_[0]);
                GalsY.Add(Gals_[1]);
                GalsZ.Add(Gals_[2]);
            }
            List<List<double>> Gals2 = new List<List<double>>
            {
                GalsX,
                GalsY,
                GalsZ
            };
            for (int i = 0; i < 3; i++)
            {
                double[] Gals_ = Gals2[i].ToArray();
                Console.WriteLine($"Max:{Gals_.Max()}");
                Console.WriteLine($"Min:{Gals_.Min()}");
                Console.WriteLine($"Max-Min={Gals_.Max() - Gals_.Min()}");
                if (Gals_.Max() - Gals_.Min() < 15 && (Gals_.Max() < 0 || Gals_.Min() > 0))
                {
                    ReConnectSend();
                    break;
                }
            }
        }
        /// <summary>
        /// データを送信して再接続します。
        /// </summary>
        /// <remarks>Arduino側で受信時リセット処理が必要です。</remarks>
        public void ReConnectSend()
        {
            Console.WriteLine($"再接続");
            SerialPort.WriteLine("RC");
            SerialPort.Close();
            SerialPort.Dispose();
            Thread.Sleep(5000);
            SerialPort.Open();
        }

        public List<List<double>> Gals1 = new List<List<double>>();
        public List<List<double>> Gals2 = new List<List<double>>();
        public View_GalInt GalInt = new View_GalInt();
        public View_GalInt2 GalInt2 = new View_GalInt2();
        public int ConvertedTime = 0;
        public double Max = 0;
        private void ReConnect_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void TimeCheck_Tick(object sender, EventArgs e)
        {
            Console.WriteLine($"時刻送信");
            TimeCheck.Enabled = false;
            SerialPort.WriteLine(DateTime.Now.ToString("yy,MM,dd,HH,mm,ss,1"));
            TimeCheck.Interval = (60 - DateTime.Now.Second) * 1000 - DateTime.Now.Millisecond;
            Console.WriteLine($"待機{TimeCheck.Interval / 1000d}s");
            TimeCheck.Enabled = true;
        }
    }
}
