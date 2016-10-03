using System;
using System.Collections.Generic;


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

            //There may be a better way to do this!

            List<EventHandler> clicks = new List<EventHandler>();
            clicks.Add(new EventHandler(this.button1_Click));
            clicks.Add(new EventHandler(this.button2_Click));
            clicks.Add(new EventHandler(this.button3_Click));
            clicks.Add(new EventHandler(this.button4_Click));
            clicks.Add(new EventHandler(this.button5_Click));
            clicks.Add(new EventHandler(this.button6_Click));
            clicks.Add(new EventHandler(this.button7_Click));
            clicks.Add(new EventHandler(this.button8_Click));
            clicks.Add(new EventHandler(this.button9_Click));
            clicks.Add(new EventHandler(this.button10_Click));
            clicks.Add(new EventHandler(this.button11_Click));
            clicks.Add(new EventHandler(this.button12_Click));
            clicks.Add(new EventHandler(this.button13_Click));
            clicks.Add(new EventHandler(this.button14_Click));
            clicks.Add(new EventHandler(this.button15_Click));
            clicks.Add(new EventHandler(this.button16_Click));
            clicks.Add(new EventHandler(this.button17_Click));
            clicks.Add(new EventHandler(this.button18_Click));

            System.Windows.Forms.Button [] buttons  = new System.Windows.Forms.Button [names.Length];
            int j = 0;            

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
                button.TabIndex = i;
                button.Text = names[i];
                button.UseVisualStyleBackColor = true;
                buttons[i] = button;
                buttons[i].Click += clicks[i];
                this.Controls.Add(buttons[i]);
                j += 60;
            }
            
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

