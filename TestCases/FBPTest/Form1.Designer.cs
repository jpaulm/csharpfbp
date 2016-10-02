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
            string[] names = {
            "Update",
            "TestInfQueue",
            "MergeAndSort",
            "CopyFileToCons",
            "Deadlock",
            "NoDeadlock",
            "TestTune",
            "RunExe",
            "LBwRandDelay",
            "TestPassthrus",
            "TSS",
            "TestArrayPorts",
            "TestDeadlockDetection",
            "TestNestedSubstreams",
            "TestSockets",
            "TestLoadBalancer",
            "VolumeTest",
            "DropOldest"
            };

            System.Windows.Forms.Button [] buttons  = new System.Windows.Forms.Button [names.Length];
            int j = 0;
            int k = 0;

            for (int i = 0; i < names.Length; i++)
            {
                System.Windows.Forms.Button button = new System.Windows.Forms.Button();

                int m;
                if (i == 0 || i == 9)                                    
                    j = 12;               
                if (i >= 9)
                    m = 255;
                else
                    m = 74;
                button.Location = new System.Drawing.Point(m, j);
                button.Name = names[i];
                button.Size = new System.Drawing.Size(111, 23);
                button.TabIndex = k;
                button.Text = names[i];
                button.UseVisualStyleBackColor = true;
                buttons[i] = button;
                this.Controls.Add(buttons[i]);
                j += 60;
                k++;
            }

            //There may be a better way to do this!

            buttons[0].Click += new System.EventHandler(this.button1_Click);
            buttons[1].Click += new System.EventHandler(this.button2_Click);
            buttons[2].Click += new System.EventHandler(this.button3_Click);
            buttons[3].Click += new System.EventHandler(this.button4_Click);
            buttons[4].Click += new System.EventHandler(this.button5_Click);
            buttons[5].Click += new System.EventHandler(this.button6_Click);
            buttons[6].Click += new System.EventHandler(this.button7_Click);
            buttons[7].Click += new System.EventHandler(this.button8_Click);
            buttons[8].Click += new System.EventHandler(this.button9_Click);
            buttons[9].Click += new System.EventHandler(this.button10_Click);
            buttons[10].Click += new System.EventHandler(this.button11_Click);
            buttons[11].Click += new System.EventHandler(this.button12_Click);
            buttons[12].Click += new System.EventHandler(this.button13_Click);
            buttons[13].Click += new System.EventHandler(this.button14_Click);
            buttons[14].Click += new System.EventHandler(this.button15_Click);
            buttons[15].Click += new System.EventHandler(this.button16_Click);
            buttons[16].Click += new System.EventHandler(this.button17_Click);
            buttons[17].Click += new System.EventHandler(this.button18_Click);

            
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 650);
            
            
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
       
    }
}

