using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using Path = System.IO.Path;


namespace DuplicateDetection
{
  
    public partial class FrmClassification : Form
    {
        string filename = "";
        public FrmClassification()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtval.Text = "";
            Cursor.Current = Cursors.WaitCursor;
            AForge.Imaging.Filters.GrayscaleBT709 grays = new AForge.Imaging.Filters.GrayscaleBT709();
        //    pictureBox1.Image = Image.FromFile(Form1.imagename);
            Bitmap bt1 = grays.Apply(new Bitmap(pictureBox1.Image));
            Bitmap bt2 = grays.Apply(new Bitmap(pictureBox2.Image));
            txtval.Text = "" + Compare(bt1, bt2, 3);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 6);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 9);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 12);
            txtval.Text = txtval.Text + ":" + Compare(bt1, bt2, 15);

            //string[] fileinput = File.ReadAllLines(Application.StartupPath + "\\metadata.csv");
            //for (int i = 0; i < fileinput.Length; i++)
            //{
            //    string []values = fileinput[i].Split(',');
            //    if (values[1].ToString() == Path.GetFileNameWithoutExtension(Form1.imagename))
            //    {
            //        //dataGridView1.Rows[0].Cells[0].Value = values[1].ToString();
            //        //dataGridView1.Rows[0].Cells[1].Value = values[2].ToString();
            //        //dataGridView1.Rows[0].Cells[2].Value = values[3].ToString();
            //        //dataGridView1.Rows[0].Cells[3].Value = values[6].ToString();
            //        //dataGridView1.Rows[0].Cells[4].Value = values[15].ToString();
            //        //dataGridView1.Rows[0].Cells[5].Value = values[19].ToString();
            //        //dataGridView1.Rows[0].Cells[6].Value = values[25].ToString();
            //    }
            //}



        }
        public float Compare(Bitmap firstBmp, Bitmap secondBmp, byte threshold = 3)
        {
            pictureBox3.Image = firstBmp.GetDifferenceImage(secondBmp, true);
            float val = firstBmp.PercentageDifference(secondBmp, threshold) * 100;
            return val;
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            //        if (txtval.Text == fileval)
            //        {
            //            relation = s[5];

            //        }
            //        label4.Text = relation;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Input not in proper format");
            //    txtval.Text = "";
            //}

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
            v = 100 - v;
            txtval.Text = "";
            //   txtED.Text = "";
            label2.Text = v.ToString();
            label4.Text = relation;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FrmClassification_Load(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();
            pictureBox1.Image = Image.FromFile(op.FileName);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmTrain ft = new FrmTrain();
            ft.ShowDialog();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string fileName = textBox1.Text;
            PdfReader reader = new PdfReader(fileName);
            PdfReaderContentParser parser = new PdfReaderContentParser(reader);

            // Extract the images from each page
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                ImageRenderListener listener = new ImageRenderListener();
                parser.ProcessContent(i, listener);

                // Save each image to a file
                foreach (Image image in listener.Images)
                {
                    string imageName = Path.GetFileNameWithoutExtension(fileName) + "_page" + i.ToString() + "_" + image.Tag.ToString() + ".jpg";
                    image.Save(imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

                    // Download the image to the user's computer
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = imageName;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        File.Copy(imageName, saveFileDialog1.FileName);
                    }
                }
            }
            reader.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
    public class ImageRenderListener : IRenderListener
    {
        public List<Image> Images { get; set; }
        public int Counter { get; set; }

        public ImageRenderListener()
        {
            Images = new List<Image>();
            Counter = 0;
        }

        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo)
        {
            try
            {
                PdfImageObject image = renderInfo.GetImage();
                if (image != null)
                {
                    Image img = image.GetDrawingImage();
                    img.Tag = Counter;
                    Images.Add(img);
                    Counter++;
                }
            }
            catch { }
        }
        public void RenderText(TextRenderInfo renderInfo) { }
    }
}
