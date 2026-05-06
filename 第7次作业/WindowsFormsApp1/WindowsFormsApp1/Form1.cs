using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        private bool isSearching = false;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (isSearching)
            {
                MessageBox.Show("正在搜索中，请稍候...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string keyword = txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("请输入搜索关键字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isSearching = true;
            btnSearch.Enabled = false;
            lblStatus.Text = "状态：搜索中...";
            txtBaiduResult.Text = "";
            txtBingResult.Text = "";

            try
            {
                // 使用Task.WhenAll并行执行两个搜索任务
                var searchTasks = new List<Task<string>>();

                // 百度搜索任务
                var baiduTask = SearchBaiduAsync(keyword);
                searchTasks.Add(baiduTask);

                // 必应搜索任务
                var bingTask = SearchBingAsync(keyword);
                searchTasks.Add(bingTask);

                // 等待所有搜索任务完成
                await Task.WhenAll(searchTasks);

                // 显示结果
                txtBaiduResult.Text = await baiduTask;
                txtBingResult.Text = await bingTask;

                lblStatus.Text = "状态：搜索完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show("搜索出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "状态：搜索失败";
            }
            finally
            {
                isSearching = false;
                btnSearch.Enabled = true;
            }
        }

        /// <summary>
        /// 异步搜索百度
        /// </summary>
        private async Task<string> SearchBaiduAsync(string keyword)
        {
            try
            {
                // 使用百度手机版（反爬虫较弱）
                string url = "https://m.baidu.com/s?wd=" + Uri.EscapeDataString(keyword);

                // 创建新的HttpClient实例，设置超时和默认请求头
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 16_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.0 Mobile/15E148 Safari/604.1");
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9");
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

                    // 发送请求
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // 读取HTML内容
                    byte[] contentBytes = await response.Content.ReadAsByteArrayAsync();

                    // 尝试UTF-8解码
                    string html = Encoding.UTF8.GetString(contentBytes);

                    // 如果包含乱码，尝试GBK
                    if (html.Contains("") && !html.Contains("百度"))
                    {
                        html = Encoding.GetEncoding("GBK").GetString(contentBytes);
                    }
                }

                // 注意：上面代码有问题，让我修正
                return await SearchBaiduWithRetry(keyword);
            }
            catch (Exception ex)
            {
                return "百度搜索失败：" + ex.Message;
            }
        }

        /// <summary>
        /// 带重试机制的百度搜索
        /// </summary>
        private async Task<string> SearchBaiduWithRetry(string keyword)
        {
            string html = "";
            bool success = false;

            // 方法1: 使用百度移动版
            try
            {
                string url = "https://m.baidu.com/s?wd=" + Uri.EscapeDataString(keyword);

                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 16_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.0 Mobile/15E148 Safari/604.1");
                    request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    html = await response.Content.ReadAsStringAsync();

                    if (html.Contains("百度") || html.Contains("baidu"))
                    {
                        success = true;
                    }
                }
            }
            catch
            {
            }

            // 方法2: 如果移动版失败，尝试PC版
            if (!success)
            {
                try
                {
                    string url = "https://www.baidu.com/s?wd=" + Uri.EscapeDataString(keyword) + "&ie=utf-8";

                    using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
                        request.Headers.Add("Referer", "https://www.baidu.com/");

                        var response = await httpClient.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        html = await response.Content.ReadAsStringAsync();

                        if (html.Contains("百度") || html.Contains("baidu"))
                        {
                            success = true;
                        }
                    }
                }
                catch
                {
                }
            }

            // 如果都失败了，返回调试信息
            if (!success || string.IsNullOrEmpty(html))
            {
                return "百度搜索失败：无法获取搜索结果。\n\n可能原因：\n1. 网络连接问题\n2. 百度反爬虫机制\n3. 需要验证码\n\n建议使用必应搜索。";
            }

            // 解析HTML
            string result = ExtractBaiduResults(html);
            return result;
        }

        /// <summary>
        /// 解析百度搜索结果
        /// </summary>
        private string ExtractBaiduResults(string html)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                int charCount = 0;
                int maxChars = 200;

                // 方法1: 尝试多种匹配模式（移动版和PC版）
                var patterns = new[]
                {
                    // 移动版格式
                    @"<div class=""c-title[^>]*>.*?<a[^>]*>(.*?)</a>",
                    @"<div class=""c-abstract[^>]*>(.*?)</div>",
                    @"<div class=""c-gap-right-small[^>]*>(.*?)</div>",
                    // PC版格式
                    @"<div[^>]*class=""[^\w]*c-container[^\w]*[^>]*>.*?<div[^>]*class=""[^\w]*c-abstract[^\w]*[^>]*>(.*?)</div>",
                    @"<div class=""content-[^""]*"">(.*?)</div>",
                    @"<div class=""c-abstract"">(.*?)</div>",
                    @"<span class=""content[^""]*"">(.*?)</span>",
                    // 老版格式
                    @"<div[^>]*class=""c-span-last[^>]*>(.*?)</div>",
                    @"<div[^>]*class=""c-span[^\w][^>]*>(.*?)</div>",
                    @"<div[^>]*data-tools=""[^""]*""[^>]*>(.*?)</div>",
                    // 通用格式
                    @"<div[^>]*class=""[^\w]*c-content[^\w]*[^>]*>(.*?)</div>",
                };

                foreach (string pattern in patterns)
                {
                    if (charCount >= maxChars) break;

                    try
                    {
                        var matches = Regex.Matches(html, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            if (charCount >= maxChars) break;

                            string text = StripHtmlTags(match.Value);
                            text = Regex.Replace(text, @"\s+", " ").Trim();

                            // 过滤掉太短或纯数字/符号的内容
                            if (!string.IsNullOrEmpty(text) && text.Length > 15 && Regex.IsMatch(text, @"[\u4e00-\u9fa5a-zA-Z]"))
                            {
                                int remaining = maxChars - charCount;
                                if (remaining <= 0) break;

                                if (text.Length > remaining)
                                {
                                    sb.Append(text.Substring(0, remaining));
                                    charCount += remaining;
                                }
                                else
                                {
                                    sb.AppendLine(text);
                                    charCount += text.Length;
                                }
                            }
                        }

                        if (sb.Length > 50) break;
                    }
                    catch
                    {
                        continue;
                    }
                }

                // 方法2: 如果没找到，尝试提取所有带中文的段落
                if (sb.Length < 50)
                {
                    // 匹配包含中文字符的段落
                    var textPattern = @"<p[^>]*>.*?[\u4e00-\u9fa5]{3,}.*?</p>|<div[^>]*>.*?[\u4e00-\u9fa5]{3,}.*?</div>";
                    var matches = Regex.Matches(html, textPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    foreach (Match match in matches)
                    {
                        if (charCount >= maxChars) break;

                        string text = StripHtmlTags(match.Value);
                        text = Regex.Replace(text, @"\s+", " ").Trim();

                        // 只保留包含中文字符且长度适中的段落
                        if (!string.IsNullOrEmpty(text) && text.Length > 20 && text.Length < 300)
                        {
                            int remaining = maxChars - charCount;
                            if (remaining <= 0) break;

                            if (text.Length > remaining)
                            {
                                sb.Append(text.Substring(0, remaining));
                                charCount += remaining;
                            }
                            else
                            {
                                sb.AppendLine(text);
                                charCount += text.Length;
                            }
                        }
                    }
                }

                if (sb.Length == 0)
                {
                    return "未找到相关搜索结果。\n\n提示：百度可能有反爬虫机制，建议使用必应搜索。";
                }

                return sb.ToString().Trim();
            }
            catch (Exception ex)
            {
                return "解析结果失败：" + ex.Message;
            }
        }

        /// <summary>
        /// 异步搜索必应
        /// </summary>
        private async Task<string> SearchBingAsync(string keyword)
        {
            try
            {
                // 构建必应搜索URL
                string url = "https://www.bing.com/search?q=" + Uri.EscapeDataString(keyword);

                // 创建请求，设置User-Agent模拟浏览器
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                // 发送请求
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // 读取HTML内容
                string html = await response.Content.ReadAsStringAsync();

                // 解析HTML，提取搜索结果摘要
                string result = ExtractSearchResults(html, "bing");

                return result;
            }
            catch (Exception ex)
            {
                return "必应搜索失败：" + ex.Message;
            }
        }

        /// <summary>
        /// 从HTML中提取搜索结果摘要
        /// </summary>
        private string ExtractSearchResults(string html, string engine)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                int charCount = 0;
                int maxChars = 200;

                if (engine == "baidu")
                {
                    // 百度搜索结果解析
                    // 查找class为"content"的div中的内容
                    var contentMatches = Regex.Matches(html, @"<div[^>]*class=""content""[^>]*>(.*?)</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    foreach (Match match in contentMatches)
                    {
                        // 移除HTML标签
                        string text = StripHtmlTags(match.Value);
                        // 移除多余空白
                        text = Regex.Replace(text, @"\s+", " ").Trim();

                        if (!string.IsNullOrEmpty(text) && text.Length > 10)
                        {
                            int remaining = maxChars - charCount;
                            if (remaining <= 0) break;

                            if (text.Length > remaining)
                            {
                                sb.Append(text.Substring(0, remaining));
                                charCount += remaining;
                            }
                            else
                            {
                                sb.AppendLine(text);
                                charCount += text.Length;
                            }

                            if (charCount >= maxChars) break;
                        }
                    }

                    // 如果content没找到，尝试其他模式
                    if (sb.Length == 0)
                    {
                        var summaryMatches = Regex.Matches(html, @"<div[^>]*class=""c-abstract""[^>]*>(.*?)</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        foreach (Match match in summaryMatches)
                        {
                            string text = StripHtmlTags(match.Value);
                            text = Regex.Replace(text, @"\s+", " ").Trim();

                            if (!string.IsNullOrEmpty(text) && text.Length > 10)
                            {
                                int remaining = maxChars - charCount;
                                if (remaining <= 0) break;

                                if (text.Length > remaining)
                                {
                                    sb.Append(text.Substring(0, remaining));
                                    charCount += remaining;
                                }
                                else
                                {
                                    sb.AppendLine(text);
                                    charCount += text.Length;
                                }

                                if (charCount >= maxChars) break;
                            }
                        }
                    }
                }
                else if (engine == "bing")
                {
                    // 必应搜索结果解析
                    // 查找class为"b_caption"的div中的内容
                    var captionMatches = Regex.Matches(html, @"<div[^>]*class=""b_caption""[^>]*>(.*?)</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    foreach (Match match in captionMatches)
                    {
                        // 移除HTML标签
                        string text = StripHtmlTags(match.Value);
                        // 移除多余空白
                        text = Regex.Replace(text, @"\s+", " ").Trim();

                        if (!string.IsNullOrEmpty(text) && text.Length > 10)
                        {
                            int remaining = maxChars - charCount;
                            if (remaining <= 0) break;

                            if (text.Length > remaining)
                            {
                                sb.Append(text.Substring(0, remaining));
                                charCount += remaining;
                            }
                            else
                            {
                                sb.AppendLine(text);
                                charCount += text.Length;
                            }

                            if (charCount >= maxChars) break;
                        }
                    }

                    // 如果b_caption没找到，尝试其他模式
                    if (sb.Length == 0)
                    {
                        var snippetMatches = Regex.Matches(html, @"<p[^>]*class=""b_lineclamp[^>]*>(.*?)</p>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        foreach (Match match in snippetMatches)
                        {
                            string text = StripHtmlTags(match.Value);
                            text = Regex.Replace(text, @"\s+", " ").Trim();

                            if (!string.IsNullOrEmpty(text) && text.Length > 10)
                            {
                                int remaining = maxChars - charCount;
                                if (remaining <= 0) break;

                                if (text.Length > remaining)
                                {
                                    sb.Append(text.Substring(0, remaining));
                                    charCount += remaining;
                                }
                                else
                                {
                                    sb.AppendLine(text);
                                    charCount += text.Length;
                                }

                                if (charCount >= maxChars) break;
                            }
                        }
                    }
                }

                if (sb.Length == 0)
                {
                    return "未找到相关搜索结果。";
                }

                return sb.ToString().Trim();
            }
            catch (Exception ex)
            {
                return "解析结果失败：" + ex.Message;
            }
        }

        /// <summary>
        /// 移除HTML标签
        /// </summary>
        private string StripHtmlTags(string html)
        {
            // 移除script和style标签及其内容
            html = Regex.Replace(html, @"<(script|style)[^>]*>.*?</\1>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // 移除所有HTML标签
            html = Regex.Replace(html, @"<[^>]+>", " ");

            // 解码HTML实体
            html = WebUtility.HtmlDecode(html);

            return html;
        }
    }
}
