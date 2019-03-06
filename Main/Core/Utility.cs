using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DreamExcel.Core
{
    public class ExcelException:Exception
    {
        public ExcelException(string str)
        {
            MessageBox.Show(str);
        }
    }

    public class Utility
    {
        public static void CheckCondition(Func<bool> condition, string str)
        {
            if(!condition())
                throw new ExcelException(str);
        }
    }
}
