﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arduino_Quake_Intensity_Viewer
{
    public partial class View_GalInt2 : Form
    {
        public View_GalInt2()
        {
            InitializeComponent();
        }

        private void Change_Tick(object sender, EventArgs e)
        {
            PGAPGV.Text = $"{PGA}\n{PGV}";
            TextInt.Text = $"PGA:\nPGV:\n推定震度: {Int}";
            if (PGA < 15)
                Back.BackColor = Color.FromArgb(25, 25, 50);
            else if (PGA < 30)
                Back.BackColor = Color.Green;
            else if (PGA < 50)
                Back.BackColor = Color.Yellow;
            else if (PGA < 100)
                Back.BackColor = Color.Orange;
            else if (PGA < 500)
                Back.BackColor = Color.Red;
            else
                Back.BackColor = Color.FromArgb(250, 0, 250);
        }
        public double PGA = 0;
        public double PGV = 0;
        public double Int = 0;

        private void View_GalInt2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
