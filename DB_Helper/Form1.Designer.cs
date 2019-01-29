namespace DB_Helper {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.testType = new System.Windows.Forms.ComboBox();
            this.waferListIn = new System.Windows.Forms.ListBox();
            this.add = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // testType
            // 
            this.testType.FormattingEnabled = true;
            this.testType.Items.AddRange(new object[] {
            "Initial",
            "After"});
            this.testType.Location = new System.Drawing.Point(159, 45);
            this.testType.Name = "testType";
            this.testType.Size = new System.Drawing.Size(125, 20);
            this.testType.TabIndex = 0;
            // 
            // waferListIn
            // 
            this.waferListIn.FormattingEnabled = true;
            this.waferListIn.ItemHeight = 12;
            this.waferListIn.Location = new System.Drawing.Point(290, 45);
            this.waferListIn.Name = "waferListIn";
            this.waferListIn.Size = new System.Drawing.Size(218, 184);
            this.waferListIn.TabIndex = 1;
            // 
            // add
            // 
            this.add.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.add.Location = new System.Drawing.Point(90, 72);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(134, 34);
            this.add.TabIndex = 2;
            this.add.Text = "Add Wafer";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(137, 21);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            label1.Location = new System.Drawing.Point(36, 23);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 19);
            label1.TabIndex = 4;
            label1.Text = "Wafer ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            label2.Location = new System.Drawing.Point(170, 23);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(99, 19);
            label2.TabIndex = 4;
            label2.Text = "Test Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            label3.Location = new System.Drawing.Point(344, 23);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 19);
            label3.TabIndex = 4;
            label3.Text = "Wafer List";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 304);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.add);
            this.Controls.Add(this.waferListIn);
            this.Controls.Add(this.testType);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox testType;
        private System.Windows.Forms.ListBox waferListIn;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.TextBox textBox1;
    }
}

