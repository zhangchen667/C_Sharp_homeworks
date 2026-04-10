using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // 存储第一个操作数
        private double firstNumber = 0;
        // 存储运算符
        private string operation = "";
        // 是否开始新的数字输入
        private bool isNewEntry = true;

        public Form1()
        {
            InitializeComponent();
        }

        // 数字按钮点击事件
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string number = btn.Text;

            if (isNewEntry)
            {
                txtDisplay.Text = number;
                isNewEntry = false;
            }
            else
            {
                if (txtDisplay.Text == "0")
                {
                    txtDisplay.Text = number;
                }
                else
                {
                    txtDisplay.Text += number;
                }
            }
        }

        // 运算符按钮点击事件
        private void OperatorButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string currentOperator = btn.Text;

            // 获取显示的数字作为第一个操作数
            firstNumber = double.Parse(txtDisplay.Text);
            operation = currentOperator;
            isNewEntry = true;

            // 更新显示为表达式
            txtDisplay.Text = firstNumber + operation;
        }

        // 等号按钮点击事件
        private void btnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                // 从显示中提取第二个操作数
                string displayText = txtDisplay.Text;
                double secondNumber = 0;

                // 如果显示的是表达式（如"18+"），则需要输入第二个数字
                if (displayText.EndsWith("+") || displayText.EndsWith("-") ||
                    displayText.EndsWith("*") || displayText.EndsWith("/"))
                {
                    isNewEntry = true;
                    return;
                }

                // 从表达式中提取第二个数字
                int opIndex = -1;
                foreach (char c in new char[] { '+', '-', '*', '/' })
                {
                    opIndex = displayText.IndexOf(c);
                    if (opIndex >= 0) break;
                }

                if (opIndex >= 0)
                {
                    operation = displayText[opIndex].ToString();
                    firstNumber = double.Parse(displayText.Substring(0, opIndex));
                    secondNumber = double.Parse(displayText.Substring(opIndex + 1));
                }
                else
                {
                    secondNumber = double.Parse(displayText);
                }

                double result = 0;

                switch (operation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "*":
                        result = firstNumber * secondNumber;
                        break;
                    case "/":
                        if (secondNumber == 0)
                        {
                            MessageBox.Show("除数不能为零！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnClear_Click(null, null);
                            return;
                        }
                        result = firstNumber / secondNumber;
                        break;
                    default:
                        return;
                }

                // 显示完整表达式和结果
                txtDisplay.Text = firstNumber + operation + secondNumber + "=" + result;
                isNewEntry = true;
                operation = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnClear_Click(null, null);
            }
        }

        // 清空按钮点击事件
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
            firstNumber = 0;
            operation = "";
            isNewEntry = true;
        }
    }
}
