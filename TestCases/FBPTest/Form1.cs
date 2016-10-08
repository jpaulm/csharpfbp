using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TestNetworks.Networks;
//using FBPLib;


namespace  FBPTest
{
    
    internal partial class Form1 : Form
    {
        
        
        internal Form1()
        {
            InitializeComponent();
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            new Update().Go();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            new TestInfQueue().Go();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new MergeAndSort().Go();
        }
               

        private void button4_Click(object sender, EventArgs e)
        {

            new CopyFileToCons().Go();
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            new Deadlock().Go();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new NoDeadlock().Go();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new TestLoadBalanceWithSubstreams().Go();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            new TestRunExe().Go();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            new TestLoadBalanceWRandDelay().Go();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            new TestPassthrus().Go();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            new TSS().Go();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            new TestArrayPorts().Go();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            new TestDeadlockDetection().Go();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            new TestNestedSubstreams().Go();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            new TestSockets().Go();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            new TestLoadBalancer().Go();
        }
        
        private void button17_Click(object sender, EventArgs e)
        {
            new VolumeTest().Go();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            new DropOldest().Go();
        }

        

    }
        
    
}
