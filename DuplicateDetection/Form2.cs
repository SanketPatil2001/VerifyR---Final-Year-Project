using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuplicateDetection
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmClassification fc = new FrmClassification();
            fc.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmTrain ft = new FrmTrain();
            ft.ShowDialog();
        }
    }
}
