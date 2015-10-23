namespace FBPTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Merge1 = new System.Windows.Forms.Button();
            this.TIQ = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.tbOut = new System.Windows.Forms.TextBox();
            this.tbOut2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.btnDO = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Merge1
            // 
            this.Merge1.Location = new System.Drawing.Point(74, 12);
            this.Merge1.Name = "Merge1";
            this.Merge1.Size = new System.Drawing.Size(111, 23);
            this.Merge1.TabIndex = 0;
            this.Merge1.Text = "Update";
            this.Merge1.UseVisualStyleBackColor = true;
            this.Merge1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TIQ
            // 
            this.TIQ.Location = new System.Drawing.Point(74, 65);
            this.TIQ.Name = "TIQ";
            this.TIQ.Size = new System.Drawing.Size(111, 23);
            this.TIQ.TabIndex = 1;
            this.TIQ.Text = "TestInfQueue";
            this.TIQ.UseVisualStyleBackColor = true;
            this.TIQ.Click += new System.EventHandler(this.TIQ_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(74, 125);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "MergeAndSort";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(74, 179);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "CopyFileToCons";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // tbOut
            // 
            this.tbOut.Location = new System.Drawing.Point(0, 0);
            this.tbOut.Name = "tbOut";
            this.tbOut.Size = new System.Drawing.Size(100, 20);
            this.tbOut.TabIndex = 0;
            // 
            // tbOut2
            // 
            this.tbOut2.Location = new System.Drawing.Point(0, 0);
            this.tbOut2.Name = "tbOut2";
            this.tbOut2.Size = new System.Drawing.Size(100, 20);
            this.tbOut2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(74, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Deadlock";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button6_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(74, 286);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "NoDeadlock";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(74, 341);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 26);
            this.button6.TabIndex = 7;
            this.button6.Text = "TestTune";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button8_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(255, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "TestPassthrus";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(255, 65);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(138, 23);
            this.button7.TabIndex = 9;
            this.button7.Text = "TSS";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(255, 125);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(138, 23);
            this.button8.TabIndex = 10;
            this.button8.Text = "TestArrayPorts";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.TAP_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(255, 179);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(138, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "TestDeadlockDetection";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(255, 236);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(138, 23);
            this.button12.TabIndex = 14;
            this.button12.Text = "TestNestedSubstreams";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(255, 286);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(138, 23);
            this.button13.TabIndex = 15;
            this.button13.Text = "TestSockets";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(255, 344);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(138, 23);
            this.button10.TabIndex = 16;
            this.button10.Text = "TestLoadBalancer";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(74, 397);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(111, 23);
            this.button14.TabIndex = 18;
            this.button14.Text = "Run Exe";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(255, 397);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(138, 23);
            this.button11.TabIndex = 19;
            this.button11.Text = "VolumeTest";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // btnDO
            // 
            this.btnDO.Location = new System.Drawing.Point(255, 450);
            this.btnDO.Name = "btnDO";
            this.btnDO.Size = new System.Drawing.Size(138, 23);
            this.btnDO.TabIndex = 20;
            this.btnDO.Text = "DropOldest";
            this.btnDO.UseVisualStyleBackColor = true;
            this.btnDO.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 500);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.btnDO);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.TIQ);
            this.Controls.Add(this.Merge1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        // tbOut, tbOut2, timer1 copied from FlowBaseForms by pm

        private System.Windows.Forms.Button Merge1;
        private System.Windows.Forms.Button TIQ;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox tbOut;
        //private System.Windows.Forms.Button btnGo;
       // private System.Windows.Forms.Button btnExit;
        // private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox tbOut2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button btnDO;  // dropOldest
    }
}

