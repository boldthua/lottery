using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大樂透開獎
{
    // Utility 工具箱
    internal class Lottery
    {
        /// <summary>
        /// 產生指定數目的不重複隨機數字
        /// </summary>
        /// <param name="count">指定隨機數字數量</param>
        /// <returns>List的隨機數字</returns>
        public static List<int> GenerateNumbers(int count,int min=1,int max=49) { 
        
            HashSet<int> numbers = new HashSet<int>();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            while (numbers.Count < count) { 

                numbers.Add(random.Next(min,max+1));
            }

            return numbers.ToList();  
        }

        public static List<int> GenerateNumbers(List<int> choosenNums, int min = 1, int max = 49)
        {

            HashSet<int> numbers = new HashSet<int>(choosenNums);
            Random random = new Random(Guid.NewGuid().GetHashCode());
            while (numbers.Count < 6)
            {
                numbers.Add(random.Next(min, max + 1));
            }
            return numbers.ToList();
        }

        public static string CheckPrize(int commonNumsCount, int ifSpecialGet)
        {
            Dictionary<string, (int, int)> prizes = new Dictionary<string, (int, int)>() {
                { "頭獎", (6, 0) },
                { "貳獎", (5, 1) },
                { "參獎", (5, 0) },
                { "肆獎", (4, 1) },
                { "伍獎", (4, 0) },
                { "陸獎", (3, 1) },
                { "柒獎", (2, 1) },
                { "普獎", (3, 0) }
            };
            int speGet = (ifSpecialGet != 0) ? 1 : 0;
            (int, int) check = (commonNumsCount, speGet);

            foreach (var prize in prizes)
            {
                if (prize.Value == check)
                {
                    return prize.Key;
                }
            }

            return "槓龜哭哭";
        }
    }
}
