namespace UHF_EPD_Demo_New
{
    partial class Form2
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Dis_Sensitivity = new System.Windows.Forms.Label();
            this.Tbox_Sensitivity = new System.Windows.Forms.TextBox();
            this.Sbar_Sensitivity = new System.Windows.Forms.HScrollBar();
            this.Dis_Power = new System.Windows.Forms.Label();
            this.Tbox_Power = new System.Windows.Forms.TextBox();
            this.Sbar_Power = new System.Windows.Forms.HScrollBar();
            this.Dis_Name = new System.Windows.Forms.Label();
            this.Tbox_Name = new System.Windows.Forms.TextBox();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Dis_Setting = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Dis_Setting);
            this.panel1.Controls.Add(this.Btn_Cancel);
            this.panel1.Controls.Add(this.Btn_Save);
            this.panel1.Controls.Add(this.Tbox_Name);
            this.panel1.Controls.Add(this.Dis_Name);
            this.panel1.Controls.Add(this.Dis_Sensitivity);
            this.panel1.Controls.Add(this.Tbox_Sensitivity);
            this.panel1.Controls.Add(this.Sbar_Sensitivity);
            this.panel1.Controls.Add(this.Dis_Power);
            this.panel1.Controls.Add(this.Tbox_Power);
            this.panel1.Controls.Add(this.Sbar_Power);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 290);
            this.panel1.TabIndex = 0;
            // 
            // Dis_Sensitivity
            // 
            this.Dis_Sensitivity.AutoSize = true;
            this.Dis_Sensitivity.Location = new System.Drawing.Point(59, 178);
            this.Dis_Sensitivity.Name = "Dis_Sensitivity";
            this.Dis_Sensitivity.Size = new System.Drawing.Size(76, 13);
            this.Dis_Sensitivity.TabIndex = 32;
            this.Dis_Sensitivity.Text = "Rx Sensitivity :";
            // 
            // Tbox_Sensitivity
            // 
            this.Tbox_Sensitivity.Location = new System.Drawing.Point(151, 175);
            this.Tbox_Sensitivity.MaxLength = 3;
            this.Tbox_Sensitivity.Name = "Tbox_Sensitivity";
            this.Tbox_Sensitivity.Size = new System.Drawing.Size(49, 20);
            this.Tbox_Sensitivity.TabIndex = 31;
            this.Tbox_Sensitivity.Text = "-60";
            // 
            // Sbar_Sensitivity
            // 
            this.Sbar_Sensitivity.Location = new System.Drawing.Point(62, 198);
            this.Sbar_Sensitivity.Maximum = 69;
            this.Sbar_Sensitivity.Name = "Sbar_Sensitivity";
            this.Sbar_Sensitivity.Size = new System.Drawing.Size(138, 20);
            this.Sbar_Sensitivity.TabIndex = 30;
            this.Sbar_Sensitivity.Tag = "";
            this.Sbar_Sensitivity.Value = 30;
            // 
            // Dis_Power
            // 
            this.Dis_Power.AutoSize = true;
            this.Dis_Power.Location = new System.Drawing.Point(59, 116);
            this.Dis_Power.Name = "Dis_Power";
            this.Dis_Power.Size = new System.Drawing.Size(73, 13);
            this.Dis_Power.TabIndex = 29;
            this.Dis_Power.Text = "Power (dBm) :";
            // 
            // Tbox_Power
            // 
            this.Tbox_Power.Location = new System.Drawing.Point(151, 113);
            this.Tbox_Power.MaxLength = 5;
            this.Tbox_Power.Name = "Tbox_Power";
            this.Tbox_Power.Size = new System.Drawing.Size(49, 20);
            this.Tbox_Power.TabIndex = 28;
            this.Tbox_Power.Text = "15";
            // 
            // Sbar_Power
            // 
            this.Sbar_Power.Location = new System.Drawing.Point(62, 136);
            this.Sbar_Power.Maximum = 89;
            this.Sbar_Power.Name = "Sbar_Power";
            this.Sbar_Power.Size = new System.Drawing.Size(138, 20);
            this.Sbar_Power.TabIndex = 27;
            this.Sbar_Power.Tag = "";
            this.Sbar_Power.Value = 20;
            // 
            // Dis_Name
            // 
            this.Dis_Name.AutoSize = true;
            this.Dis_Name.Location = new System.Drawing.Point(59, 54);
            this.Dis_Name.Name = "Dis_Name";
            this.Dis_Name.Size = new System.Drawing.Size(79, 13);
            this.Dis_Name.TabIndex = 33;
            this.Dis_Name.Text = "Reader Name :";
            // 
            // Tbox_Name
            // 
            this.Tbox_Name.Location = new System.Drawing.Point(62, 74);
            this.Tbox_Name.MaxLength = 20;
            this.Tbox_Name.Name = "Tbox_Name";
            this.Tbox_Name.Size = new System.Drawing.Size(138, 20);
            this.Tbox_Name.TabIndex = 34;
            this.Tbox_Name.Text = "impinj-14-b7-e2";
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(12, 243);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(92, 36);
            this.Btn_Save.TabIndex = 14;
            this.Btn_Save.Text = "Save";
            this.Btn_Save.UseVisualStyleBackColor = true;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(151, 243);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(92, 36);
            this.Btn_Cancel.TabIndex = 35;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Dis_Setting
            // 
            this.Dis_Setting.AutoSize = true;
            this.Dis_Setting.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Dis_Setting.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dis_Setting.Location = new System.Drawing.Point(50, 0);
            this.Dis_Setting.Name = "Dis_Setting";
            this.Dis_Setting.Size = new System.Drawing.Size(157, 26);
            this.Dis_Setting.TabIndex = 36;
            this.Dis_Setting.Text = "Reader Setting";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 312);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Dis_Sensitivity;
        private System.Windows.Forms.TextBox Tbox_Sensitivity;
        private System.Windows.Forms.HScrollBar Sbar_Sensitivity;
        private System.Windows.Forms.Label Dis_Power;
        private System.Windows.Forms.TextBox Tbox_Power;
        private System.Windows.Forms.HScrollBar Sbar_Power;
        private System.Windows.Forms.Label Dis_Name;
        private System.Windows.Forms.TextBox Tbox_Name;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Label Dis_Setting;
    }
}