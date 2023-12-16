using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuplicateDetection
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private void preprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            TextPreprocessing tp = new TextPreprocessing();
            tp.TopLevel = false;
            tp.Dock = DockStyle.Fill;
            tp.WindowState = FormWindowState.Maximized;
            this.panel2.Controls.Add(tp);
            tp.Show();
        }

        private void classificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            Classification cs = new Classification();
            cs.TopLevel = false;
            cs.Dock = DockStyle.Fill;
            cs.WindowState = FormWindowState.Maximized;
            this.panel2.Controls.Add(cs);
            cs.Show();
        }

        private void precisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void probabilitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            CountProbabilities cp = new CountProbabilities();
            cp.TopLevel = false;
            cp.Dock = DockStyle.Fill;
            cp.WindowState = FormWindowState.Maximized;
            this.panel2.Controls.Add(cp);
            cp.Show();
        }

        private void probability1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.panel2.Controls.Clear();
            //Probability1 p1 = new Probability1();
            //p1.TopLevel = false;
            //p1.Dock = DockStyle.Fill;
            //p1.WindowState = FormWindowState.Maximized;
            //this.panel2.Controls.Add(p1);
            //p1.Show();

        }

        private void graphResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void onlineDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            OnlineTweets cp = new OnlineTweets();
            cp.TopLevel = false;
            cp.Dock = DockStyle.Fill;
            cp.WindowState = FormWindowState.Maximized;
            this.panel2.Controls.Add(cp);
            cp.Show();
        }

        private void inputArticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            TrainFrm cp = new TrainFrm();
            cp.TopLevel = false;
            cp.Dock = DockStyle.Fill;
            cp.WindowState = FormWindowState.Maximized;
            this.panel2.Controls.Add(cp);
            cp.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
