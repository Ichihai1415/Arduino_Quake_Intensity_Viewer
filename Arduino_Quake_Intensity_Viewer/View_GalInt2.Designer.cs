namespace Arduino_Quake_Intensity_Viewer
{
    partial class View_GalInt2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextInt = new System.Windows.Forms.Label();
            this.Text_gal = new System.Windows.Forms.Label();
            this.PGAPGV = new System.Windows.Forms.Label();
            this.Back = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TextInt
            // 
            this.TextInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.TextInt.Location = new System.Drawing.Point(10, 10);
            this.TextInt.Name = "TextInt";
            this.TextInt.Size = new System.Drawing.Size(300, 50);
            this.TextInt.TabIndex = 0;
            this.TextInt.Text = "PGA: 0000";
            this.TextInt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Text_gal
            // 
            this.Text_gal.AutoSize = true;
            this.Text_gal.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.Text_gal.Location = new System.Drawing.Point(261, 28);
            this.Text_gal.Name = "Text_gal";
            this.Text_gal.Size = new System.Drawing.Size(46, 29);
            this.Text_gal.TabIndex = 1;
            this.Text_gal.Text = "gal\r\n";
            // 
            // PGAPGV
            // 
            this.PGAPGV.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.PGAPGV.Location = new System.Drawing.Point(120, 10);
            this.PGAPGV.Name = "PGAPGV";
            this.PGAPGV.Size = new System.Drawing.Size(120, 50);
            this.PGAPGV.TabIndex = 3;
            this.PGAPGV.Text = "00.00";
            this.PGAPGV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Back
            // 
            this.Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            this.Back.Location = new System.Drawing.Point(0, 0);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(320, 180);
            this.Back.TabIndex = 4;
            // 
            // View_GalInt2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.ClientSize = new System.Drawing.Size(320, 70);
            this.Controls.Add(this.Text_gal);
            this.Controls.Add(this.PGAPGV);
            this.Controls.Add(this.TextInt);
            this.Controls.Add(this.Back);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "View_GalInt2";
            this.Text = "AQIV - データ表示画面2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.View_GalInt2_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TextInt;
        private System.Windows.Forms.Label Text_gal;
        private System.Windows.Forms.Label PGAPGV;
        private System.Windows.Forms.Label Back;
    }
}