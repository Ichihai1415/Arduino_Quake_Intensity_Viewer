namespace Arduino_Quake_Intensity_Viewer
{
    partial class View_GalInt
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
            this.components = new System.ComponentModel.Container();
            this.View = new System.Windows.Forms.Timer(this.components);
            this.MainText = new System.Windows.Forms.Label();
            this.Text2 = new System.Windows.Forms.Label();
            this.Back = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // View
            // 
            this.View.Enabled = true;
            this.View.Interval = 10;
            this.View.Tick += new System.EventHandler(this.View_Tick);
            // 
            // MainText
            // 
            this.MainText.BackColor = System.Drawing.Color.Transparent;
            this.MainText.ForeColor = System.Drawing.Color.White;
            this.MainText.Location = new System.Drawing.Point(6, 6);
            this.MainText.Name = "MainText";
            this.MainText.Size = new System.Drawing.Size(348, 228);
            this.MainText.TabIndex = 0;
            this.MainText.Text = "0000.00gal\r\n0000.00gal\r\n0000.00gal\r\n合成加速度:0000.00gal\r\n合成加速度最大:0000.00gal\r\n震度:0.00" +
    "";
            this.MainText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Text2
            // 
            this.Text2.AutoSize = true;
            this.Text2.BackColor = System.Drawing.Color.Transparent;
            this.Text2.ForeColor = System.Drawing.Color.White;
            this.Text2.Location = new System.Drawing.Point(60, 6);
            this.Text2.Name = "Text2";
            this.Text2.Size = new System.Drawing.Size(121, 114);
            this.Text2.TabIndex = 1;
            this.Text2.Text = "加速度X:\r\n加速度Y:\r\n加速度Z:";
            this.Text2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Back
            // 
            this.Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(50)))));
            this.Back.Location = new System.Drawing.Point(0, 0);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(360, 240);
            this.Back.TabIndex = 2;
            // 
            // View_GalInt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 38F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.ClientSize = new System.Drawing.Size(360, 240);
            this.Controls.Add(this.Text2);
            this.Controls.Add(this.MainText);
            this.Controls.Add(this.Back);
            this.Font = new System.Drawing.Font("Koruri Regular", 20F);
            this.Margin = new System.Windows.Forms.Padding(6, 9, 6, 9);
            this.Name = "View_GalInt";
            this.Text = "AQIV - データ表示画面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.View_GalInt_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer View;
        private System.Windows.Forms.Label MainText;
        private System.Windows.Forms.Label Text2;
        private System.Windows.Forms.Label Back;
    }
}