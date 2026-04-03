namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textFile1 = new System.Windows.Forms.TextBox();
            this.textFile2 = new System.Windows.Forms.TextBox();
            this.btnSelectFile1 = new System.Windows.Forms.Button();
            this.btnSelectFile2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "文件2";
            // 
            // textFile1
            // 
            this.textFile1.Location = new System.Drawing.Point(147, 22);
            this.textFile1.Name = "textFile1";
            this.textFile1.ReadOnly = true;
            this.textFile1.Size = new System.Drawing.Size(300, 28);
            this.textFile1.TabIndex = 2;
            // 
            // textFile2
            // 
            this.textFile2.Location = new System.Drawing.Point(147, 66);
            this.textFile2.Name = "textFile2";
            this.textFile2.ReadOnly = true;
            this.textFile2.Size = new System.Drawing.Size(300, 28);
            this.textFile2.TabIndex = 3;
            // 
            // btnSelectFile1
            // 
            this.btnSelectFile1.Location = new System.Drawing.Point(542, 26);
            this.btnSelectFile1.Name = "btnSelectFile1";
            this.btnSelectFile1.Size = new System.Drawing.Size(100, 28);
            this.btnSelectFile1.TabIndex = 4;
            this.btnSelectFile1.Text = "选择文件1";
            this.btnSelectFile1.UseVisualStyleBackColor = true;
            this.btnSelectFile1.Click += new System.EventHandler(this.btnSelectFile1_Click);
            // 
            // btnSelectFile2
            // 
            this.btnSelectFile2.Location = new System.Drawing.Point(542, 71);
            this.btnSelectFile2.Name = "btnSelectFile2";
            this.btnSelectFile2.Size = new System.Drawing.Size(100, 28);
            this.btnSelectFile2.TabIndex = 5;
            this.btnSelectFile2.Text = "选择文件2";
            this.btnSelectFile2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(287, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 50);
            this.button1.TabIndex = 6;
            this.button1.Text = "合并";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "` 文本文件 (*.txt)|*.txt|所有文件 (.)|.`";
            this.openFileDialog1.Title = "对话框标题";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSelectFile2);
            this.Controls.Add(this.btnSelectFile1);
            this.Controls.Add(this.textFile2);
            this.Controls.Add(this.textFile1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textFile1;
        private System.Windows.Forms.TextBox textFile2;
        private System.Windows.Forms.Button btnSelectFile1;
        private System.Windows.Forms.Button btnSelectFile2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

