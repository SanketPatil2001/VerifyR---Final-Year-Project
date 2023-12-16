using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DuplicateDetection
{
    public partial class OnlineTweets : Form
    {
        public static string filename = "";
        public OnlineTweets()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://twitter.com/engineerproblem");
            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(@"D:/webdata/1.html");
            //myRequest.Method = "GET";
            //WebResponse myResponse = myRequest.GetResponse();

            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = op.FileName;
                filename = textBox1.Text;
                string[] data = File.ReadAllLines(op.FileName);
                for (int j = 0; j < 50; j++)
                {
                    richTextBox1.Text += data[j]+"\n";
                }
                
                StreamWriter sw = new StreamWriter(Application.StartupPath + "\\result.txt");
                sw.Write(richTextBox1.Text);
                sw.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] data = File.ReadAllLines(textBox1.Text);
            string[] colname = data[0].Split(',');
            for (int j = 1; j < colname.Length; j++)
            {
                dataGridView1.Columns.Add(colname[j], colname[j]);
            }

            for (int j = 1; j < 50; j++)
            {
                string []data2 = data[j].Split(',');
                string[] rowdata = data[j].Split(',');
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j-1].Cells[0].Value = rowdata[0].ToString();
                dataGridView1.Rows[j-1].Cells[1].Value = rowdata[1].ToString();
                dataGridView1.Rows[j-1].Cells[2].Value = rowdata[2].ToString();
                dataGridView1.Rows[j-1].Cells[3].Value = rowdata[3].ToString();
                dataGridView1.Rows[j-1].Cells[4].Value = rowdata[4].ToString();
                dataGridView1.Rows[j-1].Cells[5].Value = rowdata[5].ToString();
                dataGridView1.Rows[j-1].Cells[6].Value = rowdata[6].ToString();
                dataGridView1.Rows[j-1].Cells[7].Value = rowdata[7].ToString();
                string content = "";
                for (int p = 8; p < data2.Length; p++)
                {
                    content = content + data2[p].ToString();
                }
                dataGridView1.Rows[j-1].Cells[8].Value = content;
            }
        }
    }
}
