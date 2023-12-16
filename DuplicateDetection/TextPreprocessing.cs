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
    public partial class TextPreprocessing : Form
    {
        public TextPreprocessing()
        {
            InitializeComponent();
        }

        
        private void TextPreprocessing_Load(object sender, EventArgs e)
        {
            string[] data = File.ReadAllLines(OnlineTweets.filename);
            string[] colname = data[0].Split(',');
            for (int j = 1; j < colname.Length; j++)
            {
                dataGridView1.Columns.Add(colname[j], colname[j]);
            }

            for (int j = 1; j < 50; j++)
            {
                string[] data2 = data[j].Split(',');
                string[] rowdata = data[j].Split(',');
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j - 1].Cells[0].Value = rowdata[0].ToString();
                dataGridView1.Rows[j - 1].Cells[1].Value = rowdata[1].ToString();
                dataGridView1.Rows[j - 1].Cells[2].Value = rowdata[2].ToString();
                dataGridView1.Rows[j - 1].Cells[3].Value = rowdata[3].ToString();
                dataGridView1.Rows[j - 1].Cells[4].Value = rowdata[4].ToString();
                dataGridView1.Rows[j - 1].Cells[5].Value = rowdata[5].ToString();
                dataGridView1.Rows[j - 1].Cells[6].Value = rowdata[6].ToString();
                dataGridView1.Rows[j - 1].Cells[7].Value = rowdata[7].ToString();
                string content = "";
                for (int p = 8; p < data2.Length; p++)
                {
                    content = content + data2[p].ToString();
                }
                dataGridView1.Rows[j - 1].Cells[8].Value = content;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] data = File.ReadAllLines(OnlineTweets.filename);
            string[] colname = data[0].Split(',');
            for (int j = 1; j < colname.Length; j++)
            {
                dataGridView2.Columns.Add(colname[j], colname[j]);
            }

            for (int j = 1; j < 50; j++)
            {
                string[] data2 = data[j].Split(',');
                string[] rowdata = data[j].Split(',');
                dataGridView2.Rows.Add();
                dataGridView2.Rows[j - 1].Cells[0].Value = rowdata[0].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[1].Value = rowdata[1].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[2].Value = rowdata[2].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[3].Value = rowdata[3].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[4].Value = rowdata[4].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[5].Value = rowdata[5].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[6].Value = rowdata[6].Trim().ToString();
                dataGridView2.Rows[j - 1].Cells[7].Value = rowdata[7].Trim().ToString();
                string content = "";
                for (int p = 8; p < data2.Length; p++)
                {
                    content = content + data2[p].ToString();
                }
                dataGridView2.Rows[j - 1].Cells[8].Value = content.Trim();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
         
        }

        private void button6_Click(object sender, EventArgs e)
        {
           

        }
                public static string RemoveRepetedChars(String input, int maxRepeat)
                {
                
                   if (input.Length == 0)
                    {
                         return input;
                    }
                    StringBuilder b = new StringBuilder();
                    Char[] chars = input.ToCharArray();
                    Char lastChar = chars[0];
                    int repeat = 0;
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (chars[i] == lastChar && repeat > maxRepeat)
                        {
                            //b.Append(chars[i]);
                        }
                        else if(chars[i]==lastChar && repeat < maxRepeat)
                        {
                            b.Append(chars[i]);
                            repeat = repeat+1;
                            lastChar = chars[i];
                        }
                        else if (chars[i] != lastChar)
                        {
                            b.Append(chars[i]);
                            lastChar=chars[i];
                        }
             
                    }
                
                    return b.ToString();
                }

                private void button7_Click(object sender, EventArgs e)
                {
                   
                }

                private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
                {

                }

                private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
                {

                }

                private void panel1_Paint(object sender, PaintEventArgs e)
                {

                }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }    
}
