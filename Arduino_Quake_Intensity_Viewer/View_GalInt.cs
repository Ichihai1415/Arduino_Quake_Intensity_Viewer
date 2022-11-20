using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Quake_Intensity_Viewer
{
    public  partial class View_GalInt : Form
    {
        public View_GalInt()
        {
            InitializeComponent();
        }

        private void View_Tick(object sender, EventArgs e)
        {
            MainText.Text = $"{string.Format("{0:0000.00}", GalNow[0])}gal\n{string.Format("{0:0000.00}", GalNow[1])}gal\n{string.Format("{0:0000.00}", GalNow[0])}gal\n合成加速度:{string.Format("{0:0000.00}", GalNow[3])}gal\n合成加速度最大:{string.Format("{0:0000.00}", GalMax)}gal\n震度:{string.Format("{0:0.00}", IntNow)}".Replace("震度:0.00", "震度:---");
            if (GalNow[0] == 0.001)
                MainText.Text = $"加速度X:0000.00gal\n加速度Y:0000.00gal\n加速度Z:0000.00gal\n合成加速度:0000.00gal\n合成加速度最大:0000.00gal\n震度:0.00";
        }
        public double[] GalNow = { 0.0, 0.0, 0.0, 0.0 };
        public double GalMax = 0;
        public double IntNow = 0.001;

    }
}
