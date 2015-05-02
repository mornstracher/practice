using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
   public static class Utilitly
    {
       public static List<string> ExtractFieldsValue(string text)
       {
           List<string> strArray = new List<string>();
           string remainingStr = text.Trim();
           int spaceIndex = remainingStr.IndexOf(' ');
           while (spaceIndex > 0)
           {
               string fieldValue = remainingStr.Substring(0, spaceIndex);
               strArray.Add(fieldValue);
               remainingStr = remainingStr.Substring(spaceIndex).Trim();
               spaceIndex = remainingStr.IndexOf(' ');
           }
           return strArray;
       }

       public static string RemoveXDR(string stockName)
       {
           return stockName.Replace("X", "").Replace("D", "").Replace("R", "");
       }
    }
}
