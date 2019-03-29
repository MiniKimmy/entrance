/// <summary>
/// Controller的View层
/// </summary>
namespace EntranceGuard
{
    partial class Controller
    {
        /// <summary>
        /// Required designer variable.拥有的设计器变量.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.清理所有使用过的资源.
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
            this.text_ControllerSN = new System.Windows.Forms.Label();
            this.label_ControllerIP = new System.Windows.Forms.Label();
            this.text_ControllerIP = new System.Windows.Forms.Label();
            this.label_ServerIP = new System.Windows.Forms.Label();
            this.label_ServerPort = new System.Windows.Forms.Label();
            this.input_ServerPort = new System.Windows.Forms.TextBox();
            this.input_ServerIP = new System.Windows.Forms.TextBox();
            this.TestOpen = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchControllerSN
            // 
            this.SearchControllerSN.Location = new System.Drawing.Point(27, 21);
            this.SearchControllerSN.Name = "SearchControllerSN";
            this.SearchControllerSN.Size = new System.Drawing.Size(180, 23);
            this.SearchControllerSN.TabIndex = 0;
            this.SearchControllerSN.Text = "search controllerSN and IP";
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
            this.consoleBox.Location = new System.Drawing.Point(27, 369);
            this.consoleBox.Multiline = true;
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleBox.Size = new System.Drawing.Size(544, 48);
            this.consoleBox.TabIndex = 7;
            // 
            // text_ControllerSN
            // 
            this.text_ControllerSN.AutoSize = true;
            this.text_ControllerSN.Location = new System.Drawing.Point(302, 26);
            this.text_ControllerSN.Name = "text_ControllerSN";
            this.text_ControllerSN.Size = new System.Drawing.Size(0, 12);
            this.text_ControllerSN.TabIndex = 8;
            // 
            // label_ControllerIP
            // 
            this.label_ControllerIP.AutoSize = true;
            this.label_ControllerIP.Location = new System.Drawing.Point(213, 38);
            this.label_ControllerIP.Name = "label_ControllerIP";
            this.label_ControllerIP.Size = new System.Drawing.Size(83, 12);
            this.label_ControllerIP.TabIndex = 9;
            this.label_ControllerIP.Text = "Controller IP";
            // 
            // text_ControllerIP
            // 
            this.text_ControllerIP.AutoSize = true;
            this.text_ControllerIP.Location = new System.Drawing.Point(302, 38);
            this.text_ControllerIP.Name = "text_ControllerIP";
            this.text_ControllerIP.Size = new System.Drawing.Size(95, 12);
            this.text_ControllerIP.TabIndex = 10;
            this.text_ControllerIP.Text = "xxx.xxx.xxx.xxx";
            // 
            // label_ServerIP
            // 
            this.label_ServerIP.AutoSize = true;
            this.label_ServerIP.Location = new System.Drawing.Point(213, 55);
            this.label_ServerIP.Name = "label_ServerIP";
            this.label_ServerIP.Size = new System.Drawing.Size(59, 12);
            this.label_ServerIP.TabIndex = 11;
            this.label_ServerIP.Text = "Server IP";
            // 
            // label_ServerPort
            // 
            this.label_ServerPort.AutoSize = true;
            this.label_ServerPort.Location = new System.Drawing.Point(213, 82);
            this.label_ServerPort.Name = "label_ServerPort";
            this.label_ServerPort.Size = new System.Drawing.Size(71, 12);
            this.label_ServerPort.TabIndex = 12;
            this.label_ServerPort.Text = "Server Port";
            // 
            // input_ServerPort
            // 
            this.input_ServerPort.Location = new System.Drawing.Point(304, 82);
            this.input_ServerPort.Name = "input_ServerPort";
            this.input_ServerPort.Size = new System.Drawing.Size(102, 21);
            this.input_ServerPort.TabIndex = 13;
            // 
            // input_ServerIP
            // 
            this.input_ServerIP.Location = new System.Drawing.Point(304, 55);
            this.input_ServerIP.Name = "input_ServerIP";
            this.input_ServerIP.Size = new System.Drawing.Size(102, 21);
            this.input_ServerIP.TabIndex = 14;
            // 
            // TestOpen
            // 
            this.TestOpen.Location = new System.Drawing.Point(27, 71);
            this.TestOpen.Name = "TestOpen";
            this.TestOpen.Size = new System.Drawing.Size(75, 23);
            this.TestOpen.TabIndex = 15;
            this.TestOpen.Text = "testopen";
            this.TestOpen.UseVisualStyleBackColor = true;
            this.TestOpen.Click += new System.EventHandler(this.TestOpen_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(27, 109);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(544, 229);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(108, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "opencv";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Controller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TestOpen);
            this.Controls.Add(this.input_ServerIP);
            this.Controls.Add(this.input_ServerPort);
            this.Controls.Add(this.label_ServerPort);
            this.Controls.Add(this.label_ServerIP);
            this.Controls.Add(this.text_ControllerIP);
            this.Controls.Add(this.label_ControllerIP);
            this.Controls.Add(this.text_ControllerSN);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.label_ControllerSN);
            this.Controls.Add(this.SearchControllerSN);
            this.Name = "Controller";
            this.Text = "Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Controller_Destroy);
            this.Load += new System.EventHandler(this.Controller_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchControllerSN;
        private System.Windows.Forms.Label label_ControllerSN;
        private System.Windows.Forms.TextBox consoleBox;
        private System.Windows.Forms.Label text_ControllerSN;
        private System.Windows.Forms.Label label_ControllerIP;
        private System.Windows.Forms.Label text_ControllerIP;
        private System.Windows.Forms.Label label_ServerIP;
        private System.Windows.Forms.Label label_ServerPort;
        private System.Windows.Forms.TextBox input_ServerPort;
        private System.Windows.Forms.TextBox input_ServerIP;
        private System.Windows.Forms.Button TestOpen;

        private string controllerSN = null;
        public string ControllerSN { get { return controllerSN; } private set { controllerSN = value; text_ControllerSN.Text = controllerSN; } }

        private string controllerIP = null;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;

        public string ControllerIP { get { return controllerIP; } private set { controllerIP = value; text_ControllerIP.Text = controllerIP; } }

        public string Input_ServerIP { get { return input_ServerIP.Text; } }
        public string Input_ServerPort { get { return input_ServerPort.Text; } }

        public System.Windows.Forms.TextBox ConsoleBox { get { return consoleBox; } }
    }
}