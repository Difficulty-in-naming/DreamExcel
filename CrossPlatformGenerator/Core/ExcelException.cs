using System;

namespace DreamExcel.Core
{
    public class ExcelException:Exception
    {
        public ExcelException(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Beep();
        }
    }
}
