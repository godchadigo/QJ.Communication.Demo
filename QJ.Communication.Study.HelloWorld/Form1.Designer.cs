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
            this.read_btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.autoRefrresh_chechbox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
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
            this.connect_btn.Location = new System.Drawing.Point(55, 128);
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
            this.disconnect_btn.Location = new System.Drawing.Point(218, 128);
            this.disconnect_btn.Name = "disconnect_btn";
            this.disconnect_btn.Size = new System.Drawing.Size(156, 65);
            this.disconnect_btn.TabIndex = 2;
            this.disconnect_btn.Text = "斷開";
            this.disconnect_btn.UseVisualStyleBackColor = true;
            this.disconnect_btn.Click += new System.EventHandler(this.disconnect_btn_Click);
            // 
            // ip_tbox
            // 
            this.ip_tbox.Location = new System.Drawing.Point(55, 76);
            this.ip_tbox.Name = "ip_tbox";
            this.ip_tbox.Size = new System.Drawing.Size(224, 46);
            this.ip_tbox.TabIndex = 3;
            this.ip_tbox.Text = "127.0.0.1";
            // 
            // port_tbox
            // 
            this.port_tbox.Location = new System.Drawing.Point(285, 76);
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
            this.label2.Location = new System.Drawing.Point(140, 41);
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
            this.label3.Location = new System.Drawing.Point(295, 41);
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
            this.groupBox1.Location = new System.Drawing.Point(21, 161);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 221);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "連線配置";
            // 
            // read_btn
            // 
            this.read_btn.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.read_btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.read_btn.Location = new System.Drawing.Point(16, 45);
            this.read_btn.Name = "read_btn";
            this.read_btn.Size = new System.Drawing.Size(414, 65);
            this.read_btn.TabIndex = 7;
            this.read_btn.Text = "讀取4x0 10筆數據";
            this.read_btn.UseVisualStyleBackColor = true;
            this.read_btn.Click += new System.EventHandler(this.read_btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.autoRefrresh_chechbox);
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Controls.Add(this.read_btn);
            this.groupBox2.Font = new System.Drawing.Font("新細明體", 16F);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox2.Location = new System.Drawing.Point(21, 388);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(451, 716);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作選項";
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(16, 163);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(414, 531);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // autoRefrresh_chechbox
            // 
            this.autoRefrresh_chechbox.AutoSize = true;
            this.autoRefrresh_chechbox.Location = new System.Drawing.Point(16, 121);
            this.autoRefrresh_chechbox.Name = "autoRefrresh_chechbox";
            this.autoRefrresh_chechbox.Size = new System.Drawing.Size(168, 36);
            this.autoRefrresh_chechbox.TabIndex = 9;
            this.autoRefrresh_chechbox.Text = "自動刷新";
            this.autoRefrresh_chechbox.UseVisualStyleBackColor = true;
            this.autoRefrresh_chechbox.CheckedChanged += new System.EventHandler(this.autoRefrresh_chechbox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.Font = new System.Drawing.Font("新細明體", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(42, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(419, 52);
            this.label4.TabIndex = 9;
            this.label4.Text = "(ModbusTCP範例)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(494, 1127);
            this.Controls.Add(this.label4);
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
        private System.Windows.Forms.Button read_btn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.CheckBox autoRefrresh_chechbox;
        private System.Windows.Forms.Label label4;
    }
}

