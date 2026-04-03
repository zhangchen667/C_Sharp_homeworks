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

            //初始化文件选择对话框（
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            openFileDialog1.Title = "请选择要合并的文本文件";
            openFileDialog1.RestoreDirectory = true;
        }
        //选择文件1按钮点击事件
        private void btnSelectFile1_Click(object sender, EventArgs e)//object是事件源，EventArgs是事件参数
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//如果用户点击了“确定”按钮,则执行以下代码
        //showDialog()方法会显示一个对话框，等待用户选择文件。当用户选择文件并点击“确定”按钮时，ShowDialog()方法会返回DialogResult.OK。
            {
                textFile1.Text = openFileDialog1.FileName;
            }
        }
        private void btnSelectFile2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textFile2.Text = openFileDialog1.FileName;//将用户选择的文件路径显示在textFile2文本框中
            }
        }
        
        private void btnMerge_Click(object sender, EventArgs e)
        {
            //合法性校验
            if (string.IsNullOrWhiteSpace(textFile1.Text) || string.IsNullOrWhiteSpace(textFile2.Text))
            {
                MessageBox.Show("请先选择两个需要合并的文本文件", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
             //MessageBox.Show()方法用于显示一个消息框，提示用户选择文件。
             //第一个参数是要显示的消息文本，第二个参数是消息框的标题，
             //第三个参数指定了消息框上显示的按钮（这里是OK按钮），第四个参数指定了消息框的图标（这里是警告图标）。
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
                //创建Data子目录
                string exeDir = Application.StartupPath; // 获取.exe所在目录,application.StartupPath属性返回包含应用程序的可执行文件的路径，不包括可执行文件的名称。
                string dataDir = Path.Combine(exeDir, "Data"); // 拼接Data目录路径

                // 目录不存在则自动创建（无需手动建）
                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }

                // 读取两个文件的全部内容
                string content1 = File.ReadAllText(textFile1.Text, Encoding.UTF8);
                string content2 = File.ReadAllText(textFile2.Text, Encoding.UTF8);

                //合并两个文件的内容
                // 加了分隔符方便区分两个文件；也可直接写 content1 + content2
                string mergedContent = $"{content1}{Environment.NewLine}{Environment.NewLine}" +
                                       $"以下是第二个文件内容{Environment.NewLine}{Environment.NewLine}" +
                                       $"{content2}";

                //  生成新文件名,用时间命名
                string newFileName = $"合并文件_{DateTime.Now:yyyyMMddHHmmss}.txt";
                //DateTime.Now:yyyyMMddHHmmss格式化字符串，表示以年月日时分秒的形式生成一个唯一的文件名，确保每次合并都会创建一个新的文件，而不会覆盖之前的文件。
                string newFilePath = Path.Combine(dataDir, newFileName);

                // 写入合并后的内容到新文件
                File.WriteAllText(newFilePath, mergedContent, Encoding.UTF8);

                //提示操作成功 
                MessageBox.Show($"文件合并成功！\n新文件已保存至：\n{newFilePath}",
                    "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // 捕获所有错误（权限不足、文件被占用等），避免程序崩溃
                MessageBox.Show($" 合并失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
