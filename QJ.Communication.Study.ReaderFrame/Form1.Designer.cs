namespace QJ.Communication.Study.HelloWorld
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.connect_btn = new System.Windows.Forms.Button();
            this.disconnect_btn = new System.Windows.Forms.Button();
            this.ip_tbox = new System.Windows.Forms.TextBox();
            this.port_tbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.readLen_tbox = new System.Windows.Forms.TextBox();
            this.readAddress_tbox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("新細明體", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(477, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "HelloWorld入門專案";
            // 
            // connect_btn
            // 
            this.connect_btn.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.connect_btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.connect_btn.Location = new System.Drawing.Point(38, 125);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(156, 65);
            this.connect_btn.TabIndex = 1;
            this.connect_btn.Text = "連接";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // disconnect_btn
            // 
            this.disconnect_btn.Font = new System.Drawing.Font("新細明體", 16F);
            this.disconnect_btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.disconnect_btn.Location = new System.Drawing.Point(201, 125);
            this.disconnect_btn.Name = "disconnect_btn";
            this.disconnect_btn.Size = new System.Drawing.Size(156, 65);
            this.disconnect_btn.TabIndex = 2;
            this.disconnect_btn.Text = "斷開";
            this.disconnect_btn.UseVisualStyleBackColor = true;
            this.disconnect_btn.Click += new System.EventHandler(this.disconnect_btn_Click);
            // 
            // ip_tbox
            // 
            this.ip_tbox.Location = new System.Drawing.Point(38, 73);
            this.ip_tbox.Name = "ip_tbox";
            this.ip_tbox.Size = new System.Drawing.Size(224, 46);
            this.ip_tbox.TabIndex = 3;
            this.ip_tbox.Text = "127.0.0.1";
            // 
            // port_tbox
            // 
            this.port_tbox.Location = new System.Drawing.Point(268, 73);
            this.port_tbox.Name = "port_tbox";
            this.port_tbox.Size = new System.Drawing.Size(89, 46);
            this.port_tbox.TabIndex = 4;
            this.port_tbox.Text = "502";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(123, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(278, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 32);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connect_btn);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.disconnect_btn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ip_tbox);
            this.groupBox1.Controls.Add(this.port_tbox);
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 16F);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Location = new System.Drawing.Point(21, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 221);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "連線配置";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(730, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 65);
            this.button1.TabIndex = 7;
            this.button1.Text = "讀取";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "0x",
            "1x",
            "3x",
            "4x"});
            this.comboBox1.Location = new System.Drawing.Point(23, 45);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 40);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.Text = "4x";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.readLen_tbox);
            this.groupBox2.Controls.Add(this.readAddress_tbox);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Font = new System.Drawing.Font("新細明體", 16F);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox2.Location = new System.Drawing.Point(21, 332);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(876, 100);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "讀取參數";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "有符號整數16位元",
            "無符號整數16位元",
            "有符號整數32位元",
            "無符號整數32位元",
            "有符號整數64位元",
            "無符號整數64位元",
            "單精度福點數",
            "雙精度福點數"});
            this.comboBox2.Location = new System.Drawing.Point(406, 39);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(298, 40);
            this.comboBox2.TabIndex = 10;
            this.comboBox2.Text = "有符號整數16位元";
            // 
            // readLen_tbox
            // 
            this.readLen_tbox.Location = new System.Drawing.Point(284, 39);
            this.readLen_tbox.Name = "readLen_tbox";
            this.readLen_tbox.Size = new System.Drawing.Size(89, 46);
            this.readLen_tbox.TabIndex = 9;
            this.readLen_tbox.Text = "0";
            // 
            // readAddress_tbox
            // 
            this.readAddress_tbox.Location = new System.Drawing.Point(173, 39);
            this.readAddress_tbox.Name = "readAddress_tbox";
            this.readAddress_tbox.Size = new System.Drawing.Size(89, 46);
            this.readAddress_tbox.TabIndex = 7;
            this.readAddress_tbox.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(909, 644);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "QJ.Communication.Study";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Button disconnect_btn;
        private System.Windows.Forms.TextBox ip_tbox;
        private System.Windows.Forms.TextBox port_tbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox readLen_tbox;
        private System.Windows.Forms.TextBox readAddress_tbox;
    }
}

