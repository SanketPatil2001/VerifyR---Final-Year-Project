using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Drawing.Imaging;

using System.Threading.Tasks;

namespace DuplicateDetection
{
    public partial class FrmTrain : Form
    {
        public static string datasetname;
        public static string msgg;
        public static  int relation_count = 0;
        public static int mother_dau_act = 0;
        public static int mother_dau_ret = 0;
        public System.Drawing.Bitmap originalImage;
        string foldername = "";
        string[] name;
       public static  int imagepaircount = 0;
        public static int father_dau_act = 0;
        public static int father_dau_ret = 0;

        public static int father_son_act = 0;
        public static int father_son_ret = 0;

        public static int mother_son_act = 0;
        public static int mother_son_ret = 0;
        public Bitmap OriginalImage { get; set; }
        public Bitmap TransformedImage { get; set; }

        private const double w0 = 0.5;
        private const double w1 = -0.5;
        private const double s0 = 0.5;
        private const double s1 = 0.5;
        public System.Drawing.Bitmap filteredImage;
        
        private BackgroundWorker backgroundWorker;
        public FrmTrain()
        {
            InitializeComponent();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            // backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            // backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                pictureBox1.Image = new Bitmap(ofd.FileName);
                foldername = Directory.GetParent(ofd.FileName).ToString();
                name = foldername.Split('\\');
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                pictureBox2.Image = new Bitmap(ofd.FileName);
            }
            catch { }
       
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public  float  Compare(Bitmap firstBmp, Bitmap secondBmp, byte threshold = 3)
        {

          // firstBmp.GetDifferenceImage(secondBmp, true).Save("_diff.png");
           pictureBox3.Image = firstBmp.GetDifferenceImage(secondBmp, true);
           float val=firstBmp.PercentageDifference(secondBmp, threshold) * 100;
           return val;


        }



        public void FWT(double[] data)
        {
            double[] temp = new double[data.Length];

            int h = data.Length >> 1;
            for (int i = 0; i < h; i++)
            {
                int k = (i << 1);
                temp[i] = data[k] * s0 + data[k + 1] * s1;
                temp[i + h] = data[k] * w0 + data[k + 1] * w1;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }

        /// <summary>
        ///   Discrete Haar Wavelet 2D Transform
        /// </summary>
        /// 
        public void FWT(double[,] data, int iterations)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[] row = new double[cols];
            double[] col = new double[rows];

            for (int k = 0; k < iterations; k++)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < row.Length; j++)
                        row[j] = data[i, j];

                    FWT(row);

                    for (int j = 0; j < row.Length; j++)
                        data[i, j] = row[j];
                }

                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < col.Length; i++)
                        col[i] = data[i, j];

                    FWT(col);

                    for (int i = 0; i < col.Length; i++)
                        data[i, j] = col[i];
                }
            }
        }

        /// <summary>
        ///   Inverse Haar Wavelet Transform
        /// </summary>
        /// 
        public void IWT(double[] data)
        {
            double[] temp = new double[data.Length];

            int h = data.Length >> 1;
            for (int i = 0; i < h; i++)
            {
                int k = (i << 1);
                temp[k] = (data[i] * s0 + data[i + h] * w0) / w0;
                temp[k + 1] = (data[i] * s1 + data[i + h] * w1) / s0;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }

        /// <summary>
        ///   Inverse Haar Wavelet 2D Transform
        /// </summary>
        /// 
        public void IWT(double[,] data, int iterations)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[] col = new double[rows];
            double[] row = new double[cols];

            for (int l = 0; l < iterations; l++)
            {
                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < row.Length; i++)
                        col[i] = data[i, j];

                    IWT(col);

                    for (int i = 0; i < col.Length; i++)
                        data[i, j] = col[i];
                }

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < row.Length; j++)
                        row[j] = data[i, j];

                    IWT(row);

                    for (int j = 0; j < row.Length; j++)
                        data[i, j] = row[j];
                }
            }
        }

        public void ApplyHaarTransform(bool Forward, bool Safe, string sIterations)
        {
            Bitmap bmp = Forward ? new Bitmap(pictureBox1.Image) : new Bitmap(TransformedImage);

            int Iterations = 0;
            int.TryParse(sIterations, out Iterations);

            int maxScale = (int)(Math.Log(bmp.Width < bmp.Height ? bmp.Width : bmp.Height) / Math.Log(2));
            if (Iterations < 1 || Iterations > maxScale)
            {
                MessageBox.Show("Iteration must be Integer from 1 to " + maxScale);
                return;
            }

            int time = Environment.TickCount;

            double[,] Red = new double[bmp.Width, bmp.Height];
            double[,] Green = new double[bmp.Width, bmp.Height];
            double[,] Blue = new double[bmp.Width, bmp.Height];

            int PixelSize = 3;
            BitmapData bmData = null;

            if (Safe)
            {
                Color c;

                for (int j = 0; j < bmp.Height; j++)
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        c = bmp.GetPixel(i, j);
                        Red[i, j] = (double)Scale(0, 255, -1, 1, c.R);
                        Green[i, j] = (double)Scale(0, 255, -1, 1, c.G);
                        Blue[i, j] = (double)Scale(0, 255, -1, 1, c.B);
                    }
                }
            }
            else
            {
                bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    for (int j = 0; j < bmData.Height; j++)
                    {
                        byte* row = (byte*)bmData.Scan0 + (j * bmData.Stride);
                        for (int i = 0; i < bmData.Width; i++)
                        {
                            Red[i, j] = (double)Scale(0, 255, -1, 1, row[i * PixelSize + 2]);
                            Green[i, j] = (double)Scale(0, 255, -1, 1, row[i * PixelSize + 1]);
                            Blue[i, j] = (double)Scale(0, 255, -1, 1, row[i * PixelSize]);
                        }
                    }
                }
            }

            if (Forward)
            {
                FWT(Red, Iterations);
                FWT(Green, Iterations);
                FWT(Blue, Iterations);
            }
            else
            {
                IWT(Red, Iterations);
                IWT(Green, Iterations);
                IWT(Blue, Iterations);
            }

            if (Safe)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb((int)Scale(-1, 1, 0, 255, Red[i, j]), (int)Scale(-1, 1, 0, 255, Green[i, j]), (int)Scale(-1, 1, 0, 255, Blue[i, j])));
                    }
                }
            }
            else
            {
                unsafe
                {
                    for (int j = 0; j < bmData.Height; j++)
                    {
                        byte* row = (byte*)bmData.Scan0 + (j * bmData.Stride);
                        for (int i = 0; i < bmData.Width; i++)
                        {
                            row[i * PixelSize + 2] = (byte)Scale(-1, 1, 0, 255, Red[i, j]);
                            row[i * PixelSize + 1] = (byte)Scale(-1, 1, 0, 255, Green[i, j]);
                            row[i * PixelSize] = (byte)Scale(-1, 1, 0, 255, Blue[i, j]);
                        }
                    }
                }

                bmp.UnlockBits(bmData);
            }

            if (Forward)
            {
                TransformedImage = new Bitmap(bmp);
            }

            pictureBox5.Image = bmp;
        }


        public void ApplyHaarTransform1(bool Forward, bool Safe, string sIterations)
        {
            Bitmap bmp = Forward ? new Bitmap(pictureBox2.Image) : new Bitmap(TransformedImage);

            int Iterations = 0;
            int.TryParse(sIterations, out Iterations);

            int maxScale = (int)(Math.Log(bmp.Width < bmp.Height ? bmp.Width : bmp.Height) / Math.Log(2));
            if (Iterations < 1 || Iterations > maxScale)
            {
                MessageBox.Show("Iteration must be Integer from 1 to " + maxScale);
                return;
            }

            int time = Environment.TickCount;

            double[,] Red = new double[bmp.Width, bmp.Height];
            double[,] Green = new double[bmp.Width, bmp.Height];
            double[,] Blue = new double[bmp.Width, bmp.Height];

            int PixelSize = 3;
            BitmapData bmData = null;

            if (Safe)
            {
                Color c;

                for (int j = 0; j < bmp.Height; j++)
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        c = bmp.GetPixel(i, j);
                        Red[i, j] = (double)Scale(0, 255, -1, 1, c.R);
                        Green[i, j] = (double)Scale(0, 255, -1, 1, c.G);
                        Blue[i, j] = (double)Scale(0, 255, -1, 1, c.B);
                    }
                }
            }
            else
            {
                bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    for (int j = 0; j < bmData.Height; j++)
                    {
                        byte* row = (byte*)bmData.Scan0 + (j * bmData.Stride);
                        for (int i = 0; i < bmData.Width; i++)
                        {
                            Red[i, j] = (double)Scale(0, 255, -1, 1, row[i * PixelSize + 2]);
                            Green[i, j] = (double)Scale(0, 255, -1, 1, row[i * PixelSize + 1]);
                            Blue[i, j] = (double)Scale(0, 255, -1, 1, row[i * PixelSize]);
                        }
                    }
                }
            }

            if (Forward)
            {
                FWT(Red, Iterations);
                FWT(Green, Iterations);
                FWT(Blue, Iterations);
            }
            else
            {
                IWT(Red, Iterations);
                IWT(Green, Iterations);
                IWT(Blue, Iterations);
            }

            if (Safe)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb((int)Scale(-1, 1, 0, 255, Red[i, j]), (int)Scale(-1, 1, 0, 255, Green[i, j]), (int)Scale(-1, 1, 0, 255, Blue[i, j])));
                    }
                }
            }
            else
            {
                unsafe
                {
                    for (int j = 0; j < bmData.Height; j++)
                    {
                        byte* row = (byte*)bmData.Scan0 + (j * bmData.Stride);
                        for (int i = 0; i < bmData.Width; i++)
                        {
                            row[i * PixelSize + 2] = (byte)Scale(-1, 1, 0, 255, Red[i, j]);
                            row[i * PixelSize + 1] = (byte)Scale(-1, 1, 0, 255, Green[i, j]);
                            row[i * PixelSize] = (byte)Scale(-1, 1, 0, 255, Blue[i, j]);
                        }
                    }
                }

                bmp.UnlockBits(bmData);
            }

            if (Forward)
            {
                TransformedImage = new Bitmap(bmp);
            }

            pictureBox4.Image = bmp;
        }



        public double Scale(double fromMin, double fromMax, double toMin, double toMax, double x)
        {
            if (fromMax - fromMin == 0) return 0;
            double value = (toMax - toMin) * (x - fromMin) / (fromMax - fromMin) + toMin;
            if (value > toMax)
            {
                value = toMax;
            }
            if (value < toMin)
            {
                value = toMin;
            }
            return value;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            txtval.Text = "";
            Cursor.Current = Cursors.WaitCursor;
            AForge.Imaging.Filters.GrayscaleBT709 grays = new AForge.Imaging.Filters.GrayscaleBT709();

            Bitmap bt1 = grays.Apply(new Bitmap(pictureBox1.Image));
            Bitmap bt2 = grays.Apply(new Bitmap(pictureBox2.Image));
            txtval.Text = "" + Compare(bt1, bt2, 3);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 6);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 9);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 12);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 15);

          // txtval.Text =txtval.Text+":::"+Compare(new Bitmap(pictureBox1.Image), new Bitmap(pictureBox2.Image), 5);
            //DWT1();
            //DWT2();

            // 2 State
            //ApplyHaarTransform(true, true, "2");
            //ApplyHaarTransform1(true, true, "2");



          //  originalImage = (Bitmap)pictureBox1.Image.Clone();
            //backgroundWorker.RunWorkerAsync();

            Cursor.Current = Cursors.Default;
        }
        //public void DWT1()
        //{
        //    Rectangle rect = new Rectangle(0, 0, 256, 256);
        //    Bitmap b = new Bitmap(pictureBox1.Image);
        //    AForge.Imaging.Filters.Crop crop = new AForge.Imaging.Filters.Crop(rect);
        //    b = crop.Apply(b);
        //    DWT ImgFFT = new DWT(b);

        //    ImgFFT.ForwardFFT();// Finding 2D FFT of Image
        //    ImgFFT.FFTShift();
        //    ImgFFT.FFTPlot(ImgFFT.FFTShifted);
        //    pictureBox5.Image = (Image)ImgFFT.FourierPlot;

        //}
        //public void DWT2()
        //{
        //    Rectangle rect = new Rectangle(0, 0, 256, 256);
        //    Bitmap b = new Bitmap(pictureBox2.Image);
        //    AForge.Imaging.Filters.Crop crop = new AForge.Imaging.Filters.Crop(rect);
        //    b = crop.Apply(b);
        //    DWT ImgFFT = new DWT(b);

        //    ImgFFT.ForwardFFT();// Finding 2D FFT of Image
        //    ImgFFT.FFTShift();
        //    ImgFFT.FFTPlot(ImgFFT.FFTShifted);
        //    pictureBox4.Image = (Image)ImgFFT.FourierPlot;
        //}
      
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                StreamWriter sw = new StreamWriter("train.txt",true);
                sw.WriteLine(txtval.Text + ":" + comboBox1.Text);
                sw.Close();
                MessageBox.Show("train");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // StreamReader sr = new StreamReader("train.txt");
            //if (txtval.Text != "")
            //{
            //    string[] line = File.ReadAllLines("train.txt");
            //    List<double> ldiff = new List<double>();
            //    List<string> lrelation = new List<string>();

            //    List<int> idx = new List<int>();

            //    int idxnum = -1;
            //    string[] src = txtval.Text.Split(':');
            //    for (int i = 0; i < line.Length; i++)
            //    {
            //        try
            //        {
            //            string[] s = line[i].Split(':');
            //            double d = 0.000;
            //            int j = 0;
            //            for (j = 0; j < s.Length - 1; j++)
            //            {

            //                d += Math.Abs(double.Parse(src[j]) - double.Parse(s[j]));
            //            }
            //            ldiff.Add(d);
            //            lrelation.Add(s[j]);

            //        }
            //        catch { }
            //    }
            //    double v = ldiff[0];
            //    idxnum = 0;
            //    string relation = lrelation[0];
            //    for (int i = 0; i < ldiff.Count - 1; i++)
            //    {


            //        if (v > ldiff[i + 1])
            //        {
            //            v = ldiff[i + 1];
            //            relation = lrelation[i + 1];
            //        }
            //    }
            //    txtval.Text = "";
            //    //txtED.Text = "";


               ///// edited code

            //if (txtval.Text != "")
            //{
            //    string[] line = File.ReadAllLines("train.txt");
            //    string relation = "";
            //    List<double> ldiff = new List<double>();
            //    List<string> lrelation = new List<string>();

            //    List<int> idx = new List<int>();
            //    string fileval = "";
            //    string[] src = txtval.Text.Split(':');
            //    for (int i = 0; i < line.Length; i++)
            //    {
            //        string[] s = line[i].Split(':');
            //        fileval = s[0] + ":" + s[1] + ":" + s[2] + ":" + s[3] + ":" + s[4];
                    

            //     //   txtval.Text = "";

            //        if (comboBox1.Text.Trim() == s[5] && txtval.Text == fileval)
            //        {
            //             relation = s[5];
            //            relation_count++;
            //        }
            //        label4.Text = relation;
               
            //    }
            //     //  MessageBox.Show("Relation is:" + relation);
            //}

            // output change

            if (txtval.Text != "")
            {
                string[] line = File.ReadAllLines("train.txt");
                List<double> ldiff = new List<double>();
                List<string> lrelation = new List<string>();

                List<int> idx = new List<int>();

                int idxnum = -1;
                string[] src = txtval.Text.Split(':');
                for (int i = 0; i < line.Length; i++)
                {
                    try
                    {
                        string[] s = line[i].Split(':');
                        double d = 0.000;
                        int j = 0;
                        for (j = 0; j < s.Length - 1; j++)
                        {

                            d += Math.Abs(double.Parse(src[j]) - double.Parse(s[j]));
                        }
                        ldiff.Add(d);
                        lrelation.Add(s[j]);

                    }
                    catch { }
                }
                double v = ldiff[0];
                idxnum = 0;
                string relation = lrelation[0];
                for (int i = 0; i < ldiff.Count - 1; i++)
                {


                    if (v > ldiff[i + 1])
                    {
                        v = ldiff[i + 1];
                        relation = lrelation[i + 1];
                    }
                }
                txtval.Text = "";
                //   txtED.Text = "";
                if (comboBox1.Text.Trim() == relation)
                {
                    relation_count++;
                }
                label4.Text = relation;
                //  MessageBox.Show("Relation is:" + relation);
            }
            else
            {
                MessageBox.Show("Input not in proper format");
                txtval.Text = "";
            }
          
           // backgroundWorker1.CancelAsync();
            }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                relation_count = 0;
                imagepaircount = 0;
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.ShowDialog();
                string[] ar = Directory.GetFiles(fd.SelectedPath);
                try
                {
                    for (int i = 0; i < ar.Length;i++)
                    {
                        pictureBox1.Image = Image.FromFile(ar[i]);
                       // pictureBox2.Image = Image.FromFile(ar[i + 1]);
                        button3_Click(null, null);
                        button5_Click(null, null);
                       
                        imagepaircount++;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                datasetname = comboBox1.Text;
              //  MessageBox.Show("match result" + relation_count + "out of" + imagepaircount);
                msgg = "match result" + (relation_count-1) + "out of" + imagepaircount;
                //if (comboBox1.Text == "father-daughter")
                //{
                //    father_dau_act = imagepaircount;
                //    father_dau_ret = relation_count;
                //}
                //if (comboBox1.Text == "father-son")
                //{
                //    father_son_act = imagepaircount;
                //    father_son_ret = relation_count;
                //}
                //if (comboBox1.Text == "mother-daughter")
                //{
                //    mother_dau_act = imagepaircount;
                //    mother_dau_ret = relation_count;
                //}
                //if (comboBox1.Text == "mother-son")
                //{
                //    mother_son_act = imagepaircount;
                //    mother_son_ret = relation_count;
                //}
            }
            else { MessageBox.Show("Please Selece Relation to comapare"); }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            try
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.ShowDialog();
                int count = 0;
                Cursor.Current = Cursors.WaitCursor;
                string[] ar = Directory.GetFiles(fd.SelectedPath);
                for (int j = 0; j < ar.Length; j++)
                {

                    pictureBox1.Image = Image.FromFile(ar[j]);
                    // pictureBox2.Image = Image.FromFile(ar[i + 1]);
                    StreamWriter sw = new StreamWriter("train.txt", true);

                    //============

                    AForge.Imaging.Filters.GrayscaleBT709 grays = new AForge.Imaging.Filters.GrayscaleBT709();

                    Bitmap bt1 = grays.Apply(new Bitmap(pictureBox1.Image));
                    Bitmap bt2 = grays.Apply(new Bitmap(pictureBox2.Image));
                    txtval.Text = "" + Compare(bt1, bt2, 3);
                    txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 6);
                    txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 9);
                    txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 12);
                    txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 15);

                    sw.WriteLine(txtval.Text + ":" + comboBox1.Text);
                    sw.Close();
                    count++;

                    lable_count.Text = count.ToString();




                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            MessageBox.Show("train");
            Cursor.Current = Cursors.Default;
        }
        private void txtval_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
          
        }

        private void button9_Click(object sender, EventArgs e)
        {
            originalImage = (Bitmap)pictureBox1.Image.Clone();
            backgroundWorker.RunWorkerAsync();

            Cursor.Current = Cursors.Default;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.ShowDialog();
                int count = 0;
                Cursor.Current = Cursors.WaitCursor;
               
                 
                    string[] subfolder = Directory.GetFiles(fd.SelectedPath);

                    // string name = fd(fd.SelectedPath.ToString()).ToString();
                    try
                    {
                        for (int i = 0; i < subfolder.Length; i++)
                        {


                            pictureBox1.Image = Image.FromFile(subfolder[i]);
                            // pictureBox2.Image = Image.FromFile(ar[i + 1]);
                            StreamWriter sw = new StreamWriter("train.txt", true);

                            //============

                            AForge.Imaging.Filters.GrayscaleBT709 grays = new AForge.Imaging.Filters.GrayscaleBT709();

                            Bitmap bt1 = grays.Apply(new Bitmap(pictureBox1.Image));
                            Bitmap bt2 = grays.Apply(new Bitmap(pictureBox2.Image));
                            txtval.Text = "" + Compare(bt1, bt2, 3);
                            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 6);
                            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 9);
                            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 12);
                            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 15);
                            // button9_Click(null, null);

                            //148.748109231681

                            //   txtED.Text =(  GetRandomNumber(128.748109231681, 168.748109231681)).ToString();
                            //button8_Click(null, null);
                            // txtval.Text =txtval.Text+":::"+Compare(new Bitmap(pictureBox1.Image), new Bitmap(pictureBox2.Image), 5);


                            //=============
                            sw.WriteLine(txtval.Text + ":" + name[name.Length - 1]);
                            sw.Close();
                            count++;

                            lable_count.Text = count.ToString();


                        }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
               
                MessageBox.Show("train");
                Cursor.Current = Cursors.Default;
                /* if (comboBox1.Text != "")
              {
                  StreamWriter sw = new StreamWriter("train.txt",true);
                  sw.WriteLine(txtval.Text + ":" + comboBox1.Text);
                  sw.Close();
                  MessageBox.Show("train");
          
              }*/

            }
            catch { }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //ValueGraph v = new ValueGraph();
            //v.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
