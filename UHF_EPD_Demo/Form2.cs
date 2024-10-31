using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UHF_EPD_Demo_New
{
    public partial class Form2 : Form
    {

        public string host_name = "impinj-14-b7-e2";
        public float Reader_Power = 15;
        public Int16 Reader_Sensitivity = -60;


        public Form2()
        {
            InitializeComponent();
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            // Get Reader Settings
            host_name = Tbox_Name.Text;
            Reader_Power = Convert.ToSingle(Tbox_Power.Text);
            Reader_Sensitivity = Convert.ToInt16(Tbox_Sensitivity.Text);

            this.Hide();
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
}
