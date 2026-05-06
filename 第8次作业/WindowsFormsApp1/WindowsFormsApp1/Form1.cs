using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SQLiteConnection connection;
        private List<Word> words;
        private int currentIndex;
        private int correctCount;
        private int totalCount;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDatabase();
            LoadWords();
            ShowNextWord();
        }

        private void InitializeDatabase()
        {
            string dbPath = Path.Combine(Application.StartupPath, "words.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            connection = new SQLiteConnection(connectionString);

            // 创建数据库表
            connection.Open();
            string createTableSql = @"
                CREATE TABLE IF NOT EXISTS Words (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    English TEXT NOT NULL,
                    Chinese TEXT NOT NULL
                )";

            using (SQLiteCommand command = new SQLiteCommand(createTableSql, connection))
            {
                command.ExecuteNonQuery();
            }

            // 插入示例数据
            string insertDataSql = @"
                INSERT OR IGNORE INTO Words (English, Chinese) VALUES
                ('apple', '苹果'),
                ('banana', '香蕉'),
                ('orange', '橙子'),
                ('grape', '葡萄'),
                ('watermelon', '西瓜'),
                ('computer', '电脑'),
                ('keyboard', '键盘'),
                ('mouse', '鼠标'),
                ('monitor', '显示器'),
                ('program', '程序'),
                ('database', '数据库'),
                ('network', '网络'),
                ('internet', '互联网'),
                ('software', '软件'),
                ('hardware', '硬件')";

            using (SQLiteCommand command = new SQLiteCommand(insertDataSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void LoadWords()
        {
            words = new List<Word>();
            string sql = "SELECT English, Chinese FROM Words";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    words.Add(new Word
                    {
                        English = reader.GetString(0),
                        Chinese = reader.GetString(1)
                    });
                }
            }

            // 随机打乱顺序
            Random rng = new Random();
            int n = words.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Word value = words[k];
                words[k] = words[n];
                words[n] = value;
            }
        }

        private void ShowNextWord()
        {
            if (currentIndex < words.Count)
            {
                lblQuestion.Text = words[currentIndex].Chinese;
                txtAnswer.Text = "";
                txtAnswer.Focus();
                lblResult.Text = "";
                lblResult.ForeColor = Color.Black;
            }
            else
            {
                lblQuestion.Text = "练习完成！";
                txtAnswer.Enabled = false;
                btnNext.Enabled = false;
                lblResult.Text = "";
            }

            UpdateStats();
        }

        private void UpdateStats()
        {
            lblStats.Text = $"进度: {currentIndex}/{words.Count} | 正确: {correctCount} | 错误: {totalCount - correctCount}";
        }

        private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckAnswer();
            }
        }

        private void CheckAnswer()
        {
            if (currentIndex >= words.Count)
                return;

            string userAnswer = txtAnswer.Text.Trim().ToLower();
            string correctAnswer = words[currentIndex].English.ToLower();

            totalCount++;

            if (userAnswer == correctAnswer)
            {
                correctCount++;
                lblResult.Text = "正确!";
                lblResult.ForeColor = Color.Green;
            }
            else
            {
                lblResult.Text = "错误! 正确答案: " + words[currentIndex].English;
                lblResult.ForeColor = Color.Red;
            }

            lblResult.Visible = true;
            lblResult.Refresh();
            UpdateStats();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            // 先检查答案
            if (currentIndex < words.Count && string.IsNullOrEmpty(txtAnswer.Text) == false)
            {
                CheckAnswer();
                await Task.Delay(1000); // 延迟1秒让用户看清
            }

            currentIndex++;
            ShowNextWord();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            connection?.Close();
            connection?.Dispose();
            base.OnFormClosing(e);
        }
    }

    public class Word
    {
        public string English { get; set; }
        public string Chinese { get; set; }
    }
}
