using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大樂透開獎
{
    internal class LotteryService
    {
        List<int> SelectedNumbers = new List<int>();
        List<int> DrawNumbers = new List<int>();
        int SpecialNumber = 0 ;

        public (bool,string) SelectedNumber(int number)
        {
            if(SelectedNumbers.Count >= 6)
                return (false, "選取號碼已滿！請按「取消選取」來刪除已選號碼。");

            if (number<=0 || number > 49)
                return (false, "請選擇1~49中的號碼");

            if (SelectedNumbers.Contains(number))
                return (false, "該號碼已經被選擇過了!");

            SelectedNumbers.Add(number);
            return (true, "");
        }

        /// <summary>
        /// 點擊送出後的動作
        /// </summary>
        /// <returns>布林值true回傳一組由數列組成的字串，flase則回傳錯誤訊息</returns>
        public (bool, string) DrawNumber()
        {
            if (SelectedNumbers.Count < 6)
                return (false, "尚未選滿六個號碼！");

            SelectedNumbers.Sort(); // 重新排列選取數字

            // 抽六個隨機開獎號
            DrawNumbers = Lottery.GenerateNumbers(6);
            string drawNums = string.Join(",", DrawNumbers);

            DrawNumbers.Sort(); // 重新排列開獎號碼
            return (true, drawNums);
        }

        public (bool, string) QuickPickChoosenNum()
        {
            if (SelectedNumbers.Count >= 6)
                return (false, "選取號碼已滿！請按「取消選取」來刪除已選號碼。");

            // 抽滿自選號
            List<int> updatedNums = Lottery.GenerateNumbers(SelectedNumbers);
            string pickNums = string.Join(",", updatedNums.Except(SelectedNumbers).ToList());
            SelectedNumbers = updatedNums;
            return (true, pickNums);
        }


        public List<int> GetChoosenNums() { return SelectedNumbers; }

        public string GetspecialNum()
        {
            List<int> tempSpecNum = new List<int>();
            while (true)
            {
                tempSpecNum = Lottery.GenerateNumbers(1);
                string specNum = string.Join("", tempSpecNum);
                SpecialNumber = int.Parse(specNum);
                if (!(DrawNumbers.Contains(SpecialNumber)))
                    return (specNum);
            }
        }

        public (string, List<int>, int) CheckPrizeInfo()
        {
            string result = "";
            int specialGet = 0;
            List<int> commonNumber = new List<int>();
            foreach (int drawNum in DrawNumbers)
            {
                foreach (int selectedNum in SelectedNumbers)
                {
                    if(selectedNum == drawNum)
                        commonNumber.Add(drawNum);
                }

                if (drawNum == SpecialNumber)
                    specialGet = drawNum;
            }
            result = Lottery.CheckPrize(commonNumber.Count, specialGet);

            return (result, commonNumber, specialGet);
        }

        public (bool, string, int) CancleNumber()
        {
            if (SelectedNumbers.Count == 0)
            {
                return (false, "已無號碼可以取消！", 0);
            }
            int lastNumber = SelectedNumbers.Last();
            SelectedNumbers.Remove(lastNumber);
            string updatedNums = string.Join(",", SelectedNumbers);

            return (true, updatedNums, lastNumber);
        }
    }
}
