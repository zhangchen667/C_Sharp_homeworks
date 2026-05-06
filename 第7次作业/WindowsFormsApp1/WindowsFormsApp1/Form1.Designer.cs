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
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblBaidu = new System.Windows.Forms.Label();
            this.lblBing = new System.Windows.Forms.Label();
            this.txtBaiduResult = new System.Windows.Forms.TextBox();
            this.txtBingResult = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "搜索关键字：";
            //
            // txtKeyword
            //
            this.txtKeyword.Location = new System.Drawing.Point(110, 17);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(350, 23);
            this.txtKeyword.TabIndex = 1;
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(480, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // lblBaidu
            //
            this.lblBaidu.AutoSize = true;
            this.lblBaidu.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBaidu.ForeColor = System.Drawing.Color.Blue;
            this.lblBaidu.Location = new System.Drawing.Point(20, 70);
            this.lblBaidu.Name = "lblBaidu";
            this.lblBaidu.Size = new System.Drawing.Size(50, 19);
            this.lblBaidu.TabIndex = 3;
            this.lblBaidu.Text = "百度";
            //
            // lblBing
            //
            this.lblBing.AutoSize = true;
            this.lblBing.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBing.ForeColor = System.Drawing.Color.Green;
            this.lblBing.Location = new System.Drawing.Point(420, 70);
            this.lblBing.Name = "lblBing";
            this.lblBing.Size = new System.Drawing.Size(50, 19);
            this.lblBing.TabIndex = 4;
            this.lblBing.Text = "必应";
            //
            // txtBaiduResult
            //
            this.txtBaiduResult.Location = new System.Drawing.Point(20, 100);
            this.txtBaiduResult.Multiline = true;
            this.txtBaiduResult.Name = "txtBaiduResult";
            this.txtBaiduResult.ReadOnly = true;
            this.txtBaiduResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBaiduResult.Size = new System.Drawing.Size(370, 320);
            this.txtBaiduResult.TabIndex = 5;
            //
            // txtBingResult
            //
            this.txtBingResult.Location = new System.Drawing.Point(420, 100);
            this.txtBingResult.Multiline = true;
            this.txtBingResult.Name = "txtBingResult";
            this.txtBingResult.ReadOnly = true;
            this.txtBingResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBingResult.Size = new System.Drawing.Size(370, 320);
            this.txtBingResult.TabIndex = 6;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(600, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(80, 17);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "状态：就绪";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 440);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtBingResult);
            this.Controls.Add(this.txtBaiduResult);
            this.Controls.Add(this.lblBing);
            this.Controls.Add(this.lblBaidu);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "搜索引擎搜索工具";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblBaidu;
        private System.Windows.Forms.Label lblBing;
        private System.Windows.Forms.TextBox txtBaiduResult;
        private System.Windows.Forms.TextBox txtBingResult;
        private System.Windows.Forms.Label lblStatus;
    }
}

