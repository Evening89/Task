using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Utils
{
    public class Randoms
    {
        public static Random random = new Random((int)DateTime.Now.Ticks);
        public string RandomString(int numOfChars) //генерация строки случайных букв a-z
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < numOfChars; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 97)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public string RandomNumber(int numOfChars)//генерация строки случайных цифр 0-9
        {
            string result = "";
            for (int i = 0; i < numOfChars; i++)
            {
                result += random.Next(10).ToString();
            }

            return result;
        }
    }
}
