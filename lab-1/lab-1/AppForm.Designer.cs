namespace COM
{
    partial class AppForm
    {
        // speeds (bits/sec) for comboBoxes of baud rate and reception
        private readonly static string[] speeds = {
            "8",
            "16",
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "9600",
            "10000",
            "12000",
            "14000",
            "16000",
            "18000",
            "20000"
        };

        private readonly static string DEFAULT_SERIAL_PORT_SPEED = "8";

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used. Destructor of form
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
        /// Sets default values of form elements
        /// </summary>
        private void InitializeComponent()
        {
            this.receiverGroupBox = new System.Windows.Forms.GroupBox();
            this.receiverTextBoxHeaderLabel = new System.Windows.Forms.Label();
            this.receptionSpeedComboBox = new System.Windows.Forms.ComboBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.receiverTextBox = new System.Windows.Forms.TextBox();
            this.receptionSpeedLabel = new System.Windows.Forms.Label();
            this.senderGroupBox = new System.Windows.Forms.GroupBox();
            this.senderTextBoxHeaderLabel = new System.Windows.Forms.Label();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.senderTextBox = new System.Windows.Forms.TextBox();
            this.baudRateLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.receiverGroupBox.SuspendLayout();
            this.senderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // receiverGroupBox
            // 
            this.receiverGroupBox.Controls.Add(this.receiverTextBoxHeaderLabel);
            this.receiverGroupBox.Controls.Add(this.receptionSpeedComboBox);
            this.receiverGroupBox.Controls.Add(this.clearButton);
            this.receiverGroupBox.Controls.Add(this.receiverTextBox);
            this.receiverGroupBox.Controls.Add(this.receptionSpeedLabel);
            this.receiverGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.receiverGroupBox.Location = new System.Drawing.Point(323, 12);
            this.receiverGroupBox.Name = "receiverGroupBox";
            this.receiverGroupBox.Size = new System.Drawing.Size(290, 270);
            this.receiverGroupBox.TabIndex = 0;
            this.receiverGroupBox.TabStop = false;
            this.receiverGroupBox.Text = "Receiver";
            // 
            // receiverTextBoxHeaderLabel
            // 
            this.receiverTextBoxHeaderLabel.AutoSize = true;
            this.receiverTextBoxHeaderLabel.Location = new System.Drawing.Point(7, 58);
            this.receiverTextBoxHeaderLabel.Name = "receiverTextBoxHeaderLabel";
            this.receiverTextBoxHeaderLabel.Size = new System.Drawing.Size(77, 13);
            this.receiverTextBoxHeaderLabel.TabIndex = 7;
            this.receiverTextBoxHeaderLabel.Text = "Received data";
            // 
            // receptionSpeedComboBox
            // 
            this.receptionSpeedComboBox.FormattingEnabled = true;
            this.receptionSpeedComboBox.Location = new System.Drawing.Point(104, 20);
            this.receptionSpeedComboBox.Name = "receptionSpeedComboBox";
            this.receptionSpeedComboBox.Size = new System.Drawing.Size(121, 21);
            this.receptionSpeedComboBox.TabIndex = 4;
            this.receptionSpeedComboBox.Items.AddRange(speeds);
            this.receptionSpeedComboBox.SelectedIndex =
                this.receptionSpeedComboBox.Items.IndexOf(DEFAULT_SERIAL_PORT_SPEED);
            this.receptionSpeedComboBox.SelectedIndexChanged +=
                new System.EventHandler(ReceptionSpeedComboBox_SelectedIndexChanged);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(6, 234);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // receiverTextBox
            // 
            this.receiverTextBox.Location = new System.Drawing.Point(6, 77);
            this.receiverTextBox.Multiline = true;
            this.receiverTextBox.Name = "receiverTextBox";
            this.receiverTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.receiverTextBox.Size = new System.Drawing.Size(278, 150);
            this.receiverTextBox.TabIndex = 2;
            // 
            // receptionSpeedLabel
            // 
            this.receptionSpeedLabel.AutoSize = true;
            this.receptionSpeedLabel.Location = new System.Drawing.Point(7, 23);
            this.receptionSpeedLabel.Name = "receptionSpeedLabel";
            this.receptionSpeedLabel.Size = new System.Drawing.Size(91, 13);
            this.receptionSpeedLabel.TabIndex = 0;
            this.receptionSpeedLabel.Text = "Reception speed:";
            // 
            // senderGroupBox
            // 
            this.senderGroupBox.Controls.Add(this.senderTextBoxHeaderLabel);
            this.senderGroupBox.Controls.Add(this.baudRateComboBox);
            this.senderGroupBox.Controls.Add(this.sendButton);
            this.senderGroupBox.Controls.Add(this.senderTextBox);
            this.senderGroupBox.Controls.Add(this.baudRateLabel);
            this.senderGroupBox.Location = new System.Drawing.Point(12, 12);
            this.senderGroupBox.Name = "senderGroupBox";
            this.senderGroupBox.Size = new System.Drawing.Size(290, 270);
            this.senderGroupBox.TabIndex = 1;
            this.senderGroupBox.TabStop = false;
            this.senderGroupBox.Text = "Sender";
            // 
            // senderTextBoxHeaderLabel
            // 
            this.senderTextBoxHeaderLabel.AutoSize = true;
            this.senderTextBoxHeaderLabel.Location = new System.Drawing.Point(9, 58);
            this.senderTextBoxHeaderLabel.Name = "senderTextBoxHeaderLabel";
            this.senderTextBoxHeaderLabel.Size = new System.Drawing.Size(85, 13);
            this.senderTextBoxHeaderLabel.TabIndex = 6;
            this.senderTextBoxHeaderLabel.Text = "Data for sending";
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Location = new System.Drawing.Point(68, 20);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(121, 21);
            this.baudRateComboBox.TabIndex = 5;
            this.baudRateComboBox.Items.AddRange(speeds);
            this.baudRateComboBox.SelectedIndex =
                this.baudRateComboBox.Items.IndexOf(DEFAULT_SERIAL_PORT_SPEED);
            this.baudRateComboBox.SelectedIndexChanged +=
                new System.EventHandler(BaudRateComboBox_SelectedIndexChanged);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(6, 234);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 2;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // senderTextBox
            // 
            this.senderTextBox.Location = new System.Drawing.Point(6, 77);
            this.senderTextBox.Multiline = true;
            this.senderTextBox.Name = "senderTextBox";
            this.senderTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.senderTextBox.Size = new System.Drawing.Size(278, 150);
            this.senderTextBox.TabIndex = 1;
            // 
            // baudRateLabel
            // 
            this.baudRateLabel.AutoSize = true;
            this.baudRateLabel.Location = new System.Drawing.Point(6, 20);
            this.baudRateLabel.Name = "baudRateLabel";
            this.baudRateLabel.Size = new System.Drawing.Size(56, 13);
            this.baudRateLabel.TabIndex = 0;
            this.baudRateLabel.Text = "Baud rate:";
            // 
            // exitButton
            // 
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exitButton.Location = new System.Drawing.Point(505, 311);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(108, 29);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // AppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 352);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.senderGroupBox);
            this.Controls.Add(this.receiverGroupBox);
            this.Name = "AppForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SerialPort";
            this.receiverGroupBox.ResumeLayout(false);
            this.receiverGroupBox.PerformLayout();
            this.senderGroupBox.ResumeLayout(false);
            this.senderGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox receiverGroupBox;
        private System.Windows.Forms.GroupBox senderGroupBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label receiverTextBoxHeaderLabel;
        private System.Windows.Forms.ComboBox receptionSpeedComboBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.TextBox receiverTextBox;
        private System.Windows.Forms.Label receptionSpeedLabel;
        private System.Windows.Forms.Label senderTextBoxHeaderLabel;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox senderTextBox;
        private System.Windows.Forms.Label baudRateLabel;
    }
}

