namespace EntranceGuard
{
    partial class Controller
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
            this.SearchControllerSN = new System.Windows.Forms.Button();
            this.label_ControllerSN = new System.Windows.Forms.Label();
            this.consoleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SearchControllerSN
            // 
            this.SearchControllerSN.Location = new System.Drawing.Point(27, 21);
            this.SearchControllerSN.Name = "SearchControllerSN";
            this.SearchControllerSN.Size = new System.Drawing.Size(151, 23);
            this.SearchControllerSN.TabIndex = 0;
            this.SearchControllerSN.Text = "搜索控制器SN";
            this.SearchControllerSN.UseVisualStyleBackColor = true;
            this.SearchControllerSN.Click += new System.EventHandler(this.SearchControllerSN_Click);
            // 
            // label_ControllerSN
            // 
            this.label_ControllerSN.AutoSize = true;
            this.label_ControllerSN.Location = new System.Drawing.Point(213, 26);
            this.label_ControllerSN.Name = "label_ControllerSN";
            this.label_ControllerSN.Size = new System.Drawing.Size(83, 12);
            this.label_ControllerSN.TabIndex = 1;
            this.label_ControllerSN.Text = "Controller SN";
            // 
            // consoleBox
            // 
            this.consoleBox.Location = new System.Drawing.Point(27, 91);
            this.consoleBox.Multiline = true;
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.Size = new System.Drawing.Size(544, 326);
            this.consoleBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(311, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "123456789";
            // 
            // Controller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.label_ControllerSN);
            this.Controls.Add(this.SearchControllerSN);
            this.Name = "Controller";
            this.Text = "Controller";
            this.Load += new System.EventHandler(this.Controller_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchControllerSN;
        private System.Windows.Forms.Label label_ControllerSN;
        private System.Windows.Forms.TextBox consoleBox;
        private System.Windows.Forms.Label label1;

        public System.Windows.Forms.TextBox ConsoleBox { get { return consoleBox; } }
    }
}