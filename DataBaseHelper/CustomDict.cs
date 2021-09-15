using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    public class CustomDict
    {
        public int dictID { get; set; }
        public string dictName { get; set; }
        public string[] dictData { get; set; }

        public string getDictDataStringify()
        {
            string result = "";
            foreach (var line in dictData)
                result += line + ";";
            result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}
