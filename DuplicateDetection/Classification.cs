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
using System.Windows.Forms.DataVisualization.Charting;

namespace DuplicateDetection
{
    public partial class Classification : Form
    {
        public string report;
        public Classification()
        {
            InitializeComponent();
        }

      
        private void Classification_Load(object sender, EventArgs e)
        {
            int totallength = 0;
            int matchlength = 0;
            int per = 0;

            int highlightrow = -1;
            try
            {
                
                StreamReader sr = new StreamReader(Application.StartupPath + "\\hashvalue.txt");
                string[] data = sr.ReadToEnd().ToString().Split('\n');
                sr.Close();
                
                for (int i = 0; i < data.Length; i++)
                {
                    List<string> doc1 = new List<string>();
                    

                    List<string> doc2 = new List<string>();
                    

                    string []content = data[i].Split('~');
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = content[0].ToString();
                    doc1.Add(content[0].ToString());
                    dataGridView1.Rows[i].Cells[1].Value = TrainFrm.inputcontent;

                    SHA1 md5 = new SHA1CryptoServiceProvider();
                    md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(TrainFrm.inputcontent));
                    byte[] result = md5.Hash;
                    StringBuilder strBuilder = new StringBuilder();
                    for (int j = 0; j < result.Length; j++)
                    {
                        strBuilder.Append(result[j].ToString("x2"));
                    }

                    dataGridView1.Rows[i].Cells[2].Value = strBuilder;

                    dataGridView1.Rows[i].Cells[3].Value = content[1].ToString();
                    doc2.Add(content[1].ToString());
                    //  dataGridView1.Rows[i].Cells[4].Value = Calc(doc1, doc2);
                    
                    if (content[0].Contains(TrainFrm.inputcontent) == true)
                    {
                        highlightrow = i;
                        totallength = content[0].Length;
                        matchlength = TrainFrm.inputcontent.Length;
                       // CalculateSimilarity(TrainFrm.inputcontent, content[0]);
                        per = (matchlength / totallength);
                        

                    }
                    dataGridView1.Rows[i].Cells[4].Value = CalcLevenshteinDistance(TrainFrm.inputcontent, content[0].ToString());
                    dataGridView1.Rows[i].Cells[5].Value = CalculateSimilarity(TrainFrm.inputcontent, content[0])*100;
                    dataGridView1.Sort(this.dataGridView1.Columns[5], ListSortDirection.Descending);
                }
                sr.Close();
                MessageBox.Show("Total Matching Percentage is " + per);
            }
            catch { }
            finally
            { }
            try { 
            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
            }
            catch { }
        }

        double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
        int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
        private static int CalcLevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (String.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
        public static double Calc(List<string> ls1, List<string> ls2)
        {
            HashSet<string> hs1 = new HashSet<string>(ls1);
            HashSet<string> hs2 = new HashSet<string>(ls2);
            return Calc(hs1, hs2);
        }
        public static double Calc(HashSet<string> hs1, HashSet<string> hs2)
        {
            return ((double)hs1.Intersect(hs2).Count() / (double)hs1.Union(hs2).Count());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
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

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // creating Excel Application  
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
               
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < dataGridView1.Rows.Count - 2; i++)
            {
                try { 
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                catch { }
            }
            // save the application  
            workbook.SaveAs(Application.StartupPath + "\\DuplicationSimilarity.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application  
            app.Quit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int actualValue;
            string pV;
            StreamReader sr = new StreamReader(Application.StartupPath + "\\hashvalue.txt");
            string[] data = sr.ReadToEnd().ToString().Split('\n');
            sr.Close();

            int truePositives = 0;
            int falsePositives = 0;
            int trueNegatives = 0;
            int falseNegatives = 0;

            double accuracy = 0;
            double precision = 0;
            double recall = 0;
            double f1Score = 0;

            for (int i = 0; i < data.Length-1; i++)
            {
                pV = dataGridView1.Rows[i].Cells[5].Value.ToString();
                double predictedValue = Double.Parse(pV);

                if (predictedValue > 75)
                {
                    actualValue = 100;
                }
                else
                {
                    actualValue = 0;
                }



                if (actualValue >= 90 && predictedValue >= 75)
                {
                    truePositives++;
                }
                else if (actualValue <= 10 && predictedValue >= 75)
                {
                    falsePositives++;
                }
                else if (actualValue <= 10 && predictedValue <= 75)
                {
                    trueNegatives++;
                }
                else if (actualValue >= 90 && predictedValue <= 75)
                {
                    falseNegatives++;
                }


                //int truePositives = (predictedLabel == 1 && actualLabel == 1) ? 1 : 0;
                //int falsePositives = (predictedLabel == 1 && actualLabel == 0) ? 1 : 0;
                //int trueNegatives = (predictedLabel == 0 && actualLabel == 0) ? 1 : 0;
                //int falseNegatives = (predictedLabel == 0 && actualLabel == 1) ? 1 : 0;

                // Calculate accuracy metrics
                accuracy = (double)(truePositives + trueNegatives) / (double)(truePositives + trueNegatives + falsePositives + falseNegatives);
                precision = (double)truePositives / (double)(truePositives + falsePositives);
                recall = (double)truePositives / (double)(truePositives + falseNegatives);
                f1Score = 2 * ((precision * recall) / (precision + recall));

            }






            Chart chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            Series series = new Series();
            series.ChartType = SeriesChartType.Bar;
            series.Points.AddXY("F1-Score", f1Score);
            series.Points.AddXY("Recall", recall * 100);
            series.Points.AddXY("Precision", precision * 100);
            series.Points.AddXY("Accuracy", accuracy *  100);
            chart.Series.Add(series);
            chart.Height = 400;
            chart.Width= 600;

            Form metricsForm = new Form();
            metricsForm.Text = "Classification Metrics and Confusion Matrix";
            metricsForm.WindowState = FormWindowState.Maximized;

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            metricsForm.Controls.Add(panel);

            var chartImage = new MemoryStream();
            chart.SaveImage(chartImage, ChartImageFormat.Png);
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Image = Image.FromStream(chartImage);
            panel.Controls.Add(pictureBox, 0, 0);

            

            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AllowUserToAddRows = false;


            DataGridView dgv1 = new DataGridView();
            dgv1.Dock = DockStyle.Fill;
            dgv1.AllowUserToAddRows = false;

            int[,] matrix = { { truePositives, falsePositives }, { falseNegatives, trueNegatives } };



            dgv.Columns.Add("Label", "Label");
            dgv.Columns.Add("Actual Positive", "Actual Positive");
            dgv.Columns.Add("Actual Negative", "Actual Negative");

            dgv.Rows.Add("Predicted Positive", matrix[0, 0], matrix[0, 1]);
            dgv.Rows.Add("Predicted Negative", matrix[1, 0], matrix[1, 1]);

            dgv1.Columns.Add("Label", "Metrics");
            dgv1.Columns.Add("Values", "Values");
            dgv1.Rows.Add("");
            // Add the rows to the DataGridView control
            dgv1.Rows.Add("True Positives", truePositives);
            dgv1.Rows.Add("False Positives", falsePositives);
            dgv1.Rows.Add("True Negatives", trueNegatives);
            dgv1.Rows.Add("False Negatives", falseNegatives);

            dgv1.Rows.Add("");

            dgv1.Rows.Add("Accuracy", accuracy * 100);
            dgv1.Rows.Add("Precision", precision * 100);
            dgv1.Rows.Add("Recall", recall * 100);
            dgv1.Rows.Add("F1 Score", f1Score);

            // Add the DataGridView control to your form
            panel.Controls.Add(dgv, 1, 0);
            panel.Controls.Add(dgv1, 0, 0);



            // Display the accuracy report
            //report = string.Format("Accuracy: {0:F2}%\nPrecision: {1:F2}%\nRecall: {2:F2}%\nF1 Score: {3:F2}%", accuracy * 100, precision * 100, recall * 100, f1Score);
            //richTextBox1.Text = report;
            //MessageBox.Show(report);
            metricsForm.Show();



        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView1.Rows[0].Cells[5].Value.ToString(), "Percentage");
        }
    }
}
