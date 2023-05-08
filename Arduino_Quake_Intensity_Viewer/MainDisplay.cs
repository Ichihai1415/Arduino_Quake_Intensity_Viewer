using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Numerics;
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
        private void Form1_Load(object sender, EventArgs e)
        {
            SerialPort.Open();
            GalInt.Show();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string SerialData = SerialPort.ReadLine();
                Task.Run(() => { Main1(SerialData); });
                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffff -> ") + SerialData);
            }
            catch
            {

            }

        }
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
        }
        public string Data = "";
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
            SerialPort.Write("RC");
            SerialPort.Close();
            SerialPort.Dispose();
            Thread.Sleep(5000);
            SerialPort.Open();
        }
 
        public List<List<double>> Gals1 = new List<List<double>>();
        public List<List<double>> Gals2 = new List<List<double>>();
        public View_GalInt GalInt = new View_GalInt();
        public int ConvertedTime = 0;
        public double Max = 0;
        private void ReConnect_Click(object sender, EventArgs e)
        {
            ReConnectSend();
        }
    }
}
