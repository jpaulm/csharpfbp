using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Components;
using FBPLib;

namespace Concord
{
    public partial class Form1 : Form
    {
        Network net = new NetDefn();

        public Form1()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
           
            tbOut.Clear();
            tbOut2.Clear();
            tbOut3.Clear();

            try
            {
                net.parms = new TextBox[] { tbOut, tbOut2, tbOut3 };                
                net.Go();
                // timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");
            }
        }



        private void btnStop_Click(object sender, EventArgs e)
        {
            //net.Stop();
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }    
    
}
