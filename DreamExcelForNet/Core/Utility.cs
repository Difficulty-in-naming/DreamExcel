using System;

namespace DreamExcel.Core
{
    public class ExcelException:Exception
    {
        public ExcelException(string str)
        {
            Console.WriteLine(str);
            Console.Beep();
        }
    }

    public class Utility
    {
        public static void CheckCondition(Func<bool> condition, string str)
        {
            if(!condition())
                Console.WriteLine(str);
        }
    }
}
