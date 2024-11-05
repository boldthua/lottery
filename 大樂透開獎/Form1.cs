using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 大樂透開獎
{
    public partial class Form1 : Form
    {
        private int lotteryBallsCounts = 49;
        private List<Control> lotteryBalls = new List<Control>();
        private Button randomPick = new Button();
        LotteryService service = new LotteryService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i=1;i<= lotteryBallsCounts; i++)
            {
                Control button = new Button();
                button.Text = i.ToString();
                button.Width = 55;
                button.Height = 55;
                button.Click += Button_Click;
                lotteryBalls.Add(button);
                flowLayoutPanel1.Controls.Add(button);
            }

            randomPick.Text = "快";
            randomPick.Width = 55;
            randomPick.Height = 55;
            randomPick.Click += Button_Click;
            flowLayoutPanel1.Controls.Add(randomPick);
        }

        private void Button_Click(object sender, EventArgs e) // 選取下方50個按鈕
        {
            (bool, string) result = (true, "");
            Button clickedButton = sender as Button; // 獲取觸發事件的按鈕
            if (clickedButton.Text == "快")
            {
                result = service.QuickPickChoosenNum();
            }
            else
            {
                int selecedNumber = int.Parse(clickedButton.Text);
                result = service.SelectedNumber(selecedNumber);
            }

            if(!result.Item1)
            {
                MessageBox.Show(result.Item2, "警告訊息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (result.Item2 == "")
            {
                clickedButton.Enabled = false;
                Label label = new Label();
                label.Width = 35;
                label.Height = 35;
                label.Text = clickedButton.Text;
                label.Font = new Font("微軟正黑體", 10, FontStyle.Regular); // 設定字型和大小
                label.BackColor = Color.White; // 設定背景顏色為白色
                label.ForeColor = Color.Black; // 設定字型顏色為黑色
                label.TextAlign = ContentAlignment.MiddleCenter; // 文字居中對齊
                flowLayoutPanel2.Controls.Add(label);
            }
            else
            {
                string[] choosenNums = result.Item2.Split(',');
                foreach (string choosenNum in choosenNums)
                {
                    for (int i = 0; i < lotteryBalls.Count; i++)
                    {
                        if (lotteryBalls[i].Text == choosenNum)
                        {
                            lotteryBalls[i].Enabled = false;
                            Label label = new Label();
                            label.Width = 35;
                            label.Height = 35;
                            label.Text = lotteryBalls[i].Text;
                            label.Font = new Font("微軟正黑體", 10, FontStyle.Regular); // 設定字型和大小
                            label.BackColor = Color.White; // 設定背景顏色為白色
                            label.ForeColor = Color.Black; // 設定字型顏色為黑色
                            label.TextAlign = ContentAlignment.MiddleCenter; // 文字居中對齊
                            flowLayoutPanel2.Controls.Add(label);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) // 送出
        {
            var result = service.DrawNumber();

            if (!result.Item1)
            {
                MessageBox.Show(result.Item2, "警告訊息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] drawNums = result.Item2.Split(',');

            string specialNum = service.GetspecialNum(); // 單獨抽出特別號

            for (int i = 0; i < lotteryBalls.Count; i++)
            {
                lotteryBalls[i].Enabled = false; // 所有按鈕 disable
            }
            randomPick.Enabled = false;
            button1.Enabled = false; // 送出按鈕 disable
            button2.Enabled = false; // 取消選取按鈕 disable


            for (int i = 0; i < drawNums.Length; i++)
            {
                Label drawLabel = new Label();    // 顯示中獎號
                drawLabel.Text = "";
                drawLabel.Width = 35;
                drawLabel.Height = 35;
                drawLabel.Text = drawNums[i].ToString();
                drawLabel.Font = new Font("微軟正黑體", 10, FontStyle.Regular); // 設定字型和大小
                drawLabel.BackColor = Color.Orange; // 設定背景顏色為橙色
                drawLabel.ForeColor = Color.Black; // 設定字型顏色為黑色
                drawLabel.TextAlign = ContentAlignment.MiddleCenter; // 文字居中對齊
                flowLayoutPanel3.Controls.Add(drawLabel);
            }

            Label specialLabel = new Label();  // 顯示特別號
            specialLabel.Text = "";
            specialLabel.Width = 35;
            specialLabel.Height = 35;
            specialLabel.Text = specialNum;
            specialLabel.Font = new Font("微軟正黑體", 10, FontStyle.Regular); // 設定字型和大小
            specialLabel.BackColor = Color.Red; // 設定背景顏色為紅色
            specialLabel.ForeColor = Color.Yellow; // 設定字型顏色為黃色
            specialLabel.TextAlign = ContentAlignment.MiddleCenter; // 文字居中對齊
            flowLayoutPanel3.Controls.Add(specialLabel);


            var prizeInfo = service.CheckPrizeInfo();

            List<int> sameNum = prizeInfo.Item2;
            int sameSpecialNum = prizeInfo.Item3;

            flowLayoutPanel2.Controls.Clear();
            foreach (int chooseNum in service.GetChoosenNums())
            {
                Label label = new Label();
                label.Text = "";
                label.Width = 35;
                label.Height = 35;
                label.Text = chooseNum.ToString();
                label.ForeColor = Color.Black; // 設定字型顏色為黑色
                label.BackColor = Color.LightGray; // 設定背景顏色為淡灰色
                label.Font = new Font("微軟正黑體", 10, FontStyle.Regular); // 設定字型和大小
                label.TextAlign = ContentAlignment.MiddleCenter; // 文字居中對齊
                foreach (int commonNum in sameNum)
                if (commonNum.ToString() == label.Text)
                    {
                        label.ForeColor = Color.White; // 設定字型顏色為白色
                        label.BackColor = Color.DarkOrange; // 設定背景顏色為深橘色
                    }
                else if (label.Text == sameSpecialNum.ToString())
                    {
                        label.ForeColor = Color.White; // 設定字型顏色為白色
                        label.BackColor = Color.Red; // 設定背景顏色為紅色
                    }

                flowLayoutPanel2.Controls.Add(label);
            }
            textBox3.Text = prizeInfo.Item1;
        }            

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // 取消選取
        {
            var result = service.CancleNumber();
            if (!result.Item1)
            {
                MessageBox.Show(result.Item2, "警告訊息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flowLayoutPanel2.Controls.Clear();
                return;
            }


            string[] updatedNumber = result.Item2.Split(',');

            for (int i = 0; i < lotteryBalls.Count; i++)
            {
                if (lotteryBalls[i].Text == result.Item3.ToString())
                    lotteryBalls[i].Enabled = true;
            }
            flowLayoutPanel2.Controls.Clear();
            if (result.Item2 == "")
                return;
            for (int i = 0; i < updatedNumber.Length; i++)
            {
                Label label = new Label();
                label.Width = 35;
                label.Height = 35;
                label.Text = updatedNumber[i].ToString();
                label.Font = new Font("微軟正黑體", 10, FontStyle.Regular); // 設定字型和大小
                label.BackColor = Color.White; // 設定背景顏色為白色
                label.ForeColor = Color.Black; // 設定字型顏色為黑色
                label.TextAlign = ContentAlignment.MiddleCenter; // 文字居中對齊
                flowLayoutPanel2.Controls.Add(label);
            }
        }

        private void button3_Click(object sender, EventArgs e) //再買一張
        {
            button1.Enabled = true;
            button2.Enabled = true;
            service = new LotteryService();
            lotteryBalls = new List<Control>();
            randomPick = new Button();
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            Form1_Load(null, null);
            textBox3.Text = "";
        }
    }
}
