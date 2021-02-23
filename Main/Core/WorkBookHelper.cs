using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace DreamExcel.Core
{
    public static class WorkBookHelper
    {
        /// <summary>
        /// 检查目标Excel是否支持使用此插件的功能
        /// </summary>
        public static bool IsVaildWorkBook(this Workbook wb)
        {
            var isSpWorkBook = Path.GetFileNameWithoutExtension(wb.Name).EndsWith(Config.Info.FileSuffix);
            if (!isSpWorkBook)
                return true;
            return false;
        }
    }
}
