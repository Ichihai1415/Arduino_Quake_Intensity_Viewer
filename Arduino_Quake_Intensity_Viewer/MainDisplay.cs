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
        /// <summary>
        /// galを気象庁震度階級に変換します。
        /// </summary>
        /// <param name="Gals">1秒での加速度各方向全て。</param>
        /// <returns>計算された気象庁震度階級</returns>
        public double GalToJMAInt(List<List<double>> Gals)
        {
            //try
            {
                if (Gals.Count == 0)
                    return 0.001;
                Console.WriteLine("フーリエ変換開始");
                Task<Complex[]> TX1 = Task.Run(() => { return FourierTransform(Gals[0]); });
                Task<Complex[]> TY1 = Task.Run(() => { return FourierTransform(Gals[1]); });
                Task<Complex[]> TZ1 = Task.Run(() => { return FourierTransform(Gals[2]); });
                Console.WriteLine("フィルター開始");
                Task<Complex[]> TX2 = Task.Run(() => { return Filter(TX1.Result); });
                Task<Complex[]> TY2 = Task.Run(() => { return Filter(TY1.Result); });
                Task<Complex[]> TZ2 = Task.Run(() => { return Filter(TZ1.Result); });
                Console.WriteLine("逆フーリエ変換開始");
                Task<Complex[]> TX3 = Task.Run(() => { return FourierTransform(Gals[0], -1.0); });
                Task<Complex[]> TY3 = Task.Run(() => { return FourierTransform(Gals[1], -1.0); });
                Task<Complex[]> TZ3 = Task.Run(() => { return FourierTransform(Gals[2], -1.0); });
                Console.WriteLine("合成変換開始");
                List<double> GalInts = new List<double>();
                for (int i = 0; i < Gals[0].Count; i++)
                {
                    double XMag = TX3.Result[i].Magnitude;
                    double YMag = TY3.Result[i].Magnitude;
                    double ZMag = TZ3.Result[i].Magnitude;
                    GalInts.Add(Math.Sqrt(XMag * XMag + YMag * YMag + ZMag * ZMag));
                }
                GalInts.Sort();
                GalInts.Reverse();
                int j = 1;
                double Seconds = 0;
                int Count = GalInts.Count;
                for (j = 1; Seconds < 0.3; j++)//0.3秒以上になるときのj 計算の都合上j=1
                {
                    Seconds += j / Count;//(1÷個数)x今のループ回数
                }
                return Math.Round((2 * Math.Log(GalInts[j - 1], 10)) + 0.96, 2, MidpointRounding.AwayFromZero);//ループ終了時点(0.3s以上)でj番目なので-1
            }
            //catch
            {
                return 0.001;
            }
        }
        /// <summary>
        /// フーリエ変換した結果を返します。
        /// </summary>
        /// <remarks>加速度配列を一種類のみ変換します。</remarks>
        /// <param name="Gals">加速度(cm/s/s)の単配列</param>
        /// <param name="ID">1.0で通常、-1.0で逆フーリエ変換</param>
        /// <returns>フーリエ変換された複素数</returns>
        public Complex[] FourierTransform(List<double> Gals, double ID = 1.0)
        {
            //try
            {
                List<Complex> GalList = new List<Complex>();
                for (int j = 0; j < Gals.Count; j++)
                    GalList.Add(new Complex(Gals[j], 0));
                Complex[] CmpBefore = GalList.ToArray();
                Complex[] CmpAfter;
                int n = CmpBefore.Length;
                CmpAfter = new Complex[n];
                int i, ThIndex;
                Complex CmpSum;
                Complex[] CmpTh = new Complex[n];
                double RadTmp = -ID * 2.0 * Math.PI / n;
                for (i = 0; i < n; i++)
                    CmpTh[i] = Complex.FromPolarCoordinates(1.0, RadTmp * i);
                for (int t = 0; t < n; t++)
                {
                    CmpSum = Complex.Zero;
                    for (i = 0; i < n; i++)
                    {
                        ThIndex = (i * t) % n;
                        CmpSum += CmpBefore[i] * CmpTh[ThIndex];
                    }
                    CmpAfter[t] = CmpSum;
                    if (ID < 0)
                        CmpAfter[t] /= (double)n;
                }
                return CmpAfter;
            }
            //catch
            {
                return new Complex[1];
            }
        }
        /// <summary>
        /// 3種類のフィルターをかけます。
        /// </summary>
        /// <remarks>フーリエ変換された複素数を一種類のみ変換します。</remarks>
        /// <param name="Data">フーリエ変換された複素数</param>
        /// <param name="Second">計測した秒数(基本いらない)</param>
        /// <returns>フィルターをかけた後のフーリエ変換された複素数</returns>
        public Complex[] Filter(Complex[] Data, int Second = 1)
        {
            //try
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    double Hz = i / Second;//?
                    double y = Hz * 0.1;
                    //ローカットフィルター
                    Data[i] *= Math.Pow(1 - Math.Exp(-1 * Math.Pow(Hz / 0.5, 3)), 0.5);
                    //ハイカットフィルター
                    Data[i] *= Math.Pow(1 + 0.694 * Math.Pow(y, 2) + 0.241 * Math.Pow(y, 4) + 0.0557 * Math.Pow(y, 6) + 0.009664 * Math.Pow(y, 8) + 0.00134 * Math.Pow(y, 10) + 0.000155 * Math.Pow(y, 12), -0.5);
                    //周期効果フィルター
                    Data[i] *= Math.Pow(1 / Hz, 0.5);
                }
                return Data;
            }
            //catch
            {
                return new Complex[1];
            }
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
