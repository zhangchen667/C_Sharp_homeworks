using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient httpClient = new HttpClient();

        // 中国手机号正则表达式：1开头，第二位3-9，共11位
        private static readonly Regex PhoneRegex = new Regex(
            @"1[3-9]\d{9}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        // 邮箱正则表达式
        private static readonly Regex EmailRegex = new Regex(
            @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnFetch_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(url) || url == "https://")
            {
                MessageBox.Show("请输入有效的URL地址！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 确保URL有协议前缀
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
                txtUrl.Text = url;
            }

            btnFetch.Enabled = false;
            btnFetch.Text = "获取中...";
            lstPhones.Items.Clear();
            lstEmails.Items.Clear();

            try
            {
                // 获取网页内容
                string htmlContent = await FetchWebContent(url);

                if (string.IsNullOrEmpty(htmlContent))
                {
                    MessageBox.Show("未能获取到网页内容，请检查URL是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 提取手机号码
                ExtractPhones(htmlContent);

                // 提取邮箱
                ExtractEmails(htmlContent);

                // 显示统计信息
                string message = $"提取完成！\n手机号码：{lstPhones.Items.Count} 个\n邮箱：{lstEmails.Items.Count} 个";
                MessageBox.Show(message, "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnFetch.Enabled = true;
                btnFetch.Text = "获取并提取";
            }
        }

        private async Task<string> FetchWebContent(string url)
        {
            try
            {
                // 设置用户代理，避免某些网站拒绝请求
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                // 获取网页内容
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // 读取内容
                string content = await response.Content.ReadAsStringAsync();

                // 简单的HTML解码，处理&nbsp;等实体
                content = System.Net.WebUtility.HtmlDecode(content);

                return content;
            }
            catch
            {
                return null;
            }
        }

        private void ExtractPhones(string content)
        {
            MatchCollection matches = PhoneRegex.Matches(content);

            // 使用HashSet去重
            HashSet<string> uniquePhones = new HashSet<string>();

            foreach (Match match in matches)
            {
                string phone = match.Value;
                uniquePhones.Add(phone);
            }

            foreach (string phone in uniquePhones)
            {
                lstPhones.Items.Add(FormatPhoneNumber(phone));
            }
        }

        private void ExtractEmails(string content)
        {
            MatchCollection matches = EmailRegex.Matches(content);

            // 使用HashSet去重
            HashSet<string> uniqueEmails = new HashSet<string>();

            foreach (Match match in matches)
            {
                string email = match.Value;
                uniqueEmails.Add(email);
            }

            foreach (string email in uniqueEmails.OrderBy(e => e))
            {
                lstEmails.Items.Add(email);
            }
        }

        private string FormatPhoneNumber(string phone)
        {
            // 格式化手机号：13812345678 -> 138 1234 5678
            if (phone.Length == 11)
            {
                return $"{phone.Substring(0, 3)} {phone.Substring(3, 4)} {phone.Substring(7, 4)}";
            }
            return phone;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
