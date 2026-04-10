using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // 初始化文件选择对话框
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            openFileDialog1.Title = "请选择要合并的文本文件";
            openFileDialog1.RestoreDirectory = true;
        }
        private void btnSelectFile1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textFile1.Text = openFileDialog1.FileName;
            }
        }

        private void btnSelectFile2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textFile2.Text = openFileDialog1.FileName;
            }
        }
        
        private void btnMerge_Click(object sender, EventArgs e)
        {
            // 合法性校验
            if (string.IsNullOrWhiteSpace(textFile1.Text) || string.IsNullOrWhiteSpace(textFile2.Text))
            {
                MessageBox.Show("请先选择两个需要合并的文本文件", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(textFile1.Text))
            {
                MessageBox.Show($"文件1不存在：{textFile1.Text}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(textFile2.Text))
            {
                MessageBox.Show($"文件2不存在：{textFile2.Text}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 创建Data子目录
                string exeDir = Application.StartupPath;
                string dataDir = Path.Combine(exeDir, "Data");

                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }

                // 读取两个文件的全部内容
                string content1 = File.ReadAllText(textFile1.Text, Encoding.UTF8);
                string content2 = File.ReadAllText(textFile2.Text, Encoding.UTF8);

                // 合并两个文件的内容
                string mergedContent = $"{content1}{Environment.NewLine}{Environment.NewLine}" +
                                       $"======== 以下是第二个文件内容 ======== {Environment.NewLine}{Environment.NewLine}" +
                                       $"{content2}";

                // 生成新文件名（用时间命名）
                string newFileName = $"合并文件_{DateTime.Now:yyyyMMddHHmmss}.txt";
                string newFilePath = Path.Combine(dataDir, newFileName);

                // 写入合并后的内容到新文件
                File.WriteAllText(newFilePath, mergedContent, Encoding.UTF8);

                // 提示操作成功
                MessageBox.Show($"✓ 文件合并成功！\n\n保存位置：\n{newFilePath}",
                    "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"合并失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
