using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;   // for ImageFormat

namespace UHF_EPD_Demo_New
{
    public partial class Form3 : Form
    {
        private string load_path;
        public int gray_filter = 127;


        public Form3()
        {
            InitializeComponent();
        }

        private Form1 mainForm = null;
        public Form3(Form callingForm)
        {
            mainForm = callingForm as Form1;
            load_path = mainForm.updateGray;
            InitializeComponent();
        }

        void doGray()
        {
            // Step 1: 利用 Bitmap 將 image 包起來
            Bitmap bimage = new Bitmap(Pbox_EPD.Image);
            int Height = bimage.Height;
            int Width = bimage.Width;
//          int[,,] rgbData = new int[Width, Height, 3];

            // Step 2: 取得像點顏色資訊
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color org_color = bimage.GetPixel(x, y);

//                  rgbData[x, y, 0] = color.R;
//                  rgbData[x, y, 1] = color.G;
//                  rgbData[x, y, 2] = color.B;

                    int grayScale = (int) ((org_color.R * 0.3) + (org_color.G * 0.59) + (org_color.B * 0.11));

                    if (grayScale > gray_filter)
                        grayScale = 255;
                    else
                        grayScale = 0;

                    Color new_color = Color.FromArgb(org_color.A, grayScale, grayScale, grayScale);

                    bimage.SetPixel(x, y, new_color);
                }
            }

            Pbox_EPD.Image = bimage;
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            if (load_path != null)
            {
                Pbox_EPD.Image = Image.FromFile(load_path);
//              Pbox_EPD.Image = Image.FromFile(mainForm.updateGray);
                doGray();

                Pbox_EPD.Visible = true;
            }
        }

        private void Sbar_GrayScale_Scroll(object sender, ScrollEventArgs e)
        {
            Pbox_EPD.Visible = false;

            Pbox_EPD.Image = Image.FromFile(load_path);
            gray_filter = Sbar_GrayScale.Value;
            doGray();

            Pbox_EPD.Visible = true;
        }

        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            Pbox_EPD.Visible = false;

            Pbox_EPD.Image = Image.FromFile(load_path);
            Sbar_GrayScale.Value = 127;
            gray_filter = 127;
            doGray();

            Pbox_EPD.Visible = true;
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            /*
            int bmp_width = Pbox_EPD.Size.Width;
            int bmp_height = Pbox_EPD.Size.Height;

            Bitmap bmp = new Bitmap(bmp_width, bmp_height);

            Pbox_EPD.DrawToBitmap(bmp, new Rectangle(0, 0, bmp_width, bmp_height));

            try
            {
                bmp.Save(@"C:\_tmp\tmpBitmap.bmp", ImageFormat.Bmp);

                mainForm.updateGray = @"C:\_tmp\tmpBitmap.bmp";
            }
            catch
            {
                Bitmap bmp_1 = new Bitmap(bmp_width, bmp_height);

                Pbox_EPD.DrawToBitmap(bmp_1, new Rectangle(0, 0, bmp_width, bmp_height));

                bmp.Dispose();
                bmp_1.Save(@"C:\_tmp\tmpBitmap_1.bmp", ImageFormat.Bmp);
                bmp_1.Dispose();

                mainForm.updateGray = @"C:\_tmp\tmpBitmap_1.bmp";
            }
            */

            mainForm.updateGray = Convert.ToString(gray_filter);

            this.Hide();
            this.Close();
        }

    }
}
