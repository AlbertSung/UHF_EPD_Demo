namespace UHF_EPD_Demo_New
{
    partial class Form3
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
            this.Sbar_GrayScale = new System.Windows.Forms.HScrollBar();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Btn_Reset = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Dis_Image = new System.Windows.Forms.Label();
            this.Pbox_EPD = new System.Windows.Forms.PictureBox();
            this.Dis_Gray = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pbox_EPD)).BeginInit();
            this.SuspendLayout();
            // 
            // Sbar_GrayScale
            // 
            this.Sbar_GrayScale.Location = new System.Drawing.Point(117, 262);
            this.Sbar_GrayScale.Maximum = 264;
            this.Sbar_GrayScale.Name = "Sbar_GrayScale";
            this.Sbar_GrayScale.Size = new System.Drawing.Size(231, 20);
            this.Sbar_GrayScale.TabIndex = 31;
            this.Sbar_GrayScale.Tag = "";
            this.Sbar_GrayScale.Value = 127;
            this.Sbar_GrayScale.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Sbar_GrayScale_Scroll);
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(256, 293);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(92, 36);
            this.Btn_Save.TabIndex = 37;
            this.Btn_Save.Text = "Save";
            this.Btn_Save.UseVisualStyleBackColor = true;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // Btn_Reset
            // 
            this.Btn_Reset.Location = new System.Drawing.Point(117, 293);
            this.Btn_Reset.Name = "Btn_Reset";
            this.Btn_Reset.Size = new System.Drawing.Size(92, 36);
            this.Btn_Reset.TabIndex = 36;
            this.Btn_Reset.Text = "Reset";
            this.Btn_Reset.UseVisualStyleBackColor = true;
            this.Btn_Reset.Click += new System.EventHandler(this.Btn_Reset_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Dis_Image);
            this.panel1.Controls.Add(this.Pbox_EPD);
            this.panel1.Controls.Add(this.Dis_Gray);
            this.panel1.Controls.Add(this.Btn_Save);
            this.panel1.Controls.Add(this.Btn_Reset);
            this.panel1.Controls.Add(this.Sbar_GrayScale);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 340);
            this.panel1.TabIndex = 38;
            // 
            // Dis_Image
            // 
            this.Dis_Image.AutoSize = true;
            this.Dis_Image.Location = new System.Drawing.Point(7, 19);
            this.Dis_Image.Name = "Dis_Image";
            this.Dis_Image.Size = new System.Drawing.Size(67, 13);
            this.Dis_Image.TabIndex = 40;
            this.Dis_Image.Text = "Gray Image :";
            // 
            // Pbox_EPD
            // 
            this.Pbox_EPD.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Pbox_EPD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pbox_EPD.Location = new System.Drawing.Point(10, 35);
            this.Pbox_EPD.Name = "Pbox_EPD";
            this.Pbox_EPD.Size = new System.Drawing.Size(444, 192);
            this.Pbox_EPD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pbox_EPD.TabIndex = 39;
            this.Pbox_EPD.TabStop = false;
            this.Pbox_EPD.Visible = false;
            // 
            // Dis_Gray
            // 
            this.Dis_Gray.AutoSize = true;
            this.Dis_Gray.Location = new System.Drawing.Point(114, 242);
            this.Dis_Gray.Name = "Dis_Gray";
            this.Dis_Gray.Size = new System.Drawing.Size(65, 13);
            this.Dis_Gray.TabIndex = 38;
            this.Dis_Gray.Text = "Gray Scale :";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 362);
            this.Controls.Add(this.panel1);
            this.Name = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pbox_EPD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar Sbar_GrayScale;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.Button Btn_Reset;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Dis_Gray;
        private System.Windows.Forms.PictureBox Pbox_EPD;
        private System.Windows.Forms.Label Dis_Image;
    }
}