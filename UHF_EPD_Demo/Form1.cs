using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;   // for ImageFormat

using System.Timers;

using Impinj.OctaneSdk;
using GenCode128;



namespace UHF_EPD_Demo_New
{
    public partial class Form1 : Form
    {
        static Int16 gFormat_Mode = 0;
        static Int16 update_amount = 0;

        static string key_epc, key_tid;
        static bool scanTarget_start = false, searchTAG_start = false;

        static int TAG_Count = 0;
        static char[] header_EPC, header_TID;
        static string string_EPC, string_TID;

        byte[] bytes;

        // Create an instance of the ImpinjReader class.
        static ImpinjReader reader = new ImpinjReader();

        // A string to hold the tag data
        static string tagData;
        static uint tagValue;

        // Keep track of how many operations were added and 
        // how many have been executed, so we know when we're done.
//      static ushort numOpsAdded, numOpsExecuted, numWordsWritten;
        static ushort numWordsWritten;
        static ushort numReadAdded, numWriteAdded, numReadExecuted, numWriteExecuted;
        static ushort numReadSucceed, numWriteSucceed, numReadMaximum, numWriteMaximum;

        static int read_result = 0, write_result = 0;

        // Create a Dictionary to store the tags we've read.
        static Dictionary<string, Tag> tagsRead = new Dictionary<string, Tag>();

        // Create a timer for reading / writing tag timeout
        static System.Timers.Timer rdTimer, wrTimer;


        Form2 settingForm = new Form2();
        Form3 grayForm = new Form3();



        public Form1()
        {
            InitializeComponent();

            // Default format mode
            gFormat_Mode = 1;       // ID Card

            Btn_ID_Card.Enabled = false;
        }


        static void BulkRead(TagData accessPassword, MemoryBank bank, ushort wordPointer, ushort wordCount)
        {
            TagOpSequence seq;
            TagReadOp op;

            // Initialize variables
            tagData = "";
//          numOpsExecuted = 0;
//          numOpsAdded = 0;
            numReadAdded = 0;
            numReadSucceed = 0;
            numReadExecuted = 0;
            numReadMaximum = 100;

            // Each TagReadOp can only access up to 32 words.
            // So, we need to break this read up into multiple operations. 
            while (wordCount > 0)
            {
                // Define a new tag operation sequence.
                seq = new TagOpSequence();

                if (header_EPC != null)
                {
                    // Specify a target tag based on the EPC.
                    seq.TargetTag.MemoryBank = MemoryBank.Epc;
                    seq.TargetTag.BitPointer = BitPointers.Epc;
                    // Setting this to null will specify any tag.
                    // Replace this line with the one below it to target a particular tag.
                    seq.TargetTag.Data = string_EPC;//header_EPC.ToString();
//                  seq.TargetTag.Mask = Mask_ESL;
                }
                else // if (header_TID != null)
                {
                    // Specify a target tag based on the TID.
                    seq.TargetTag.MemoryBank = MemoryBank.Tid;
//                  seq.TargetTag.BitPointer = BitPointers.Epc;
                    // Setting this to null will specify any tag.
                    // Replace this line with the one below it to target a particular tag.
                    seq.TargetTag.Data = string_TID;//header_TID.ToString();
//                  seq.TargetTag.Mask = Mask_ESL;
                }

                // Define a tag read operation
                op = new TagReadOp();
                op.AccessPassword = accessPassword;
                op.MemoryBank = bank;
                op.WordPointer = wordPointer;
                op.WordCount = (wordCount < 32) ? wordCount : (ushort)32;

                // Add the read op to the operation sequence
                seq.Ops.Add(op);

                // Adjust the word count and pointer for the next reader operation
                wordCount -= op.WordCount;
                wordPointer += op.WordCount;

                // Add the operation sequence to the reader
                reader.AddOpSequence(seq);
//              numOpsAdded++;
                numReadAdded++;
            }

            numReadMaximum = numReadAdded;
        }

        static void BulkWrite(TagData accessPassword, MemoryBank bank, ushort wordPointer, TagData data)
        {
            TagOpSequence seq;
            TagWriteOp op;

            // How many words are we going to write?
            ushort wordCount = (ushort)(data.CountBytes / 2);
            ushort wordIndex = 0;

            // Initialize variables
//          numOpsExecuted = 0;
//          numOpsAdded = 0;
            numWriteAdded = 0;
            numWordsWritten = 0;
            numWriteSucceed = 0;
            numWriteExecuted = 0;
            numWriteMaximum = 100;

            // Each TagWriteOp can only access up to 32 words.
            // So, we need to break this write up into multiple operations. 
            while (wordCount > 0)
            {
                // Define a new tag operation sequence.
                seq = new TagOpSequence();

                if (header_EPC != null)
                {
                    // Specify a target tag based on the EPC.
                    seq.TargetTag.MemoryBank = MemoryBank.Epc;
                    seq.TargetTag.BitPointer = BitPointers.Epc;
                    // Setting this to null will specify any tag.
                    // Replace this line with the one below it to target a particular tag.
                    seq.TargetTag.Data = string_EPC;//header_EPC.ToString();
//                  seq.TargetTag.Mask = Mask_ESL;
                }
                else // if (header_TID != null)
                {
                    // Specify a target tag based on the TID.
                    seq.TargetTag.MemoryBank = MemoryBank.Tid;
//                  seq.TargetTag.BitPointer = BitPointers.Epc;
                    // Setting this to null will specify any tag.
                    // Replace this line with the one below it to target a particular tag.
                    seq.TargetTag.Data = string_TID;//header_TID.ToString();
//                  seq.TargetTag.Mask = Mask_ESL;
                }

                // If you are using Monza 4, Monza 5 or Monza X tag chips,
                // uncomment these two lines. This enables 32-bit block writes
                // which significantly improves write performance.
                seq.BlockWriteEnabled = true;
                seq.BlockWriteWordCount = 2;

                // Define a tag read operation
                op = new TagWriteOp();
                op.AccessPassword = accessPassword;
                op.MemoryBank = bank;
                op.WordPointer = wordPointer;
                ushort opSizeWords = (wordCount < 32) ? wordCount : (ushort)32;
//              op.Data = TagData.FromWordList(data.ToList().GetRange(wordPointer, opSizeWords));
                op.Data = TagData.FromWordList(data.ToList().GetRange(wordIndex, opSizeWords));

                // Add the write op to the operation sequence
                seq.Ops.Add(op);

                // Adjust the word count and pointer for the next reader operation
                wordCount -= opSizeWords;
                wordPointer += opSizeWords;
                wordIndex += opSizeWords;

                // Add the operation sequence to the reader
                reader.AddOpSequence(seq);
//              numOpsAdded++;
                numWriteAdded++;
            }

            numWriteMaximum = numWriteAdded;
        }

        static void HandleReadOpComplete(TagReadOpResult readResult)
        {
            if (numReadExecuted >= numReadAdded)
                return;

            // A read operation has occurred. Increment the count.
//          numOpsExecuted++;
            numReadExecuted++;

            // Check the result of the read (pass / fail)
            if (readResult.Result == ReadResultStatus.Success)
            {
                tagData += readResult.Data.ToHexWordString() + " ";

                tagValue = readResult.Data.ToUnsignedInt();

                numReadSucceed++;

                // Have all the read operations executed?
//              if (numOpsExecuted == numOpsAdded)
                if (numReadSucceed == numReadMaximum)
                {
                    Console.WriteLine("\nBulk read complete. Tag data :\n\n{0}", tagData);

//                  read_result = 1;
                }
//              else
//                  read_result = 2;
            }
            else
            {
                Console.WriteLine("Read operation failed : {0}", readResult.Result);
                // Data for this section of memory is unknown. 
                // Add a marker in the output to show this.
                tagData += "<missing data>";

//              read_result = 3;
            }

            if (numReadExecuted == numReadMaximum)
            {
                if (numReadSucceed == numReadMaximum)
                    read_result = 1;
                else
                    read_result = 2;
            }
        }

        static void HandleWriteOpComplete(TagWriteOpResult writeResult)
        {
            if (numWriteExecuted >= numWriteAdded)
                return;
            
            // A write operation has occurred. Increment the count.
//          numOpsExecuted++;
            numWriteExecuted++;

            // Check the result of the read (pass / fail)
            if (writeResult.Result == WriteResultStatus.Success)
            {
                numWordsWritten += writeResult.NumWordsWritten;

                numWriteSucceed++;

                // Have all the write operations executed?
//              if (numOpsExecuted == numOpsAdded)
                if (numWriteSucceed == numWriteMaximum)
                {
                    Console.WriteLine("\nBulk write complete. {0} words written.", numWordsWritten);

//                  write_result = 1;
                }
//              else
//                  write_result = 2;
            }
            else
            {
                Console.WriteLine("Write operation failed : {0}", writeResult.Result);

//              write_result = 3;
            }

            if (numWriteExecuted == numWriteMaximum)
            {
                if (numWriteSucceed == numWriteMaximum)
                    write_result = 1;
                else
                    write_result = 2;
            }
        }

        // This event handler will be called when tag 
        // operations have been executed by the reader.
        static void OnTagOpComplete(ImpinjReader reader, TagOpReport report)
        {
            // Loop through all the completed tag operations
            foreach (TagOpResult result in report)
            {
                // Was this completed operation a tag read operation?
                if (result is TagReadOpResult)
                {
                    // Cast it to the correct type.
                    TagReadOpResult readResult = result as TagReadOpResult;

                    // Process the read results
                    HandleReadOpComplete(readResult);
                }
                // Was it a tag write operation?
                else if (result is TagWriteOpResult)
                {
                    // Cast it to the correct type.
                    TagWriteOpResult writeResult = result as TagWriteOpResult;

                    // Process the write results
                    HandleWriteOpComplete(writeResult);
                }
            }
        }


        void doGray(int gray_filter)
        {
            // Step 1: 利用 Bitmap 將 image 包起來
            Bitmap bimage = new Bitmap(Image.FromFile(Dis_EPD.ImageLocation));//(Dis_EPD.Image);
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

            Dis_EPD.Image = bimage;

            // Convert to binary pixels
            int i = 0, j = 0;
            byte[] tmp_bytes = new byte[(128 / 8) * 296];

            int bmp_width = Dis_EPD.Size.Width;
            int bmp_height = Dis_EPD.Size.Height;

            Bitmap gbmp = new Bitmap(bmp_width, bmp_height);
            Dis_EPD.DrawToBitmap(gbmp, new Rectangle(0, 0, bmp_width, bmp_height));

            for (int x = 0; x < 296; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    Color new_color = gbmp.GetPixel(x * 3 / 2, 191 - (y * 3 / 2));      // bimage height = 192

                    if (new_color.R == 255)
                    {
                        tmp_bytes[i] |= (byte)(0x80 >> j);
                    }
                    else
                    {
                        tmp_bytes[i] &= (byte)~(0x80 >> j);
                    }

                    if (++j == 8)
                    {
                        i++;
                        j = 0;
                    }
                }
            }

            bytes = tmp_bytes;
        }

        public string updateGray
        {
            get { return Dis_EPD.ImageLocation; }
            set { //Pbox_EPD.Image = Image.FromFile(value);
                  doGray(Convert.ToInt16(value)); }
        }


        void RdOnTimedEvent(Object source, ElapsedEventArgs e)
        {
            scanTarget_start = false;
            searchTAG_start = false;
        }


        static void OnTagsReported(ImpinjReader sender, TagReport report)
        {
//            string key;

            // This event handler is called asynchronously 
            // when tag reports are available.
            // Loop through each tag in the report 
            // and print the data.
            foreach (Tag tag in report)
            {
                if (false)//(tag.IsFastIdPresent)
                {
                    // If the TID is available through FastID, use it as the key
                    key_tid = tag.Tid.ToHexString();
                }
                else
                {
                    // Otherwise use the EPC
                    key_epc = tag.Epc.ToHexString();
                    key_tid = tag.Tid.ToHexString();
                }

                // If this tag hasn't been read before, print out the EPC and TID
                if (!tagsRead.ContainsKey(key_tid))
                {
                    if (searchTAG_start == true)
                    {
                        int i;
                        char[] arr_epc = key_epc.ToCharArray(0, key_epc.Length);
                        char[] arr_tid = key_tid.ToCharArray(0, key_tid.Length);

                        // Compare EPC / TID / EPC+TID
                        if (header_EPC != null)
                        {
                            for (i = 0; i < header_EPC.Length; i++)
                            {
                                if (arr_epc[i] == header_EPC[i])
                                {
                                    if (i < (header_EPC.Length - 1))
                                        continue;
                                    else
                                    {
                                        if (header_TID != null)
                                            break;
                                        else
                                        {
                                            TAG_Count++;
                                            goto CHECK_END;
                                        }
                                    }
                                }
                                goto CHECK_END;
                            }
                        }

                        if (header_TID != null)
                        {
                            for (i = 0; i < header_TID.Length; i++)
                            {
                                if (arr_tid[i] == header_TID[i])
                                {
                                    if (i < (header_TID.Length - 1))
                                        continue;
                                    else
                                    {
                                        TAG_Count++;
                                        goto CHECK_END;
                                    }
                                }
                                goto CHECK_END;
                            }
                        }

CHECK_END:
                        Console.WriteLine("Read Count : {0}, Search Count : {1}", tagsRead.Count, TAG_Count);
                    }

//                  Console.WriteLine("EPC : {0}, TID : {1}", tag.Epc, tag.Tid);
                    // Add this tag to the list of tags we've read.
                    tagsRead.Add(key_tid, tag);
                }

            }

        }



        private void Btn_Setting_Click(object sender, EventArgs e)
        {
            settingForm.ShowDialog();
        }

        private void Btn_ID_Card_Click(object sender, EventArgs e)
        {
            gFormat_Mode = 1;       // ID Card

            Btn_ID_Card.Enabled = false;
            Btn_Logistics_Box.Enabled = true;
            Btn_ESL.Enabled = true;
            Btn_Serial_Demo.Enabled = true;

            Btn_Clear_Click(this, null);

            Label_Data_1.Text = "Company Logo";
            Label_Data_2.Text = "Headshot";
            Label_Data_3.Text = "Department";
            Label_Data_4.Text = "Name";

            Tbox_Data_1.Visible = false;
            Btn_Data_1.Visible = true;
            Tbox_Data_2.Visible = false;
            Btn_Data_2.Visible = true;

            Label_Data_3.Visible = true;
            Tbox_Data_3.Visible = true;
            Label_Data_4.Visible = true;
            Tbox_Data_4.Visible = true;

            Dis_EPD.Visible = false;
            Panel_Log_Box.Visible = false;
            Panel_ESL.Visible = false;
            Panel_Serial_Demo.Visible = false;
            Panel_ID_Card.Visible = true;
        }

        private void Btn_Logistics_Box_Click(object sender, EventArgs e)
        {
            gFormat_Mode = 2;       // Logistics Box

            Btn_ID_Card.Enabled = true;
            Btn_Logistics_Box.Enabled = false;
            Btn_ESL.Enabled = true;
            Btn_Serial_Demo.Enabled = true;

            Btn_Clear_Click(this, null);

            Label_Data_1.Text = "Tracking Number";
            Label_Data_2.Text = "Product Name";
            Label_Data_3.Text = "Shipping Date";
            Label_Data_4.Text = "";

            Btn_Data_1.Visible = false;
            Tbox_Data_1.Visible = true;
            Btn_Data_2.Visible = false;
            Tbox_Data_2.Visible = true;

            Label_Data_3.Visible = true;
            Tbox_Data_3.Visible = true;
            Label_Data_4.Visible = false;
            Tbox_Data_4.Visible = false;

            Dis_EPD.Visible = false;
            Panel_ID_Card.Visible = false;
            Panel_ESL.Visible = false;
            Panel_Serial_Demo.Visible = false;
            Panel_Log_Box.Visible = true;
        }

        private void Btn_ESL_Click(object sender, EventArgs e)
        {
            gFormat_Mode = 3;       // ESL

            Btn_ID_Card.Enabled = true;
            Btn_Logistics_Box.Enabled = true;
            Btn_ESL.Enabled = false;
            Btn_Serial_Demo.Enabled = true;

            Btn_Clear_Click(this, null);

            Label_Data_1.Text = "Product Name";
            Label_Data_2.Text = "Price";
            Label_Data_3.Text = "Serial Number";
            Label_Data_4.Text = "";

            Tbox_Data_1.Visible = false;
            Btn_Data_1.Visible = true;
            Btn_Data_2.Visible = false;
            Tbox_Data_2.Visible = true;

            Label_Data_3.Visible = true;
            Tbox_Data_3.Visible = true;
            Label_Data_4.Visible = false;
            Tbox_Data_4.Visible = false;

            Dis_EPD.Visible = false;
            Panel_ID_Card.Visible = false;
            Panel_Log_Box.Visible = false;
            Panel_Serial_Demo.Visible = false;
            Panel_ESL.Visible = true;
        }

        private void Btn_Demo_Click(object sender, EventArgs e)
        {
            gFormat_Mode = 4;       // Serial Number Demo

            Btn_ID_Card.Enabled = true;
            Btn_Logistics_Box.Enabled = true;
            Btn_ESL.Enabled = true;
            Btn_Serial_Demo.Enabled = false;

            Btn_Clear_Click(this, null);

            Label_Data_1.Text = "Initial Serial Number";
            Label_Data_2.Text = "Update Amount";
            Label_Data_3.Text = "";
            Label_Data_4.Text = "";

            Btn_Data_1.Visible = false;
            Tbox_Data_1.Visible = true;
            Btn_Data_2.Visible = false;
            Tbox_Data_2.Visible = true;

            Label_Data_3.Visible = false;
            Tbox_Data_3.Visible = false;
            Label_Data_4.Visible = false;
            Tbox_Data_4.Visible = false;

            Dis_EPD.Visible = false;
            Panel_ID_Card.Visible = false;
            Panel_Log_Box.Visible = false;
            Panel_ESL.Visible = false;
            Panel_Serial_Demo.Visible = true;
        }

        private void Tbox_Data_1_TextChanged(object sender, EventArgs e)
        {
            if (gFormat_Mode == 2)
            {
                try
                {
                    Image myimg = Code128Rendering.MakeBarcodeImage(Tbox_Data_1.Text, 2, true);
                    Dis_Log_TrackNum.Image = myimg;
                    Dis_Log_TrackNum.Visible = true;

                    Dis_Log_TrackChar.Text = Tbox_Data_1.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, this.Text);
                }
            }
            else if (gFormat_Mode == 4)
            {
                try
                {
                    Image myimg = Code128Rendering.MakeBarcodeImage(Tbox_Data_1.Text, 2, true);
                    Dis_Demo_SerialNum.Image = myimg;
                    Dis_Demo_SerialNum.Visible = true;

                    Dis_Demo_SerialChar.Text = Tbox_Data_1.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, this.Text);
                }
            }
        }

        private void Tbox_Data_2_TextChanged(object sender, EventArgs e)
        {
            if (gFormat_Mode == 2)
                Dis_Log_Product.Text = Tbox_Data_2.Text;
            else if (gFormat_Mode == 3)
                Dis_ESL_Price.Text = Tbox_Data_2.Text;
            else if (gFormat_Mode == 4)
                update_amount = Convert.ToInt16(Tbox_Data_2.Text);
        }

        private void Tbox_Data_3_TextChanged(object sender, EventArgs e)
        {
            if (gFormat_Mode == 1)
                Dis_ID_Depart.Text = Tbox_Data_3.Text;
            else if (gFormat_Mode == 2)
                Dis_Log_ShipDate.Text = Tbox_Data_3.Text;
            else if (gFormat_Mode == 3)
            {
                try
                {
                    Image myimg = Code128Rendering.MakeBarcodeImage(Tbox_Data_3.Text, 2, true);
                    Dis_ESL_SerialNum.Image = myimg;
                    Dis_ESL_SerialNum.Visible = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, this.Text);
                }
            }
        }

        private void Tbox_Data_4_TextChanged(object sender, EventArgs e)
        {
            if (gFormat_Mode == 1)
                Dis_ID_Name.Text = Tbox_Data_4.Text;
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            Tbox_Data_1.Text = "";
            Tbox_Data_2.Text = "";
            Tbox_Data_3.Text = "";
            Tbox_Data_4.Text = "";

            Dis_ID_Company.Visible = false;
            Dis_ID_Headshot.Visible = false;
            Dis_ID_Depart.Text = "";
            Dis_ID_Name.Text = "";

            Dis_Log_TrackNum.Visible = false;
            Dis_Log_TrackChar.Text = "";
            Dis_Log_Product.Text = "";
            Dis_Log_ShipDate.Text = "";

            Dis_ESL_ProductImg.Visible = false;
            Dis_ESL_Price.Text = "";
            Dis_ESL_SerialNum.Visible = false;

            Dis_Demo_SerialNum.Visible = false;
            Dis_Demo_SerialChar.Text = "";
        }

        private void Btn_Update_Click(object sender, EventArgs e)
        {
            /* Need to decide whether Quick mode (Command) or Full update (Bitmap) will be used */

            Int16 i, j;

            // Write command to tag memory (2 x N bytes with Write or 4 x N bytes with BulkWrite) :

            TagData data;

            // same action with Btn_Search_Click() +++
            header_EPC = null;
            header_TID = null;

            TAG_Count = 0;

            if ((Tbox_EPC.TextLength > 0) || (Tbox_TID.TextLength > 0))
            {
                if (Tbox_EPC.TextLength > 0)
                {
                    header_EPC = Tbox_EPC.Text.ToCharArray(0, Tbox_EPC.TextLength);
                    string_EPC = Tbox_EPC.Text;
                }
                if (Tbox_TID.TextLength > 0)
                {
                    header_TID = Tbox_TID.Text.ToCharArray(0, Tbox_TID.TextLength);
                    string_TID = Tbox_TID.Text;
                }

                searchTAG_start = true;
            }

            tagsRead.Clear();

            try
            {
                // Connect to the reader.
                reader.Connect(settingForm.host_name);

                // Get the default settings
                // We'll use these as a starting point
                // and then modify the settings we're 
                // interested in.
                Settings settings = reader.QueryDefaultSettings();
                settings.RfMode = 1002;//1000;
                settings.SearchMode = SearchMode.DualTarget;
                settings.Session = 1;
                settings.Antennas.DisableAll();
                settings.Antennas.GetAntenna(1).IsEnabled = true;
                settings.Antennas.GetAntenna(1).TxPowerInDbm = settingForm.Reader_Power;
                settings.Antennas.GetAntenna(1).RxSensitivityInDbm = settingForm.Reader_Sensitivity;

                // Tell the reader to include the TID
                // in all tag reports. We will use FastID
                // to do this. FastID is supported
                // by Impinj Monza 4 and later tags.
                settings.Report.IncludeFastId = true;

                // Apply the newly modified settings.
                reader.ApplySettings(settings);

                // Assign the TagsReported event handler.
                // This specifies which method to call
                // when tags reports are available.
                reader.TagsReported += OnTagsReported;

                // Start the timer
                rdTimer = new System.Timers.Timer(5000);
                rdTimer.Elapsed += RdOnTimedEvent;
                rdTimer.AutoReset = false;
                rdTimer.Enabled = true;

                // Start reading.
                reader.Start();

                while (searchTAG_start == true) { };

                // Stop reading.
                reader.Stop();

                // End the timer
                rdTimer.Stop();
                rdTimer.Dispose();

                reader.TagsReported -= OnTagsReported;

                if (TAG_Count != 1)
                {
                    // Disconnect from the reader.
                    reader.Disconnect();

                    return;
                }
            }
            catch (OctaneSdkException a)
            {
                // Handle Octane SDK errors.
                Console.WriteLine("Octane SDK exception: {0}", a.Message);

            }
            catch (Exception a)
            {
                // Handle other .NET errors.
                Console.WriteLine("Exception : {0}", a.Message);

            }
            // same action with Btn_Search_Click() ---

            // Assign the TagOpComplete event handler.
            // This specifies which method to call
            // when tag operations are complete.
            reader.TagOpComplete += OnTagOpComplete;

            // Start reading.
            reader.Start();

#if false   //QUICK_MODE

            byte[] Command_Data = new byte[32];

            data = TagData.FromByteArray(Command_Data);

#else       //FULL_IMAGE

            int Remain_LineNum = (Panel_ID_Card.Size.Width) * 2 / 3;//296;
            int Remain_PixelNum = (Panel_ID_Card.Size.Height) * 2 / 3;//128

            byte[] Full_Data = new byte[16 * (1 + 59)];
            byte[] Remain_Data = new byte[16 * (1 + (Remain_LineNum % 59))];//56];
            byte[] Full_Command = new byte[4];
            byte[] Control_Method = new byte[2];

            int Written_LineNum = 0;
            int Written_LineOffset = 0;
            uint Full_SeqNum = 0;

            ushort TotalWordsWritten = 0;

            while (Remain_LineNum > 0)
            {
                if (Remain_LineNum >= 59)
                    Written_LineNum = 59;
                else
                    Written_LineNum = Remain_LineNum;

                /* Create Tag Data for full pixels update */
                if (true)//(Written_LineNum == 59)
                {
//                  System.Buffer.BlockCopy(bytes, 16 * Written_LineOffset, Full_Data, 0, 16 * Written_LineNum);
//                  data = TagData.FromByteArray(Full_Data);
                    for (i = 0; i < Written_LineNum; i++)
                    {
                        System.Buffer.BlockCopy(bytes, (Remain_PixelNum / 8) * (Written_LineOffset + i), Full_Data, 16 * i/*16 * (i + 1)*/, (Remain_PixelNum / 8));
                        if ((Remain_PixelNum / 8) < 16)
                        {
                            for (j = 0; j < (16 - (Remain_PixelNum / 8)); j++)
                                System.Buffer.SetByte(Full_Data, (16 * i/*16 * (i + 1)*/) + ((Remain_PixelNum / 8) + j), 0xFF);
                        }
                    }
                }

CHECK_BUSY_1:
                /* Check busy status & SeqNum */
                BulkRead(null, MemoryBank.User, 472, 1);        // Byte address: (1008d - 64d)

                while (read_result == 0) {};

                reader.DeleteAllOpSequences();

                if (read_result == 1)
                {
                    read_result = 0;
                }
                else
                {
                    read_result = 0;

                    goto CHECK_BUSY_1;
                }

//              if ((tagValue & 0x0800) == 0x00)
                if ((tagValue & 0x0600) != 0x0200)
                    goto CHECK_BUSY_1;

                //Full_SeqNum = tagValue & 0x0400;
                //Full_SeqNum ^= 0x0400;
                Full_SeqNum = 0x0400;

                /* Write command for Memory format */
                Full_Command[0] = (byte)(((Written_LineNum << 4) & 0xFF) | (int)(Full_SeqNum >> 8) | (0x02));
                if (true)//(Remain_LineNum < 150)//((Remain_LineNum - Written_LineNum) == 0)//((Written_LineOffset + Written_LineNum) == CurrentImage.RestoreBounds.Height)//296)//
                    Full_Command[0] |= 0x01;
                Full_Command[1] = (byte)(Written_LineNum >> 4);
                Full_Command[2] = (byte)(Written_LineOffset & 0xFF);
                Full_Command[3] = (byte)(Written_LineOffset >> 8);

                if (true)//(Written_LineNum == 59)
                {
                    System.Buffer.BlockCopy(Full_Command, 0, Full_Data, 16 * 59/*0*/, 4);
                    data = TagData.FromByteArray(Full_Data);
                }
                else
                {
                    System.Buffer.BlockCopy(Full_Command, 0, Remain_Data, 0, 4);
                    data = TagData.FromByteArray(Remain_Data);
                }

WRITE_DATA_1:
                /* Write data for Memory format */
                BulkWrite(null, MemoryBank.User, 0, data);    // Byte address: (64d - 64d)

                // Delay to wait response
//              Thread.Sleep(1000);

                while (write_result == 0) {};

                reader.DeleteAllOpSequences();

                if (write_result == 1)
                {
                    write_result = 0;

                    TotalWordsWritten += numWordsWritten;
                }
                else
                {
                    write_result = 0;

                    goto WRITE_DATA_1;
                }

CHECK_BUSY_2:
                /* Check if Tag is busy */
                BulkRead(null, MemoryBank.User, 505, 1);        // Byte address: (1074d - 64d)

                while (read_result == 0) {};
                if (read_result == 1)
                {
                    read_result = 0;
                }
                else
                {
                    read_result = 0;

                    goto CHECK_BUSY_2;
                }

                if ((tagValue & 0x0800) == 0x00)
                    goto CHECK_BUSY_2;

READ_DATA_1:
                /* Read SeqNum */
                BulkRead(null, MemoryBank.User, 510, 1);        // Byte address: (1084d - 64d)

                while (read_result == 0) {};
                if (read_result == 1)
                {
                    read_result = 0;
                }
                else
                {
                    read_result = 0;

                    goto READ_DATA_1;
                }

                Full_SeqNum = tagValue & 0x0400;
                Full_SeqNum ^= 0x0400;

                /* Write command for Memory format */
                Full_Command[0] = (byte)(((Written_LineNum << 4) & 0xFF) | (int)(Full_SeqNum >> 8));
                Full_Command[1] = (byte)(Written_LineNum >> 4);
                Full_Command[2] = (byte)(Written_LineOffset & 0xFF);
                Full_Command[3] = (byte)(Written_LineOffset >> 8);
                data = TagData.FromByteArray(Full_Command);

WRITE_DATA_2:
                BulkWrite(null, MemoryBank.User, 510, data);      // Byte address: (1084d - 64d)

                // Delay to wait response
//              Thread.Sleep(2000);

                while (write_result == 0) {};
                if (write_result == 1)
                {
                    write_result = 0;
                }
                else
                {
                    write_result = 0;

                    goto WRITE_DATA_2;
                }

                Written_LineOffset += Written_LineNum;
                Remain_LineNum -= Written_LineNum;
            }

//          if (TotalWordsWritten != (8 * (300 + 2)))
            if (TotalWordsWritten != (8 * 360))
            {
                reader.Stop();

                reader.DeleteAllOpSequences();
                reader.TagOpComplete -= OnTagOpComplete;

                reader.Disconnect();

                return;
            }

            if (gFormat_Mode == 4)      // Serial Number Demo
            {
                Int64 serial_num;
                if (--update_amount > 0)
                {
                    // Contineous update
                    serial_num = Convert.ToInt64(Tbox_Data_1.Text);
                    serial_num++;
                    Tbox_Data_1.Text = Convert.ToString(serial_num);
                }
                else
                {
                    Btn_Update.Enabled = false;

                    // Pop-up message
                    MessageBox.Show("Update completed.", "Message");
                }
            }

            // Stop reading.
            reader.Stop();

            // Remove all operation sequences from the reader that haven't executed.
            reader.DeleteAllOpSequences();
            reader.TagOpComplete -= OnTagOpComplete;

            // Disconnect from the reader.
            reader.Disconnect();

#endif

        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            int bmp_width = Panel_ID_Card.Size.Width;
            int bmp_height = Panel_ID_Card.Size.Height;

            Bitmap bmp = new Bitmap(bmp_width, bmp_height);

            if (gFormat_Mode == 1)
                Panel_ID_Card.DrawToBitmap(bmp, new Rectangle(0, 0, bmp_width, bmp_height));
            else if (gFormat_Mode == 2)
                Panel_Log_Box.DrawToBitmap(bmp, new Rectangle(0, 0, bmp_width, bmp_height));
            else if (gFormat_Mode == 3)
                Panel_ESL.DrawToBitmap(bmp, new Rectangle(0, 0, bmp_width, bmp_height));
            else if (gFormat_Mode == 4)
                Panel_Serial_Demo.DrawToBitmap(bmp, new Rectangle(0, 0, bmp_width, bmp_height));
            else if (gFormat_Mode == 5)
                Dis_EPD.DrawToBitmap(bmp, new Rectangle(0, 0, bmp_width, bmp_height));

            bmp.Save(@"C:\_tmp\EpdBitmap.bmp", ImageFormat.Bmp);
        }

        private void Btn_LoadImage_Click(object sender, EventArgs e)
        {
            // Step 1: 建立 OpenFiledialog 元件
            OpenFileDialog openFileDialog_load = new OpenFileDialog();
            openFileDialog_load.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog_load.InitialDirectory = @"c:\";
            openFileDialog_load.Title = "Please select an image file";

            // Step 2: 顯示出對話框
            if (openFileDialog_load.ShowDialog() == DialogResult.OK)
            {
                gFormat_Mode = 5;       // Load Image

                Dis_EPD.Image = Image.FromFile(openFileDialog_load.FileName);
//              grayForm.load_path = openFileDialog_load.FileName;
                Dis_EPD.ImageLocation = openFileDialog_load.FileName;

                Btn_ID_Card.Enabled = true;
                Btn_Logistics_Box.Enabled = true;
                Btn_ESL.Enabled = true;
                Btn_Serial_Demo.Enabled = true;

                Panel_ID_Card.Visible = false;
                Panel_Log_Box.Visible = false;
                Panel_ESL.Visible = false;
                Panel_Serial_Demo.Visible = false;
                Dis_EPD.Visible = true;
            }
        }

        private void Btn_EditPhoto_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(this);
//            grayForm.ShowDialog();
            frm.ShowDialog();
        }

        private void Btn_Data_1_Click(object sender, EventArgs e)
        {
            // Step 1: 建立 OpenFiledialog 元件
            OpenFileDialog openFileDialog_1 = new OpenFileDialog();
            openFileDialog_1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog_1.InitialDirectory = @"c:\";
            openFileDialog_1.Title = "Please select an image file";

            // Step 2: 顯示出對話框
            if (openFileDialog_1.ShowDialog() == DialogResult.OK)
            {
                if (gFormat_Mode == 1)
                {
                    Dis_ID_Company.Image = Image.FromFile(openFileDialog_1.FileName);
                    Dis_ID_Company.Visible = true;
                }
                else if (gFormat_Mode == 3)
                {
                    Dis_ESL_ProductImg.Image = Image.FromFile(openFileDialog_1.FileName);
                    Dis_ESL_ProductImg.Visible = true;
                }
            }
        }

        private void Btn_Data_2_Click(object sender, EventArgs e)
        {
            // Step 1: 建立 OpenFiledialog 元件
            OpenFileDialog openFileDialog_2 = new OpenFileDialog();
            openFileDialog_2.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog_2.InitialDirectory = @"c:\";
            openFileDialog_2.Title = "Please select an image file";

            // Step 2: 顯示出對話框
            if (openFileDialog_2.ShowDialog() == DialogResult.OK)
            {
                Dis_ID_Headshot.Image = Image.FromFile(openFileDialog_2.FileName);
                Dis_ID_Headshot.Visible = true;
            }
        }

        private void Btn_Target_EPC_Click(object sender, EventArgs e)
        {
            if (true)//(scanTarget_start == false)
            {
                scanTarget_start = true;

                Btn_Target_EPC.Enabled = false;
                Btn_Target_TID.Enabled = false;
                Btn_Search.Enabled = false;


                tagsRead.Clear();

                try
                {
                    // Connect to the reader.
                    reader.Connect(settingForm.host_name);

                    // Get the default settings
                    // We'll use these as a starting point
                    // and then modify the settings we're 
                    // interested in.
                    Settings settings = reader.QueryDefaultSettings();
                    settings.RfMode = 1002;//1000;
                    settings.SearchMode = SearchMode.DualTarget;
                    settings.Session = 1;
                    settings.Antennas.DisableAll();
                    settings.Antennas.GetAntenna(1).IsEnabled = true;
                    settings.Antennas.GetAntenna(1).TxPowerInDbm = settingForm.Reader_Power;
                    settings.Antennas.GetAntenna(1).RxSensitivityInDbm = settingForm.Reader_Sensitivity;

                    // Tell the reader to include the TID
                    // in all tag reports. We will use FastID
                    // to do this. FastID is supported
                    // by Impinj Monza 4 and later tags.
                    settings.Report.IncludeFastId = true;

                    // Apply the newly modified settings.
                    reader.ApplySettings(settings);

                    // Assign the TagsReported event handler.
                    // This specifies which method to call
                    // when tags reports are available.
                    reader.TagsReported += OnTagsReported;

                    // Start the timer
                    rdTimer = new System.Timers.Timer(5000);
                    rdTimer.Elapsed += RdOnTimedEvent;
                    rdTimer.AutoReset = false;
                    rdTimer.Enabled = true;

                    // Start reading.
                    reader.Start();

                    while (scanTarget_start == true) { };

                    // Stop reading.
                    reader.Stop();

                    // End the timer
                    rdTimer.Stop();
                    rdTimer.Dispose();

                    reader.TagsReported -= OnTagsReported;

                    if (tagsRead.Count == 0)
                    {
                        Tbox_EPC.Text = "";
                        Tbox_TID.Text = "";
                    }
                    else if (tagsRead.Count == 1)
                    {
                        Tbox_EPC.Text = key_epc;
                        Tbox_TID.Text = "";
                    }
                    else
                    {
                        // Pop-up message
                        MessageBox.Show("Multiple tags found, please place only one tag in front of the antenna.", "Warning");
                    }

                    Btn_Target_EPC.Enabled = true;
                    Btn_Target_TID.Enabled = true;
                    Btn_Search.Enabled = true;

                }
                catch (OctaneSdkException a)
                {
                    // Handle Octane SDK errors.
                    Console.WriteLine("Octane SDK exception: {0}", a.Message);
                }
                catch (Exception a)
                {
                    // Handle other .NET errors.
                    Console.WriteLine("Exception : {0}", a.Message);
                }
            }

        }

        private void Btn_Target_TID_Click(object sender, EventArgs e)
        {
            if (true)//(scanTarget_start == false)
            {
                scanTarget_start = true;

                Btn_Target_EPC.Enabled = false;
                Btn_Target_TID.Enabled = false;
                Btn_Search.Enabled = false;


                tagsRead.Clear();

                try
                {
                    // Connect to the reader.
                    reader.Connect(settingForm.host_name);

                    // Get the default settings
                    // We'll use these as a starting point
                    // and then modify the settings we're 
                    // interested in.
                    Settings settings = reader.QueryDefaultSettings();
                    settings.RfMode = 1002;//1000;
                    settings.SearchMode = SearchMode.DualTarget;
                    settings.Session = 1;
                    settings.Antennas.DisableAll();
                    settings.Antennas.GetAntenna(1).IsEnabled = true;
                    settings.Antennas.GetAntenna(1).TxPowerInDbm = settingForm.Reader_Power;
                    settings.Antennas.GetAntenna(1).RxSensitivityInDbm = settingForm.Reader_Sensitivity;

                    // Tell the reader to include the TID
                    // in all tag reports. We will use FastID
                    // to do this. FastID is supported
                    // by Impinj Monza 4 and later tags.
                    settings.Report.IncludeFastId = true;

                    // Apply the newly modified settings.
                    reader.ApplySettings(settings);

                    // Assign the TagsReported event handler.
                    // This specifies which method to call
                    // when tags reports are available.
                    reader.TagsReported += OnTagsReported;

                    // Start the timer
                    rdTimer = new System.Timers.Timer(5000);
                    rdTimer.Elapsed += RdOnTimedEvent;
                    rdTimer.AutoReset = false;
                    rdTimer.Enabled = true;

                    // Start reading.
                    reader.Start();

                    while (scanTarget_start == true) { };

                    // Stop reading.
                    reader.Stop();

                    // End the timer
                    rdTimer.Stop();
                    rdTimer.Dispose();

                    reader.TagsReported -= OnTagsReported;

                    if (tagsRead.Count == 0)
                    {
                        Tbox_EPC.Text = "";
                        Tbox_TID.Text = "";
                    }
                    else if (tagsRead.Count == 1)
                    {
                        Tbox_EPC.Text = "";
                        Tbox_TID.Text = key_tid;
                    }
                    else
                    {
                        // Pop-up message
                        MessageBox.Show("Multiple tags found, please place only one tag in front of the antenna.", "Warning");
                    }

                    Btn_Target_EPC.Enabled = true;
                    Btn_Target_TID.Enabled = true;
                    Btn_Search.Enabled = true;

                }
                catch (OctaneSdkException a)
                {
                    // Handle Octane SDK errors.
                    Console.WriteLine("Octane SDK exception: {0}", a.Message);
                }
                catch (Exception a)
                {
                    // Handle other .NET errors.
                    Console.WriteLine("Exception : {0}", a.Message);
                }
            }

        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            if (true)//(searchTAG_start == false)
            {
                header_EPC = null;
                header_TID = null;

                TAG_Count = 0;

                if ((Tbox_EPC.TextLength > 0) || (Tbox_TID.TextLength > 0))
                {
                    if (Tbox_EPC.TextLength > 0)
                        header_EPC = Tbox_EPC.Text.ToCharArray(0, Tbox_EPC.TextLength);
                    if (Tbox_TID.TextLength > 0)
                        header_TID = Tbox_TID.Text.ToCharArray(0, Tbox_TID.TextLength);

                    searchTAG_start = true;
                }

                Btn_Target_EPC.Enabled = false;
                Btn_Target_TID.Enabled = false;
                Btn_Search.Enabled = false;


                tagsRead.Clear();

                try
                {
                    // Connect to the reader.
                    reader.Connect(settingForm.host_name);

                    // Get the default settings
                    // We'll use these as a starting point
                    // and then modify the settings we're 
                    // interested in.
                    Settings settings = reader.QueryDefaultSettings();
                    settings.RfMode = 1002;//1000;
                    settings.SearchMode = SearchMode.DualTarget;
                    settings.Session = 1;
                    settings.Antennas.DisableAll();
                    settings.Antennas.GetAntenna(1).IsEnabled = true;
                    settings.Antennas.GetAntenna(1).TxPowerInDbm = settingForm.Reader_Power;
                    settings.Antennas.GetAntenna(1).RxSensitivityInDbm = settingForm.Reader_Sensitivity;

                    // Tell the reader to include the TID
                    // in all tag reports. We will use FastID
                    // to do this. FastID is supported
                    // by Impinj Monza 4 and later tags.
                    settings.Report.IncludeFastId = true;

                    // Apply the newly modified settings.
                    reader.ApplySettings(settings);

                    // Assign the TagsReported event handler.
                    // This specifies which method to call
                    // when tags reports are available.
                    reader.TagsReported += OnTagsReported;

                    // Start the timer
                    rdTimer = new System.Timers.Timer(5000);
                    rdTimer.Elapsed += RdOnTimedEvent;
                    rdTimer.AutoReset = false;
                    rdTimer.Enabled = true;

                    // Start reading.
                    reader.Start();

                    while (searchTAG_start == true) { };

                    // Stop reading.
                    reader.Stop();

                    // End the timer
                    rdTimer.Stop();
                    rdTimer.Dispose();

                    reader.TagsReported -= OnTagsReported;

                    if (TAG_Count == 0)
                    {
                        // Pop-up message
                        MessageBox.Show("TAG not found.", "Warning");
                    }
                    else if (TAG_Count == 1)
                    {
                        // Pop-up message
                        MessageBox.Show("Search successfully.", "Message");

                        goto CONTROL_GPIO;
                    }
                    else
                    {
                        // Pop-up message
                        MessageBox.Show("Multiple tags found, please specify EPC/TID for only one tag.", "Warning");
                    }

                    // Disconnect from the reader.
                    reader.Disconnect();

                    Btn_Target_EPC.Enabled = true;
                    Btn_Target_TID.Enabled = true;
                    Btn_Search.Enabled = true;

                    return;
                }
                catch (OctaneSdkException a)
                {
                    // Handle Octane SDK errors.
                    Console.WriteLine("Octane SDK exception: {0}", a.Message);
                }
                catch (Exception a)
                {
                    // Handle other .NET errors.
                    Console.WriteLine("Exception : {0}", a.Message);
                }

CONTROL_GPIO:

                // Assign the TagOpComplete event handler.
                // This specifies which method to call
                // when tag operations are complete.
                reader.TagOpComplete += OnTagOpComplete;

                // Start reading.
                reader.Start();

                // Turn on the onboard LED
                TagData data;
                byte[] Ctrl_Command = new byte[2];

                Ctrl_Command[0] = (byte)((0x01) | (0x10) | (0x80)); // value = 1, enable = gpio[0], new data
                Ctrl_Command[1] = (byte)((0x03));                   // 3 seconds

                data = TagData.FromByteArray(Ctrl_Command);

WRITE_DATA:
                /* Write data for Memory format */
                BulkWrite(null, MemoryBank.User, 485, data);    // Byte address: (1034d - 64d)

                while (write_result == 0) {};

                // for testing
                reader.DeleteAllOpSequences();

                if (write_result == 1)
                {
                    write_result = 0;
                }
                else
                {
                    write_result = 0;

                    goto WRITE_DATA;
                }

                // Stop reading.
                reader.Stop();

                // Remove all operation sequences from the reader that haven't executed.
//              reader.DeleteAllOpSequences();
                reader.TagOpComplete -= OnTagOpComplete;

                // Disconnect from the reader.
                reader.Disconnect();

            }

        }
    }
}
