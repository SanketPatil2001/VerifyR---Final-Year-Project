using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Excel = Microsoft.Office.Interop.Excel;

namespace DuplicateDetection
{
    public partial class TrainFrm : Form
    {
        public static string inputcontent = "";
        public TrainFrm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Field cannot be empty!");
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "Field cannot be empty!");
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

     
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            inputcontent = richTextBox1.Text.Trim();

            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string pdfFilePath = textBox4.Text;

            // Specify the heading to extract text under
            string headingToExtract = "Title";
            string headingToExtract1 = "Publicatio n";
            string headingToExtract2 = "Author";
            string headingToExtract3 = "Content";

            // Open the PDF file
            PdfReader pdfReader = new PdfReader(pdfFilePath);

            // Loop through the pages in the PDF file
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                // Extract the text on the current page
                string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, page);

                // Check if the heading exists on the current page
                int headingIndex = pageText.IndexOf(headingToExtract);
                if (headingIndex != -1)
                {
                    // Get the text under the heading
                    int nextHeadingIndex = pageText.IndexOf("Publicatio n", headingIndex + 1);
                    string textUnderHeading;
                    if (nextHeadingIndex != -1)
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract.Length, nextHeadingIndex - (headingIndex + headingToExtract.Length));
                    }
                    else
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract.Length);
                    }

                    // Display the text under the heading in a textbox
                    textBox1.AppendText(textUnderHeading.Trim() + Environment.NewLine);

                    // Exit the loop once the heading is found
                    break;
                }
            }

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                // Extract the text on the current page
                string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, page);

                // Check if the heading exists on the current page
                int headingIndex = pageText.IndexOf(headingToExtract1);
                if (headingIndex != -1)
                {
                    // Get the text under the heading
                    int nextHeadingIndex = pageText.IndexOf("Author", headingIndex + 1);
                    string textUnderHeading;
                    if (nextHeadingIndex != -1)
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract1.Length, nextHeadingIndex - (headingIndex + headingToExtract1.Length));
                    }
                    else
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract1.Length);
                    }

                    // Display the text under the heading in a textbox
                    textBox2.AppendText(textUnderHeading.Trim() + Environment.NewLine);

                    // Exit the loop once the heading is found
                    break;
                }
            }

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                // Extract the text on the current page
                string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, page);

                // Check if the heading exists on the current page
                int headingIndex = pageText.IndexOf(headingToExtract2);
                if (headingIndex != -1)
                {
                    // Get the text under the heading
                    int nextHeadingIndex = pageText.IndexOf("Content", headingIndex + 1);
                    string textUnderHeading;
                    if (nextHeadingIndex != -1)
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract2.Length, nextHeadingIndex - (headingIndex + headingToExtract2.Length));
                    }
                    else
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract2.Length);
                    }

                    // Display the text under the heading in a textbox
                    textBox3.AppendText(textUnderHeading.Trim() + Environment.NewLine);

                    // Exit the loop once the heading is found
                    break;
                }
            }

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                // Extract the text on the current page
                string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, page);
                string pageText1 = "";
                if (Convert.ToBoolean(page + 1))
                {
                    pageText1 = PdfTextExtractor.GetTextFromPage(pdfReader, page + 1);
                }
                else
                {
                    pageText1 = "";

                }

                // Check if the heading exists on the current page
                int headingIndex = pageText.IndexOf(headingToExtract3);
                if (headingIndex != -1)
                {
                    // Get the text under the heading
                    int nextHeadingIndex = pageText.IndexOf("END", headingIndex + 1);
                    string textUnderHeading;
                    if (nextHeadingIndex != -1)
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract3.Length);
                    }
                    else
                    {
                        textUnderHeading = pageText.Substring(headingIndex + headingToExtract3.Length);
                    }

                    // Display the text under the heading in a textbox
                    richTextBox1.AppendText(textUnderHeading.Trim() + Environment.NewLine);

                    richTextBox1.AppendText(pageText1.Trim());

                    // Exit the loop once the heading is found
                    break;
                }
            }

            // Close the PDF reader
            pdfReader.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string textboxValue = textBox1.Text;
            string textboxValue1 = textBox2.Text;
            string textboxValue2 = textBox3.Text;
            string textboxValue3 = richTextBox1.Text;

            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Open(@"E:\Final Year Project\articles1_edited_copy");

            // Select the worksheet you want to modify
            Excel.Worksheet worksheet = workbook.Sheets["articles1_edited"];

            // Find the next empty row in the worksheet
            int row = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row + 1;

            // Write the textbox value to the worksheet
            worksheet.Cells[row, 3] = textboxValue;
            worksheet.Cells[row, 4] = textboxValue1;
            worksheet.Cells[row, 5] = textboxValue2;
            worksheet.Cells[row, 10] = textboxValue3;

            // Save and close the Excel file
            workbook.Save();
            workbook.Close();
            excel.Quit();
            MessageBox.Show("Article successfully added to Dataset");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                errorProvider1.SetError(richTextBox1, "Field cannot be empty!");
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }

 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Field cannot be empty!");
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }
    }
}
