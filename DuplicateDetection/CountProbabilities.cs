using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace DuplicateDetection
{
    public partial class CountProbabilities : Form
    {
        public CountProbabilities()
        {
            InitializeComponent();
        }

        private void CountProbabilities_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(Application.StartupPath + "\\Doubleword.txt");
                string data = sr.ReadToEnd();
                string row = data.ToString();
                string[] str;
                str = row.Split('\n');

                for (int i = 0; i < str.Length; i++)
                {
                    dataGridView1.Rows.Add(str[i]);

                }
                sr.Close();

            }
            catch { }
            finally
            { }
        }


        private void button2_Click(object sender, EventArgs e)
        {
           
        }



        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\hashvalue.txt");
            string[] data = File.ReadAllLines(OnlineTweets.filename);


            for (int j = 1; j < 50; j++)
            {
                string[] data2 = data[j].Split(',');
                string[] rowdata = data[j].Split(',');
                dataGridView1.Rows.Add();

                string content = "";
                for (int p = 8; p < data2.Length; p++)
                {
                    content = content + data2[p].ToString();
                }
                dataGridView1.Rows[j - 1].Cells[0].Value = content.Trim();


                SHA1 md5 = new SHA1CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(content));
                byte[] result = md5.Hash;
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    strBuilder.Append(result[i].ToString("x2"));
                }

                dataGridView1.Rows[j - 1].Cells[1].Value = strBuilder;
                sw.Write(content + "~" + strBuilder+"\n");
                //File.WriteAllText(SkyDriveFolder + "\\Hospital\\" + fileNameWithoutPath + ".txt", strBuilder.ToString());

            }
            sw.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
        }


        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }


}
